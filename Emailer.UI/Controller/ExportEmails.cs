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
        private Dictionary<string, ModelDataDetails> _modelLoginDetails; 

        public ExportEmails(MainEmailerForm TheForm)
        {
            _form = TheForm;
        }

        public void DoExport()
        {
            //Import Dictionary from Jack
            ModelLoginDetails login = new ModelLoginDetails
            {
                Domain = string.Empty,
                ExchangeServerAddress = Settings.Default.LocationOfExchangeServer,
                Password = "",
                UserName = "raza.toosy@nhs.net"
            };

            SendEmail send = new SendEmail(login);

            BodyType type = new BodyType();
            type.BodyType1 = BodyTypeType.Text;

            send.DetailsWithAttachment(
                new ModelEmailDetails
                {
                    SubjectOfEmail = "This is the Subject of the Email",
                    BodyOfEmail = "This is the body of the Email",
                    SenderEmail = "raza.toosy@nhs.net",
                    RecepientEmail = "raza.toosy@nhs.net",
                    AttachmentLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                @"GitHub\NhsCommissioningMailer\CommissioningMailer\SampleData\") + "Surgery.csv",
                    BodyType = type,
                    ContentType = "text/comma-separated-values"
                });
        }
         

    }
}
