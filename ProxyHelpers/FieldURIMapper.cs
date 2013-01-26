//  Inside Microsoft Exchange 2007 Web Services
//  ProxyHelpers
//
//  Copyright (c) 2007 David Sterling, Ben Spain, Mark Taylor, Huw Upshall, Michael Mainer.  
//  All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;

namespace ProxyHelpers.EWS
{
    /// <summary>
    /// Holds maps of UnindexedFieldURI and DictionaryUri values to xml strings for those values
    /// (Listing 18-10)
    /// </summary>
    public static class FieldURIMapper
    {
        private static Dictionary<string, UnindexedFieldURIType> stringToFieldURIMap =
            new Dictionary<string, UnindexedFieldURIType>();
        private static Dictionary<UnindexedFieldURIType, string> fieldURIToStringMap = 
            new Dictionary<UnindexedFieldURIType,string>();
        private static Dictionary<string, DictionaryURIType> stringToIndexedMap =
            new Dictionary<string, DictionaryURIType>();
        private static Dictionary<DictionaryURIType, string> indexedToStringMap =
            new Dictionary<DictionaryURIType, string>();

        /// <summary>
        /// Static constructor used to fill our maps.  This isn't cheap, so we only want to do it once.
        /// </summary>
        static FieldURIMapper()
        {
            CreateMapping<UnindexedFieldURIType>(stringToFieldURIMap, fieldURIToStringMap);
            CreateMapping<DictionaryURIType>(stringToIndexedMap, indexedToStringMap);
        }

        /// <summary>
        /// Generic method for reflecting the XmlEnum attributes on a given type's fields and mapping
        /// those to the actual field value.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="stringToMap">Dictionary holding string to T mappings</param>
        /// <param name="toStringMap">Dictionary holding T to string mappings</param>
        /// 
        private static void CreateMapping<T>(Dictionary<string, T> stringToMap, Dictionary<T, string> toStringMap)
        {
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                // we are only interested in the fields with XmlEnum attributes
                //
                object[] attributes = field.GetCustomAttributes(typeof(XmlEnumAttribute), false);
                if (attributes.Length == 1)
                {
                    XmlEnumAttribute enumAttribute = attributes[0] as XmlEnumAttribute;
                    T enumValue = (T)field.GetRawConstantValue();
                    // Add to both the lookup and reverse map
                    //
                    stringToMap.Add(enumAttribute.Name, enumValue);
                    toStringMap.Add(enumValue, enumAttribute.Name);
                }
            }
        }

        /// <summary>
        /// Returns the xml string value for a given UnindexedFieldUri
        /// </summary>
        /// <param name="fieldUri">FieldURI to look up</param>
        /// <returns>xml string for the enum value</returns>
        /// 
        public static string FieldUriToString(UnindexedFieldURIType fieldUri)
        {
            return fieldURIToStringMap[fieldUri];
        }

        /// <summary>
        /// Returns the UnindexedFieldURI value for a given xml string.  Note that if it is not found,
        /// an exception will be thrown.
        /// </summary>
        /// <param name="fieldUriString">string to look up</param>
        /// <returns>UnindexedFieldURIValue</returns>
        /// 
        public static UnindexedFieldURIType StringToFieldUri(string fieldUriString)
        {
            return stringToFieldURIMap[fieldUriString];
        }

        /// <summary>
        /// Returns the xml string for a given DictionaryURI value
        /// </summary>
        /// <param name="fieldUri">Dictionary URI value</param>
        /// <returns>xml string</returns>
        /// 
        public static string IndexedFieldUriToString(DictionaryURIType fieldUri)
        {
            return indexedToStringMap[fieldUri];
        }

        /// <summary>
        /// Returns the DictionaryURI value for a given xml string.  Note that if it is not found, an
        /// exception will be thrown by the dictionary.
        /// </summary>
        /// <param name="fieldUriString">String to look up</param>
        /// <returns>DictionaryURI</returns>
        /// 
        public static DictionaryURIType StringToIndexedFieldUri(string fieldUriString)
        {
            return stringToIndexedMap[fieldUriString];
        }


        /// <summary>
        /// Tries to get unindexed and indexed field URIs from a response message
        /// </summary>
        /// <param name="responseMessage">Response message to examine</param>
        /// <param name="paths">OUT property paths</param>
        /// <returns>True if it found property paths</returns>
        /// 
        public static bool TryExtractFieldURIsFromResponseMessage(
                                        ResponseMessageType responseMessage,
                                        out BasePathToElementType[] paths)
        {
            List<BasePathToElementType> pathsList = new List<BasePathToElementType>();

            if ((responseMessage.ResponseCode == ResponseCodeType.NoError) ||
                (responseMessage.MessageXml == null)) 
            {
                paths = null;
                return false;
            }
            XmlElement[] elements = responseMessage.MessageXml.Any;
            foreach (XmlElement element in elements)
            {
                switch (element.LocalName)
                { 
                    case "FieldURI":
                        XmlAttribute fieldURIAttribute = element.Attributes["FieldURI"] as XmlAttribute;
                        UnindexedFieldURIType fieldURI = StringToFieldUri(fieldURIAttribute.Value);
                        PathToUnindexedFieldType propertyPath = new PathToUnindexedFieldType();
                        propertyPath.FieldURI = fieldURI;
                        pathsList.Add(propertyPath);
                        break;

                    case "IndexedFieldURI":
                        XmlAttribute indexedFieldURIAttribute = element.Attributes["FieldURI"] as XmlAttribute;
                        XmlAttribute fieldIndexAttribute = element.Attributes["FieldIndex"] as XmlAttribute;
                        PathToIndexedFieldType indexedPropertyPath = new PathToIndexedFieldType();
                        indexedPropertyPath.FieldURI = StringToIndexedFieldUri(indexedFieldURIAttribute.Value);
                        indexedPropertyPath.FieldIndex = fieldIndexAttribute.Value;
                        pathsList.Add(indexedPropertyPath);
                        break;

                    case "ExtendedFieldURI":
                        // Homework...
                        //
                        throw new NotImplementedException();
                }
            }

            paths = (pathsList.Count == 0) ? null : pathsList.ToArray();
            return paths != null;
        }
    }
}
