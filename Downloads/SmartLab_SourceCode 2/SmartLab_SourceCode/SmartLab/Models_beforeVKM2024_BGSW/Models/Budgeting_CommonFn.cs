using LC_Reports_V1.Controllers;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Configuration;

namespace LC_Reports_V1.Models
{
    public class Budgeting_CommonFn
    {

        //RW01- Write below string to text file periodically
        static StringBuilder logs = new StringBuilder();




        public static void emailNotification_VKM(Emailnotify emailnotify)

        {
            //bool enableLogs = true;
            //var path = Server.MapPath(@"~/TextFiles/ActiveUsers.txt");
            //var path = HttpContext.Current.Server.MapPath(@"~/Content/LEP_MailLogs.txt");

            //if (enableLogs)
            //{
            // var file = File.Create("C:\USER_DRIVE\MAE9COB\Temp\SmartLab_ProductionServer_May4_2021\LC_Reports_V1_deployedMay9_TestDB\LC_Reports_V1\LC_Reports_V1\Content\LEP_Logs.txt");
            //var file = File.Create(path);
            //file.Close();

            //TextWriter sw = new StreamWriter(path, true); //have 2 logs; critical events & errors - clear biweekly - prevent a step of local checking
            //sw.Write(logs.ToString());
            //sw.Close();
            //Enter log fn - input : 
            //nlog - logging dll; while booking tool api; it comes

            //FileStream ostrm;
            //StreamWriter writer;
            //TextWriter oldOut = Console.Out;
            //try
            //{
            //    ostrm = new FileStream("./Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    writer = new StreamWriter(ostrm);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Cannot open Redirect.txt for writing");
            //    Console.WriteLine(e.Message);
            //    return;
            //}
            //Console.SetOut(writer);

            //}
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
            string fromEmail = "Cob_RBEI_SmartLabDevTeam@bcn.bosch.com ";
            string toEmail = "";
            string ccEmail = "";
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;


            //add CC or BCC
            MailAddress bcarbonCopy = new MailAddress("Meena.MadhevanpillaiArunadevi@in.bosch.com");
            //MailAddress bcarbonCopy = new MailAddress("external.Bharani.G @in.bosch.com");
            mail.Bcc.Add(bcarbonCopy);
            // bcarbonCopy = new MailAddress("valson.joshuaninan@in.bosch.com");
            // mail.Bcc.Add(bcarbonCopy); //there is an option of mail.Bcc also 



            SmtpClient client = new SmtpClient("rb-smtp-int.bosch.com", 25);
            client.Timeout = 200000;
            // Credentials are necessary if the server requires the client
            // to authenticate before it will send email on the client's behalf.
            client.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
                {

                    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;



                    if (emailnotify.ReviewLevel == "L1" && emailnotify.is_ApprovalorSendback)
                    {

                        //L1->L2
                        if (emailnotify.RequestID_foremail == 1999999999)
                        {

                            var templist = emailnotify.Requests_foremail.GroupBy(x => x.DHNT);

                            foreach (var dhgroup in templist)
                            {
                                MailMessage mail1 = new MailMessage();
                                mail1.IsBodyHtml = true;
                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(dhgroup.Key.ToLower().Trim())) != null)
                                {
                                    toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(dhgroup.Key.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                else
                                    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(dhgroup.Key.ToLower().Trim())).NTID.ToLower().Trim();

                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())) != null)
                                {
                                    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                else
                                    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                if (toEmail == "idm1cob")
                                {
                                    toEmail = "din2cob";
                                }

                                int count = 0;
                                decimal totalAmount = 0;
                                foreach (var item in dhgroup)
                                {
                                    totalAmount += (decimal)item.TotalPrice;
                                    count++;
                                }
                                mail1.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Request";

                                //mail1.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + count + " item(s) amounting to $" + totalAmount + " submitted by " + presentUserName + " are available in your Queue for L2 approval." + "<br />" + "<br />" + "Click <a href = 'http://smartlab.apac.bosch.com/BudgetingApprovals'> here</a> to review the item request.  <br />" + "<br />" + "Thank you. " + "<br />" + "<br />Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";

                                mail1.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + count + " item(s) amounting to $" + totalAmount + " submitted by " + presentUserName + " are available in your Queue for Tech/Program Director Review." + "<br />" + "<br />" + "Click <a href = 'http://smartlab.apac.bosch.com/BudgetingApprovals'> here</a> to review the item request.  <br />" + "<br />" + "Thank you. " + "<br />" + "<br />Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                                DirectorySearcher search1 = new DirectorySearcher(); //class to perform a search against LDAP connection

                                // specify the search filter
                                search1.Filter = "(&(objectClass=user)(anr=" + toEmail + "))";
                                search1.PropertiesToLoad.Add("mail");        // smtp mail address

                                // perform the search
                                SearchResult result11 = search1.FindOne();
                                toEmail = result11.Properties["mail"][0].ToString();


                                search1.Filter = "(&(objectClass=user)(anr=" + ccEmail + "))";
                                search1.PropertiesToLoad.Add("mail");        // smtp mail address

                                // perform the search
                                SearchResult result111 = search1.FindOne();
                                ccEmail = result111.Properties["mail"][0].ToString();


                                mail1.From = new MailAddress(fromEmail);
                                mail1.To.Add(new MailAddress(toEmail));



                                mail1.CC.Add(new MailAddress(ccEmail));
                                // mail.Bcc.Add(bcarbonCopy);


                                bool failedIndicator1 = false;
                                try
                                {
                                    client.Send(mail1);
                                }
                                catch (Exception ex)
                                {

                                    WriteLog("Error - emailNotification_VKM - Planning - L1->L2 Submit All: NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                                    if (ex.ToString().Contains("timed out"))
                                    {

                                        failedIndicator1 = true;
                                    }

                                }
                                if (failedIndicator1)
                                {
                                    WriteLog("Retrying Mail to..." + toEmail);
                                    try
                                    {
                                        client.Send(mail1);
                                        WriteLog("Retry success");
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLog("Error - emailNotification_VKM - Planning - Retry failed L1->L2 Submit All: NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                                    }
                                }

                            }


                            //if(BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_toEmail.ToLower().Trim())) != null)
                            //{
                            //    toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_toEmail.ToLower().Trim())).NTID.ToLower().Trim();

                            //}
                            //else
                            //    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_toEmail.ToLower().Trim())).NTID.ToLower().Trim();

                            //if(BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_ccEmail.ToLower().Trim())) != null)
                            //{
                            //    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_ccEmail.ToLower().Trim())).NTID.ToLower().Trim();

                            //}
                            //else
                            //    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_ccEmail.ToLower().Trim())).NTID.ToLower().Trim();

                            //mail.Subject = "VKM" + DateTime.Now.Year + " Item Request";

                            //mail.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + emailnotify.Count + " item(s) amounting to $" + emailnotify.TotalAmount + " submitted by " + presentUserName + " are available in your Queue for L2 approval." + "<br />" + "<br />" + "Click <a href = 'http://smartlab.apac.bosch.com/BudgetingApprovals'> here</a> to review the item request.  <br />" + "<br />" + "Thank you. " + "<br />" + "<br />Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";

                            //mail.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + emailnotify.Count + " item(s) amounting to $" + emailnotify.TotalAmount + " submitted by " + presentUserName + " are available in your Queue for Tech/Program Director Review." + "<br />" + "<br />" + "Click <a href = 'http://smartlab.apac.bosch.com/BudgetingApprovals'> here</a> to review the item request.  <br />" + "<br />" + "Thank you. " + "<br />" + "<br />Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                        }
                        else
                        {
                            RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == emailnotify.RequestID_foremail).FirstOrDefault<RequestItems_Table>();
                            if (BudgetingController.lstItems != null && BudgetingController.lstItems.Count() > 0)
                            {
                                var dept = BudgetingController.lstDEPTs.Find(dpt => dpt.ID.Equals(int.Parse(item.DEPT))).DEPT;
                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim())) != null)
                                {

                                    if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim()) && user.Department.ToUpper().Trim().Equals(dept.ToUpper().Trim())) != null)  //name in new spot on but dept is in old spot on
                                        toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim()) && user.Department.ToUpper().Trim().Equals(dept.ToUpper().Trim())).NTID.ToLower().Trim();
                                    else if (BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim()) && user.Department.ToUpper().Trim().Equals(dept.ToUpper().Trim())) != null)
                                    {
                                        toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim()) && user.Department.ToUpper().Trim().Equals(dept.ToUpper().Trim())).NTID.ToLower().Trim();

                                    }
                                    else
                                        toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim())).NTID.ToLower().Trim();
                                }
                                else
                                {
                                    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim()) && user.Department.ToUpper().Trim().Equals(dept.ToUpper().Trim())).NTID.ToLower().Trim();
                                }
                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())) != null)
                                {
                                    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();
                                }
                                else
                                {
                                    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                if (toEmail == "idm1cob")
                                {
                                    toEmail = "din2cob";
                                }


                                mail.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Request";


                                //mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for L2 approval. Please kindly find the Request Details below." +
                                //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br />" + " Requested Quantity: " + item.ReqQuantity + "<br />" + " Total Amount: $" + item.TotalPrice + "<br />" + " Requestor Name: " + item.RequestorNT + "<br />" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com/BudgetingApprovals'> here</a> to review the item request." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                                //mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for Tech/Program Director review. Please kindly find the Request Details below." +
                                //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br />" + " Requested Quantity: " + item.ReqQuantity + "<br />" + " Total Amount: $" + item.TotalPrice + "<br />" + " Requestor Name: " + item.RequestorNT + "<br />" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com/BudgetingApprovals'> here</a> to review the item request." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                                //mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for L2 approval. Please kindly find the Request Details below." +
                                //    "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>Requestor Name</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "</td><td>" + item.ReqQuantity + "</td><td>$" + item.TotalPrice + "</td><td>" + item.RequestorNT + "</td></tr></table>" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com/BudgetingApprovals'> here</a> to review the item request." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                                mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for Tech/Program Director review. Please kindly find the Request Details below." +
                                    "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>Tech/Program Lead</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "</td><td>" + item.ReqQuantity + "</td><td>$" + item.TotalPrice + "</td><td>" + item.RequestorNT + "</td></tr></table>" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com/BudgetingApprovals'> here</a> to review the item request." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                            }


                        }

                    }
                    else if (emailnotify.ReviewLevel == "L2" && emailnotify.is_ApprovalorSendback)
                    {


                        //L2->L3
                        if (emailnotify.RequestID_foremail == 1999999999)
                        {
                            List<RequestItems_Table> templist = new List<RequestItems_Table>();
                            //templist = db.RequestItems_Table.ToList();
                            //if(BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_toEmail.ToLower().Trim())) != null)
                            //{
                            //    toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_toEmail.ToLower().Trim())).NTID.ToLower().Trim();

                            //        //}
                            //        //else
                            //        //    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_toEmail.ToLower().Trim())).NTID.ToLower().Trim();

                            //        //if (toEmail == "idm1cob")
                            //        //{
                            //        //    toEmail = "din2cob";
                            //        //}

                            //        //if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_ccEmail.ToLower().Trim())) != null)
                            //        //{
                            //        //    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_ccEmail.ToLower().Trim())).NTID.ToLower().Trim();

                            //        //}
                            //        //else
                            //        //    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_ccEmail.ToLower().Trim())).NTID.ToLower().Trim();

                            //        //mail.Subject = "VKM" + DateTime.Now.Year + " Item Request";

                            //        //mail.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + emailnotify.Count + " item(s) amounting to $" + emailnotify.TotalAmount + " submitted by " + presentUserName + " are available in your Queue for L3 approval." + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            //        //mail.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + emailnotify.Count + " item(s) amounting to $" + emailnotify.TotalAmount + " submitted by " + presentUserName + " are available in your Queue for VKM SPOC review." + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";



                            var templist1 = emailnotify.Requests_foremail.GroupBy(x => x.SHNT);

                            foreach (var dhgroup in templist1)
                            {
                                MailMessage mail1 = new MailMessage();
                                mail1.IsBodyHtml = true;
                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(dhgroup.Key.ToLower().Trim())) != null)
                                {
                                    toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(dhgroup.Key.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                else
                                    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(dhgroup.Key.ToLower().Trim())).NTID.ToLower().Trim();

                                if (toEmail == "idm1cob")
                                {
                                    toEmail = "din2cob";
                                }

                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())) != null)
                                {
                                    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                else
                                    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                int count = 0;
                                decimal totalAmount = 0;
                                foreach (var item in dhgroup)
                                {
                                    totalAmount += (decimal)item.ApprCost;
                                    count++;
                                }
                                mail1.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Request";

                                //mail1.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + count + " item(s) amounting to $" + totalAmount + " submitted by " + presentUserName + " are available in your Queue for L3 approval." + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                                mail1.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + count + " item(s) amounting to $" + totalAmount + " submitted by " + presentUserName + " are available in your Queue for VKM SPOC review." + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


                                DirectorySearcher search1 = new DirectorySearcher(); //class to perform a search against LDAP connection

                                // specify the search filter
                                search1.Filter = "(&(objectClass=user)(anr=" + toEmail + "))";
                                search1.PropertiesToLoad.Add("mail");        // smtp mail address

                                // perform the search
                                SearchResult result11 = search1.FindOne();
                                toEmail = result11.Properties["mail"][0].ToString();


                                search1.Filter = "(&(objectClass=user)(anr=" + ccEmail + "))";
                                search1.PropertiesToLoad.Add("mail");        // smtp mail address

                                // perform the search
                                SearchResult result111 = search1.FindOne();
                                ccEmail = result111.Properties["mail"][0].ToString();


                                mail1.From = new MailAddress(fromEmail);
                                mail1.To.Add(new MailAddress(toEmail));



                                mail1.CC.Add(new MailAddress(ccEmail));
                                // mail.Bcc.Add(bcarbonCopy);


                                bool failedIndicator1 = false;
                                try
                                {
                                    client.Send(mail1);
                                }
                                catch (Exception ex)
                                {

                                    WriteLog("Error - emailNotification_VKM - Planning : L2->L3 Submit All NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                                    if (ex.ToString().Contains("timed out"))
                                        failedIndicator1 = true;
                                }
                                if (failedIndicator1)
                                {

                                    WriteLog("Retrying email to : " + toEmail);
                                    try
                                    {
                                        client.Send(mail1);
                                        WriteLog("Retrying success to : " + toEmail);
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLog("Error - emailNotification_VKM - Planning : L2->L3 Submit All Retry Error - NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                                    }
                                }

                            }
                        }
                        else
                        {
                            RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == emailnotify.RequestID_foremail).FirstOrDefault<RequestItems_Table>();

                            if (BudgetingController.lstItems != null && BudgetingController.lstItems.Count() > 0)
                            {
                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.SHNT.ToLower().Trim())) != null)
                                {
                                    toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.SHNT.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                else
                                    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.SHNT.ToLower().Trim())).NTID.ToLower().Trim();

                                if (toEmail == "idm1cob")
                                {
                                    toEmail = "din2cob";
                                }

                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())) != null)
                                {
                                    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                else
                                    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim())).NTID.ToLower().Trim();


                                mail.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Request";

                                //mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for L3 approval. Please kindly find the Request Details below." +
                                //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br /> Requested Quantity: " + item.ApprQuantity + "<br /> Total Amount: $" + item.ApprCost + "<br /> L2 Reviewer: " + item.DHNT + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br />Thank you. " + "<br />" + "<br />" + " Regards," + " ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                                //mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for VKM SPOC review. Please kindly find the Request Details below." +
                                //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br /> Requested Quantity: " + item.ApprQuantity + "<br /> Total Amount: $" + item.ApprCost + "<br /> Tech/Program Director: " + item.DHNT + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br />Thank you. " + "<br />" + "<br />" + " Regards," + " ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                                mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for VKM SPOC review review. Please kindly find the Request Details below." +
                               "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>Tech/Program Director</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "</td><td>" + item.ApprQuantity + "</td><td>$" + item.ApprCost + "</td><td>" + item.DHNT + "</td></tr></table>" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com/Budgetingvkm'> here</a> to review the item request." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";

                            }

                        }

                    }
                    else if (emailnotify.ReviewLevel == "L2" && !emailnotify.is_ApprovalorSendback)
                    {

                        //L2->L1
                        RequestItems_Table item_returned = db.RequestItems_Table.Where(x => x.RequestID == emailnotify.RequestID_foremail).FirstOrDefault<RequestItems_Table>();
                        if (BudgetingController.lstItems != null && BudgetingController.lstItems.Count() > 0)
                        {
                            if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned.RequestorNT.ToLower().Trim())) != null)
                            {
                                toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                            }
                            else
                                toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                            if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned.DHNT.ToLower().Trim())) != null)
                            {
                                ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned.DHNT.ToLower().Trim())).NTID.ToLower().Trim();

                            }
                            else
                                ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned.DHNT.ToLower().Trim())).NTID.ToLower().Trim();
                            var sendback_comments = (item_returned.L2_Remarks != "" && item_returned.L2_Remarks != null) ? item_returned.L2_Remarks : "-";
                            mail.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Sent Back";

                            //mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your HoE is available in your Queue. Please kindly find the Item Details below." +

                            //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned.ItemName)).Item_Name + "<br /> Requested Quantity: " + item_returned.ReqQuantity + "<br /> Total Amount: $" + item_returned.TotalPrice + "<br /> L2 Reviewer: " + item_returned.DHNT + "<br /> Comments: " + sendback_comments + "<br />" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned.ItemName)).Item_Name + "<br /> Requested Quantity: " + item_returned.ReqQuantity + "<br /> Total Amount: $" + item_returned.TotalPrice + "<br /> Tech/Program Director: " + item_returned.DHNT + "<br /> Comments: " + sendback_comments + "<br />" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


                            //mail.Body = " Dear Tech/Program Lead" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your HoE is available in your Queue. Please kindly find the Item Details below." +
                            //    "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>Tech/Program Director</B></td><td><B>Comments</B></td></tr><tr><td> " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned.ItemName)).Item_Name + "</td><td>" + item_returned.ReqQuantity + "</td><td> $" + item_returned.TotalPrice + "</td><td>" + item_returned.DHNT + "</td><td> " + sendback_comments + "</td></tr></table>" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


                            mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your HoE is available in your Queue. Please kindly find the Item Details below." +
                                "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>L2 Reviewer</B></td><td><B>Comments</B></td></tr><tr><td> " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned.ItemName)).Item_Name + "</td><td>" + item_returned.ReqQuantity + "</td><td> $" + item_returned.TotalPrice + "</td><td>" + item_returned.DHNT + "</td><td> " + sendback_comments + "</td></tr></table>" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";



                        }

                    }
                    else if (emailnotify.ReviewLevel == "L3" && !emailnotify.is_ApprovalorSendback)
                    {

                        //L3->L1
                        RequestItems_Table item_returned1 = db.RequestItems_Table.Where(x => x.RequestID == emailnotify.RequestID_foremail).FirstOrDefault<RequestItems_Table>();
                        if (BudgetingController.lstItems != null && BudgetingController.lstItems.Count() > 0)
                        {
                            if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.RequestorNT.ToLower().Trim())) != null)
                            {
                                toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                            }
                            else
                                toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                            if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.SHNT.ToLower().Trim())) != null)
                            {
                                ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.SHNT.ToLower().Trim())).NTID.ToLower().Trim();

                            }
                            else
                                ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.SHNT.ToLower().Trim())).NTID.ToLower().Trim();

                            if (toEmail == "idm1cob")
                            {
                                toEmail = "din2cob";
                            }
                            var sendback_comments = (item_returned1.L3_Remarks != "" && item_returned1.L3_Remarks != null) ? item_returned1.L3_Remarks : "-";
                            mail.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Sent Back";

                            //mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your VKM SPOC is available in your Queue. Please kindly find the Item Details below." +

                            //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned1.ItemName)).Item_Name + "<br /> Requested Quantity: " + item_returned1.ReqQuantity + "<br /> Total Amount: $" + item_returned1.TotalPrice + "<br /> L3 Reviewer: " + item_returned1.SHNT + "<br /> Comments: " + sendback_comments + "<br />" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            //mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your VKM SPOC is available in your Queue. Please kindly find the Item Details below." +
                            //    "<br />" + "<br /><table border=1><tr><td><B>Item Name<B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>L3 Reviewer</B></td><td><B>Comments</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned1.ItemName)).Item_Name + "</td><td>" + item_returned1.ReqQuantity + "</td><td>" + item_returned1.TotalPrice + "</td><td>" + item_returned1.SHNT + "</td><td>" + sendback_comments + "</td></tr></table>" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned1.ItemName)).Item_Name + "<br /> Requested Quantity: " + item_returned1.ReqQuantity + "<br /> Total Amount: $" + item_returned1.TotalPrice + "<br /> VKM SPOC: " + item_returned1.SHNT + "<br /> Comments: " + sendback_comments + "<br />" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            mail.Body = " Dear Tech/Program Lead" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your VKM SPOC is available in your Queue. Please kindly find the Item Details below." +
                                "<br />" + "<br /><table border=1><tr><td><B>Item Name<B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>VKM SPOC</B></td><td><B>Comments</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned1.ItemName)).Item_Name + "</td><td>" + item_returned1.ReqQuantity + "</td><td>" + item_returned1.TotalPrice + "</td><td>" + item_returned1.SHNT + "</td><td>" + sendback_comments + "</td></tr></table>" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


                        }

                    }








                }


            }
            catch (Exception ex)
            {
                WriteLog("Error in Process - emailNotification_VKM - Planning - :  NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

            }

            if (emailnotify.ReviewLevel == "L1" && emailnotify.is_ApprovalorSendback && emailnotify.RequestID_foremail == 1999999999)
            {

            }
            else if (emailnotify.ReviewLevel == "L2" && emailnotify.is_ApprovalorSendback && emailnotify.RequestID_foremail == 1999999999)
            {

            }
            else
            {

                DirectorySearcher search = new DirectorySearcher(); //class to perform a search against LDAP connection

                // specify the search filter
                search.Filter = "(&(objectClass=user)(anr=" + toEmail + "))";
                search.PropertiesToLoad.Add("mail");        // smtp mail address

                // perform the search
                SearchResult result = search.FindOne();
                toEmail = result.Properties["mail"][0].ToString();


                search.Filter = "(&(objectClass=user)(anr=" + ccEmail + "))";
                search.PropertiesToLoad.Add("mail");        // smtp mail address

                // perform the search
                SearchResult result1 = search.FindOne();
                ccEmail = result1.Properties["mail"][0].ToString();


                mail.From = new MailAddress(fromEmail);
                mail.To.Add(new MailAddress(toEmail));



                mail.CC.Add(new MailAddress(ccEmail));
                // mail.Bcc.Add(bcarbonCopy);


                bool failedIndicator = false;
                try
                {
                    if (toEmail.Trim() != "sbr2kor")
                        client.Send(mail);
                }
                catch (Exception ex)
                {

                    WriteLog("Error - emailNotification_VKM - Planning :  NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                    if (ex.ToString().Contains("timed out"))
                        failedIndicator = true;
                }
                if (failedIndicator)
                {

                    WriteLog("Retrying email to : " + toEmail);
                    try
                    {
                        client.Send(mail);
                        WriteLog("Retry Success");
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Error - emailNotification_VKM - Planning : Retry error NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                    }
                }
            }


        }


        #region Ordering Mail
        public static void emailNotification_VKM(Emailnotify_OrderStage emailnotify)
        {

            string fromEmail = "Cob_RBEI_SmartLabDevTeam@bcn.bosch.com ";
            string toEmail = "";
            string ccEmail = "";
            string Mailid_from_ntid = "";
            bool production = false;
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;

            string EmailFlag = ConfigurationManager.AppSettings["TestEmailFlag"].ToString();
            if (EmailFlag == "0")
            {
                production = true;
            }
            else
            {
                production = false;
            }


            //add CC or BCC
            MailAddress bcarbonCopy = new MailAddress("Meena.MadhevanpillaiArunadevi@in.bosch.com");
            //MailAddress bcarbonCopy = new MailAddress("external.Bharani.G @in.bosch.com");
            mail.Bcc.Add(bcarbonCopy);
            // bcarbonCopy = new MailAddress("valson.joshuaninan@in.bosch.com");
            //mail.Bcc.Add(bcarbonCopy);

            SmtpClient client = new SmtpClient("rb-smtp-int.bosch.com", 25);
            client.Timeout = 200000;

            client.Credentials = CredentialCache.DefaultNetworkCredentials;

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;


                if (emailnotify.is_RequesttoOrder == 2)//true => LabTeam->Requestor
                {


                    RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == emailnotify.RequestID_orderemail).FirstOrDefault<RequestItems_Table>();
                    if (BudgetingController.lstItems != null && BudgetingController.lstItems.Count() > 0)
                    {
                        if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())) != null)
                        {
                            toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                        }
                        else
                            toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                        Mailid_from_ntid = toEmail;
                        if (production)
                        {
                            if (item.BU.Trim() == "5")
                                ccEmail = "PS.LabManagementBmh@bcn.bosch.com";
                            else
                                ccEmail = "NE2-VS.PURSupport@bcn.bosch.com";
                        }
                        else
                        {
                            ccEmail = "Meena.MadhevanpillaiArunadevi@in.bosch.com";
                            //ccEmail = "external.Bharani.G @in.bosch.com";
                        }

                        mail.Subject = "VKM" + /*DateTime.Now.AddYears(-1).Year */ item.SubmitDate.Value.Year + " Item Order";

                        //mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " The order status of your item " +

                        mail.Body = " Dear Tech/Program Lead" + ", <br /> " + "<br /> " + "<br />" + " The order status of your item " +

                             BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + " has been updated to " + BudgetingController.lstOrderStatus.Find(item1 => item1.ID.ToString().Equals(item.OrderStatus.Trim())).OrderStatus + " stage." + "<br /> " + "PO Remarks: " + item.PORemarks + "<br />" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com/BudgetingOrder'> here</a> to view the status." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                    }


                }
                else if (emailnotify.is_RequesttoOrder == 1)//false => Requestor->LabTeam
                {
                    if (production)
                    {
                        ccEmail = "NE2-VS.PURSupport@bcn.bosch.com";
                    }
                    else
                    {
                        toEmail = "Meena.MadhevanpillaiArunadevi@in.bosch.com";
                        //toEmail = "external.Bharani.G@in.bosch.com";
                    }

                    if (emailnotify.RequestID_orderemail == 1999999999)
                    {

                        List<RequestItems_Table> templist = new List<RequestItems_Table>();
                        templist = db.RequestItems_Table.ToList();
                        if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_ccEmail.ToLower().Trim())) != null)
                        {
                            ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_ccEmail.ToLower().Trim())).NTID.ToLower().Trim();

                        }
                        else
                            ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_ccEmail.ToLower().Trim())).NTID.ToLower().Trim();
                        Mailid_from_ntid = ccEmail;

                        mail.Subject = "VKM" + emailnotify.SubmitDate_ofRequest.Year/*DateTime.Now.AddYears(-1).Year*/ + " Item Order";
                        mail.Body = " Dear Lab Team" + ", <br /> " + " <br /> " + " <br /> " + presentUserName + " has opened the order Request for " + emailnotify.Count + " item(s) amounting to $" + emailnotify.TotalAmount + " " + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/Budgetingvkm'> here</a> to view the order request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                    }
                    else
                    {
                        RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == emailnotify.RequestID_orderemail).FirstOrDefault<RequestItems_Table>();

                        if (production)
                        {
                            toEmail = "NE2-VS.PURSupport@bcn.bosch.com";
                            if (item.BU.Trim() == "5")
                                ccEmail = "PS.LabManagementBmh@bcn.bosch.com";
                            else
                                ccEmail = "NE2-VS.PURSupport@bcn.bosch.com";
                        }
                        else
                        {
                            toEmail = "Meena.MadhevanpillaiArunadevi@in.bosch.com";
                            //toEmail = "external.Bharani.G@in.bosch.com";
                        }

                        if (BudgetingController.lstItems != null && BudgetingController.lstItems.Count() > 0)
                        {
                            if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())) != null)
                            {
                                ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                            }
                            else
                                ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();
                            Mailid_from_ntid = ccEmail;

                            mail.Subject = "VKM" + /*DateTime.Now.AddYears(-1).Year*/item.SubmitDate.Value.Year + " Item Order";

                            //mail.Body = " Dear Lab Team" + ", <br /> " + "<br /> " + "<br />" + item.RequestorNT + " has opened the order Request for the item details mentioned below. " +
                            //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br /> Reviewed Quantity: " + item.ApprQuantity + "<br /> Reviewed Price: $" + item.ApprCost + "<br /> Requestor: " + item.RequestorNT + "<br /> Required Date: " + item.RequiredDate.Value.ToString("dd-MM-yyyy") + "<br /> " + "PO Remarks: " + item.PORemarks + "<br /> Fund: " + BudgetingController.lstFund.Find(item1 => item1.ID.ToString().Equals(item.Fund)).Fund + "<br />" + "<br /> Click <a href = 'http://banen1093154.apac.bosch.com/Budgetingvkm'> here</a> to view the order request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


                            mail.Body = " Dear Lab Team" + ", <br /> " + "<br /> " + "<br />" + item.RequestorNT + " has opened the order Request for the item details mentioned below. " +
                                "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Reviewed Quantity</B></td><td><B>Reviewed Price</B></td><td><B>Requestor</B></td><td><B>Required Date</B></td><td><B>PO Remarks</B></td><td><B>Fund</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "</td><td>" + item.ApprQuantity + "</td><td>$" + item.ApprCost + "</td><td>" + item.RequestorNT + "</td><td>" + item.RequiredDate.Value.ToString("dd-MM-yyyy") + "</td><td>" + item.PORemarks + "</td><td>" + BudgetingController.lstFund.Find(item1 => item1.ID.ToString().Equals(item.Fund)).Fund + "</td></tr></table>" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com/Budgetingvkm'> here</a> to view the order request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                        }

                    }

                }
                else if (emailnotify.is_RequesttoOrder == 3)// UNPLANNED F02 - LabTeam->VKM SPOC
                {


                    //RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == emailnotify.RequestID_orderemail).FirstOrDefault<RequestItems_Table>();
                    //if (BudgetingController.lstItems != null && BudgetingController.lstItems.Count() > 0)
                    //{
                    //    if (production)
                    //    {
                    //        if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.SHNT.ToLower().Trim())) != null)
                    //        {
                    //            toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.SHNT.ToLower().Trim())).NTID.ToLower().Trim();


                    //        }
                    //        else
                    //            toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.SHNT.ToLower().Trim())).NTID.ToLower().Trim();

                    //        if (toEmail == "idm1cob")
                    //        {
                    //            toEmail = "din2cob";
                    //        }

                    //    }
                    //    else
                    //    {
                    //        toEmail = "mae9cob";
                    //    }

                    //    Mailid_from_ntid = toEmail;
                    //    if (production)
                    //    {
                    //        if (item.BU.Trim() == "5")
                    //            ccEmail = "PS.LabManagementBmh@bcn.bosch.com";
                    //        else
                    //            ccEmail = "NE2-VS.PURSupport@bcn.bosch.com";
                    //    }
                    //    else
                    //    {
                    //        ccEmail = "Meena.MadhevanpillaiArunadevi@in.bosch.com";
                    //    }

                    //    mail.Subject = "VKM " + item.SubmitDate.Value.Year + " Unplanned F02 Item Order";
                    //    mail.Body = " Dear VKM SPOC" + ", <br /> " + "<br /> " + "<br />" + " An unplanned F02 item has been added into the Item List. Please kindly find the Details below." +
                    //            "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br />" + " Quantity: " + item.ReqQuantity + "<br />" + " Total Amount: $" + item.TotalPrice + "<br /> " + "PO Remarks: " + item.PORemarks + "<br />" + "<br />" + "<br />" + " Click <a href = 'http://banen1093154.apac.bosch.com/Budgetingvkm'> here</a> to view the item." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                    //}


                }




            }

            DirectorySearcher search = new DirectorySearcher(); //class to perform a search against LDAP connection
            if (emailnotify.is_RequesttoOrder == 2 || emailnotify.is_RequesttoOrder == 3) // commented bcoz windows service ll run
            {
                // specify the search filter
                search.Filter = "(&(objectClass=user)(anr=" + Mailid_from_ntid + "))";
                search.PropertiesToLoad.Add("mail");        // smtp mail address

                // perform the search
                SearchResult result = search.FindOne();
                toEmail = result.Properties["mail"][0].ToString();


            }
            if (emailnotify.is_RequesttoOrder == 1)
            {
                // specify the search filter
                search.Filter = "(&(objectClass=user)(anr=" + Mailid_from_ntid + "))";
                search.PropertiesToLoad.Add("mail");        // smtp mail address

                // perform the search
                SearchResult result = search.FindOne();
                ccEmail = result.Properties["mail"][0].ToString();


            }
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(new MailAddress(toEmail));
            mail.CC.Add(new MailAddress(ccEmail));




            bool failedIndicator = false;
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {

                logs.AppendLine("Exception:" + ex.ToString());
                if (ex.ToString().Contains("timed out"))
                    failedIndicator = true;
            }
            if (failedIndicator)
            {

                logs.AppendLine("Retrying email to : " + toEmail);
                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    // Console.WriteLine("Exception: {0}",
                    //     ex.ToString());
                }
            }
            //}


        }
        #endregion Ordering Mail
        public static void WriteLog(string Message)
        {
            string execPath = AppDomain.CurrentDomain.BaseDirectory;
            execPath = execPath + "Budgeting_Log\\log" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
            StreamWriter file = new StreamWriter(execPath, append: true);
            file.WriteLine(Message + "\r\n");
            file.Close();
        }
    }
}
