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
	/// Partial class extension of EmailAddressType for ease of use (Listing 7-2)
	/// </summary>
	public partial class EmailAddressType
	{
		/// <summary>
		/// Constructor needed for xml serialization
		/// </summary>
		public EmailAddressType() { }

		/// <summary>
		/// Constructor that takes an email address
		/// </summary>
		/// <param name="emailAddress">email address of recipient</param>
		/// 
		public EmailAddressType(string emailAddress)
		{
			this.emailAddressField = emailAddress;
		}

		/// <summary>
		/// Constructor that takes all three properties
		/// </summary>
		/// <param name="name">Name for the recipient address</param>
		/// <param name="emailAddress">email address of the recipient</param>
		/// <param name="routingType">routing type for the email address</param>
		/// 
		public EmailAddressType(string name, string emailAddress, string routingType)
		{
			this.nameField = name;
			this.emailAddressField = emailAddress;
			this.routingTypeField = routingType;
		}

		/// <summary>
		/// Constructor that takes the ItemId of a private DL
		/// </summary>
		/// <param name="privateDLItemId">Id of a private DL</param>
		/// 
		public EmailAddressType(ItemIdType privateDLItemId)
		{
			this.itemIdField = privateDLItemId;
		}
	}
}
