//  Inside Microsoft Exchange 2007 Web Services
//  ProxyHelpers
//
//  Copyright (c) 2007 David Sterling, Ben Spain, Mark Taylor, Huw Upshall, Michael Mainer.  
//  All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;

namespace ProxyHelpers.EWS
{
    /// <summary>
    /// Exception class thrown when we encounter either a failure response message or a soap exception
    /// Listing 18-18
    /// </summary>
    /// 
    [Serializable]
    public class EWSException: Exception
    {
        private ResponseMessageType responseMessage;
        private bool fromSoapFault;
        private BasePathToElementType[] propertyPaths = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">ResponseMessage to build it from</param>
        /// 
        public EWSException(ResponseMessageType responseMessage) : base(responseMessage.MessageText)
        {
            this.responseMessage = responseMessage;
            this.fromSoapFault = false;

            // Look at the MessageXml and see if there are any property paths
            //
            FieldURIMapper.TryExtractFieldURIsFromResponseMessage(responseMessage, out this.propertyPaths);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="soapException">SoapException to build it from</param>
        /// 
        public EWSException(SoapException soapException) : base(soapException.Message, soapException)
        {
            this.responseMessage = BuildResponseMessageFromSoapFault(soapException);
            this.fromSoapFault = true;
        }

        /// <summary>
        /// Accessor for the contained response message
        /// </summary>
        public ResponseMessageType ResponseMessage
        {
            get
            {
                return this.responseMessage;
            }
        }

        /// <summary>
        /// Returns true if this was generated from a soap fault
        /// </summary>
        public bool FromSoapFault
        {
            get
            {
                return this.fromSoapFault;
            }
        }

        /// <summary>
        /// Accessor for any offending property paths
        /// </summary>
        public BasePathToElementType[] PropertyPaths
        {
            get
            {
                return this.propertyPaths;
            }
        }

        /// <summary>
        /// Helper method for building a ResponseMessageType instance from a soap fault
        /// </summary>
        /// <param name="soapException">Soap exception</param>
        /// <returns>ResponseMessage</returns>
        /// 
        private ResponseMessageType BuildResponseMessageFromSoapFault(SoapException soapException)
        { 
            ResponseMessageType result = new ResponseMessageType();
            result.DescriptiveLinkKey = 0;
            result.DescriptiveLinkKeySpecified = true;

            result.MessageText = soapException.Message;
            XmlElement detailElement = soapException.Detail as XmlElement;
            XmlElement responseCodeElement = detailElement[
                            "ResponseCode", 
                            "http://schemas.microsoft.com/exchange/services/2006/errors"];
            result.ResponseCode = (ResponseCodeType)Enum.Parse(
                            typeof(ResponseCodeType), 
                            responseCodeElement.InnerText);
            result.ResponseCodeSpecified = true;
            result.ResponseClass = ResponseClassType.Error;
            result.MessageXml = null;
            return result;
        }

        /// <summary>
        /// Helper method for examining a response message and throwing an exception if it is an error
        /// </summary>
        /// <param name="responseMessage">ResponseMessage to examine</param>
        /// 
        public static void ThrowIfError(ResponseMessageType responseMessage)
        {
            // NOTE:  You will need to change this if you want to fail on warnings.
            //
            if (responseMessage.ResponseClass == ResponseClassType.Error)
            {
                throw new EWSException(responseMessage);
            }
        }
    }
}
