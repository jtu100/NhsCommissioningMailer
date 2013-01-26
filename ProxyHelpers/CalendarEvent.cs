//  Inside Microsoft Exchange 2007 Web Services
//  ProxyHelpers
//
//  Copyright (c) 2007 David Sterling, Ben Spain, Mark Taylor, Huw Upshall, Michael Mainer.  
//  All rights reserved.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ProxyHelpers.EWS
{
    ///<summary>
    /// Proxy extension for CalendarEvent that implements IXmlSerializable.
    /// The purpose of this extension is to control the XML for the StartTime 
    /// and EndTime properties during de-serialization due to an issue in the
    /// Exchange Server where the date/time strings in these properties 
    /// incorrect contains the UTC offset of the Client Access Server
    /// processing the GetUserAvailabilityRequest. Listing 21-37
    ///</summary>
    ///<remarks>
    /// For this to work, the XmlTypeAttribute that the proxy generator places on
    /// this class in the auto-generated .cs file must be removed
    /// E.g.
    ///<code> 
    ///     [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    ///     [System.SerializableAttribute()]
    ///     [System.Diagnostics.DebuggerStepThroughAttribute()]
    ///     [System.ComponentModel.DesignerCategoryAttribute("code")]
    ///     //[System.Xml.Serialization.XmlTypeAttribute(Namespace="...")]
    ///     public partial class CalendarEvent {...}
    ///</code>   
    ///</remarks>
    public partial class CalendarEvent : IXmlSerializable
    {
        /// <summary>
        /// Empty constructor, required for partial class implementations
        /// </summary>
        public CalendarEvent()
        { }

        /// <summary>
        /// Returns an XmlSchema for the CalendarEvent that describes the
        /// XML representation of the output that is produced by the WriteXml
        /// method and consumed by the ReadXmlMethod 
        /// </summary>
        /// <returns>XmlSchema for the CalendarEvent</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            // This method must return
            //<xs:schema 
            //        id="types"
            //        elementFormDefault="qualified"
            //        version="Exchange2007" 
            //        xmlns:t=".../types"
            //        targetNamespace=".../types"
            //        xmlns:tns=".../types"
            //        xmlns:xs="http://www.w3.org/2001/XMLSchema">
            //  <xs:complexType name="CalendarEvent">
            //    <xs:sequence>
            //      <xs:element minOccurs="1" maxOccurs="1" name="StartTime"
            //                  type="xs:dateTime" />
            //      <xs:element minOccurs="1" maxOccurs="1" name="EndTime"
            //                  type="xs:dateTime" />
            //      <xs:element minOccurs="1" maxOccurs="1" name="BusyType"
            //                  type="t:LegacyFreeBusyType" />
            //      <xs:element minOccurs="0" maxOccurs="1" 
            //                  name="CalendarEventDetails"
            //                  type="t:CalendarEventDetails" />
            //    </xs:sequence>
            //  </xs:complexType>
            string xsTypes =
                "http://schemas.microsoft.com/exchange/services/2006/types";
            string xsSchema = "http://www.w3.org/2001/XMLSchema";

            XmlSchema schema = new XmlSchema();
            schema.Id = "types";
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            schema.Version = "Exchange2007";
            schema.TargetNamespace = xsTypes;

            // <xs:complexType ... >
            XmlSchemaComplexType xmlct1 = new XmlSchemaComplexType();
            schema.Items.Add(xmlct1);
            xmlct1.Name = "CalendarEvent";

            //  <xs:sequence ... >
            XmlSchemaSequence xmlsq1 = new XmlSchemaSequence();
            xmlct1.Particle = xmlsq1;

            //    <xs:element ... name="StartTime" ... />
            XmlSchemaElement xmle1 = new XmlSchemaElement();
            xmlsq1.Items.Add(xmle1);
            xmle1.Name = "StartTime";
            xmle1.MinOccurs = 1;
            xmle1.MaxOccurs = 1;
            xmle1.SchemaTypeName = new XmlQualifiedName("dateTime", xsSchema);

            //    <xs:element ... name="EndTime" ... />
            XmlSchemaElement xmle2 = new XmlSchemaElement();
            xmlsq1.Items.Add(xmle2);
            xmle2.Name = "EndTime";
            xmle2.MinOccurs = 1;
            xmle2.MaxOccurs = 1;
            xmle2.SchemaTypeName = new XmlQualifiedName("dateTime", xsSchema);

            //    <xs:element ... name="BusyType" ... />
            XmlSchemaElement xmle3 = new XmlSchemaElement();
            xmlsq1.Items.Add(xmle3);
            xmle3.Name = "BusyType";
            xmle3.MinOccurs = 1;
            xmle3.MaxOccurs = 1;
            xmle3.SchemaTypeName = new XmlQualifiedName(
                "LegacyFreeBusyType", xsTypes);

            //    <xs:element ... name="CalendarEventDetails" ... />
            XmlSchemaElement xmle4 = new XmlSchemaElement();
            xmlsq1.Items.Add(xmle4);
            xmle4.Name = "CalendarEventDetails";
            xmle4.MinOccurs = 0;
            xmle4.MaxOccurs = 1;
            xmle4.SchemaTypeName = new XmlQualifiedName(
                "CalendarEventDetails", xsTypes);

            return schema;
        }

        /// <summary>
        /// Generates a CalendarEvent object from it's XML representation
        /// </summary>
        /// <param name="reader">XmlReader posistioned at the start node
        /// of the CalendarEvent XML</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            string xsTypes =
                "http://schemas.microsoft.com/exchange/services/2006/types";

            // Store the LocalName of the element we are currently at.
            // This should be "CalendarEvent".  
            //
            // This also serves as our key to our position in the stream.
            // Once we reach an EndElement with this name, then we
            // are done with our portion of the XmlStream.
            //
            string toplevelElementName = reader.LocalName;
            reader.Read();

            while (true)
            {
                // Check to see if we are done processing
                if ((reader.NodeType == XmlNodeType.EndElement) &&
                    (0 == String.Compare(reader.LocalName, toplevelElementName)))
                {
                    // We are done, consume this EndElement and stop processing
                    reader.Read();
                    break;
                }

                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    // This means we are at the closing tag of 
                    // </CalendarEventDetails>
                    // No data here to process.
                    reader.Read();
                    continue;
                }

                // Consume StartTime or EndTime
                if ((0 == String.Compare(reader.LocalName, "StartTime")) ||
                    (0 == String.Compare(reader.LocalName, "EndTime")))
                {
                    // Store the localName, we'll need this to determine if this is
                    // the StartTime or EndTime field later.
                    string localName = reader.LocalName;

                    // StartTime or EndTime is the reason we needed to implement 
                    // IXmlSerializable, the server will always append a UTC offset 
                    // to the CalendarEvent date/time strings, and this offset
                    // can not be trusted.  The 'time' of the event is
                    // always valid if treated as Local time.
                    //
                    // We will use a Regular Expression to extract whatever was 
                    // supplied as a local time only
                    //
                    string timeValue = reader.ReadElementContentAsString();
                    System.Text.RegularExpressions.Regex regex =
                        new System.Text.RegularExpressions.Regex(
                            @"(?<untimezoned>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})",
                            System.Text.RegularExpressions.RegexOptions.Compiled);

                    string nonUTCOffsettedString =
                        regex.Match(timeValue).Result("${untimezoned}");
                    DateTime parsedDateTime = DateTime.Parse(nonUTCOffsettedString);

                    // Set to the appropriate field
                    if (0 == String.Compare(localName, "StartTime"))
                    {
                        this.startTimeField = parsedDateTime;
                    }
                    else
                    {
                        this.endTimeField = parsedDateTime;
                    }
                }

                // Consume BusyType
                if (0 == String.Compare(reader.LocalName, "BusyType"))
                {
                    string value = reader.ReadElementContentAsString();
                    this.busyTypeField =
                        (LegacyFreeBusyType)Enum.Parse(
                            typeof(LegacyFreeBusyType), value);
                }

                // Consume CalendarEventDetails, we are going to create an 
                // XmlSerializer for this to allow that type's default 
                // serialization process to occur.
                if (0 == String.Compare(reader.LocalName,
                    "CalendarEventDetails"))
                {
                    using (System.IO.StringReader strdr =
                        new System.IO.StringReader(reader.ReadOuterXml()))
                    {
                        XmlSerializer xmls =
                            new XmlSerializer(
                                typeof(CalendarEventDetails), xsTypes);

                        this.calendarEventDetailsField =
                            (CalendarEventDetails)xmls.Deserialize(strdr);
                    }
                }
            }
        }

        /// <summary>
        /// Converts a CalendarEvent object into its XML representation
        /// </summary>
        /// <param name="writer">XmlWriter positioned at the point 
        /// to which the XML for this object is to be written to</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            string xsTypes =
                "http://schemas.microsoft.com/exchange/services/2006/types";

            // Our position in the writer already includes a StartElement 
            // for our type, therefore, our job is to pickup writing to the
            // stream all of our properties.

            // Write StartTime
            writer.WriteElementString("StartTime",
                System.Xml.XmlConvert.ToString((DateTime)this.startTimeField,
                    XmlDateTimeSerializationMode.RoundtripKind));

            // Write EndTime
            writer.WriteElementString("EndTime",
                System.Xml.XmlConvert.ToString((DateTime)this.endTimeField,
                    XmlDateTimeSerializationMode.RoundtripKind));

            // Write BusyType
            writer.WriteElementString("BusyType", xsTypes,
                this.busyTypeField.ToString());

            // Write CalendarEventDetails
            XmlSerializer xmls =
                new XmlSerializer(typeof(CalendarEventDetails), xsTypes);
            xmls.Serialize(writer, this.calendarEventDetailsField);
        }
    }

}
