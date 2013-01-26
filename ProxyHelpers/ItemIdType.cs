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
    /// Partial class extension to ItemIdType (Listing 12-17)
    /// </summary>
    public partial class ItemIdType
    {
        /// <summary>
        /// Default constructor needed for xml serialization (since we are defining a 
        /// non default constructor)
        /// </summary>
        /// 
        public ItemIdType()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rootItemId">RootItemId instance to initialize from</param>
        /// 
        public ItemIdType(RootItemIdType rootItemId)
        {
            this.Id = rootItemId.RootItemId;
            this.ChangeKey = rootItemId.RootItemChangeKey;
        }
    }
}
