using System;
using System.Collections;
using System.IO;
using System.IO.Ports;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using ABSDrive.SMSOps;
using Quartz;
using Common.Logging;
using System.Text;
using ABSDrive.Repositories;

namespace ABSDrive.SMSServiceAgent
{
    public class ReadSMSJob:IJob
    {
        SerialPort port;
        ShortMessageCollection objShortMessageCollection;
        ISMSOperations<ShortMessageCollection> sms;
        //clsSMS sms;
        virtualPorts vport;


        // default port settings
        string portname = string.Empty;
        Int32 baudrate = 9600;
        Int32 databits = 8;
        Int32 readtimeout = 300;
        Int32 writetimeout = 300;

        private static readonly ILog Log = LogManager.GetLogger(typeof(ReadSMSJob));

        public ReadSMSJob() { 
        
        }


        public void Execute(IJobExecutionContext context)
        {
            try
            {
                sms = new  clsSMS();
                port = new SerialPort();
                objShortMessageCollection = new ShortMessageCollection();

                ILog log = LogManager.GetLogger(typeof(ReadSMSJob));

                //get the connected port values
                vport = new virtualPorts();
                IDictionary portNames = vport.GetPortNames();

                foreach (DictionaryEntry pn in portNames)
                {
                    if (pn.Value.ToString().Contains("UI Interface"))
                    {
                        //pick up the port name
                        portname = pn.Key.ToString();
                        ErrorLog("Portname or Key: " + portname);
                    }
                }
                if (this.port.IsOpen == false)
                {
                    //Open communication port 
                    this.port = sms.OpenPort(this.portname, this.baudrate, this.databits, this.readtimeout, this.writetimeout);
                    ErrorLog("Port Opened " + portname);

                }
                if (this.port != null)
                {
                    ErrorLog("------- Device is connected at PORT " + this.portname + "------------");


                        //count SMS 
                        int uCountSMS = sms.CountSMSmessages(this.port);
                        if (uCountSMS > 0)
                        {
                            ErrorLog("------- Reading " + uCountSMS + " SMS from Device--------------");

                            // If SMS exist then read all SMSes
                            ReadAllSMS();
                            ErrorLog("------- Begining the Delete Operation------------");
                            //DeleteAllSMS();
                        }
                        else
                        {
                            ErrorLog("------- No SMS message exists in Device--------------");
                        }

                    //close port
                    this.port.Close();
                    ErrorLog("------- Port Closedx------------");
                }
                else
                {
                    ErrorLog("------- Invalid Port Settings ------------");
                }

            }
            catch (Exception ex)
            {
                ErrorLog("Erroring away............... " + ex.Message);
            }
        }

        private void ReadAllSMS(){

            #region Read SMS
            //.............................................. Read all SMS ....................................................

            ErrorLog("------- Before the read loop of Read into DB------------");

            objShortMessageCollection = sms.ReadSMS(this.port);
            try
            {
                foreach (var msg in objShortMessageCollection)
                {
                    //byte[] bytes = System.Text.Encoding.Default.GetBytes(msg.Message.ToString());
                    //msg.Message = System.Text.Encoding.UTF8.GetString(bytes);

                    ErrorLog("------- SMS being Read " + msg.Index + ", " + msg.Sender + ", " + msg.Message + ", ------------");
                    ErrorLog("------- SMS being Read into DB------------");
                    MessageType(msg.Message);

                    if (msg.Message != "Status") { //online status check, do not persist in DB
                    
                        if (validSMSLength(msg.Sender.ToString(), msg.Message.ToString())) 
                        {
                            SaveToDB(msg.Sender.ToString(), "LAGOS", UnicodeToUTF8(removeCommas(msg.Message.ToString())), removeDateSeperators(DateTime.Now), string.Empty, "Pending");
                            ErrorLog("------- Read into DB successful------------");
                        }
                        else
                        { 
                            ErrorLog("-------ERROR: SMS NOT Read " + msg.Index + ", " + msg.Sender + ", " + msg.Message + ",  Sent Back---------");
                        }
                    }
                    else
                    {
                        OnlineStatus(msg.Sender.ToString(), msg.Message.ToString());

                    }
                }
            }
            catch (Exception v)
            {
                ErrorLog("database insertion error " + v.Message);
                throw;
            }

            
            #endregion

        }

        private Boolean OnlineStatus(string phoneno, string msg)
        {
            Boolean sendSuccess = false;

            if (msg == "Status"){
                sendSuccess = sms.sendMsg(this.port, phoneno, "ABSDrive IS Online! *1*=3rdParty *2*=TravelIns Format=*1*Email*Surnme*FirstNme*ContactAddr*CarReg*Make eg TOYOTA*Type eg SALOON*ScratchCads#");
                if (sendSuccess)
                {
                    ErrorLog("-------Sent online status message to the user " + phoneno + ", ---------");
                    return true;
                }

            }
            return false;


        }
        private Boolean validSMSLength(string phoneno,string msg)
        {
            Boolean sendSuccess = false;
            String[] str =  msg.Split('*');

            if (str.Length != 12) //direct business and product code =1 - third party then
            {
               sendSuccess = sms.sendMsg(this.port, phoneno, "ABSDrive:format c d '*'--> *101*Email*Surname*FirstName*ContactAddress*Car Regno*EngNo*Make eg TOYOTA*Type eg SALOON*YRofMake*coverdate eg yyyymmdd*Scratch Cards#");
               if (!sendSuccess)
                ErrorLog("-------ERROR: Unable to send Error message to the user " + phoneno + ", " + msg + ", ---------");

                return false;

            }
            else
                return true;


        }
        public virtual void SaveToDB(string v_source_phone, string v_long_code, string v_message, string v_received_date, string v_processed_date, string v_status)
        {

            string mysqlConn = "Server=localhost; Database=ABSDrive; User ID=absdrive; Password=absdrive234";
            MySqlConnection conn;
            MySqlCommand command;
            int result;

            try
            {

                conn = new MySqlConnection(mysqlConn);
                command = new MySqlCommand("absfn_UpdateSMSInbox", conn);
                command.CommandType = CommandType.StoredProcedure;


                // Add your parameters here if you need them
                MySqlParameter inputparameter1 = command.CreateParameter();
                inputparameter1.ParameterName = "p_source_phone";
                inputparameter1.DbType = DbType.String;
                inputparameter1.Size = 20;
                inputparameter1.Direction = ParameterDirection.Input;
                inputparameter1.Value = v_source_phone;

                command.Parameters.Add(inputparameter1);


                MySqlParameter inputparameter2 = command.CreateParameter();
                inputparameter2.ParameterName = "p_long_code";
                inputparameter2.DbType = DbType.String;
                inputparameter2.Size = 20;
                inputparameter2.Direction = ParameterDirection.Input;
                inputparameter2.Value = v_long_code;

                command.Parameters.Add(inputparameter2);

                MySqlParameter inputparameter3 = command.CreateParameter();
                inputparameter3.ParameterName = "p_message";
                inputparameter3.DbType = DbType.String;
                inputparameter3.Size = 250;
                inputparameter3.Direction = ParameterDirection.Input;
                inputparameter3.Value = v_message;

                command.Parameters.Add(inputparameter3);

                MySqlParameter inputparameter4 = command.CreateParameter();
                inputparameter4.ParameterName = "p_received_date";
                inputparameter4.DbType = DbType.String;
                inputparameter4.Size = 25;
                inputparameter4.Direction = ParameterDirection.Input;
                inputparameter4.Value = v_received_date;

                command.Parameters.Add(inputparameter4);

                MySqlParameter inputparameter5 = command.CreateParameter();
                inputparameter5.ParameterName = "p_processed_date";
                inputparameter5.DbType = DbType.String;
                inputparameter5.Size = 25;
                inputparameter5.Direction = ParameterDirection.Input;
                inputparameter5.Value = v_processed_date;

                command.Parameters.Add(inputparameter5);

                MySqlParameter inputparameter6 = command.CreateParameter();
                inputparameter6.ParameterName = "p_status";
                inputparameter6.DbType = DbType.String;
                inputparameter6.Size = 10;
                inputparameter6.Direction = ParameterDirection.Input;
                inputparameter6.Value = v_status;

                command.Parameters.Add(inputparameter6);
                conn.Open();

                result = (int)command.ExecuteNonQuery();
                ErrorLog("------- DB Operations Successful: Return Value: " + result.ToString() + "------------");
            }
            catch (Exception v)
            {

                ErrorLog("------- DB Operations Error! " + v.Message + "------------");
            }

        }
        public void ErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                string sPathName = @"C:\James Stuff\VS2013 VERSION\ABSDrive\ABSDrive.SMS\";

                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();

                string sErrorTime = sDay + "-" + sMonth + "-" + sYear;

                sw = new StreamWriter(sPathName + "SMSapplication_ErrorLog_" + sErrorTime + ".txt", true);

                sw.WriteLine(sLogFormat + Message);
                sw.Flush();

            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                    sw.Close();
                }
            }


        }
        public static string removeDateSeperators(DateTime dte)
        {
            string dy = dte.Day.ToString();
            string myday = dte.Day.ToString();
            string mt = dte.Month.ToString();
            string mth = dte.Month.ToString();
            if (mt.Length == 1)
                mth = "0" + mt;
            if (dy.Length == 1)
                myday = "0" + dy;

            string ky = dte.Year.ToString() + mth + myday;
            return ky;
        }

        //get message type
        /// <summary>
        /// mtype
        /// 1 -- Motor Vehicle (Third Party)
        /// 2 -- Travel Insurance
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private void MessageType(String msg)
        {

            String mtype = msg.Substring(0);
            if (mtype == "1")
            {
                if (ValidateRequestUser(msg))
                    ExecuteRequest();
            }

        }
        //validate request user
        private bool ValidateRequestUser(String msg)
        {
            string username = string.Empty;
            string password = string.Empty;


            return true;

        }
        private string ExecuteRequest()
        {
            //read from outbox, construct an SMS, Send to User who made request
            return string.Empty;

        }
        /// <summary>
        /// removes commas from the body of the text
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string removeCommas(String p){
            String  s = p.Replace(',',' ');
            return s;
        }

        private string UnicodeToUTF8(string strFrom)
        {
            byte[] bytSrc;
            byte[] bytDestination;
            string strTo = String.Empty;

            bytSrc = Encoding.Unicode.GetBytes(strFrom);
            bytDestination = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytSrc);
            strTo = Encoding.UTF8.GetString(bytDestination);

            return strTo;
        }
    }
}
