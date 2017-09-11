using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.Office.Interop.Word;
using ABSDrive.Repositories;
using Word = Microsoft.Office.Interop.Word;
using System.Threading;

namespace ABSDrive.SMSServiceAgent
{
    public class EmailNotification : INotifications
    {
        private string fromAddress = String.Empty;
        private string toAddress = String.Empty;
        private string displayName = String.Empty;
        private string adminPassword = String.Empty;
        private string emailSubject = String.Empty;
        string sPathName = @"C:\James Stuff\VS2013 VERSION\ABSDrive\PolicyDocs\";
        Object fileName;
        Attachment attachPolicyDoc;
        Attachment attachCertificate;
        IList<Attachment> colAttach;
        DataSet ds = new DataSet();
        //Attachment attachReceipt;

        //constructor
        //read _fromAddress, displayname, password from the settings table
        public EmailNotification(string _fromAddress, string _toAddress, string _displayName, string _password, string _subject)
        {
            this.displayName = _displayName;
            this.fromAddress = _fromAddress;
            this.toAddress = _toAddress.Replace("\0", "@").Replace("", "_");
            this.adminPassword = _password;
            this.emailSubject = _subject;

        }
        public bool Send()
        {
            try
            {
                var frmAddress = new MailAddress(fromAddress, displayName);
                //            var toAddress = new MailAddress("jcc_nnannah@yahoo.co.uk", "James Nnannah");ooolanipekun@gmail.com
                var toAddr = new MailAddress(toAddress); //("olusola@eonenergyresearch.com"); //SubscriberEmailAddresses);//("ooolanipekun@gmail.com");
                string fromPassword = adminPassword;
                string subject = emailSubject;
               string ThirdPartyPolicyDoc_Template = sPathName + "ThirdPartyPolicyDoc";
                string ThirdPartyCertificate_Template = sPathName + "ThirdPartyCertificate";

                colAttach = new List<Attachment>(); // = new AttachmentCollection();

                //get trans from DB
                GetTransactionInfo();
                MergeDocuments(ds, ThirdPartyPolicyDoc_Template, "Policy Doc");
                //Wait for 2 seconds for process to be completed (COM resources to be used and discarded or else file not available error will be shown )
                Thread.Sleep(2000);
                attachPolicyDoc = new Attachment(fileName.ToString());

                fileName = null;

                MergeDocuments(ds, ThirdPartyCertificate_Template, "Certificate");
                //Wait for 2 seconds for process to be completed (COM resources to be used and discarded or else file not available error will be shown )
                Thread.Sleep(2000);
                attachCertificate = new Attachment(fileName.ToString());

                colAttach.Add(attachPolicyDoc);
                colAttach.Add(attachCertificate);

                DateTime tdate = DateTime.Now;
                string trandate = String.Empty;
                trandate = HashHelper.removeDateSeperators(tdate);
                string body = "The purchase of insurance policy -- third party car insurance";

                var smtp = new SmtpClient
                {

                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress, adminPassword),
                    Timeout = 20000
                };
                using (var message = new System.Net.Mail.MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body


                })
                {
                    //add all the attachments to the mail message
                    foreach (var attaches in colAttach)
                    {
                        message.Attachments.Add(attaches);
                    }
                    smtp.Send(message);
                    ActivityLog.ErrorLog("Email(s) Successfully Sent to " + toAddress, "SMSapplication_ErrorLog_");
                    return true;
                }


            }

            catch (Exception ex)
            {
                ActivityLog.ErrorLog("Error: Failed to send mail to " + toAddress + " Details " + ex.Message, "SMSapplication_ErrorLog_");
                throw ex;
                //return false;
            }
        }
        public void GetTransactionInfo()
        {

            string mysqlConn = "Server=localhost; Database=ABSDrive; User ID=absdrive; Password=absdrive234";
            MySqlConnection conn;
            MySqlDataAdapter adp;

            try
            {

                conn = new MySqlConnection(mysqlConn);
                //adp = new MySqlDataAdapter("Select * from customer_transactions where email = " + toAddress + " and entrydate =" + DateTime.Now.Date.ToString(), conn);
                adp = new MySqlDataAdapter("Select * from customer_transactions a inner join customer_profile b on a.PhoneNo=b.PhoneNo where a.email = '" + toAddress + "'", conn);
                conn.Open();
                adp.Fill(ds);
                //colAttach.Add(attachReceipt);

            }
            catch (Exception v)
            {

                ActivityLog.ErrorLog("------- Data retrieval by emai address, document merge and attachment process Error! " + v.Message + "------------", "SMSapplication_ErrorLog_");
            }

        }
        private void MergeDocuments(DataSet ds, String TemplateFileName, String DocType)
        {
            Word.Application wordApp = new Application();
            Word.Document wordDoc = new Word.Document();
            //add the Third party insurance template document to the Word application
            wordApp.Visible = false;

            wordDoc = wordApp.Documents.Add(TemplateFileName);

            //select all the names from database and loop through each row
            for (int rowno = 0; rowno < ds.Tables[0].Rows.Count; rowno++)
            {
                //Incase there are more than one merge field
                foreach (Word.MailMergeField myField in wordDoc.MailMerge.Fields)
                {
                    Range rngFieldCode = myField.Code;
                    String fieldText = rngFieldCode.Text;

                    if (fieldText.StartsWith(" MERGEFIELD"))
                    {

                        Int32 endMerge = fieldText.IndexOf("\\");
                        Int32 fieldNameLength = fieldText.Length - endMerge;
                        String fieldName = fieldText.Substring(11, endMerge - 11);

                        // GIVES THE FIELDNAMES AS THE USER HAD ENTERED IN .dot FILE
                        fieldName = fieldName.Trim();

                        //select merge field
                        myField.Select();
                        switch (fieldName)
                        {
                            case "Policy_No":
                                wordApp.Selection.TypeText("Policy Number from database");
                                break;
                            case "Start_Date":
                                wordApp.Selection.TypeText("Start Date from database");
                                break;
                            case "End_Date":
                                wordApp.Selection.TypeText("End Date from database");
                                break;
                            case "Full_Name":
                                wordApp.Selection.TypeText(ds.Tables[0].Rows[rowno]["LastName"].ToString() + " " + ds.Tables[0].Rows[rowno]["OtherName"].ToString());
                                break;
                            case "RegNo":
                                wordApp.Selection.TypeText("Reg No from Database");
                                break;
                            case "Chassis_No":
                                wordApp.Selection.TypeText("Chasis number from the database");
                                break;
                            case "Make":
                                wordApp.Selection.TypeText("Car Make from the database");
                                break;
                            case "Type":
                                wordApp.Selection.TypeText("Car type from the database");
                                break;


                            default:
                                break;
                        }
                    }
                }
                fileName = sPathName + DocType + "_" + ds.Tables[0].Rows[rowno]["PhoneNo"].ToString() + "_" + HashHelper.removeDateandTimeSeperators(DateTime.Now) + ".docx";

                //wordDoc.SaveAs(ref ds.Tables[0].Rows[rowno]["Name"].ToString()+ ".doc",,,,,,,,,,,,,,,);
                wordDoc.SaveAs(ref fileName);


            }
            wordApp.Quit(Word.WdSaveOptions.wdSaveChanges);
            wordDoc = null;
            wordApp = null;
        }

        private void Dispose()
        {


        }
    }
}
