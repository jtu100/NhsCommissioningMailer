using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommissioningMailer.Models;
using Emailer.UI.Properties;
using ProxyHelpers.EWS;

namespace Emailer.UI.Controller
{
    class ExportEmails
    {
        private MainEmailerForm _form;

        public ExportEmails(MainEmailerForm TheForm)
        {
            _form = TheForm;
        }

        //Might need to put this in a background work and have a progress bar on the GUI?
        public void DoExport()
        {
            Dictionary<string, ModelDataDetails> dataDetails = new Dictionary<string, ModelDataDetails>();
            //Import Dictionary from Jack and place into dataDetails

            Login login = new Login(Settings.Default.LocationOfExchangeServer);
            login.ShowDialog();

            BodyType type = new BodyType();
            type.BodyType1 =  (BodyTypeType) Enum.Parse(typeof (BodyType), Settings.Default.BodyType);

            foreach (KeyValuePair<string, ModelDataDetails> dataDetail in dataDetails)
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
            }
        }
    }
}
