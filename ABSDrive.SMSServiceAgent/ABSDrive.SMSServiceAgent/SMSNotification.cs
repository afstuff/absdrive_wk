using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using ABSDrive.Repositories;
using ABSDrive.SMSOps;


namespace ABSDrive.SMSServiceAgent
{
    public class SMSNotification:INotifications
    {
        private SerialPort srport = null;
        private ISMSOperations<ShortMessageCollection> objSMS = null;
        private string portname = String.Empty;
        private Int32 baudrate = 0;
        private Int32 databits = 0;
        private Int32 readtimeout = 0;
        private Int32 writetimeout = 0;
        private string PhoneNo = String.Empty;
        private string Message = String.Empty;

        public SMSNotification(SerialPort _srport,
                                    ISMSOperations<ShortMessageCollection> _objSMSoperation,
                                        string _portname,
                                        Int32 _baudrate,
                                        Int32 _databits,
                                        Int32 _readtimeout,
                                        Int32 _writetimeout,
                                        string _PhoneNo,
                                        string _Message
                                    )
        {
            this.baudrate = _baudrate;
            this.databits = _databits;
            this.Message = _Message;
            this.objSMS = _objSMSoperation;
            this.PhoneNo = _PhoneNo;
            this.portname = _portname;
            this.srport = _srport;
            this.writetimeout = _writetimeout;
            this.readtimeout = _readtimeout;


        }
        public bool Send() 
        { 
            //connect to Device
            var sms = objSMS;
            var port = srport;
            
            
            try
            {
                
                if (port.IsOpen == false)
                {
                //Open communication port 

                port = sms.OpenPort(portname, baudrate, databits, readtimeout, writetimeout);
                }

                if (port != null) 
                {
                    ActivityLog.ErrorLog("------- Device is connected at PORT " + portname + " (send out sms for multiple clients)------------","SMSapplication_ErrorLog_");

                    try
                    {
                        if (sms.sendMsg(port, PhoneNo, Message))
                        {
                          // success ok;
                            return true;
                        }
                        else //if not

                        {
                            ActivityLog.ErrorLog("------- Failed to send message for Phone No : " + PhoneNo + " --------------","SMSapplication_ErrorLog_");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ActivityLog.ErrorLog(ex.Message, "SMSapplication_ErrorLog_");
                    }

                    //close port
                    port.Close();
                    ActivityLog.ErrorLog("------- Port Closed (send out sms for multiple clients)------------","SMSapplication_ErrorLog_");
                }
                else
                {
                    ActivityLog.ErrorLog("------- Invalid Port Settings (send out sms for multiple clients)------------","SMSapplication_ErrorLog_");
                    
                }
            }
            catch (Exception ex)
            {
                ActivityLog.ErrorLog(ex.Message, "SMSapplication_ErrorLog_");
                
            }

            return false;
        }

    }
}
