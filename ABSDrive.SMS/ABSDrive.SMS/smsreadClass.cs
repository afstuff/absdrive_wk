using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;

namespace ABSDrive.SMS
{
   public class smsreadClass
    {
       //string mysqlConn = "Server=localhost; Database=ABSDrive; User ID=driveuser; Password=driveuser234";
        string source_phone;
        String long_code;
        string message;
        string received_date;
        string processed_date;
        string status;

        //constructor
        public smsreadClass()
        { 
        
        }
        

        public void SaveToDB(string v_source_phone, String v_long_code, string v_message, string v_received_date, string v_processed_date, string v_status)
        {

            string mysqlConn = "Server=localhost; Database=ABSDrive; User ID=absdrive; Password=absdrive234";
            MySqlConnection conn;
            MySqlCommand command;
            int result;

            this.long_code = v_long_code;
            this.message = v_message;
            this.processed_date = v_processed_date;
            this.received_date = v_received_date;
            this.source_phone = v_source_phone;
            this.status = v_status;

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
                inputparameter1.Value = source_phone;

                command.Parameters.Add(inputparameter1);


                MySqlParameter inputparameter2 = command.CreateParameter();
                inputparameter2.ParameterName = "p_long_code";
                inputparameter2.DbType = DbType.String;
                inputparameter2.Size = 20;
                inputparameter2.Direction = ParameterDirection.Input;
                inputparameter2.Value = long_code;

                command.Parameters.Add(inputparameter2);

                MySqlParameter inputparameter3 = command.CreateParameter();
                inputparameter3.ParameterName = "p_message";
                inputparameter3.DbType = DbType.String;
                inputparameter3.Size = 250;
                inputparameter3.Direction = ParameterDirection.Input;
                inputparameter3.Value = message;

                command.Parameters.Add(inputparameter3);

                MySqlParameter inputparameter4 = command.CreateParameter();
                inputparameter4.ParameterName = "p_received_date";
                inputparameter4.DbType = DbType.String;
                inputparameter4.Size = 25;
                inputparameter4.Direction = ParameterDirection.Input;
                inputparameter4.Value = received_date;

                command.Parameters.Add(inputparameter4);

                MySqlParameter inputparameter5 = command.CreateParameter();
                inputparameter5.ParameterName = "p_processed_date";
                inputparameter5.DbType = DbType.String;
                inputparameter5.Size = 25;
                inputparameter5.Direction = ParameterDirection.Input;
                inputparameter5.Value = processed_date;

                command.Parameters.Add(inputparameter5);

                MySqlParameter inputparameter6 = command.CreateParameter();
                inputparameter6.ParameterName = "p_status";
                inputparameter6.DbType = DbType.String;
                inputparameter6.Size = 10;
                inputparameter6.Direction = ParameterDirection.Input;
                inputparameter6.Value = status;

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

                sw = new StreamWriter(sPathName + "ABSDrive_ErrorLog_" + sErrorTime + ".txt", true);

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


    }
}
