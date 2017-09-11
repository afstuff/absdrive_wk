using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using ABSDrive.SMSOps;
using System.Collections;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace ABSDrive.SMS
{
    public partial class frmSMSConsole : Form
    {
        public frmSMSConsole()
        {
            InitializeComponent();
        }
         #region Private Variables
        SerialPort port = new SerialPort();
        clsSMS objclsSMS = new clsSMS();
        virtualPorts portInfo; 
        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        #endregion


        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                this.gboPortSettings.Enabled = true;
                objclsSMS.ClosePort(this.port);

                //Remove tab pages
                this.tabSMSapplication.TabPages.Remove(tbSendSMS);
                this.tabSMSapplication.TabPages.Remove(tbReadSMS);
                this.tabSMSapplication.TabPages.Remove(tbDeleteSMS);

                this.lblConnectionStatus.Text = "Not Connected";
                this.btnDisconnect.Enabled = false;

            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {
            smsreadClass rd = new smsreadClass();
            rd.SaveToDB("221","221","221","221","221","221");
            ////.............................................. Send SMS ....................................................
            //try
            //{

            //    if (objclsSMS.sendMsg(this.port, this.txtSIM.Text, this.txtMessage.Text))
            //    {
            //        MessageBox.Show("Message has sent successfully");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Failed to send message");
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ErrorLog(ex.Message);
            //}
        }

        private void btnReadSMS_Click(object sender, EventArgs e)
        {
            try
            {
                //count SMS 
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                if (uCountSMS > 0)
                {
                    // If SMS exist then read SMS
                    #region Read SMS
                    //.............................................. Read all SMS ....................................................
                    objShortMessageCollection = objclsSMS.ReadSMS(this.port);
                    foreach (ShortMessage msg in objShortMessageCollection)
                    {

                        ListViewItem item = new ListViewItem(new string[] { msg.Index, msg.Sender, msg.Message });
                        item.Tag = msg;
                        lvwMessages.Items.Add(item);

                    }
                    #endregion
                }
                else
                {
                    lvwMessages.Clear();
                    MessageBox.Show("There is no message in SIM");


                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnCountSMS_Click(object sender, EventArgs e)
        {

           try
            {
                //Count SMS
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                this.txtCountSMS.Text = uCountSMS.ToString();
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        #region Error Log
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

                sw = new StreamWriter(sPathName + "SMSOperations_Log_" + sErrorTime + ".txt", true);

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
        #endregion 

        private void btnDeleteSMS_Click(object sender, EventArgs e)
        {
             try
            {
                //Count SMS 
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                if (uCountSMS > 0)
                {
                    DialogResult dr = MessageBox.Show("Are u sure u want to delete the SMS?", "Delete confirmation", MessageBoxButtons.YesNo);

                    if (dr.ToString() == "Yes")
                    {
                        #region Delete SMS

                        if (this.rbDeleteAllSMS.Checked)
                        {                           
                            //...............................................Delete all SMS ....................................................

                            #region Delete all SMS
                            string strCommand = "AT+CMGD=1,4";
                            if (objclsSMS.DeleteMsg(this.port, strCommand))
                            {
                                MessageBox.Show("Messages has deleted successfuly ");
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete messages ");
                            }
                            #endregion
                            
                        }
                        else if (this.rbDeleteReadSMS.Checked)
                        {                          
                            //...............................................Delete Read SMS ....................................................

                            #region Delete Read SMS
                            string strCommand = "AT+CMGD=1,3";
                            if (objclsSMS.DeleteMsg(this.port, strCommand))
                            {
                                MessageBox.Show("Messages has deleted successfuly ");
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete messages ");
                            }
                            #endregion

                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
         try
            {
                //Open communication port 
                this.port = objclsSMS.OpenPort(this.cboPortName.SelectedValue as String, Convert.ToInt32(this.cboBaudRate.Text), Convert.ToInt32(this.cboDataBits.Text), Convert.ToInt32(this.txtReadTimeOut.Text), Convert.ToInt32(this.txtWriteTimeOut.Text));

                if (this.port != null)
                {
                    //this.tabSMSapplication.TabPages.Remove(tbPortSettings);
                    this.gboPortSettings.Enabled = false;

                    MessageBox.Show("Modem is connected at PORT " + this.cboPortName.Text);

                    //Add tab pages
                    this.tabSMSapplication.TabPages.Add(tbSendSMS);
                    this.tabSMSapplication.TabPages.Add(tbReadSMS);
                    this.tabSMSapplication.TabPages.Add(tbDeleteSMS);

                    this.lblConnectionStatus.Text = "Connected at " + this.cboPortName.SelectedValue as String;
                    this.btnDisconnect.Enabled = true;
                }

                else
                {
                    MessageBox.Show("Invalid port settings");
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                #region Display all available COM Ports
                    portInfo = new virtualPorts();
                    IDictionary portNames = portInfo.GetPortNames();
                    cboPortName.DataSource = new BindingSource(portNames,null);
                    cboPortName.DisplayMember = "Value";
                    cboPortName.ValueMember = "Key";
                
                #endregion

                //Remove tab pages
                this.tabSMSapplication.TabPages.Remove(tbSendSMS);
                this.tabSMSapplication.TabPages.Remove(tbReadSMS);
                this.tabSMSapplication.TabPages.Remove(tbDeleteSMS);

                this.btnDisconnect.Enabled = false;
            }
            catch(Exception ex)
            {
                ErrorLog(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void butExit_Click(object sender, EventArgs e)
        {
            
        }

        //  //string mysqlConn = "Server=localhost; Database=ABSDrive; User ID=driveuser; Password=driveuser234";
        //string mysqlConn = "Server=localhost; Database=ABSDrive; User ID=absdrive; Password=absdrive234";
        //MySqlConnection conn;
        //MySqlCommand command;
        //int result;
        
        //public void SMSDBInbox(string v_source_phone, String v_long_code, string v_message, string v_received_date, string v_processed_date, string v_status)
        //{
        //    try
        //    {

        //    conn = new MySqlConnection(mysqlConn);
        //    command = new MySqlCommand("absfn_UpdateSMSInbox", conn);
        //    command.CommandType = CommandType.StoredProcedure;


        //    // Add your parameters here if you need them
        //    MySqlParameter inputparameter1 = command.CreateParameter();
        //    inputparameter1.ParameterName = "p_source_phone";
        //    inputparameter1.DbType = DbType.String;
        //    inputparameter1.Size = 20;
        //    inputparameter1.Direction = ParameterDirection.Input;
        //    inputparameter1.Value = v_source_phone;

        //    command.Parameters.Add(inputparameter1);


        //    MySqlParameter inputparameter2 = command.CreateParameter();
        //    inputparameter2.ParameterName = "p_long_code";
        //    inputparameter2.DbType = DbType.String;
        //    inputparameter2.Size = 20;
        //    inputparameter2.Direction = ParameterDirection.Input;
        //    inputparameter2.Value = v_long_code;

        //    command.Parameters.Add(inputparameter2);

        //    MySqlParameter inputparameter3 = command.CreateParameter();
        //    inputparameter3.ParameterName = "p_message";
        //    inputparameter3.DbType = DbType.String;
        //    inputparameter3.Size = 250;
        //    inputparameter3.Direction = ParameterDirection.Input;
        //    inputparameter3.Value = v_message;

        //    command.Parameters.Add(inputparameter3);

        //    MySqlParameter inputparameter4 = command.CreateParameter();
        //    inputparameter4.ParameterName = "p_received_date";
        //    inputparameter4.DbType = DbType.String;
        //    inputparameter4.Size = 25;
        //    inputparameter4.Direction = ParameterDirection.Input;
        //    inputparameter4.Value = v_received_date;

        //    command.Parameters.Add(inputparameter4);

        //    MySqlParameter inputparameter5 = command.CreateParameter();
        //    inputparameter5.ParameterName = "p_processed_date";
        //    inputparameter5.DbType = DbType.String;
        //    inputparameter5.Size = 25;
        //    inputparameter5.Direction = ParameterDirection.Input;
        //    inputparameter5.Value = v_processed_date;

        //    command.Parameters.Add(inputparameter5);

        //    MySqlParameter inputparameter6 = command.CreateParameter();
        //    inputparameter6.ParameterName = "p_status";
        //    inputparameter6.DbType = DbType.String;
        //    inputparameter6.Size = 10;
        //    inputparameter6.Direction = ParameterDirection.Input;
        //    inputparameter6.Value = v_status;

        //    command.Parameters.Add(inputparameter6);
        //    conn.Open();

        //    result = (int)command.ExecuteNonQuery();
        //    ErrorLog("------- DB Operations Successful: Return Value: " + result.ToString() + "------------");
        //    }
        //    catch (Exception v)
        //    {

        //        ErrorLog("------- DB Operations Error! " + v.Message + "------------");
        //    }

        //}
    }
}
