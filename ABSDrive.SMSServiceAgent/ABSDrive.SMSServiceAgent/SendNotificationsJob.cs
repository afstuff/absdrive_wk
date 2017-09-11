using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Common.Logging;
using ABSDrive.SMSOps;
using System.IO.Ports;
using MySql.Data.MySqlClient;
using System.Data;
using ABSDrive.Repositories;

namespace ABSDrive.SMSServiceAgent
{
     public class SendNotificationsJob:IJob
    {
        SerialPort port = new SerialPort();
        ISMSOperations<ShortMessageCollection> sms;
        virtualPorts vport;


        // default port settings
        string portname = string.Empty;
        Int32 baudrate = 9600;
        Int32 databits = 8;
        Int32 readtimeout = 300;
        Int32 writetimeout = 300;

        private static readonly ILog Log = LogManager.GetLogger(typeof(SendNotificationsJob));
        private INotifications smsNotify;
        private INotifications emailNotify;
         public SendNotificationsJob()
         {

         }

         public void Execute(IJobExecutionContext context)
         {
             try
             {
                 //ILog Log = LogManager.GetLogger(typeof(SendNotificationsJob));   

                 //get the details of the connected port 
                 vport = new virtualPorts();
                 IDictionary portNames = vport.GetPortNames();
                 ActivityLog.ErrorLog("Sending SMS Notifications Initializing... ", "SMSapplication_ErrorLog_");

                 foreach (DictionaryEntry pn in portNames)
                 {
                     if (pn.Value.ToString().Contains("UI Interface"))
                     {
                         //pick 
                         portname = pn.Key.ToString();
                         ActivityLog.ErrorLog("Connected to Port: " + portname, "SMSapplication_ErrorLog_");
                     }
                 }
                 port = new SerialPort();
                 sms = new clsSMS();
                 


                 DataTable tab = GetTodaysTransactionInfo();
                 string smsMessage = string.Empty;
                 foreach(DataRow row in tab.Rows)
                 {
                     smsNotify = new SMSNotification(port
                                                     , sms
                                                     , portname
                                                     , baudrate
                                                     , databits
                                                     , readtimeout
                                                     , writetimeout
                                                     , row["PhoneNo"].ToString(), "messages to be sent!");
                     smsNotify.Send();

                     //read _fromAddress, displayname, password from the settings table

                     emailNotify = new EmailNotification("jamesnnannah@gmail.com"
                                                         , row["Email"].ToString()
                                                         ,"ABSDrive Email Service"
                                                         , "heavenred",
                                                         "Policy Purchase Successful. Policy Number ");
                     emailNotify.Send(); 
                 }   
             }
             catch (Exception x)
             {

                 ActivityLog.ErrorLog("Error " + x.Message, "SMSapplication_ErrorLog_");
             }

         }
         /// <summary>
         /// Get today's transactions not yet posted (i.e. client had not been SMSed and emailed)
         /// </summary>
         /// <returns>dataset table object containing unposted transactions</returns>
         public DataTable GetTodaysTransactionInfo()
         {

             string mysqlConn = "Server=localhost; Database=ABSDrive; User ID=absdrive; Password=absdrive234";
             MySqlConnection conn;
             MySqlDataAdapter adp;

             DataSet ds = new DataSet();
             

             try
             {

                 conn = new MySqlConnection(mysqlConn);
//                 adp = new MySqlDataAdapter("Select * from customer_transactions where entrydate =" + DateTime.Now.Date.ToString() + " and PostStatus = 'Unposted'", conn);
                 adp = new MySqlDataAdapter("Select * from customer_transactions", conn);
                 conn.Open();
                 adp.Fill(ds);



             }
             catch (Exception v)
             {

                 ActivityLog.ErrorLog("------- Data retrieval by emai address Error! " + v.Message + "------------", "SMSapplication_ErrorLog_");
             }

             return ds.Tables[0];

         }

    }
}
