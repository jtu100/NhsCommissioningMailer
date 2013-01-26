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
    public static class ErrorHelpers
    {
        /// <summary>
        /// Grab the response code from within a soap exception (Listing 18-16)
        /// </summary>
        /// <param name="soapException">Soap Exception to examine</param>
        /// <returns>True if parse was successful</returns>
        ///
        public static bool TryGetResponseCodeFromSoapException(
                                            SoapException soapException,
                                            out ResponseCodeType responseCode)
        {
            responseCode = ResponseCodeType.NoError;
            XmlElement detailElement = (XmlElement)soapException.Detail;
            if (detailElement == null)
            {
                return false;
            }
            XmlElement responseCodeElement = detailElement[
                       "ResponseCode",
                       "http://schemas.microsoft.com/exchange/services/2006/errors"];
            if (responseCodeElement == null)
            {
                return false;
            }
            responseCode = (ResponseCodeType)Enum.Parse(
                            typeof(ResponseCodeType),
                            responseCodeElement.InnerText);
            return true;
        }

    }
}
