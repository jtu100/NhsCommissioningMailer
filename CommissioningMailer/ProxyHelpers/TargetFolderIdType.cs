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
    /// Partial class extension of TargetFolderIdType
    /// </summary>
    public partial class TargetFolderIdType
    {
        /// <summary>
        /// Constructor needed for xml serialization
        /// </summary>
        public TargetFolderIdType()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Distinguished folder id enum value</param>
        /// 
        public TargetFolderIdType(DistinguishedFolderIdNameType id)
        {
            this.Item = new DistinguishedFolderIdType(id);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Distinguished folder id enum value</param>
        /// <param name="mailboxPrimarySmtpAddress">Primary smtp of the mailbox owner</param>
        /// 
        public TargetFolderIdType(
                        DistinguishedFolderIdNameType id, 
                        string mailboxPrimarySmtpAddress)
        {
            this.Item = new DistinguishedFolderIdType(id, mailboxPrimarySmtpAddress);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="folderId">Id of the target folder</param>
        /// 
        public TargetFolderIdType(BaseFolderIdType folderId)
        {
            this.Item = folderId;
        }
    }
}
