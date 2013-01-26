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
    /// Partial class extension of ItemResponseShapeType to include useful constructor overloads.
    /// (Listing 3-5 - remainder of listing in FolderResponseShapeType.cs)
    /// </summary>
    public partial class ItemResponseShapeType
    {
        /// <summary>
        /// Default constructor needed for Xml serialization
        /// </summary>
        public ItemResponseShapeType() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseShape">BaseShape associated with this response shape</param>
        /// <param name="additionalProperties">OPTIONAL list of additional properties for this shape</param>
        /// 
        public ItemResponseShapeType(
                        DefaultShapeNamesType baseShape, 
                        params BasePathToElementType[] additionalProperties)
        {
            this.BaseShape = baseShape;
            if ((additionalProperties != null) && (additionalProperties.Length > 0))
            {
                this.AdditionalProperties = additionalProperties;
            }
        }
    }
}
