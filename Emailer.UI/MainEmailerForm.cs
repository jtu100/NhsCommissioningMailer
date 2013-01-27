using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emailer.UI.Controller;

namespace Emailer.UI
{
    public partial class MainEmailerForm : Form
    {
        private string _locationOfDataFile;
        private List<Control> _textBoxsToQuery; 

        public MainEmailerForm()
        {
            InitializeComponent();
            this.buttonSendEmail.Click += new System.EventHandler(buttonSendEmail_Click);

            _textBoxsToQuery = new List<Control>();
            _textBoxsToQuery.Add(this.textBoxBody);
            _textBoxsToQuery.Add(this.textBoxSender);
            _textBoxsToQuery.Add(this.textBoxSubject);
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

        public ProgressBar ProgressBarForEmails
        {
            get { return this.progressBarForEmails; }
            set { this.progressBarForEmails = value; }
        }

        public Button ButtonSendEmail
        {
            get { return this.buttonSendEmail; }
            set { this.buttonSendEmail = value; }
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
                    this.labelNameOfFile.Text = Path.GetFileName(this.openFileDialog1.FileName);
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

            if (CheckToSeeIfControlsNotEmpty())
            {
                Controller.ExportEmails exportEmails = new ExportEmails(this);

                switch (this.buttonSendEmail.Text)
                {
                    case "Send Emails":
                        exportEmails.OWorker.RunWorkerAsync();
                        this.buttonSendEmail.Text = "Cancel";
                        break;
                    case "Cancel":
                        exportEmails.BtnCancelClick(this, e);
                        this.buttonSendEmail.Text = "Send Emails";
                        break;
                }
            }

        }

        private bool CheckToSeeIfControlsNotEmpty()
        {

            bool isSuccessful = true;

            var result = (from t in _textBoxsToQuery
                          where t.Text == string.Empty
                          select t).ToList();

            if ((result.Count > 1) || (this.openFileDialog1.FileName == string.Empty))
            {
                MessageBox.Show("Each Entry must have text\nand you need to pick a file");
                isSuccessful = false;
            }

            return isSuccessful;
        }
    }
}
