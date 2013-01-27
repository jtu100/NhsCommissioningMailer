using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommissioningMailer.Models;
using Emailer.UI.Properties;
using ProxyHelpers.EWS;

namespace Emailer.UI.Controller
{
    class ExportEmails
    {
        private MainEmailerForm _form;
        private BackgroundWorker _oWorker;

        public ExportEmails(MainEmailerForm TheForm)
        {
            _form = TheForm;
            OWorker = new BackgroundWorker();
            OWorker.DoWork += new DoWorkEventHandler(oWorker_DoWork);
            OWorker.ProgressChanged += new ProgressChangedEventHandler(oWorker_ProgressChanged);
            OWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(oWorker_RunWorkerCompleted);
            OWorker.WorkerReportsProgress = true;
            OWorker.WorkerSupportsCancellation = true;
 
        }

        public BackgroundWorker OWorker
        {
            get { return _oWorker; }
            set { _oWorker = value; }
        }


        public void oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<string, ModelDataDetail> dataDetails = new Dictionary<string, ModelDataDetail>();
            //Import Dictionary from Jack and place into dataDetails

            Login login = new Login(Settings.Default.LocationOfExchangeServer);
            login.ShowDialog();

            BodyType type = new BodyType();
            type.BodyType1 = (BodyTypeType)Enum.Parse(typeof(BodyTypeType), Settings.Default.BodyType);

            int currentpercentage = 0;

            foreach (KeyValuePair<string, ModelDataDetail> dataDetail in dataDetails)
            {
                login.Send.DetailsWithAttachment(new ModelEmailDetails
                {
                    SubjectOfEmail = string.Format("{0} for {1}", _form.TextBoxSubject, dataDetail.Value.SurgeryKey),
                    BodyOfEmail = _form.TextBoxBody,
                    SenderEmail = _form.TextBoxSender,
                    RecepientEmail = dataDetail.Key,
                    //AttachmentLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    //                            @"GitHub\NhsCommissioningMailer\CommissioningMailer\SampleData\") + "Surgery.csv",
                    AttachmentLocation = dataDetail.Value.LocationOfDataFile,
                    BodyType = type,
                    ContentType = Settings.Default.ContentType
                });
                int percentage = (currentpercentage / dataDetails.Count) * 100;
                _oWorker.ReportProgress(percentage);

                if (_oWorker.CancellationPending)
                {
                    e.Cancel = true;
                    _oWorker.ReportProgress(0);
                    return;
                }
            }
        }


        public void BtnCancelClick(object sender, EventArgs e)
        {
            if (OWorker.IsBusy)
            {
                //Stop/Cancel the async operation here
                OWorker.CancelAsync();
            }
        }


        /// <summary>
        /// On completed do the appropriate task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If it was cancelled midway
            if (e.Cancelled)
            {
                MessageBox.Show("Operation Cancelled");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error while performing background operation.");
            }
            else
            {
                MessageBox.Show("Emails Completed...");
            }
            
            _form.ButtonSendEmail.Text = "Send Emails";
            //btnStartAsyncOperation.Enabled = true;
           // btnCancel.Enabled = false;
        }


        /// <summary>
        /// Notification is performed here to the progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Here you play with the main UI thread
            _form.ProgressBarForEmails.Value = e.ProgressPercentage;
        }

    }
}
