//  Inside Microsoft Exchange 2007 Web Services
//  ProxyHelpers
//
//  Copyright (c) 2007 David Sterling, Ben Spain, Mark Taylor, Huw Upshall, Michael Mainer.  
//  All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace ProxyHelpers.EWS
{
	/// <summary>
	/// Extension of PullSubscriptionRequestType to add some helpful overloads (Listing 17-17)
	/// and methods
	/// </summary> 
	public partial class PushSubscriptionRequestType
	{
		/// <summary>
		/// Constructor required for XML Serialization
		/// </summary>
		/// 
		public PushSubscriptionRequestType()
		{ }

		/// Overloaded constructor which helps in the creation of the Subscribe
		/// request for a Push Subscription
		/// </summary>
		/// <param name="subscriptionFolders">The folders you wish to  subscribe to</param>
		/// <param name="eventTypes">The events to subscribe for</param>
		/// <param name="statusFrequency">Frequency in minutes of server ping</param>
		/// <param name="url">URL for client Notifications web service</param>
		/// <param name="watermark">Watermark for recreating a previous Subscription</param>
		/// 
		public PushSubscriptionRequestType(
									BaseFolderIdType[] subscriptionFolders,
									NotificationEventTypeType[] eventTypes,
									int statusFrequency,
									string url,
									string watermark)
		{
			this.FolderIds = subscriptionFolders;
			this.EventTypes = eventTypes;
			this.StatusFrequency = statusFrequency;
			this.URL = url;

			// If we have a Watermark then set it on the Subscribe request
			//
			if (!string.IsNullOrEmpty(watermark))
			{
				this.Watermark = watermark;
			}
		}
	}

}
