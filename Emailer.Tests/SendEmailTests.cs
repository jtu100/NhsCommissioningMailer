using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using ProxyHelpers.EWS;

namespace Emailer.Tests
{
    [TestFixture]
    public class SendEmailTests
    {

        [Test]
        public void CheckIfEmailingAttachement_ReturnsResponseMessage()
        {
            string nameOfExchangeserver = @"https://outlook.nhs.net/EWS/exchange.asmx";

            ModelLoginDetails login = new ModelLoginDetails
                                          {
                                              Domain = string.Empty,
                                              ExchangeServerAddress = @"https://outlook.nhs.net/EWS/exchange.asmx",
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

        Assert.True(send.ResponseMessage.Contains("NoError"));

        }


    }
}
