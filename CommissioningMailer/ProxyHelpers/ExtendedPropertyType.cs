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
    /// Extension of the ExtendedPropertyType (Listing 13-8)
    /// </summary>
    public partial class ExtendedPropertyType
    {
        /// <summary>
        /// Constructor needed for xml serialization since we are providing overloads
        /// </summary>
        public ExtendedPropertyType()
        {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fieldURI">FieldURI representing metadata about the property</param>
        /// <param name="value">Value for the property</param>
        /// 
        public ExtendedPropertyType(PathToExtendedFieldType fieldURI, string value)
        {
            this.ExtendedFieldURI = fieldURI;
            this.Item = value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fieldURI">FieldURI representing metadata about the property</param>
        /// <param name="values">PARAMS array of values for multivalued property</param>
        /// 
        public ExtendedPropertyType(PathToExtendedFieldType fieldURI, params string[] values)
        {
            this.ExtendedFieldURI = fieldURI;
            NonEmptyArrayOfPropertyValuesType array = new NonEmptyArrayOfPropertyValuesType();
            array.Items = new string[values.Length];
            
            int index = 0;
            foreach (string value in values)
            {
                array.Items[index++] = value;
            }

            this.Item = array;
        }
    }
}
