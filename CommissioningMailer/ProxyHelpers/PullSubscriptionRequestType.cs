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
	/// Partial class extension of the PullSubscriptionRequestType (Listing 17-3)
	/// </summary>
	public partial class PullSubscriptionRequestType
	{
		/// <summary>
		/// Default constructor needed for XML serialization
		/// </summary>
		public PullSubscriptionRequestType()
		{}

		/// <summary>
		/// Overloaded constructor which helps in the creation of the Subscribe
		/// request for a Pull Subscription
		/// </summary>
		/// <param name="subscriptionFolders">The Distinguished folders
		/// you wish to subscribe to</param>
		/// <param name="eventTypes">The events to subscribe for</param>
		/// <param name="watermark">Watermark for recreating a previous
		/// Subscription</param>
		/// <param name="timeout">Timeout for Subscription in minutes</param>
		/// 
		public PullSubscriptionRequestType(
			BaseFolderIdType[] subscriptionFolders,
			NotificationEventTypeType[] eventTypes,
			string watermark,
			int timeout)
		{
			this.FolderIds = subscriptionFolders;
			this.EventTypes = eventTypes;

			// If we have a Watermark then set it on the Subscribe request
			//
			if (!string.IsNullOrEmpty(watermark))
			{
				this.Watermark = watermark;
			}

			this.Timeout = timeout;
		}
	}
}
