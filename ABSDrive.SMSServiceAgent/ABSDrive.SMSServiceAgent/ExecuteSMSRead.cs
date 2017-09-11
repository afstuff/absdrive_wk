using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   
using System.IO;
using System.IO.Ports;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using ABSDrive.SMSOps;
using Quartz;
using Common.Logging;


namespace ABSDrive.SMSServiceAgent
{
    public class ExecuteSMSRead : IJob
    {
        SerialPort port;
        ShortMessageCollection objShortMessageCollection;
        clsSMS sms;
        virtualPorts vport;


        // default port settings
        string portname = string.Empty;
        Int32 baudrate = 9600;
        Int32 databits = 8;
        Int32 readtimeout = 300;
        Int32 writetimeout = 300;

        public ExecuteSMSRead()
        {
        }
        public void Execute(IJobExecutionContext context)
        {


            //Device connection settings
            try
            {
                sms = new clsSMS();
                port = new SerialPort();
                objShortMessageCollection = new ShortMessageCollection();

                ILog log = LogManager.GetLogger(typeof(ExecuteSMSRead));

                //get the connected port values
                vport = new virtualPorts();
                IDictionary portNames = vport.GetPortNames();

                foreach (DictionaryEntry pn in portNames)
                {
                    if (pn.Value.ToString().Contains("UI Interface"))
                    {
                        //pick 
                        portname = pn.Key.ToString();
                        ErrorLog("Portname or Key: " + portname);
                    }
                    else
                    {
                        string portnamex = pn.Key.ToString();
                        // portNames.Remove(pn.Key);
                        ErrorLog("Portname or Key removed " + portnamex + " with value " + pn.Value.ToString());

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

                    try
                    {
                        ErrorLog("------- Begin Read SMS from Device--------------");

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
                    }
                    catch (Exception ex)
                    {
                        ErrorLog(ex.Message);
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
                ErrorLog(ex.Message);
            }
        }

        private void ReadAllSMS()
        {

            #region Read SMS
            //.............................................. Read all SMS ....................................................

            ErrorLog("------- Before the read loop of Read into DB------------");

            objShortMessageCollection = sms.ReadSMS(this.port);


            foreach (ShortMessage msg in objShortMessageCollection)
            {

                ErrorLog("------- SMS being Read " + msg.Index + ", " + msg.Sender + ", " + msg.Message + ", ------------");
                ErrorLog("------- SMS being Read into DB------------");
                MessageType(msg.Message);
                //SMSDBInbox TransferSMSToDB = new SMSDBInbox();
                //TransferSMSToDB.SaveToDB(msg.Sender, "LAGOS", msg.Message, removeDateSeperators(DateTime.Now), null, "Pending");
                //TransferSMSToDB = null;
                SaveToDB(msg.Sender, "LAGOS", msg.Message, removeDateSeperators(DateTime.Now), string.Empty, "Pending");
                ErrorLog("------- Read into DB successful------------");
            }
            #endregion

        }
        private void DeleteAllSMS()
        {
            String delCommand = "AT+CMGD=1,4";
            if (sms.DeleteMsg(this.port, delCommand))
            {
                ErrorLog("------- Delete Operation Successful------------");
            }
            else
            {
                ErrorLog("------- Failed to delete messages ------------");
            }

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

        public virtual void SaveToDB(string v_source_phone, string v_long_code, string v_message, string v_received_date, string v_processed_date, string v_status)
        {
             //ErrorLog("phone param " + v_source_phone + v_long_code+ v_message+ v_received_date+ v_processed_date+ v_status);

            string mysqlConn = "Server=localhost; Database=ABSDrive; User ID=absdrive; Password=absdrive234";
            MySqlConnection conn;
            MySqlCommand command;
            int result;

            //this.long_code = v_long_code;
            //this.message = v_message;
            //this.processed_date = v_processed_date;
            //this.received_date = v_received_date;
            //this.source_phone = v_source_phone;
            //this.status = v_status;

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

    }
}

