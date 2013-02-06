using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Comissioning.Data;
using Commissioning.Data;
using Emailer.UI.Properties;
using ProxyHelpers.EWS;

namespace Emailer.UI.Controller
{
    class ExportEmails
    {
        private MainEmailerForm _form;
        private BackgroundWorker _oWorker;

        public ExportEmails(MainEmailerForm theForm)
        {
            _form = theForm;
            OWorker = new BackgroundWorker();
            OWorker.DoWork += oWorker_DoWork;
            OWorker.ProgressChanged += oWorker_ProgressChanged;
            OWorker.RunWorkerCompleted += oWorker_RunWorkerCompleted;
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
            
            Login login = new Login(Settings.Default.LocationOfExchangeServer);
            login.ShowDialog();

            BodyType type = new BodyType();
            type.BodyType1 = (BodyTypeType)Enum.Parse(typeof(BodyTypeType), Settings.Default.BodyType);

           
            var surgeries = new KeyedEmailAddressRepository(Settings.Default.KeyEmailFilePath).GetAll();
            var data = new KeyedDataRepository(_form.KeyedDataFilePath).GetAll();

            IEnumerable<DataEmailAddressGroup> _mailerInfo = DataEmailAddressGroup.CreateGroups(surgeries, data);

            int emailsSent = 0;
            int totalEmailsToSend = _mailerInfo.SelectMany(recipient => recipient.EmailAddresses).Count();

            foreach (DataEmailAddressGroup mailInfo in _mailerInfo)
            {
                var attachmentPath = CsvWriter.WriteFile(mailInfo.Data);

                foreach (var addressee in mailInfo.EmailAddresses)
                {
                    login.Send.DetailsWithAttachment(new ModelEmailDetails
                    {
                        SubjectOfEmail = string.Format("{0} for {1}", _form.TextBoxSubject, addressee.Key),
                        BodyOfEmail = _form.TextBoxBody,
                        SenderEmail = _form.TextBoxSender,
                        RecepientEmail = addressee.EmailAddress,
                        //AttachmentLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        //                            @"GitHub\NhsCommissioningMailer\CommissioningMailer\SampleData\") + "KeyEmailAddressPair.csv",
                        AttachmentLocation = attachmentPath.FullName,
                        BodyType = type,
                        ContentType = Settings.Default.ContentType
                    });

                    double percentage = ((double)emailsSent / totalEmailsToSend) * 100;
                    _oWorker.ReportProgress(Convert.ToInt32(percentage));
                    emailsSent++;

                    if (_oWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        _oWorker.ReportProgress(0);
                        return;
                    }
                }
            }

            OWorker.ReportProgress(100);
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

            removeTempFiles();

        }

        private void removeTempFiles()
        {
            try
            {
                string[] picList = Directory.GetFiles(Path.GetTempPath(), "*.csv");
                foreach (string file in picList)
                {
                    File.Delete(file);
                }
            }
            catch (IOException ex)
            {
                Logger.LogWriter.Instance.WriteToLog(string.Format("Clean Up Delete File Message--{0}\n\n", ex.Message));
            }
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
