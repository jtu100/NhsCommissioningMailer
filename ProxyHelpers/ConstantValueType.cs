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
    /// Partial class extension of ConstantValueType
    /// </summary>
    public partial class ConstantValueType
    {
        /// <summary>
        /// Constructor needed for XmlSerialization
        /// </summary>
        /// 
        public ConstantValueType() { }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Constant value to associate with this instance</param>
        /// 
        public ConstantValueType(string value)
        {
            this.valueField = value;
        }
    }
}
