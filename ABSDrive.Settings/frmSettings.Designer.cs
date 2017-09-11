namespace ABSDriveSettings
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.chkPolicyDocs = new System.Windows.Forms.CheckBox();
            this.chkNIID = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAdminEmail = new System.Windows.Forms.Label();
            this.txtAdminEmail = new System.Windows.Forms.TextBox();
            this.txtEmailPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEmailServer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.butSave = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Email Policy Docs";
            // 
            // chkPolicyDocs
            // 
            this.chkPolicyDocs.AutoSize = true;
            this.chkPolicyDocs.Location = new System.Drawing.Point(109, 35);
            this.chkPolicyDocs.Name = "chkPolicyDocs";
            this.chkPolicyDocs.Size = new System.Drawing.Size(15, 14);
            this.chkPolicyDocs.TabIndex = 1;
            this.chkPolicyDocs.UseVisualStyleBackColor = true;
            // 
            // chkNIID
            // 
            this.chkNIID.AutoSize = true;
            this.chkNIID.Location = new System.Drawing.Point(109, 90);
            this.chkNIID.Name = "chkNIID";
            this.chkNIID.Size = new System.Drawing.Size(15, 14);
            this.chkNIID.TabIndex = 3;
            this.chkNIID.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Update NIID";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(109, 62);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Send SMS ";
            // 
            // lblAdminEmail
            // 
            this.lblAdminEmail.AutoSize = true;
            this.lblAdminEmail.Location = new System.Drawing.Point(219, 38);
            this.lblAdminEmail.Name = "lblAdminEmail";
            this.lblAdminEmail.Size = new System.Drawing.Size(105, 13);
            this.lblAdminEmail.TabIndex = 6;
            this.lblAdminEmail.Text = "Admin Email Address";
            // 
            // txtAdminEmail
            // 
            this.txtAdminEmail.Location = new System.Drawing.Point(330, 35);
            this.txtAdminEmail.Name = "txtAdminEmail";
            this.txtAdminEmail.Size = new System.Drawing.Size(234, 20);
            this.txtAdminEmail.TabIndex = 7;
            // 
            // txtEmailPassword
            // 
            this.txtEmailPassword.Location = new System.Drawing.Point(330, 90);
            this.txtEmailPassword.Name = "txtEmailPassword";
            this.txtEmailPassword.PasswordChar = '*';
            this.txtEmailPassword.Size = new System.Drawing.Size(234, 20);
            this.txtEmailPassword.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(219, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Email Password";
            // 
            // txtEmailServer
            // 
            this.txtEmailServer.Location = new System.Drawing.Point(330, 62);
            this.txtEmailServer.Name = "txtEmailServer";
            this.txtEmailServer.Size = new System.Drawing.Size(234, 20);
            this.txtEmailServer.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(219, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Email Server";
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(386, 145);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(81, 35);
            this.butSave.TabIndex = 12;
            this.butSave.Text = "Save";
            this.butSave.UseVisualStyleBackColor = true;
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(483, 145);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(81, 35);
            this.butCancel.TabIndex = 13;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 192);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butSave);
            this.Controls.Add(this.txtEmailServer);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtEmailPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtAdminEmail);
            this.Controls.Add(this.lblAdminEmail);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkNIID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkPolicyDocs);
            this.Controls.Add(this.label1);
            this.Name = "frmSettings";
            this.Text = "frmSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkPolicyDocs;
        private System.Windows.Forms.CheckBox chkNIID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAdminEmail;
        private System.Windows.Forms.TextBox txtAdminEmail;
        private System.Windows.Forms.TextBox txtEmailPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEmailServer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.Button butCancel;
    }
}