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
    /// Extension of PathToExtendedFieldType to add some helpful overloads and methods
    /// Listing 13-2, 13-3, 13-4
    /// </summary>
    public partial class PathToExtendedFieldType
    {
        private static Dictionary<int, SingleAndArrayPair> mapping = new Dictionary<int, SingleAndArrayPair>();

        /// <summary>
        /// Static constructor.  Used to fill up our dictionary
        /// </summary>
        static PathToExtendedFieldType()
        {
            mapping.Add(2, new SingleAndArrayPair(MapiPropertyTypeType.Short, MapiPropertyTypeType.ShortArray));
            mapping.Add(3, new SingleAndArrayPair(MapiPropertyTypeType.Integer, MapiPropertyTypeType.IntegerArray));
            mapping.Add(4, new SingleAndArrayPair(MapiPropertyTypeType.Float, MapiPropertyTypeType.FloatArray));
            mapping.Add(5, new SingleAndArrayPair(MapiPropertyTypeType.Double, MapiPropertyTypeType.DoubleArray));
            mapping.Add(6, new SingleAndArrayPair(MapiPropertyTypeType.Currency, MapiPropertyTypeType.CurrencyArray));
            mapping.Add(7, new SingleAndArrayPair(MapiPropertyTypeType.ApplicationTime, MapiPropertyTypeType.ApplicationTimeArray));
            mapping.Add(0xB, new SingleAndArrayPair(MapiPropertyTypeType.Boolean));
            mapping.Add(0x14, new SingleAndArrayPair(MapiPropertyTypeType.Long, MapiPropertyTypeType.LongArray));
            mapping.Add(0x1E, new SingleAndArrayPair(MapiPropertyTypeType.String, MapiPropertyTypeType.StringArray));
            mapping.Add(0x1F, new SingleAndArrayPair(MapiPropertyTypeType.String, MapiPropertyTypeType.StringArray));
            mapping.Add(0x40, new SingleAndArrayPair(MapiPropertyTypeType.SystemTime, MapiPropertyTypeType.SystemTimeArray));
            mapping.Add(0x48, new SingleAndArrayPair(MapiPropertyTypeType.CLSID, MapiPropertyTypeType.CLSIDArray));
            mapping.Add(0x102, new SingleAndArrayPair(MapiPropertyTypeType.Binary, MapiPropertyTypeType.BinaryArray));
        }

        /// <summary>
        /// constructor needed for XML Serialization
        /// </summary>
        public PathToExtendedFieldType() { }

        /// <summary>
        /// Returns true if the prop tag type passed in represents an array type
        /// </summary>
        /// <param name="propTagType">Property tag type</param>
        /// <returns>True if array type</returns>
        /// 
        private static bool IsArrayType(ushort propTagType)
        {
            return (propTagType & 0xF000) !=0;
        }

        /// <summary>
        /// Extracts the raw type from the prop tag.  Will be the same for single and multivalued types
        /// </summary>
        /// <param name="propTagType">Type to examine</param>
        /// <returns>Raw type</returns>
        /// 
        private static ushort ExtractTypeFromArrayType(ushort propTagType)
        {
            return (ushort)(propTagType & 0x0FFF);
        }
            
        
        /// <summary>
        /// Converts from a full property tag to the corresponding MapiPropertyTypeType schema value.
        /// </summary>
        /// <param name="fullPropertyTag">Full proptag including type part</param>
        /// <returns>MapiPropertyTypeType</returns>
        /// 
        public static MapiPropertyTypeType GetMapiPropertyType(int fullPropertyTag)
        {
            // The type is in the low word.  Mask it off
            //
            ushort type = (ushort)(fullPropertyTag & 0xFFFF);
            ushort rawType = ExtractTypeFromArrayType(type);

            SingleAndArrayPair pair;
            if (!mapping.TryGetValue(rawType, out pair))
            {
                throw new ArgumentException("Unsupported property type: " + type);
            }

            if (IsArrayType(type))
            {
                if (pair.ArrayValueType.HasValue)
                {
                    return pair.ArrayValueType.Value;
                }
                else
                {
                    throw new ArgumentException("No array type provided for type: " + type);
                }
            }
            else
            {
                return pair.SingleValueType;
            }
        }

        /// <summary>
        /// Creates a prop tag extended field uri (proxy)
        /// </summary>
        /// <param name="propId">16-bit Id of property tag</param>
        /// <param name="propType">property type</param>
        /// <returns>PathToExtendedFieldType proxy object</returns>
        ///
        public static PathToExtendedFieldType BuildPropertyTag(
                                                ushort propId,
                                                MapiPropertyTypeType propType)
        {
            PathToExtendedFieldType result = new PathToExtendedFieldType();
            result.PropertyTag = string.Format("0x{0:x}", propId);
            result.PropertyType = propType;
            return result;
        }

        /// <summary>
        /// Creates a GuidId extended field uri 
        /// </summary>
        /// <param name="guid">Guid representing the property set</param>
        /// <param name="propId">Property id of the named property</param>
        /// <param name="propType">Property type</param>
        /// <returns>PathToExtendedFieldType proxy object</returns>
        ///
        public static PathToExtendedFieldType BuildGuidId(
                                                    Guid guid,
                                                    int propId,
                                                    MapiPropertyTypeType propType)
        {
            PathToExtendedFieldType result = new PathToExtendedFieldType();
            result.PropertyId = propId;
            // Don’t forget to set the specified property to true for optional value 
            // types!!
            //
            result.PropertyIdSpecified = true;
            result.PropertySetId = guid.ToString("D");
            result.PropertyType = propType;
            return result;
        }

        /// <summary>
        /// Creates a GuidId extended field URI for a distinguished property set id
        /// </summary>
        /// <param name="propertySet">DistinguishedPropertySetId</param>
        /// <param name="propId">dispatch Id</param>
        /// <param name="propType">Property type</param>
        /// <returns>PathToExtendedFieldType</returns>
        /// 
        public static PathToExtendedFieldType BuildGuidId(
                          DistinguishedPropertySetType propertySet,
                          int propId,
                          MapiPropertyTypeType propType)
        {
            PathToExtendedFieldType result = new PathToExtendedFieldType();
            result.PropertyId = propId;
            result.PropertyIdSpecified = true;
            result.DistinguishedPropertySetId = propertySet;
            result.DistinguishedPropertySetIdSpecified = true;
            result.PropertyType = propType;
            return result;
        }

        /// <summary>
        /// Builds a guid/name extended property
        /// </summary>
        /// <param name="guid">Property set guid</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="propType">Property type</param>
        /// <returns>Guid/Name extended property</returns>
        /// 
        public static PathToExtendedFieldType BuildGuidName(
                                                Guid guid,
                                                string propertyName,
                                                MapiPropertyTypeType propType)
        {
            PathToExtendedFieldType result = new PathToExtendedFieldType();
            result.PropertySetId = guid.ToString("D");
            result.PropertyName = propertyName;
            result.PropertyType = propType;
            return result;
        }

        /// <summary>
        /// Build a guid/name extended property path with DisinguishedPropertySetId
        /// </summary>
        /// <param name="propertySetId">DistinguishedPropertySetId</param>
        /// <param name="propertyName">Property Name</param>
        /// <param name="propertyType">Property Type</param>
        /// <returns>PathToExtendedFieldType</returns>
        /// 
        public static PathToExtendedFieldType BuildGuidName(
                             DistinguishedPropertySetType propertySetId,
                             string propertyName,
                             MapiPropertyTypeType propertyType)
        {
            PathToExtendedFieldType result = new PathToExtendedFieldType();
            result.DistinguishedPropertySetId = propertySetId;
            result.DistinguishedPropertySetIdSpecified = true;
            result.PropertyName = propertyName;
            result.PropertyType = propertyType;
            return result;
        }

        /// <summary>
        /// Nested class for holding MapiPropertyTypeType values that are related
        /// </summary>
        private class SingleAndArrayPair
        {
            private MapiPropertyTypeType singleValue;
            private MapiPropertyTypeType? arrayValue;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="singleValue">Type for single valued items</param>
            /// <param name="arrayValue">OPTIONAL type for multi-valued items.  There is no bool[] for instance</param>
            /// 
            public SingleAndArrayPair(MapiPropertyTypeType singleValue, MapiPropertyTypeType arrayValue)
            {
                this.singleValue = singleValue;
                this.arrayValue = arrayValue;
            }

            /// <summary>
            /// Constructor to use for single valued items only
            /// </summary>
            /// <param name="singleValue">Type for single valued items</param>
            /// 
            public SingleAndArrayPair(MapiPropertyTypeType singleValue)
            {
                this.singleValue = singleValue;
                this.arrayValue = null;
            }

            /// <summary>
            /// Accessor for the single value type
            /// </summary>
            public MapiPropertyTypeType SingleValueType
            {
                get { return this.singleValue; }
            }

            /// <summary>
            /// Accessor for the array value type
            /// </summary>
            public MapiPropertyTypeType? ArrayValueType
            {
                get { return this.arrayValue; }
            }
        }
    }
}
