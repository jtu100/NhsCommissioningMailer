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
    public partial class RestrictionType
    {
        /// <summary>
        /// Creates an IsEqualTo clause for our search key (Listing 5-14 cont'd)
        /// </summary>
        /// <param name="searchKeyPath">Search key path</param>
        /// <param name="value">base64 search key to look for</param>
        /// <returns>IsEqualTo clause</returns>
        /// 
        public static IsEqualToType CreateIsEqualToSearchKey(
                                PathToExtendedFieldType searchKeyPath,
                                string value)
        {
            IsEqualToType isEqualTo = new IsEqualToType();
            isEqualTo.Item = searchKeyPath;
            isEqualTo.FieldURIOrConstant = new FieldURIOrConstantType();
            ConstantValueType constant = new ConstantValueType();
            constant.Value = value;
            isEqualTo.FieldURIOrConstant.Item = constant;
            return isEqualTo;
        }

    }
}
