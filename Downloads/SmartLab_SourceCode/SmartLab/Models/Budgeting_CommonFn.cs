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
            string toEmail1 = "";
            string toEmail2 = "";
            string ccEmail = "";
            string ccEmail1 = "";
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;


            //add CC or BCC
            MailAddress bcarbonCopy = new MailAddress("Meena.MadhevanpillaiArunadevi@in.bosch.com");
            //MailAddress bcarbonCopy = new MailAddress("external.Bharani.G @in.bosch.com");
            mail.Bcc.Add(bcarbonCopy);
            // bcarbonCopy = new MailAddress("valson.joshuaninan@in.bosch.com");
            // mail.Bcc.Add(bcarbonCopy); //there is an option of mail.Bcc also 



            SmtpClient client = new SmtpClient("rb-smtp-bosch2bosch.rbesz01.com", 25);
            client.Timeout = 200000;
            // Credentials are necessary if the server requires the client
            // to authenticate before it will send email on the client's behalf.
            client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.EnableSsl = true;

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
                                //else
                                //    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(dhgroup.Key.ToLower().Trim())).NTID.ToLower().Trim();

                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())) != null)
                                {
                                    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                //else
                                //    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

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

                                //mail1.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + count + " item(s) amounting to $" + totalAmount + " submitted by " + presentUserName + " are available in your Queue for L2 approval." + "<br />" + "<br />" + "Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingApprovals'> here</a> to review the item request.  <br />" + "<br />" + "Thank you. " + "<br />" + "<br />Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";

                                mail1.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + count + " item(s) amounting to $" + totalAmount + " submitted by " + presentUserName + " are available in your Queue for Tech/Program Director Review." + "<br />" + "<br />" + "Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingApprovals'> here</a> to review the item request.  <br />" + "<br />" + "Thank you. " + "<br />" + "<br />Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


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

                            //mail.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + emailnotify.Count + " item(s) amounting to $" + emailnotify.TotalAmount + " submitted by " + presentUserName + " are available in your Queue for L2 approval." + "<br />" + "<br />" + "Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingApprovals'> here</a> to review the item request.  <br />" + "<br />" + "Thank you. " + "<br />" + "<br />Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";

                            //mail.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + emailnotify.Count + " item(s) amounting to $" + emailnotify.TotalAmount + " submitted by " + presentUserName + " are available in your Queue for Tech/Program Director Review." + "<br />" + "<br />" + "Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingApprovals'> here</a> to review the item request.  <br />" + "<br />" + "Thank you. " + "<br />" + "<br />Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


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
                                    //else if (BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim()) && user.Department.ToUpper().Trim().Equals(dept.ToUpper().Trim())) != null)
                                    //{
                                    //    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim()) && user.Department.ToUpper().Trim().Equals(dept.ToUpper().Trim())).NTID.ToLower().Trim();

                                    //}
                                    else
                                        toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim())).NTID.ToLower().Trim();
                                }
                                //else
                                //{
                                //    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim()) && user.Department.ToUpper().Trim().Equals(dept.ToUpper().Trim())).NTID.ToLower().Trim();
                                //}
                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())) != null)
                                {
                                    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();
                                }
                                //else
                                //{
                                //    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                //}
                                if (toEmail == "idm1cob")
                                {
                                    toEmail = "din2cob";
                                }


                                mail.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Request";


                                //mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for L2 approval. Please kindly find the Request Details below." +
                                //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br />" + " Requested Quantity: " + item.ReqQuantity + "<br />" + " Total Amount: $" + item.TotalPrice + "<br />" + " Requestor Name: " + item.RequestorNT + "<br />" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingApprovals'> here</a> to review the item request." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                                //mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for Tech/Program Director review. Please kindly find the Request Details below." +
                                //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br />" + " Requested Quantity: " + item.ReqQuantity + "<br />" + " Total Amount: $" + item.TotalPrice + "<br />" + " Requestor Name: " + item.RequestorNT + "<br />" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingApprovals'> here</a> to review the item request." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                                //mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for L2 approval. Please kindly find the Request Details below." +
                                //    "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>Requestor Name</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "</td><td>" + item.ReqQuantity + "</td><td>$" + item.TotalPrice + "</td><td>" + item.RequestorNT + "</td></tr></table>" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingApprovals'> here</a> to review the item request." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                                mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for Tech/Program Director review. Please kindly find the Request Details below." +
                                    "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>Tech/Program Lead</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "</td><td>" + item.ReqQuantity + "</td><td>$" + item.TotalPrice + "</td><td>" + item.RequestorNT + "</td></tr></table>" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingApprovals'> here</a> to review the item request." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


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

                            //        //mail.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + emailnotify.Count + " item(s) amounting to $" + emailnotify.TotalAmount + " submitted by " + presentUserName + " are available in your Queue for L3 approval." + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            //        //mail.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + emailnotify.Count + " item(s) amounting to $" + emailnotify.TotalAmount + " submitted by " + presentUserName + " are available in your Queue for VKM SPOC review." + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";



                            var templist1 = emailnotify.Requests_foremail.GroupBy(x => x.SHNT);

                            foreach (var dhgroup in templist1)
                            {
                                MailMessage mail1 = new MailMessage();
                                mail1.IsBodyHtml = true;
                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(dhgroup.Key.ToLower().Trim())) != null)
                                {
                                    toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(dhgroup.Key.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                //else
                                //    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(dhgroup.Key.ToLower().Trim())).NTID.ToLower().Trim();

                                if (toEmail == "idm1cob")
                                {
                                    toEmail = "din2cob";
                                }

                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())) != null)
                                {
                                    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                //else
                                //    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                int count = 0;
                                decimal totalAmount = 0;
                                foreach (var item in dhgroup)
                                {
                                    totalAmount += (decimal)item.ApprCost;
                                    count++;
                                }
                                mail1.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Request";

                                //mail1.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + count + " item(s) amounting to $" + totalAmount + " submitted by " + presentUserName + " are available in your Queue for L3 approval." + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                                mail1.Body = " Dear Reviewer" + ", <br /> " + " <br /> " + " <br /> " + count + " item(s) amounting to $" + totalAmount + " submitted by " + presentUserName + " are available in your Queue for VKM SPOC review." + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


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
                                //else
                                //    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.SHNT.ToLower().Trim())).NTID.ToLower().Trim();

                                if (toEmail == "idm1cob")
                                {
                                    toEmail = "din2cob";
                                }

                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())) != null)
                                {
                                    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(presentUserName.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                //else
                                //    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.DHNT.ToLower().Trim())).NTID.ToLower().Trim();


                                mail.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Request";

                                //mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for L3 approval. Please kindly find the Request Details below." +
                                //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br /> Requested Quantity: " + item.ApprQuantity + "<br /> Total Amount: $" + item.ApprCost + "<br /> L2 Reviewer: " + item.DHNT + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br />Thank you. " + "<br />" + "<br />" + " Regards," + " ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                                mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Request is available in your Queue for VKM SPOC review. Please kindly find the Request Details below." +
                                    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br /> Requested Quantity: " + item.ApprQuantity + "<br /> Total Amount: $" + item.ApprCost + "<br /> Tech/Program Director: " + item.DHNT + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br />Thank you. " + "<br />" + "<br />" + " Regards," + " ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


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
                            //else
                            //    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                            if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned.DHNT.ToLower().Trim())) != null)
                            {
                                ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned.DHNT.ToLower().Trim())).NTID.ToLower().Trim();

                            }
                            //else
                            //    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned.DHNT.ToLower().Trim())).NTID.ToLower().Trim();
                            var sendback_comments = (item_returned.L2_Remarks != "" && item_returned.L2_Remarks != null) ? item_returned.L2_Remarks : "-";
                            mail.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Sent Back";

                            //mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your HoE is available in your Queue. Please kindly find the Item Details below." +

                            //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned.ItemName)).Item_Name + "<br /> Requested Quantity: " + item_returned.ReqQuantity + "<br /> Total Amount: $" + item_returned.TotalPrice + "<br /> L2 Reviewer: " + item_returned.DHNT + "<br /> Comments: " + sendback_comments + "<br />" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned.ItemName)).Item_Name + "<br /> Requested Quantity: " + item_returned.ReqQuantity + "<br /> Total Amount: $" + item_returned.TotalPrice + "<br /> Tech/Program Director: " + item_returned.DHNT + "<br /> Comments: " + sendback_comments + "<br />" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


                            //mail.Body = " Dear Tech/Program Lead" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your HoE is available in your Queue. Please kindly find the Item Details below." +
                            //    "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>Tech/Program Director</B></td><td><B>Comments</B></td></tr><tr><td> " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned.ItemName)).Item_Name + "</td><td>" + item_returned.ReqQuantity + "</td><td> $" + item_returned.TotalPrice + "</td><td>" + item_returned.DHNT + "</td><td> " + sendback_comments + "</td></tr></table>" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


                            mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your HoE is available in your Queue. Please kindly find the Item Details below." +
                                "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>L2 Reviewer</B></td><td><B>Comments</B></td></tr><tr><td> " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned.ItemName)).Item_Name + "</td><td>" + item_returned.ReqQuantity + "</td><td> $" + item_returned.TotalPrice + "</td><td>" + item_returned.DHNT + "</td><td> " + sendback_comments + "</td></tr></table>" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";



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
                            else if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.RFOReqNTID.ToLower().Trim())) != null)
                            {
                                toEmail = item_returned1.RFOReqNTID.Trim();

                            }
                            //else
                            //    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                            if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.SHNT.ToLower().Trim())) != null)
                            {
                                ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.SHNT.ToLower().Trim())).NTID.ToLower().Trim();

                            }
                            //else
                            //    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item_returned1.SHNT.ToLower().Trim())).NTID.ToLower().Trim();

                            if (toEmail == "idm1cob")
                            {
                                toEmail = "din2cob";
                            }
                            var sendback_comments = (item_returned1.L3_Remarks != "" && item_returned1.L3_Remarks != null) ? item_returned1.L3_Remarks : "-";
                            mail.Subject = "VKM" + DateTime.Now.AddYears(1).Year + " Item Sent Back";

                            //mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your VKM SPOC is available in your Queue. Please kindly find the Item Details below." +

                            //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned1.ItemName)).Item_Name + "<br /> Requested Quantity: " + item_returned1.ReqQuantity + "<br /> Total Amount: $" + item_returned1.TotalPrice + "<br /> L3 Reviewer: " + item_returned1.SHNT + "<br /> Comments: " + sendback_comments + "<br />" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            //mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your VKM SPOC is available in your Queue. Please kindly find the Item Details below." +
                            //    "<br />" + "<br /><table border=1><tr><td><B>Item Name<B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>L3 Reviewer</B></td><td><B>Comments</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned1.ItemName)).Item_Name + "</td><td>" + item_returned1.ReqQuantity + "</td><td>" + item_returned1.TotalPrice + "</td><td>" + item_returned1.SHNT + "</td><td>" + sendback_comments + "</td></tr></table>" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned1.ItemName)).Item_Name + "<br /> Requested Quantity: " + item_returned1.ReqQuantity + "<br /> Total Amount: $" + item_returned1.TotalPrice + "<br /> VKM SPOC: " + item_returned1.SHNT + "<br /> Comments: " + sendback_comments + "<br />" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            mail.Body = " Dear Tech/Program Lead" + ", <br /> " + "<br /> " + "<br />" + " Item Sent Back by your VKM SPOC is available in your Queue. Please kindly find the Item Details below." +
                                "<br />" + "<br /><table border=1><tr><td><B>Item Name<B></td><td><B>Requested Quantity</B></td><td><B>Total Amount</B></td><td><B>VKM SPOC</B></td><td><B>Comments</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item_returned1.ItemName)).Item_Name + "</td><td>" + item_returned1.ReqQuantity + "</td><td>" + item_returned1.TotalPrice + "</td><td>" + item_returned1.SHNT + "</td><td>" + sendback_comments + "</td></tr></table>" + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingRequest'> here</a> to view the sent back item.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


                        }

                    }
                    else if (emailnotify.ReviewLevel == "L3" && emailnotify.is_ApprovalorSendback)
                    {


                        //L3->ELO 
                        BudgetingOrderController rfo = new BudgetingOrderController();
                        var templist = emailnotify.Requests_foremail.GroupBy(x => x.RFOReqNTID);

                        foreach (var rfoReqGroup in templist)
                        {
                            string Content = "";
                            string Subject_RequestIDs = "";
                            MailMessage mail1 = new MailMessage();
                            mail1.IsBodyHtml = true;
                            mail1.Bcc.Add(bcarbonCopy);

                            string POSPOC_NTIDs = string.Empty;
                          
                            Content += " Dear Lab Team" + ", <br /> " + "<br />" + " Order Request has been approved by VKM SPOC. Kindly find the item details mentioned below. " +
                              "<br />" + "<br /><table border=1><tr><td><B>Request ID</B></td><td><B>Item Name</B></td><td><B>Reviewed Quantity</B></td><td><B>Reviewed Price</B></td><td><B>Requestor</B></td><td><B>Required Date</B></td><td><B>Item Justification</B></td><td><B>Fund</B></td></tr>";

                            var RequestorName = (rfoReqGroup.Key != null ? BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.ToLower().Trim().Equals(rfoReqGroup.Key.ToLower().Trim())).EmployeeName : "");

                            foreach (var item in rfoReqGroup)
                            {
                                Subject_RequestIDs = Subject_RequestIDs.Trim() != "" ? Subject_RequestIDs + ", " + item.RequestID : item.RequestID.ToString(); 


                                if (emailnotify.getTOemail == null || emailnotify.getTOemail.Trim() == "")
                                    emailnotify.getTOemail = rfo.GetCommonMail(item.RequestID);
                                var POSPOCs = rfo.GetSectionCoordinatorsNTID(item.DEPT).Trim();
                                POSPOC_NTIDs = POSPOC_NTIDs.Trim() != "" ? (POSPOC_NTIDs.Contains(POSPOCs) ? POSPOC_NTIDs : POSPOC_NTIDs + "," + POSPOCs) : POSPOCs; //TO

                                Content += ("<tr><td>" + item.RequestID + "</td><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "</td><td>" + item.ApprQuantity + "</td><td>$" + item.ApprCost + "</td><td>" + RequestorName + "</td><td>" + item.RequiredDate.Value.ToString("dd-MM-yyyy") + "</td><td>" + item.PORemarks + "</td><td>" + BudgetingController.lstFund.Find(item1 => item1.ID.ToString().Equals(item.Fund)).Fund + "</td></tr>");



                            }
                            Content += "</table>" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to view the order request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";
                            mail1.Body = Content;
                            mail1.Subject = "VKM" + DateTime.Now.Year + " Item Order - Request ID: " + Subject_RequestIDs;
                            mail1.From = new MailAddress(fromEmail);
                            if (emailnotify.getTOemail != null && emailnotify.getTOemail.Trim() != "") // has value for VKM SPOC -> ELO Team
                            {
                                toEmail = emailnotify.getTOemail;
                                mail1.To.Add(new MailAddress(toEmail));
                            }
                            if (POSPOC_NTIDs != null && POSPOC_NTIDs.Trim() != "") // has value for VKM SPOC -> ELO Team
                            {
                                toEmail2 = GetEmailFromNTID(POSPOC_NTIDs);
                                string[] toMailAddresses = toEmail2.Split(';');
                                for (int i = 0; i < toMailAddresses.Length; i++)
                                {
                                    mail1.To.Add(new MailAddress(toMailAddresses[i]));
                                }
                            }
                            if (rfoReqGroup.Key.Trim() != null && rfoReqGroup.Key.Trim() != "") // has value for VKM SPOC -> ELO Team
                            {
                                ccEmail1 = GetEmailFromNTID(rfoReqGroup.Key.Trim());
                                mail1.CC.Add(new MailAddress(ccEmail1));
                            }
                            if (emailnotify.VKMSPOC_NTID != null && emailnotify.VKMSPOC_NTID.Trim() != "") // has value for VKM SPOC -> ELO Team
                            {
                                ccEmail = GetEmailFromNTID(emailnotify.VKMSPOC_NTID);
                                mail1.CC.Add(new MailAddress(ccEmail));
                            }
                            bool failedIndicator1 = false;
                            try
                            {
                                client.Send(mail1);
                            }
                            catch (Exception ex)
                            {

                                WriteLog("Error - emailNotification_VKM - Planning - L3->ELO Submit : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

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
                                    WriteLog("Error - emailNotification_VKM - Planning - Retry failed L3->ELO Submit: NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                                }
                            }

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
            else if(emailnotify.ReviewLevel == "L3" && emailnotify.is_ApprovalorSendback) //since already mail logic is handled and mail is sent
            {

            }
            else
            {

                //DirectorySearcher search = new DirectorySearcher(); //class to perform a search against LDAP connection

                //// specify the search filter
                //search.Filter = "(&(objectClass=user)(anr=" + toEmail + "))";
                //search.PropertiesToLoad.Add("mail");        // smtp mail address

                //// perform the search
                //SearchResult result = search.FindOne();
                //toEmail = result.Properties["mail"][0].ToString();


                //search.Filter = "(&(objectClass=user)(anr=" + ccEmail + "))";
                //search.PropertiesToLoad.Add("mail");        // smtp mail address

                //// perform the search
                //SearchResult result1 = search.FindOne();
                //ccEmail = result1.Properties["mail"][0].ToString();

                
                //mail.From = new MailAddress(fromEmail);
                //mail.To.Add(new MailAddress(toEmail));
                //mail.CC.Add(new MailAddress(ccEmail));



                //////////////////////////////////////////
                mail.From = new MailAddress(fromEmail);
                if (toEmail != null && toEmail.Trim() != "") // has value for all cases except VKM SPOC -> ELO Team
                {
                    toEmail = GetEmailFromNTID(toEmail);
                    mail.To.Add(new MailAddress(toEmail));
                }
                if (ccEmail != null && ccEmail.Trim() != "") // has value for all cases except VKM SPOC -> ELO Team
                {
                    ccEmail = GetEmailFromNTID(ccEmail);
                    mail.CC.Add(new MailAddress(ccEmail));
                }

                if (emailnotify.getTOemail != null && emailnotify.getTOemail.Trim() != "") // has value for VKM SPOC -> ELO Team
                {
                    toEmail = emailnotify.getTOemail;
                    mail.To.Add(new MailAddress(toEmail));
                }
                if (emailnotify.POSPOC_NTID != null && emailnotify.POSPOC_NTID.Trim() != "") // has value for VKM SPOC -> ELO Team
                {
                    toEmail2 = GetEmailFromNTID(emailnotify.POSPOC_NTID);
                    string[] toMailAddresses = toEmail2.Split(';');
                    for (int i = 0; i < toMailAddresses.Length; i++)
                    {
                        mail.To.Add(new MailAddress(toMailAddresses[i]));
                    }
                }
                if (emailnotify.RFOReqNTID != null && emailnotify.RFOReqNTID.Trim() != "") // has value for VKM SPOC -> ELO Team
                {
                    ccEmail1 = GetEmailFromNTID(emailnotify.RFOReqNTID);
                    mail.CC.Add(new MailAddress(ccEmail1));
                }
                if (emailnotify.VKMSPOC_NTID != null && emailnotify.VKMSPOC_NTID.Trim() != "") // has value for VKM SPOC -> ELO Team
                {
                    ccEmail = GetEmailFromNTID(emailnotify.VKMSPOC_NTID);
                    mail.CC.Add(new MailAddress(ccEmail));
                }

                /////////////////////////////////////////


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
            string toEmail1 = "";
            string toEmail2 = "";
            string ccEmail = "";
            string ccEmail1 = "";
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

            SmtpClient client = new SmtpClient("rb-smtp-bosch2bosch.rbesz01.com", 25);
            client.Timeout = 200000;

            client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.EnableSsl = true;
            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;


                if (emailnotify.is_RequesttoOrder == 2)//true => LabTeam->Requestor
                {


                    RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == emailnotify.RequestID_orderemail).FirstOrDefault<RequestItems_Table>();
                    if (BudgetingController.lstItems != null && BudgetingController.lstItems.Count() > 0)
                    {
                        if (item.RequestorNT != null && item.RequestorNT.Trim() != "")
                        {
                            if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())) != null)
                            {
                                toEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                            }
                            //else
                            //    toEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                            Mailid_from_ntid = toEmail;
                        }
                        if (emailnotify.RFOReqNTID != null)
                        {
                            toEmail1 = emailnotify.RFOReqNTID;
                        }

                        if (emailnotify.GoodsRecipientID != null)
                        {
                            toEmail2 = emailnotify.GoodsRecipientID;
                        }

                        if (production)
                        {
                            ccEmail = emailnotify.getCCemail;
                            ccEmail1 = emailnotify.POSPOC_NTID;
                            //if (item.BU.Trim() == "5")
                            //    ccEmail = "PS.LabManagementBmh@bcn.bosch.com";
                            //else
                            //    ccEmail = "NE2-VS.PURSupport@bcn.bosch.com";
                        }
                        else
                        {
                            ccEmail = "Meena.MadhevanpillaiArunadevi@in.bosch.com";
                            //ccEmail = "external.Bharani.G @in.bosch.com";
                        }

                        mail.Subject = "VKM" + /*DateTime.Now.AddYears(-1).Year */ item.VKM_Year + " Item Order";

                        //mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " The order status of your item " +

                        mail.Body = " Dear Tech/Program Lead" + ", <br /> " + "<br /> " + "<br />" + " The order status of your item " +

                             BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + " has been updated to " + BudgetingController.lstOrderStatus.Find(item1 => item1.ID.ToString().Equals(item.OrderStatus.Trim())).OrderStatus  + ((item.OrderDescriptionID != null && item.OrderDescriptionID != 0)? (" - " + BudgetingController.lstOrderDescription.Find(item1 => item1.ID.Equals(item.OrderDescriptionID)).Description) : "") + " stage." + "<br /> " + "Item Justification: " + item.PORemarks + "<br />" + "<br />" + " Click <a href = 'http://smartlab.apac.bosch.com:8052/BudgetingOrder'> here</a> to view the status." + "<br />" + "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


                    }


                }
                else if (emailnotify.is_RequesttoOrder == 1)//false => Requestor->LabTeam / VKM SPOC
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
                        //else
                        //    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(emailnotify.NTID_ccEmail.ToLower().Trim())).NTID.ToLower().Trim();
                        Mailid_from_ntid = ccEmail;

                        mail.Subject = "VKM" + emailnotify.SubmitDate_ofRequest.Year/*DateTime.Now.AddYears(-1).Year*/ + " Item Order";
                        mail.Body = " Dear Lab Team" + ", <br /> " + " <br /> " + " <br /> " + presentUserName + " has opened the order Request for " + emailnotify.Count + " item(s) amounting to $" + emailnotify.TotalAmount + " " + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to view the order request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards," + "<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                    }
                    else
                    {
                        RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == emailnotify.RequestID_orderemail).FirstOrDefault<RequestItems_Table>();

                        if (production)
                        {
                            toEmail = "NE2-VS.PURSupport@bcn.bosch.com";
                        }
                        else
                        {
                            toEmail = "Meena.MadhevanpillaiArunadevi@in.bosch.com";
                            //toEmail = "external.Bharani.G@in.bosch.com";
                        }

                        if (BudgetingController.lstItems != null && BudgetingController.lstItems.Count() > 0)
                        {
                            if (item.RequestorNT != null && item.RequestorNT.Trim() != "")
                            {
                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())) != null)
                                {
                                    ccEmail = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                                //else
                                //    ccEmail = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.RequestorNT.ToLower().Trim())).NTID.ToLower().Trim();
                                Mailid_from_ntid = ccEmail;
                            }
                            mail.Subject = "VKM" + /*DateTime.Now.AddYears(-1).Year*/item.VKM_Year + " Item Order - Request ID: " + item.RequestID + " for Item: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name;

                            var RequestorName = (item.RFOReqNTID != null ? BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.ToLower().Trim().Equals(item.RFOReqNTID.ToLower().Trim())).EmployeeName : "");
                            if (item.RequestSource != null && item.RequestSource.Trim() == "RFO" && item.BudgetCode.Trim().StartsWith("2")) //new RFO item - sent to VKM SPOC
                            {

                                //mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " Item Order Request is available in your Queue for VKM SPOC review. Please kindly find the Request Details below to move it to ELO Queue." +
                                //   "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br /> Requested Quantity: " + item.ApprQuantity + "<br /> Total Amount: $" + item.ApprCost + "<br /> Requestor: " + RequestorName + "<br />" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to review the item request.  <br />" + "<br />Thank you. " + "<br />" + "<br />" + " Regards," + " ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                                mail.Body = " Dear Reviewer" + ", <br /> " + "<br /> " + "<br />" + " VKM "+ item.VKM_Year +" Item Order Request is available in your Queue for VKM SPOC review. Kindly find the Request Details, inorder to move it to ELO Queue." +
                                "<br />" + "<br /><table border=1><tr><td><B>Request ID</B></td><td><B>Item Name</B></td><td><B>Requested Quantity</B></td><td><B>Total Price</B></td><td><B>Requestor</B></td><td><B>Required Date</B></td><td><B>Item Justification</B></td><td><B>Fund</B></td></tr><tr><td>" + item.RequestID + "</td><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "</td><td>" + item.ApprQuantity + "</td><td>$" + item.ApprCost + "</td><td>" + RequestorName + "</td><td>" + item.RequiredDate.Value.ToString("dd-MM-yyyy") + "</td><td>" + item.PORemarks + "</td><td>" + BudgetingController.lstFund.Find(item1 => item1.ID.ToString().Equals(item.Fund)).Fund + "</td></tr></table>" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to review the order request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            }
                            else //VKM Approved item - sent to labteam
                            {
                                mail.Body = " Dear Lab Team" + ", <br /> " + "<br />" + RequestorName + " has opened the order request for the item details mentioned below. " +
                            "<br />" + "<br /><table border=1><tr><td><B>Request ID</B></td><td><B>Item Name</B></td><td><B>Reviewed Quantity</B></td><td><B>Reviewed Price</B></td><td><B>Requestor</B></td><td><B>Required Date</B></td><td><B>Item Justification</B></td><td><B>Fund</B></td><td><B>Budget Code</B></td><td><B>Budget Center</B></td></tr><tr><td>" + item.RequestID + "</td><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "</td><td>" + item.ApprQuantity + "</td><td>$" + item.ApprCost + "</td><td>" + RequestorName + "</td><td>" + item.RequiredDate.Value.ToString("dd-MM-yyyy") + "</td><td>" + item.PORemarks + "</td><td>" + BudgetingController.lstFund.Find(item1 => item1.ID.ToString().Equals(item.Fund)).Fund  +"</td><td>" + item.BudgetCode + "</td><td>"+ ((item.BudgetCenterID != null && item.BudgetCenterID.Trim() != "")? BudgetingController.BudgetCenterList.Find(item1 => item1.ID.ToString().Equals(item.BudgetCenterID)).BudgetCenter : "-") +  "</td></tr></table>" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to view the order request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

                            }

                            //mail.Body = " Dear Lab Team" + ", <br /> " + "<br /> " + "<br />" + item.RequestorNT + " has opened the order Request for the item details mentioned below. " +
                            //    "<br />" + "<br /> Item Name: " + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "<br /> Reviewed Quantity: " + item.ApprQuantity + "<br /> Reviewed Price: $" + item.ApprCost + "<br /> Requestor: " + item.RequestorNT + "<br /> Required Date: " + item.RequiredDate.Value.ToString("dd-MM-yyyy") + "<br /> " + "PO Remarks: " + item.PORemarks + "<br /> Fund: " + BudgetingController.lstFund.Find(item1 => item1.ID.ToString().Equals(item.Fund)).Fund + "<br />" + "<br /> Click <a href = 'http://banen1093154.apac.bosch.com/Budgetingvkm'> here</a> to view the order request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";


                            //mail.Body = " Dear Lab Team" + ", <br /> " + "<br /> " + "<br />" + item.RequestorNT + " has opened the order Request for the item details mentioned below. " +
                            //    "<br />" + "<br /><table border=1><tr><td><B>Item Name</B></td><td><B>Reviewed Quantity</B></td><td><B>Reviewed Price</B></td><td><B>Requestor</B></td><td><B>Required Date</B></td><td><B>PO Remarks</B></td><td><B>Fund</B></td></tr><tr><td>" + BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item.ItemName)).Item_Name + "</td><td>" + item.ApprQuantity + "</td><td>$" + item.ApprCost + "</td><td>" + item.RequestorNT + "</td><td>" + item.RequiredDate.Value.ToString("dd-MM-yyyy") + "</td><td>" + item.PORemarks + "</td><td>" + BudgetingController.lstFund.Find(item1 => item1.ID.ToString().Equals(item.Fund)).Fund + "</td></tr></table>" + "<br /> Click <a href = 'http://smartlab.apac.bosch.com:8052/Budgetingvkm'> here</a> to view the order request.  <br />" + "<br /> Thank you. " + "<br />" + "<br />Regards,<br /> ESP-IS5 SmartLab Team<br />" + "<br />--- Automatically triggered email. Please Do Not Reply ---";

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
                if (Mailid_from_ntid != null && Mailid_from_ntid.Trim() != "")
                {
                    toEmail = GetEmailFromNTID(Mailid_from_ntid);
                }
                if (toEmail != null && toEmail.Trim() != "")
                {
                    mail.To.Add(new MailAddress(toEmail));
                }
                if (emailnotify.RFOReqNTID != null)
                {
                    mail.To.Add(new MailAddress(GetEmailFromNTID(toEmail1)));
                }
                if (emailnotify.GoodsRecipientID != null)
                {
                    mail.To.Add(new MailAddress(GetEmailFromNTID(toEmail2)));
                }
                if (emailnotify.getCCemail != null)
                {
                    mail.To.Add(new MailAddress(emailnotify.getCCemail));
                }
                if (production == true)
                {
                    mail.CC.Add(new MailAddress(GetEmailFromNTID(ccEmail1)));
                }

            }
            if (emailnotify.is_RequesttoOrder == 1)
            {

                if (Mailid_from_ntid != null && Mailid_from_ntid.Trim() != "")
                {
                    ccEmail = GetEmailFromNTID(Mailid_from_ntid);
                    mail.CC.Add(new MailAddress(ccEmail));
                }
                if (emailnotify.getTOemail != null)
                {
                    toEmail = emailnotify.getTOemail;
                    mail.To.Add(new MailAddress(toEmail));
                }
                if (emailnotify.POSPOC_NTID != null)
                {
                    toEmail2 = GetEmailFromNTID(emailnotify.POSPOC_NTID);
                    string[] toMailAddresses = toEmail2.Split(';');
                    for (int i = 0; i < toMailAddresses.Length; i++)
                    {
                        mail.To.Add(new MailAddress(toMailAddresses[i]));
                    }
                }
                if (emailnotify.RFOReqNTID != null)
                {
                    ccEmail1 = GetEmailFromNTID(emailnotify.RFOReqNTID);
                    mail.CC.Add(new MailAddress(ccEmail1));
                }
                if (emailnotify.VKMSPOC_NTID != null)
                {
                    toEmail = GetEmailFromNTID(emailnotify.VKMSPOC_NTID);
                    mail.To.Add(new MailAddress(toEmail));
                }
            }

            //if (emailnotify.RFOApprover != null && emailnotify.RFOApprover.ToString() !="")
            //{
            //    mail.To.Add(new MailAddress(GetEmailFromNTID(emailnotify.RFOApprover.ToString())));
            //}

            mail.From = new MailAddress(fromEmail);






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

        public static void emailNotification_RFOApprover(Emailnotify_RFOApprover emailnotifyRFOApprover)
        {
            if (emailnotifyRFOApprover.RFOApprover != null && emailnotifyRFOApprover.RFOApprover != "")
            {
                string fromEmail = "Cob_RBEI_SmartLabDevTeam@bcn.bosch.com ";
                string toEmail = "";
                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                string project = (emailnotifyRFOApprover.Project != null) ? emailnotifyRFOApprover.Project.ToString() : "";
                string remarks = (emailnotifyRFOApprover.Remarks != null) ? emailnotifyRFOApprover.Remarks.ToString() : "";
                //add CC or BCC
                //MailAddress bcarbonCopy = new MailAddress("Meena.MadhevanpillaiArunadevi@in.bosch.com");
                //MailAddress bcarbonCopy = new MailAddress("external.Bharani.G @in.bosch.com");
                MailAddress bcarbonCopy = new MailAddress("SmartLab.Mailbox@in.bosch.com");
                //SmtpClient client = new SmtpClient("rb-smtp-int.bosch.com", 25);
                SmtpClient client = new SmtpClient("rb-smtp-bosch2bosch.rbesz01.com", 25);
                
                //SmtpClient client = new SmtpClient("rb-smtp-int.bosch.com", 587);
                client.Timeout = 200000;
                client.Credentials = CredentialCache.DefaultNetworkCredentials;
                client.EnableSsl = true;
                DirectorySearcher search = new DirectorySearcher();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(new MailAddress(GetEmailFromNTID(emailnotifyRFOApprover.RFOApprover)));
                //mail.Bcc.Add(bcarbonCopy);

                mail.Subject = "New purchase request added - " + emailnotifyRFOApprover.RequestID.ToString();


                //mail.Body = " Dear Requestor" + ", <br /> " + "<br /> " + "<br />" + " The order status of your item " +


                mail.Body = " Dear " + emailnotifyRFOApprover.RFOApproverName + ", <br /> " + "<br /> " + "There is a new purchase request raised. Kindly approve the same. " + "<br />" + "Details are given below." + "<br />" + "<br />" +



                     "Project/PIF: " + project.ToString() + "<br /> " + "Description: " + emailnotifyRFOApprover.ItemDescription.ToString() + "<br /> " + "Quantity: " + emailnotifyRFOApprover.Quantity.ToString() + "<br /> " +
                     "Total Cost: INR " + emailnotifyRFOApprover.TotalPrice.ToString() + "<br /> " + "Justification: " + remarks.ToString() + "<br /> " +
                     "<br />" + " Thank you. " + "<br />" + "<br />" + "Regards," + "<br />" + " ESP-IS5 SmartLab Team" + "<br />" + "--- Automatically triggered email. Please Do Not Reply ---";


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
            }
        }

        private static string GetEmailFromNTID(string NTIDs)
        {
            string emailIDs = "";
            string[] values = NTIDs.Split(',');
            DirectorySearcher search = new DirectorySearcher(); //class to perform a search against LDAP connection
            // specify the search filter
            if (values.Length > 0)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if(values[i].Trim() != "")
                    {
                        search.Filter = "(&(objectClass=user)(anr=" + values[i].Trim() + "))";
                        search.PropertiesToLoad.Add("mail");        // smtp mail address



                        // perform the search
                        SearchResult result = search.FindOne();
                        if (result != null)
                        {
                            if (i == 0)
                            {
                                emailIDs = result.Properties["mail"][0].ToString();
                            }
                            else
                            {
                                emailIDs = emailIDs + ';' + result.Properties["mail"][0].ToString();
                            }
                        }
                    }

                }
            }



            return emailIDs;
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
