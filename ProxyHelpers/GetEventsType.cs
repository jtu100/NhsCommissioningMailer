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
	/// Extension of GetEventsType to add some helpful overloads and methods (Listing 17-8)
	/// </summary>
	public partial class GetEventsType
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public GetEventsType()
		{ }

		/// <summary>
		/// Overloaded constructor for creating GetEvents request
		/// </summary>
		/// <param name="subscriptionId">ID of Subscription</param>
		/// <param name="watermark">Subscription watermark</param>
		public GetEventsType(string subscriptionId, string watermark)
		{
			this.SubscriptionId = subscriptionId;
			this.Watermark = watermark;
		}
	
	}
}
