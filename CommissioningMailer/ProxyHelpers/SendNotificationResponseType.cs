//  Inside Microsoft Exchange 2007 Web Services
//  ProxyHelpers
//
//  Copyright (c) 2007 David Sterling, Ben Spain, Mark Taylor, Huw Upshall, Michael Mainer.  
//  All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProxyHelpers.EWS
{
    /// <summary>
    /// Partial class extension of SendNotificationResponseType so that we can add the XmlRoot attribute
    /// used during Push Notification deserialization (Listing 17-22)
    /// </summary>
    [XmlRoot(ElementName = "SendNotification", 
             Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SendNotificationResponseType : BaseResponseMessageType
    {
        /// <summary>
        /// Return the single response message cast to the proper type
        /// </summary>
        /// <returns>SendNotificationResponseMessageType</returns>
        /// 
        public SendNotificationResponseMessageType GetResponseMessage()
        {
            return this.ResponseMessages.Items[0] as SendNotificationResponseMessageType;
        }
}
}
