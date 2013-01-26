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
    /// Extension the the PathToIndexedFieldType to increase usability (Listing 3-2)
    /// </summary>
    public partial class PathToIndexedFieldType
    {
        /// <summary>
        /// Enum to expose only the physical address field uris
        /// </summary>
        public enum PhysicalAddressPart
        {
            Street = DictionaryURIType.contactsPhysicalAddressStreet,
            City = DictionaryURIType.contactsPhysicalAddressCity,
            State = DictionaryURIType.contactsPhysicalAddressState,
            PostalCode = DictionaryURIType.contactsPhysicalAddressPostalCode,
            Country = DictionaryURIType.contactsPhysicalAddressCountryOrRegion,
        }

        /// <summary>
        /// Constructor required for Xml serialization
        /// </summary>
        public PathToIndexedFieldType()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fieldURI">Dictionary FieldURI to use</param>
        /// <param name="fieldIndex">Key or index to use</param>
        /// 
        public PathToIndexedFieldType(DictionaryURIType fieldURI, string fieldIndex)
        {
            this.FieldURI = fieldURI;
            this.FieldIndex = fieldIndex;
        }

        
        /// <summary>
        /// Factory method for creating an email address dictionary field uri
        /// </summary>
        /// <param name="fieldIndex">Email address field index</param>
        /// <returns>Indexed Field URI</returns>
        /// 
        public static PathToIndexedFieldType CreateEmailAddress(EmailAddressKeyType fieldIndex)
        {
            return new PathToIndexedFieldType(DictionaryURIType.contactsEmailAddress, fieldIndex.ToString());
        }

        /// <summary>
        /// Factory method for creating an Im address dictionary field uri
        /// </summary>
        /// <param name="fieldIndex">Im address field index</param>
        /// <returns>Indexed field uri</returns>
        /// 
        public static PathToIndexedFieldType CreateImAddress(ImAddressKeyType fieldIndex)
        {
            return new PathToIndexedFieldType(DictionaryURIType.contactsImAddress, fieldIndex.ToString());
        }

        /// <summary>
        /// Factory method for creating a physical address part field uri
        /// </summary>
        /// <param name="part">Indicates which part of a physical address to use</param>
        /// <param name="fieldIndex">Indicates the address category (home, business, etc...)</param>
        /// <returns>Indexed field uri</returns>
        /// 
        public static PathToIndexedFieldType CreatePhysicalAddress(PhysicalAddressPart part, PhysicalAddressKeyType fieldIndex)
        {
            // Since we defined our enum to have the same values as the corresponding dictionary uris,
            // we can just cast our enum value here.
            //
            return new PathToIndexedFieldType((DictionaryURIType)part, fieldIndex.ToString());
        }

        /// <summary>
        /// Factory method for creating a phone number dictionary field uri
        /// </summary>
        /// <param name="fieldIndex">Indicates the type of phone number</param>
        /// <returns>Indexed field uri</returns>
        /// 
        public static PathToIndexedFieldType CreatePhoneNumber(PhoneNumberKeyType fieldIndex)
        {
            return new PathToIndexedFieldType(DictionaryURIType.contactsPhoneNumber, fieldIndex.ToString());
        }
    }
}
