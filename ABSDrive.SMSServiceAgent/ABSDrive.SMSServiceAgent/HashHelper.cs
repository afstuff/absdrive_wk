using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.Security;
using System.Threading;


namespace ABSDrive.SMSServiceAgent
{
    public class HashHelper
    {
        public static string CreateSalt(int size)
        {
            // Generate a cryptographic random number using the cryptographic
            // service provider
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        public static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            //string hashedPwd =
            //      FormsAuthentication.HashPasswordForStoringInConfigFile(
            //                                           saltAndPwd, "SHA1");
            //return hashedPwd;
            return String.Empty;
        }


        public static bool VerifyPassword(object _user, string suppliedPassword)
        {
            bool passwordMatch = false;
            // Get the salt and pwd from the database based on the user name.
            try
            {
               //string dbPasswordHash = _user.Password;
               // string salt = _user.Salt;
                // Now take the salt and the password entered by the user
                // and concatenate them together.
                //string passwordAndSalt = String.Concat(suppliedPassword, salt);
                // Now hash them
                //string hashedPasswordAndSalt =
                //           FormsAuthentication.HashPasswordForStoringInConfigFile(
                //                                           passwordAndSalt, "SHA1");
                //// Now verify them.
                //passwordMatch = hashedPasswordAndSalt.Equals(dbPasswordHash);
            }
            catch (Exception ex)
            {
                throw new Exception("Execption verifying password. " + ex.Message);
            }
            finally
            {
            }
            return passwordMatch;
        }

        // Generate random numbers from the specified Random object.
        static Int64 RunIntRandoms(Random randObj)
        {
            return randObj.Next();
        }

        // Create a random object with a timer-generated seed.
        public static string AutoSeedRandom()
        {
            // Wait to allow the timer to advance.
            Thread.Sleep(1);

            Random autoRand = new Random();

            return RunIntRandoms(autoRand).ToString();
        }

        public static DateTime GoodDate(string yr, string mth, string dy)
        {
            string strDte = yr + "/" + mth + "/" + dy;
            return checkDate(strDte);
        }

        private static DateTime checkDate(string dte)
        {
            DateTime myDate = DateTime.Today;
            try
            {
                myDate = Convert.ToDateTime(dte);
            }
            catch (Exception c)
            {
                throw new Exception("Invalid Date!");
            }
            return myDate;

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
        /// <summary>
        /// Removes date and time seperators
        /// </summary>
        /// <param name="dte">Valid datetime</param>
        /// <returns>Returns date and time as string with format "yyyymmddhhmmss" </returns>
        public static string removeDateandTimeSeperators(DateTime dte)
        {
            String hh = dte.Hour.ToString();
            String mm = dte.Minute.ToString();
            String ss = dte.Second.ToString();

            string dy = dte.Day.ToString();
            string myday = dte.Day.ToString();
            string mt = dte.Month.ToString();
            string mth = dte.Month.ToString();
            if (mt.Length == 1)
                mth = "0" + mt;
            if (dy.Length == 1)
                myday = "0" + dy;

            if (hh.Length == 1)
                hh = "0" + hh;

            if (mm.Length == 1)
                mm = "0" + mm;

            if (ss.Length == 1)
                ss = "0" + ss;


            string ky = dte.Year.ToString() + mth + myday + hh + mm + ss;
            return ky;
        }
        public static Double RealNumberNoSpaces(string str)
        {
            if (str == "")
                return 0;
            return Math.Round(Convert.ToDouble(str), 2);

        }
        public static DateTime RealDateNoSpaces(string str)
        {
            if (str == "")
                return DateTime.Now;
            return Convert.ToDateTime(str);

        }
    }
}
