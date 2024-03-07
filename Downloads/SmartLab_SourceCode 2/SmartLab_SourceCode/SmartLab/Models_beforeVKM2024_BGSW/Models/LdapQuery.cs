using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace LC_Reports_V1.Models
{
  public sealed class SubDomainLdapQuery : ALdapQueryBase, IDisposable
  {
    public SubDomainLdapQuery(string baseDomain) : base(baseDomain)
    {
      mSearcherDict = new Dictionary<string, DirectorySearcher>();
    }
  
    private Dictionary<string, DirectorySearcher> mSearcherDict;

    protected override DirectorySearcher GetDirectorySearcher(string subDomain)
    {
      subDomain = subDomain.ToLower();
      if (!mSearcherDict.TryGetValue(subDomain, out DirectorySearcher directorySearcher))
      {
        directorySearcher = CreateDirectorySearcher(subDomain);
        mSearcherDict.Add(subDomain, directorySearcher);
      }
      return directorySearcher;
    }

    public void Dispose()
    {
      foreach (var s in mSearcherDict.Values) s.Dispose();
      mSearcherDict = null;
    }
  }

  public sealed class DomainLdapQuery : ALdapQueryBase, IDisposable
  {
    public DomainLdapQuery(string domain) : base(domain)
    {
      mSearcher = CreateDirectorySearcher();
    }

    private DirectorySearcher mSearcher;

    protected override DirectorySearcher GetDirectorySearcher(string subDomain)
    {
      return mSearcher;
    }

    public void Dispose()
    {
      mSearcher.Dispose();
      mSearcher = null;
    }
  }




  public abstract class ALdapQueryBase 
  {
    protected ALdapQueryBase(string baseDomain)
    {
      mBaseDomain = baseDomain.Split('.'); 
    }

    protected abstract DirectorySearcher GetDirectorySearcher(string subDomain);
    protected DirectorySearcher CreateDirectorySearcher(string subDomain = null)
    {
      var list = new List<string>();
      if (!string.IsNullOrEmpty(subDomain)) list.Add($"DC={subDomain}");
      foreach (var p in mBaseDomain) list.Add($"DC={p}");
      return new DirectorySearcher(new DirectoryEntry("LDAP://" + string.Join(",", list)));
    }

    #region Fields
    private string[] mBaseDomain;
    private Dictionary<string, DomainEntity> mUsersDict;

    private const string CN = "cn";
    private const string samAccountName = "samaccountname";
    private const string Member = "member";
    private const string ObjectClass = "objectClass";
    private const string Description = "description";
    #endregion

    #region Methods
    public IEnumerable<DomainEntity> ResolveGroup(string groupSamAccountName, string subDomain, out bool resolved)
    {
      mUsersDict = new Dictionary<string, DomainEntity>();
      resolved = GroupCollectUsersRecursiveBySamAccountName(groupSamAccountName, subDomain);
      return mUsersDict.Values;
    }

    public Dictionary<string, string> GetProperties(DomainEntity entity, params string[] propertiesToLoad)
    {
      return GetProperties(entity.Name, entity.Domain, propertiesToLoad);
    }


    public Dictionary<string,string> GetProperties(string sam, string subDomain, params string[] propertiesToLoad)
    {
      var result = Search(subDomain, $"({samAccountName}={sam})", propertiesToLoad).FindOne();
      if(null != result)
      {
        var dict = new Dictionary<string, string>();
        foreach(string key in result.Properties.PropertyNames)
        {
          if(key != "adspath") dict.Add(key, result.Properties[key][0].ToString());
        }
        return dict;
      }
      else
      {
        return null;
      }
    }


    public DirectorySearcher Search(string subDomain, string filter, params string[] propertiesToLoad)
    {
      var s = GetDirectorySearcher(subDomain);
      s.Filter = filter;
      s.PropertiesToLoad.Clear();
      foreach(var prop in propertiesToLoad) s.PropertiesToLoad.Add(prop);
      return s;
    }

    private bool CheckIsUserByCName(string id, string subDomain)
    {
      var result = Search(subDomain, $"({CN}={id})", ObjectClass).FindOne();
      if (null != result)
      {
        foreach(string value in result.Properties[ObjectClass])
        {
          if ("user" == value || "person" == value) return true;
        }
      }
      return false;
    }

    private string GetSamAccountNameByCName(string cName, string subDomain)
    {
      var result = Search(subDomain, $"({CN}={cName})", samAccountName).FindOne();
      return (null != result) ? (string)result.Properties[samAccountName][0] : null;
    }

    private bool GroupCollectUsersRecursiveBySamAccountName(string groupName, string subDomain)
    {
      var result = Search(subDomain, $"({samAccountName}={groupName})", Member).FindOne();
      if (result != null)
      {
        foreach(string member in result.Properties[Member])
        {
          var memberSubDomain = string.Empty;
          var memberCn = string.Empty;

          foreach (var memberEntry in member.Split(','))
          {
            var parts = memberEntry.Split('=');
            if(parts.Length == 2)
            {
              // Valid Entry
              switch(parts[0])
              {
                case "CN":
                  if (string.IsNullOrEmpty(memberCn))
                  {
                    memberCn = parts[1].ToLower();
                  }
                  break;

                case "DC":
                  if (string.IsNullOrEmpty(memberSubDomain))
                  {
                    if(!mBaseDomain.Contains(parts[1])) memberSubDomain = parts[1];
                  }
                  break;
              }
            }
          }

          if (CheckIsUserByCName(memberCn, memberSubDomain))
          {
            var entity = new DomainEntity(memberCn, memberSubDomain);
            var key = entity.ToString();
            if (!mUsersDict.ContainsKey(key)) mUsersDict.Add(key, entity);
          }
          else
          {           
            var nextGroupSamAccountName = GetSamAccountNameByCName(memberCn, memberSubDomain);
            if (null != nextGroupSamAccountName) GroupCollectUsersRecursiveBySamAccountName(nextGroupSamAccountName, memberSubDomain);
          }
        }
        return true;
      }
      else
      {
        return false;
      }
    }
    #endregion
  }



  public class DomainEntity
  {
    public static DomainEntity Parse(string content, string defaultDomain = null)
    {
      var parts = content.Split('\\');
      if (parts.Length == 2)
      {
        return new DomainEntity(parts[1], parts[0]);
      }
      else
      {
        return new DomainEntity(content, defaultDomain);
      }
    }

    internal DomainEntity(string user, string domain)
    {
      Name = user;
      Domain = domain;
    }

    public string Name { get; }
    public string Domain { get; }

    public override string ToString()
    {
      return null != Domain ? $"{Domain}\\{Name}" : Name;
    }
  }
}



/*  Available Properties of a User Entry @bosch.com
 * 
    [objectclass] => top
 => [cn] => EBL2SI   Attention: CN is not always equal to samaccoutnname (equality is Bosch Domain specific)
 => [sn] => Ebel
 => [c] => DE
 => [l] => Abstatt
    [st] => Baden-Wuerttemberg
    [description] => 201523
    [postalcode] => 74232
 => [physicaldeliveryofficename] => Abt 101/2 A
 => [telephonenumber] => +49(7062)911-4298
    [facsimiletelephonenumber] => +49(711)811-5184090
    [usercertificate] => 0‚–0‚~ Cf|f0*†H†÷
 => [givenname] => Christian
    [distinguishedname] => CN=EBL2SI,OU=E,OU=Useraccounts,OU=Abt,DC=de,DC=bosch,DC=com
    [instancetype] => 4
    [whencreated] => 20030110103224.0Z
    [whenchanged] => 20180227052211.0Z
 => [displayname] => Ebel Christian (CC/ESM3)
    [othertelephone] => 4298
    [usncreated] => 4752771
    [memberof] => CN=CI/OSE-Artifactory-Users,OU=OTHERS,OU=TEMP,OU=DISTRIBUTIONLISTS,OU=SDE,OU=Applications,DC=de,DC=bosch,DC=com
    [usnchanged] => 476482673
 => [co] => Germany
 => [department] => CC/ESM3
    [company] => Robert Bosch GmbH
    [proxyaddresses] => SIP:Christian.Ebel@bosch.com
    [homemdb] => CN=DAG220-EMEA-0029,CN=Databases,CN=Exchange Administrative Group (FYDIBOHF23SPDLT),CN=Administrative Groups,CN=BOSCH,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=bosch,DC=com
    [streetaddress] => Robert-Bosch-Allee 1
    [garbagecollperiod] => 1209600
    [mdbusedefaults] => TRUE
    [mapirecipient] => FALSE
    [extensionattribute2] => 201523
    [extensionattribute4] => Forschung und Entwicklung
    [mailnickname] => Christian.Ebel
    [protocolsettings] => MAPIÂ§Â§Â§Â§Â§0Â§Â§Â§
    [internetencoding] => 0
    [extensionattribute14] => christian.ebel@ucc1.bosch.com
    [replicatedobjectversion] => 0
    [name] => EBL2SI
    [objectguid] => ~‹kH®SL…PÿZLØD
    [useraccountcontrol] => 512
    [badpwdcount] => 0
    [codepage] => 0
    [countrycode] => 276
    [homedirectory] => \\FE00FS47.DE.BOSCH.COM\EBL2SI$
    [homedrive] => U:
    [badpasswordtime] => 131641818925876093
    [lastlogon] => 131099248237282070
    [scriptpath] => LOGON-ABT.CMD
    [logonhours] => ÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿ
    [pwdlastset] => 131583311296664541
    [primarygroupid] => 513
    [objectsid] => 
    [comment] => 653467
    [accountexpires] => 9223372036854775807
    [logoncount] => 0
 => [samaccountname] => EBL2SI
    [samaccounttype] => 805306368
    [showinaddressbook] => CN=All Users,CN=All Address Lists,CN=Address Lists Container,CN=BOSCH,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=bosch,DC=com
    [legacyexchangedn] => /o=BOSCH/ou=MAIL04/cn=Recipients/cn=Christian.Ebel
 => [userprincipalname] => ebl2si@bosch.com
    [lockouttime] => 0
    [ipphone] => +49(711)811-3621725
    [objectcategory] => CN=Person,CN=Schema,CN=Configuration,DC=bosch,DC=com
    [dscorepropagationdata] => 20170405102451.0Z
    [ms-ds-consistencyguid] => ~‹kH®SL…PÿZLØD
    [lastlogontimestamp] => 131640982844055125
    [textencodedoraddress] => X400:C=DE;A=DBP;P=BOSCH-01;O=X400;S=Ebel;G=Christian;OU1=MAIL04;
 => [mail] => Christian.Ebel@de.bosch.com
    [pager] => +497118113621725
    [thumbnailphoto] => ÿØÿà
    [msexchhomeservername] => /o=BOSCH/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Configuration/cn=Servers/cn=FE-MBX2014
    [replicationsignature] => AMôY‡MšÍeÄ¬fÂ
    [msexchalobjectversion] => 5939
    [msexchadcglobalnames] => EX5:cn=Christian.Ebel,cn=Recipients,ou=MAIL04,o=BOSCH:organizationalperson$person$top000000009C12EE3A7596C501
    [msexchmailboxsecuritydescriptor] => 
    [msexchuseraccountcontrol] => 0
    [msexchmailboxguid] => ˆ™n	@¡qV¹b3#
    [dlmemdefault] => 1
    [msexchomaadminwirelessenable] => 4
    [msexchpoliciesexcluded] => {26491CFC-9E50-4857-861B-0CB8DF22B5D7}
    [msexchumdtmfmap] => reversedPhone:8924
    [msexchmailboxtemplatelink] => CN=RB - Retention Policy (Personal Tags),CN=Retention Policies Container,CN=BOSCH,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=bosch,DC=com
    [msexchrecipientdisplaytype] => 1073741824
    [msexchelcmailboxflags] => 18
    [msexchsafesendershash] => rY=#ÏÓ,qÕ
    [msexchuserculture] => de-DE
    [msexchmessagehygienescljunkthreshold] => -2147483641
    [msexchversion] => 88218628259840
    [msexchrecipienttypedetails] => 1
    [msexchmobilemailboxflags] => 1
    [msrtcsip-internetaccessenabled] => TRUE
    [msrtcsip-userroutinggroupid] => Á°Çzx]ª-V
    [msrtcsip-deploymentlocator] => SRV:
    [msrtcsip-userpolicies] => 1=12
    [msexchblockedsendershash] => u»wl¢+@C-8
    [msrtcsip-userenabled] => TRUE
    [msexchextensionattribute33] => VSS
    [msexchwhenmailboxcreated] => 20140531065836.0Z
    [msexchrbacpolicylink] => CN=Bosch Default Role Assignment Policy,CN=Policies,CN=RBAC,CN=BOSCH,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=bosch,DC=com
    [msrtcsip-primaryhomeserver] => CN=Lc Services,CN=Microsoft,CN=2:1,CN=Pools,CN=RTC Service,CN=Services,CN=Configuration,DC=bosch,DC=com
    [msexchtextmessagingstate] => 302120705
    [msrtcsip-primaryuseraddress] => sip:Christian.Ebel@bosch.com
    [msrtcsip-optionflags] => 257
    [msrtcsip-line] => tel:+4970629114298
    [msrtcsip-federationenabled] => TRUE


  */