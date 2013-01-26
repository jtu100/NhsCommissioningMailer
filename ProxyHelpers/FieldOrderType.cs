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
    /// Partial class extension of the FieldOrderType (Listing 5-23)
    /// </summary>
    public partial class FieldOrderType
    {
        /// <summary>
        /// Default constructor.  Since we are providing an overload below, we must explicitly declare the 
        /// default constructor so that XML serialization will be happy.
        /// </summary>
        public FieldOrderType() { }

        /// <summary>
        /// Convenience constructor
        /// </summary>
        /// <param name="sortDirection">Direction of the sort</param>
        /// <param name="propertyPath">Property path used for this field order</param>
        /// 
        public FieldOrderType(SortDirectionType sortDirection, BasePathToElementType propertyPath)
        {
            this.orderField = sortDirection;
            this.itemField = propertyPath;
        }
    }
}
