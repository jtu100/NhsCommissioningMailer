//  Inside Microsoft Exchange 2007 Web Services
//  ProxyHelpers
//
//  Copyright (c) 2007 David Sterling, Ben Spain, Mark Taylor, Huw Upshall, Michael Mainer.  
//  All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Net;

namespace ProxyHelpers.EWS
{
    public class EWSHelpers
    {
        /// <summary>
        /// Returns the inbox and deleted items folders
        /// </summary>
        /// <param name="binding">Binding which is used to call the EWS web service</param>
        /// <returns>Array containing the inbox and deleted items folders</returns>
        /// 
        public static BaseFolderType[] GetInboxAndDeletedItemsFolders(ExchangeServiceBinding binding)
        {
            // Create our request object and set the response shape
            //
            GetFolderType request = new GetFolderType();
            request.FolderShape = new FolderResponseShapeType();
            request.FolderShape.BaseShape = DefaultShapeNamesType.Default;

            // Set the folders that we wish to request
            //
            DistinguishedFolderIdType inboxId = new DistinguishedFolderIdType();
            inboxId.Id = DistinguishedFolderIdNameType.inbox;
            DistinguishedFolderIdType deletedItemsId = new DistinguishedFolderIdType();
            deletedItemsId.Id = DistinguishedFolderIdNameType.deleteditems;

            request.FolderIds = new BaseFolderIdType[] { inboxId, deletedItemsId };

            // Now make the actual request
            //
            GetFolderResponseType response = binding.GetFolder(request);

            // There should be two response messages - one for each folder we requested
            //
            Debug.Assert(response.ResponseMessages.Items.Length == 2);
            BaseFolderType[] result = new BaseFolderType[2];

            FolderInfoResponseMessageType firstFolderResponse =
                     (FolderInfoResponseMessageType)response.ResponseMessages.Items[0];
            result[0] = firstFolderResponse.Folders[0];

            FolderInfoResponseMessageType secondFolderResponse =
                     (FolderInfoResponseMessageType)response.ResponseMessages.Items[1];
            result[1] = secondFolderResponse.Folders[0];
            return result;
        }

        /// <summary>
        /// Create a folder
        /// </summary>
        /// <param name="binding">Binding to use for the call</param>
        /// <param name="displayName">DisplayName of the folder.  Must be unique within parent folder</param>
        /// <param name="parentFolder">Id of parent folder to store folder in.</param>
        /// <returns>Id of created folder</returns>
        /// 
        public static FolderIdType CreateFolder(
                        ExchangeServiceBinding binding,
                        string displayName,
                        BaseFolderIdType parentFolder)
        {
            CreateFolderType createRequest = new CreateFolderType();
            FolderType folder = new FolderType();
            folder.DisplayName = displayName;
            createRequest.ParentFolderId = new TargetFolderIdType(parentFolder);
            createRequest.Folders = new BaseFolderType[] { folder };
            CreateFolderResponseType response = binding.CreateFolder(createRequest);
            FolderInfoResponseMessageType responseMessage =
                response.ResponseMessages.Items[0] as FolderInfoResponseMessageType;
            if (responseMessage.ResponseCode != ResponseCodeType.NoError)
            {
                throw new Exception("Create folder failed.  Response code: " +
                      responseMessage.ResponseCode.ToString());
            }
            else
            {
                return responseMessage.Folders[0].FolderId;
            }
        }

        /// <summary>
        /// Create the passed item
        /// </summary>
        /// <param name="binding">Binding to use</param>
        /// <param name="itemToCreate">Item to create</param>
        /// <returns>Id of the created item</returns>
        /// 
        public static ItemIdType CreateItem(
                        ExchangeServiceBinding binding,
                        ItemType itemToCreate)
        {
            CreateItemType request = new CreateItemType();
            if (itemToCreate is MessageType)
            {
                request.MessageDisposition = MessageDispositionType.SaveOnly;
                request.MessageDispositionSpecified = true;
            }
            request.Items = new NonEmptyArrayOfAllItemsType();
            request.Items.Items = new ItemType[1];
            request.Items.Items[0] = itemToCreate;

            // Make the call
            //
            CreateItemResponseType response = binding.CreateItem(request);

            // Parse the result.  CreateItem returns ItemInfoResponseMessageType
            // instances
            //
            ItemInfoResponseMessageType responseMessage =
                response.ResponseMessages.Items[0] as ItemInfoResponseMessageType;
            if (responseMessage.ResponseCode == ResponseCodeType.NoError)
            {
                return responseMessage.Items.Items[0].ItemId;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Creates a message with the specified subject
        /// </summary>
        /// <param name="binding">Binding to use for the call</param>
        /// <param name="subject">Subject for the message</param>
        /// <returns>Id of created item</returns>
        /// 
        public static ItemIdType CreateMessage(
                        ExchangeServiceBinding binding,
                        string subject)
        {
            // Create our request
            //
            CreateItemType createRequest = new CreateItemType();

            // We want to create a single, empty item (no properties set)
            //
            MessageType message = new MessageType();
            message.Subject = subject;
            createRequest.Items = new NonEmptyArrayOfAllItemsType();
            createRequest.Items.Items = new ItemType[1];
            createRequest.Items.Items[0] = message;

            createRequest.MessageDisposition = MessageDispositionType.SaveOnly;
            createRequest.MessageDispositionSpecified = true;

            // Make the call
            //
            CreateItemResponseType response = binding.CreateItem(createRequest);

            // Parse the result.  CreateItem returns ItemInfoResponseMessageType
            // instances
            //
            ItemInfoResponseMessageType responseMessage =
                response.ResponseMessages.Items[0] as ItemInfoResponseMessageType;
            if (responseMessage.ResponseCode == ResponseCodeType.NoError)
            {
                return responseMessage.Items.Items[0].ItemId;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Return the item in question with the specified response shape
        /// </summary>
        /// <param name="binding">Binding to use</param>
        /// <param name="itemId">Id of item to retrieve</param>
        /// <param name="responseShape">response shape of item or NULL</param>
        /// <returns>Item retrieved</returns>
        /// 
        public static ItemType SimpleGetItem(
                        ExchangeServiceBinding binding,
                        ItemIdType itemId,
                        ItemResponseShapeType responseShape)
        {
            // Create our request type
            //
            GetItemType getItemRequest = new GetItemType();

            // Set our single id on our request
            //
            getItemRequest.ItemIds = new ItemIdType[] { itemId };

            // Build up the response shape (the properties we want back)
            //
            if (responseShape == null)
            {
                getItemRequest.ItemShape = new ItemResponseShapeType();
                getItemRequest.ItemShape.BaseShape = DefaultShapeNamesType.Default;
            }
            else
            {
                getItemRequest.ItemShape = responseShape;
            }

            // Now make the call
            // 
            GetItemResponseType getItemResponse = binding.GetItem(getItemRequest);

            // GetItem returns ItemInfoResponseMessages.  Since we only requested 
            // one item, we should only get back one response message.
            //
            ItemInfoResponseMessageType getItemResponseMessage =
                    getItemResponse.ResponseMessages.Items[0] as
                          ItemInfoResponseMessageType;

            // Like all good, happy and compliant developers, we should check our 
            // response code...
            //
            if (getItemResponseMessage.ResponseCode == ResponseCodeType.NoError)
            {
                return getItemResponseMessage.Items.Items[0];
            }
            else
            {
                throw new Exception(
                    "Failed to get the item.  ResponseCode: " +
                        getItemResponseMessage.ResponseCode.ToString());
            }
        }

        /// <summary>
        /// Binds to an item and returns the item with its default shape (Listing 5-3)
        /// </summary>
        /// <param name="binding">Binding to use</param>
        /// <param name="id">Id of item to bind to</param>
        /// <returns>ItemType instance or null if error</returns>
        /// 
        public static ItemType SimpleGetItem(
                            ExchangeServiceBinding binding,
                            ItemIdType id)
        {
            // Create our request type
            //
            GetItemType getItemRequest = new GetItemType();

            // Set our single id on our request
            //
            getItemRequest.ItemIds = new ItemIdType[] { id };

            // Build up the response shape (the properties we want back)
            //
            getItemRequest.ItemShape = new ItemResponseShapeType();
            getItemRequest.ItemShape.BaseShape = DefaultShapeNamesType.Default;

            // Now make the call
            // 
            GetItemResponseType getItemResponse = binding.GetItem(getItemRequest);

            // GetItem returns ItemInfoResponseMessages.  Since we only requested 
            // one item, we should only get back one response message.
            //
            ItemInfoResponseMessageType getItemResponseMessage =
                      getItemResponse.ResponseMessages.Items[0] as
                            ItemInfoResponseMessageType;

            // Like all good, happy and compliant developers, we should check our 
            // response code...
            //
            if (getItemResponseMessage.ResponseCode == ResponseCodeType.NoError)
            {
                return getItemResponseMessage.Items.Items[0];
            }
            else
            {
                throw new Exception(
                    "Failed to get the item.  ResponseCode: " +
                        getItemResponseMessage.ResponseCode.ToString());
            }
        }


        /// <summary>
        /// Create an item with no properties set (Listing 5-8)
        /// </summary>
        /// <param name="binding">Binding to use for web method call</param>
        /// <returns>Id of newly created item or NULL if failure</returns>
        ///
        public static ItemIdType SimpleCreateItem(ExchangeServiceBinding binding)
        {
            // Create our request
            //
            CreateItemType request = new CreateItemType();
            request.Items = new NonEmptyArrayOfAllItemsType();
            // We want to create a single, empty item (no properties set)
            //
            request.Items.Items = new ItemType[] { new ItemType() };
            // Make the call...
            //
            CreateItemResponseType response = binding.CreateItem(request);
            // Parse the result. CreateItem returns ItemInfoResponseMessageType
            // instances
            //
            ItemInfoResponseMessageType responseMessage =
            response.ResponseMessages.Items[0] as ItemInfoResponseMessageType;
            if (responseMessage.ResponseCode == ResponseCodeType.NoError)
            {
                return responseMessage.Items.Items[0].ItemId;
            }
            else
            {
                throw new Exception("Failed to create the item. Response code: " +
                    responseMessage.ResponseCode.ToString());
            }
        }

        /// <summary>
        /// Returns the UTF8 MIME string contents from the base64 string (Listing 5-39)
        /// </summary>
        /// <param name="base64MimeContent">Base64 content</param>
        /// <returns>MIME string</returns>
        /// 
        public static string GetMimeText(string base64MimeContent)
        {
            // Content is base-64 encoded.  We need the byte[]
            //
            byte[] bytes = System.Convert.FromBase64String(base64MimeContent);

            // Of course, for other encodings, you would replace the UTF8 singleton 
            // reference here accordingly.
            //
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Finds all the items within a folder
        /// </summary>
        /// <param name="binding">Binding to use</param>
        /// <param name="folderId">folder id of parent folder</param>
        /// <param name="responseShape">Response shape for returned items</param>
        /// <returns>Found items</returns>
        /// 
        public static ItemType[] FindAllItemsInFolder(
            ExchangeServiceBinding binding,
            BaseFolderIdType folderId,
            ItemResponseShapeType responseShape)
        {
            FindItemType request = new FindItemType();
            if (responseShape == null)
            {
                responseShape = new ItemResponseShapeType(
                    DefaultShapeNamesType.Default);
            }
            request.ItemShape = responseShape;
            request.ParentFolderIds = new BaseFolderIdType[] { folderId };
            request.Traversal = ItemQueryTraversalType.Shallow;

            FindItemResponseType response = binding.FindItem(request);
            FindItemResponseMessageType responseMessage =
                response.ResponseMessages.Items[0] as FindItemResponseMessageType;
            if (responseMessage.ResponseCode == ResponseCodeType.NoError)
            {
                return responseMessage.RootFolder.GetNormalResults();
            }
            else
            {
                throw new Exception("FindItem failed with response code: " +
                    responseMessage.ResponseCode.ToString());
            }
        }

        /// <summary>
        /// Returns the Content type for the passed file based on the extension
        /// </summary>
        /// <param name="filename">Filename to determine mime type for</param>
        /// <returns>Content type string</returns>
        /// 
        public static string GetMimeTypeForFile(string filename)
        {
            // There are two places in the registry where we might be able to find 
            // content type information for a given extension.  The first, and definitely
            // the quickest, is to look in the HKEY_CLASSES_ROOT hive. Each registered
            // extension is supposed to have its own subkey under HKCR.
            // If we don’t find our extension using the first approach, we can look at
            // the registered MIME types.  Unfortunately there, we have to walk through
            // all of those keys and see if that MIME type is associated with our 
            // extension of interest.
            //
            // First try to retrieve the content type via the extension directly.
            //
            string extension = Path.GetExtension(filename);
            using (RegistryKey extensionKey =
                          Registry.ClassesRoot.OpenSubKey(
                                     extension,
                                     false /* need writable */))
            {
                if (extensionKey != null)
                {
                    string contentType = (string)extensionKey.GetValue(
                                 "Content Type");
                    if (!String.IsNullOrEmpty(contentType))
                    {
                        return contentType;
                    }
                }
            }

            // If we are here, then we couldn't find the content type value using the
            // extension name. So, as a fallback, look at the 
            // HKEY_CLASSES_ROOT\MIME\Database\Content Type key
            // and look at the Extension key for the correct file extension
            //
            using (RegistryKey mimeDatabaseKey =
                       Registry.ClassesRoot.OpenSubKey(
                              @"MIME\Database\Content Type",
                              false /* need writable */))
            {
                if (mimeDatabaseKey != null)
                {
                    string[] subkeys = mimeDatabaseKey.GetSubKeyNames();
                    // Enuemrate across all of the registered MIME types.
                    //
                    foreach (string subkey in subkeys)
                    {
                        using (RegistryKey contentTypeKey =
                                  mimeDatabaseKey.OpenSubKey(subkey))
                        {
                            string contentTypeValue =
                                    (string)contentTypeKey.GetValue("Content Type");
                            if (contentTypeValue == extension)
                            {
                                return subkey;
                            }
                        }
                    }
                }
            }

            // We didn't find it.
            //
            return String.Empty;
        }

        /// <summary>
        /// Determine if the given item supports the templated response object type (Listing 7-33)
        /// </summary>
        /// <typeparam name="T">Type of response object to check support for</typeparam>
        /// <param name="item">Item to check</param>
        /// <returns>True if the item supports the supplied response object type</returns>
        /// 
        public static bool CanPerformResponseObject<T>(ItemType item) where T : ResponseObjectType
        {
            foreach (ResponseObjectType responseObject in item.ResponseObjects)
            {
                if (responseObject is T)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Enumeration allowing us to indicate whether we want internal or external URLs
        /// </summary>
        public enum AutodiscoverProtocol
        {
            Internal,
            External,
        }

        /// <summary>Access the Autodiscover service to configure client application. (Listing 20-13)</summary>
        /// <param name="userSMTPAddress">User's primary SMTP</param>
        /// <param name="redirections">Redirections to Autodiscover service. Use zero as the
        /// initial value</param>
        /// <returns>The Exchange Web Services URL.</returns>
        /// 
        public static string GetExchangeWebServicesURL(
                                     string userSMTPAddress,
                                     AutodiscoverProtocol autodiscoverProtocol,
                                     int redirections)
        {
            // Check redirection counter. We don't want to get in an endless loop.
            //
            if (redirections > 2)
            {
                throw new Exception("Maximum of 2 redirections.");
            }

            // Check the SMTP address.
            //
            if (userSMTPAddress == string.Empty)
            {
                throw new Exception("Provide an SMTP address.");
            }

            // Get the username and domain from the SMTP address.
            //
            string[] smtpParts = userSMTPAddress.Split('@');
            if (smtpParts.Length != 2)
            {
                throw new Exception("Invalid Smtp address format");
            }
            string username = smtpParts[0];
            string domain = smtpParts[1];

            // Construct the Autodiscover URL if it was not passed in.
            //
            string autoDiscoverURL =
                    @"https://" + domain + @"/autodiscover/autodiscover.xml";

            // Create the Outlook provider Autodiscover request.
            //
            StringBuilder request = new StringBuilder();
            request.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            request.Append(
                "<Autodiscover xmlns=\"http://schemas.microsoft.com/exchange/autodiscover/" +
                "outlook/requestschema/2006\">");
            request.Append("  <Request>");
            request.Append("    <EMailAddress>" + userSMTPAddress + "</EMailAddress>");
            request.Append(
                "<AcceptableResponseSchema>http://schemas.microsoft.com/exchange/" +
                "autodiscover/outlook/responseschema/2006a</AcceptableResponseSchema>");
            request.Append("  </Request>");
            request.Append("</Autodiscover>");

            string requestString = request.ToString();
            // Create a WebRequest object to send our request to the server.
            // Add a password and domain for the user.
            //
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(autoDiscoverURL);
            webRequest.Credentials = new NetworkCredential("username", "password", "domain");
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml; charset=utf-8";
            webRequest.ContentLength = requestString.Length;

            using (Stream requestStream = webRequest.GetRequestStream())
            {
                byte[] requestBytes = Encoding.ASCII.GetBytes(requestString);
                // Write the request to the output stream.
                //
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            // Get the response
            //
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            // Throw exception if the request failed.
            //
            if (HttpStatusCode.OK != response.StatusCode)
            {
                throw new InvalidOperationException(response.StatusDescription);
            }

            string responseXML;
            // Otherwise, read the content of the response and return it.
            //
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                responseXML = sr.ReadToEnd();
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseXML);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace(
                 "r",
                 @"http://schemas.microsoft.com/exchange/autodiscover/" +
                 "outlook/responseschema/2006a");

            // Get the EWS URL if it is available.
            //
            string protocolString = (autodiscoverProtocol == AutodiscoverProtocol.Internal) ?
                      "EXCH" :
                      "EXPR";

            XmlNodeList ewsProtocolNodes =
                      xmlDoc.SelectNodes(@"//r:Protocol", namespaceManager);

            // For each protocol in the response, look for the appropriate protocol type 
            // (internal/external)
            // and then look for the corresponding ASUrl element.
            //
            foreach (XmlNode node in ewsProtocolNodes)
            {
                string type = node.SelectSingleNode(@"r:Type", namespaceManager).InnerText;
                if (type == protocolString)
                {
                    XmlNodeList nodes = node.SelectNodes(@"r:ASUrl[1]", namespaceManager);
                    if ((nodes != null) || (nodes.Count > 0))
                    {
                        return nodes[0].InnerText;
                    }
                }
            }

            // If we are here, then either Autodiscovery returned an error or there is a
            // redirect address to retry the Autodiscover lookup.
            //
            XmlNodeList redirectAddr =
                    xmlDoc.SelectNodes(@"//r:RedirectAddr[1]", namespaceManager);

            // Check if redirect URL or redirect address is in response.
            if ((redirectAddr != null) && (redirectAddr.Count > 0))
            {
                redirections++;
                // Retry with the redirect address.
                //
                return GetExchangeWebServicesURL(
                               redirectAddr[0].InnerText,
                               autodiscoverProtocol,
                               redirections);
            }
            else
            {
                throw new Exception("Autodiscovery call failed unexpectedly");
            }
        }

        /// <summary>
        /// Takes a CalendarEventDetails ID, from a GetUserAvailabiltyResponse for 
        /// example, and finds the CalendarItem that matches it. (Listing B-22)
        /// </summary>
        /// <param name="calendarEventDetailsId">
        /// ID property value from a CalendarEventDetails</param>
        /// <param name="binding">Binding to make the call with</param>
        /// <returns>ItemId of the associated calendar item</returns>
        /// 
        public static ItemIdType GetCalendarItemFromCalendarEvent(
            string calendarEventDetailsId,
            ExchangeServiceBinding binding)
        {
            // The ID from CalendarEventDetails is a hexidecimal PR_ENTRY_ID.  In
            // order to use it as a restriction on FindItem, we need to convert it to
            // a byte-array and base64 encode it.
            //
            int i = 0;
            string hexEntryId = calendarEventDetailsId;
            List<byte> byteArray = new List<byte>(hexEntryId.Length / 2);
            while (i < hexEntryId.Length)
            {
                byteArray.Add(Convert.ToByte(hexEntryId.Substring(i, 2), 16));
                i = i + 2;
            }

            string base64EntryId = System.Convert.ToBase64String(byteArray.ToArray());

            // Build the conditions of the FindItem request
            //
            PathToExtendedFieldType extendedProp = new PathToExtendedFieldType();
            extendedProp.PropertyTag = "0x0FFF";  // PR_ENTRYID
            extendedProp.PropertyType = MapiPropertyTypeType.Binary;

            ConstantValueType propValue = new ConstantValueType();
            propValue.Value = base64EntryId;

            IsEqualToType equalToRestriction = new IsEqualToType();
            equalToRestriction.Item = extendedProp;
            equalToRestriction.FieldURIOrConstant = new FieldURIOrConstantType();
            equalToRestriction.FieldURIOrConstant.Item = propValue;

            RestrictionType restriction = new RestrictionType();
            restriction.Item = equalToRestriction;

            FindItemType request = new FindItemType();
            request.Restriction = restriction;
            request.ItemShape = new ItemResponseShapeType();
            request.ItemShape.BaseShape = DefaultShapeNamesType.IdOnly;

            // We know that CalendarEvents can only come from the default calendar
            // 
            request.ParentFolderIds =
                new DistinguishedFolderIdType[] { new DistinguishedFolderIdType() };
            ((DistinguishedFolderIdType)request.ParentFolderIds[0]).Id =
                DistinguishedFolderIdNameType.calendar;

            // Perform the FindItem request.
            FindItemResponseType response = binding.FindItem(request);
            if (response.ResponseMessages.Items[0].ResponseClass !=
                ResponseClassType.Success)
            {
                throw new Exception("Unable to perform FindItem for an " +
                    "associated Calendar Item.\r\n" +
                    response.ResponseMessages.Items[0].ResponseCode.ToString() +
                    "\r\n" +
                    response.ResponseMessages.Items[0].MessageText.ToString());
            }

            FindItemResponseMessageType firmt =
                (FindItemResponseMessageType)response.ResponseMessages.Items[0];
            if (firmt.RootFolder.TotalItemsInView != 1)
            {
                throw new Exception("Find Item request failed, request returned " +
                    firmt.RootFolder.TotalItemsInView + " items.");
            }
            ArrayOfRealItemsType itemArray =
                (ArrayOfRealItemsType)firmt.RootFolder.Item;
            return itemArray.Items[0].ItemId;
        }

    }
}