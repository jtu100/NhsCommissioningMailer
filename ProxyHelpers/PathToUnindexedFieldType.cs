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
    /// Extension of the unindexed field uri (from Chapter 3)
    /// </summary>
    public partial class PathToUnindexedFieldType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyUriEnumValue">enum value that indicates which property we are 
        /// dealing with</param>
        /// 
        public PathToUnindexedFieldType(UnindexedFieldURIType propertyUriEnumValue)
        {
            this.fieldURIField = propertyUriEnumValue;
        }

        /// <summary>
        /// Default constructor needed for serialization to work
        /// </summary>
        public PathToUnindexedFieldType() { }
    }
}
