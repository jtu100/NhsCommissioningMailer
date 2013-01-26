using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using ProxyHelpers.EWS;

namespace Emailer
{
    class SendEmail
    {
       
        private string _userName;
        private string _password;
        private string _domainName;
        private string _exchangeServerName;
        private string _responseMessage;

        public SendEmail(string UserName, string Password, string DomainName, string ExchangeServerName)
        {
            _userName = UserName;
            _password = Password;
            _domainName = DomainName;
            _exchangeServerName = ExchangeServerName;
        }

        public string ResponseMessage
        {
            get { return _responseMessage; }
            set { _responseMessage = value; }
        }

        public bool DetailsWithAttachment(string subject, string body, string from, string to, string attachment)
        {
            bool isSuccessful = true;
            
            try
            {
                
                ExchangeServiceBinding esb = new ExchangeServiceBinding();
                esb.Credentials = new NetworkCredential(this._userName, this._password, this._domainName);
                esb.Url = _exchangeServerName;

                //Text for Just sending Email Only
                //MessageType emailMessage = new MessageType();

                ////Add the sender/recipient to the email message
                //emailMessage.ToRecipients = new EmailAddressType[1];
                //emailMessage.ToRecipients[0] = new EmailAddressType();
                //emailMessage.ToRecipients[0].EmailAddress = to; //Currently there is only one recipient

                //emailMessage.From = new SingleRecipientType();  //set up a single sender
                //emailMessage.From.Item = new EmailAddressType();
                //emailMessage.From.Item.EmailAddress = from;

                //emailMessage.Subject = subject;

                //emailMessage.Body = new BodyType();
                //emailMessage.Body.BodyType1 = BodyTypeType.HTML; //specify HTML or plain Text
                //emailMessage.Body.Value = body;

                //CreateItemType emailToSave = new CreateItemType();
                //emailToSave.Items = new NonEmptyArrayOfAllItemsType();
                //emailToSave.Items.Items = new ItemType[1];
                //emailToSave.Items.Items[0] = emailMessage;
                //emailToSave.MessageDisposition = MessageDispositionType.SendAndSaveCopy;
                //emailToSave.MessageDispositionSpecified = true;

                //CreateItemResponseType response = esb.CreateItem(emailToSave);

                //// Get the response messages.
                //ResponseMessageType[] rmta = response.ResponseMessages.Items;

                //Create an email message and initialize it with the from address, to address, subject and the body of the email.
                MessageType email = new MessageType();

                email.ToRecipients = new EmailAddressType[1];
                email.ToRecipients[0] = new EmailAddressType();
                email.ToRecipients[0].EmailAddress = to;

                email.From = new SingleRecipientType();
                email.From.Item = new EmailAddressType();
                email.From.Item.EmailAddress = from;

                email.Subject = subject;

                email.Body = new BodyType();
                email.Body.BodyType1 = BodyTypeType.Text;
                email.Body.Value = body;

                //Save the created email to the drafts folder so that we can attach a file to it.
                CreateItemType emailToSave = new CreateItemType();
                emailToSave.Items = new NonEmptyArrayOfAllItemsType();
                emailToSave.Items.Items = new ItemType[1];
                emailToSave.Items.Items[0] = email;
                emailToSave.MessageDisposition = MessageDispositionType.SaveOnly;
                emailToSave.MessageDispositionSpecified = true;

                CreateItemResponseType response = esb.CreateItem(emailToSave);
                ResponseMessageType[] rmta = response.ResponseMessages.Items;
                ItemInfoResponseMessageType emailResponseMessage = (ItemInfoResponseMessageType)rmta[0];

                //Create the file attachment.
                FileAttachmentType fileAttachment = new FileAttachmentType();
                fileAttachment.Content = System.IO.File.ReadAllBytes(attachment); ;
                fileAttachment.Name = Path.GetFileName(attachment);
                fileAttachment.ContentType = "application/ms-excel";

                CreateAttachmentType attachmentRequest = new CreateAttachmentType();
                attachmentRequest.Attachments = new AttachmentType[1];
                attachmentRequest.Attachments[0] = fileAttachment;
                attachmentRequest.ParentItemId = emailResponseMessage.Items.Items[0].ItemId;

                //Attach the file to the message.
                CreateAttachmentResponseType attachmentResponse = (CreateAttachmentResponseType)esb.CreateAttachment(attachmentRequest);
                AttachmentInfoResponseMessageType attachmentResponseMessage = (AttachmentInfoResponseMessageType)attachmentResponse.ResponseMessages.Items[0];

                //Create a new item id type using the change key and item id of the email message so that we know what email to send.
                ItemIdType attachmentItemId = new ItemIdType();
                attachmentItemId.ChangeKey = attachmentResponseMessage.Attachments[0].AttachmentId.RootItemChangeKey;
                attachmentItemId.Id = attachmentResponseMessage.Attachments[0].AttachmentId.RootItemId;
                string test = attachmentResponseMessage.Attachments[0].Name;

                //Send the email.
                SendItemType si = new SendItemType();
                si.ItemIds = new BaseItemIdType[1];
                si.SavedItemFolderId = new TargetFolderIdType();
                si.ItemIds[0] = attachmentItemId;
                DistinguishedFolderIdType siSentItemsFolder = new DistinguishedFolderIdType();
                siSentItemsFolder.Id = DistinguishedFolderIdNameType.sentitems;
                si.SavedItemFolderId.Item = siSentItemsFolder;
                si.SaveItemToFolder = true;

                SendItemResponseType siSendItemResponse = esb.SendItem(si);

                //Log Email Response if Tracing is on
                CreateItemResponseType responseToEmail = esb.CreateItem(emailToSave);
                this._responseMessage = responseToEmail.ResponseMessages.Items[0].ResponseCode.ToString();

            }
            catch (Exception err)
            {
                isSuccessful = false;
                string errorMessage = string.Format("Error in sending Emails to Server {0} - {1}", err.Message,
                                                   err.StackTrace);

                Logger.LogWriter.Instance.WriteToLog(errorMessage);
                Debug.WriteLine(errorMessage);
                MessageBox.Show(string.Format("Error in sending Emails to Server\n{0}", err.Message), "Sending Email",
                MessageBoxButtons.OK);
                return isSuccessful;
            }

            return isSuccessful; 
        }
    }
}
