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
	/// Extension of UnsubscribeType to add some helpful overloads and methods (Listing 17-13)
	/// </summary>
	public partial class UnsubscribeType
	{
		/// <summary>
		/// Constructor required for XML serialization
		/// </summary>
		public UnsubscribeType()
		{ }

		/// <summary>
		/// Overloaded constructor which takes the Subscription ID you wish
		/// to unsubscribe from
		/// </summary>
		/// <param name="subscriptionId">Subscription ID</param>
		public UnsubscribeType(string subscriptionId)
		{
			this.SubscriptionId = subscriptionId;
		}
	}

}
