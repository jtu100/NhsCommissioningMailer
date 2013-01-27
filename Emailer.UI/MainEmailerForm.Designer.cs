namespace Emailer.UI
{
    partial class MainEmailerForm
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
            this.textBoxSender = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSubject = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxBody = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonSendEmail = new System.Windows.Forms.Button();
            this.buttonOpenLocationOfFile = new System.Windows.Forms.Button();
            this.progressBarForEmails = new System.Windows.Forms.ProgressBar();
            this.labelNameOfFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "I\'d like to send an email from";
            // 
            // textBoxSender
            // 
            this.textBoxSender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSender.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSender.Location = new System.Drawing.Point(294, 22);
            this.textBoxSender.Name = "textBoxSender";
            this.textBoxSender.Size = new System.Drawing.Size(350, 26);
            this.textBoxSender.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(64, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 28);
            this.label2.TabIndex = 0;
            this.label2.Text = "with the following Title";
            // 
            // textBoxSubject
            // 
            this.textBoxSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSubject.Location = new System.Drawing.Point(293, 63);
            this.textBoxSubject.Name = "textBoxSubject";
            this.textBoxSubject.Size = new System.Drawing.Size(352, 26);
            this.textBoxSubject.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(32, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(244, 28);
            this.label3.TabIndex = 0;
            this.label3.Text = "The Content of the Email is";
            // 
            // textBoxBody
            // 
            this.textBoxBody.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBody.Location = new System.Drawing.Point(294, 103);
            this.textBoxBody.Multiline = true;
            this.textBoxBody.Name = "textBoxBody";
            this.textBoxBody.Size = new System.Drawing.Size(351, 131);
            this.textBoxBody.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 251);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 28);
            this.label4.TabIndex = 0;
            this.label4.Text = "Location of Data to Mail";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Csv files|*.csv";
            this.openFileDialog1.Title = "Pick Name of Data File to Split";
            // 
            // buttonSendEmail
            // 
            this.buttonSendEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSendEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSendEmail.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSendEmail.Location = new System.Drawing.Point(379, 303);
            this.buttonSendEmail.Name = "buttonSendEmail";
            this.buttonSendEmail.Size = new System.Drawing.Size(266, 69);
            this.buttonSendEmail.TabIndex = 5;
            this.buttonSendEmail.Text = "Send Emails";
            this.buttonSendEmail.UseVisualStyleBackColor = true;
            // 
            // buttonOpenLocationOfFile
            // 
            this.buttonOpenLocationOfFile.Image = global::Emailer.UI.Properties.Resources.FolderImage1;
            this.buttonOpenLocationOfFile.Location = new System.Drawing.Point(250, 251);
            this.buttonOpenLocationOfFile.Name = "buttonOpenLocationOfFile";
            this.buttonOpenLocationOfFile.Size = new System.Drawing.Size(40, 40);
            this.buttonOpenLocationOfFile.TabIndex = 4;
            this.buttonOpenLocationOfFile.UseVisualStyleBackColor = true;
            this.buttonOpenLocationOfFile.Click += new System.EventHandler(this.buttonOpenLocationOfFile_Click);
            // 
            // progressBarForEmails
            // 
            this.progressBarForEmails.Location = new System.Drawing.Point(26, 319);
            this.progressBarForEmails.Name = "progressBarForEmails";
            this.progressBarForEmails.Size = new System.Drawing.Size(343, 31);
            this.progressBarForEmails.Step = 1;
            this.progressBarForEmails.TabIndex = 6;
            // 
            // labelNameOfFile
            // 
            this.labelNameOfFile.AutoSize = true;
            this.labelNameOfFile.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNameOfFile.Location = new System.Drawing.Point(296, 254);
            this.labelNameOfFile.Name = "labelNameOfFile";
            this.labelNameOfFile.Size = new System.Drawing.Size(0, 28);
            this.labelNameOfFile.TabIndex = 0;
            // 
            // MainEmailerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 386);
            this.Controls.Add(this.progressBarForEmails);
            this.Controls.Add(this.buttonSendEmail);
            this.Controls.Add(this.buttonOpenLocationOfFile);
            this.Controls.Add(this.textBoxBody);
            this.Controls.Add(this.textBoxSubject);
            this.Controls.Add(this.textBoxSender);
            this.Controls.Add(this.labelNameOfFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainEmailerForm";
            this.Text = "Commissioning Mailer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSender;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSubject;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxBody;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonOpenLocationOfFile;
        private System.Windows.Forms.Button buttonSendEmail;
        private System.Windows.Forms.ProgressBar progressBarForEmails;
        private System.Windows.Forms.Label labelNameOfFile;
    }
}