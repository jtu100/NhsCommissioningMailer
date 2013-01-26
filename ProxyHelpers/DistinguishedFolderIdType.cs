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
	/// Partial class extension of DistinguishedFolderIdType (Listing 1-8)
	/// </summary>
	public partial class DistinguishedFolderIdType
	{
		/// <summary>
		/// Default constructor needed for Xml serialization
		/// </summary>
		public DistinguishedFolderIdType() { }

		/// <summary>
		/// Constructor for sanity's sake
		/// </summary>
		/// <param name="folderName">Folder id enum to indicate which distinguished folder we
		/// are dealing with</param>
		/// 
		public DistinguishedFolderIdType(DistinguishedFolderIdNameType folderName)
		{
			this.idField = folderName;
		}

		/// <summary>
		/// Constructor for sanity's sake
		/// </summary>
		/// <param name="folderName">Folder id enum to indicate which distinguished folder we are dealing
		/// with</param>
		/// <param name="primarySmtpAddress">Primary Smtp Address of the mailbox we are trying to
		/// access</param>
		/// 
		public DistinguishedFolderIdType(DistinguishedFolderIdNameType folderName, string primarySmtpAddress) :
			this(folderName)
		{
			this.mailboxField = new EmailAddressType();
			this.mailboxField.EmailAddress = primarySmtpAddress;
		}
	}
}
