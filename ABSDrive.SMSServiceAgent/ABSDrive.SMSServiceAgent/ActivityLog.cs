using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace ABSDrive.SMSServiceAgent
{
     public class ActivityLog
    {
         public ActivityLog()
         {

         }

         public static void ErrorLog(string Message, string logfileName)
         {
             StreamWriter sw = null;
             try
             {
                 string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                 //string sPathName = @"C:\James Stuff\VS2013 VERSION\ABSDrive\ABSDrive.SMS\";
                 string sPathName = ConfigurationManager.AppSettings["MessageLogLocation"];

                 string sYear = DateTime.Now.Year.ToString();
                 string sMonth = DateTime.Now.Month.ToString();
                 string sDay = DateTime.Now.Day.ToString();

                 string sErrorTime = sDay + "-" + sMonth + "-" + sYear;

                 //sw = new StreamWriter(sPathName + "SMSapplication_ErrorLog_" + sErrorTime + ".txt", true);
                 sw = new StreamWriter(sPathName + logfileName + sErrorTime + ".txt", true);

                 sw.WriteLine(sLogFormat + Message);
                 sw.Flush();

             }
             catch (Exception ex)
             {
                 throw ex;
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
