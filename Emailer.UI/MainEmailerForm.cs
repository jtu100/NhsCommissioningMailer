using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emailer.UI.Controller;

namespace Emailer.UI
{
    public partial class MainEmailerForm : Form
    {
        private string _locationOfDataFile;

        public MainEmailerForm()
        {
            InitializeComponent();
        }

        public string TextBoxSender
        {
            get { return this.textBoxSender.Text; } 
            set { this.textBoxSender.Text = value; }
        }

        public string TextBoxSubject
        {
            get { return this.textBoxSubject.Text; }
            set { this.textBoxSubject.Text = value; }
        }

        public string TextBoxBody
        {
            get { return this.textBoxBody.Text; }
            set { this.textBoxBody.Text = value; }
        }

        public string LocationOfDataFiles
        {
            get { return _locationOfDataFile; }
            set { _locationOfDataFile = value; }
        }

        private void buttonOpenLocationOfFile_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog1.Title = "Please Locate File to Mail to Users";

                DialogResult result = this.openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this._locationOfDataFile = this.openFileDialog1.FileName;
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message + " - " + err.StackTrace);
                Logger.LogWriter.Instance.WriteToLog("Error Locating Opening File to Export: " + err.Message + " - " + err.StackTrace);
            }
        }

        private void buttonSendEmail_Click(object sender, EventArgs e)
        {
            Controller.ExportEmails exportEmails = new ExportEmails(this);
        }
    }
}
