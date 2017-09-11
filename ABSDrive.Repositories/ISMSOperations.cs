using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ABSDrive.Repositories
{
    public interface ISMSOperations<TD> where TD:class
    {
         bool sendMsg(SerialPort port, string PhoneNo, string Message);
         SerialPort OpenPort(string p_strPortName, int p_uBaudRate, int p_uDataBits, int p_uReadTimeout, int p_uWriteTimeout);
         void ClosePort(SerialPort port);
         TD ReadSMS(SerialPort port);
         int CountSMSmessages(SerialPort port);
         bool DeleteMsg(SerialPort port, string p_strCommand);


    }
}
