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
using System.Xml.Serialization;
using System.Xml.Schema;

namespace ProxyHelpers.EWS
{
    ///<summary>
    /// Proxy extension for TimeChangeType that implements IXmlSerializable.  
    /// The purpose of this extension is to control the XML for the Time 
    /// property during serialization due to an issue with the .NET 
    /// Framework not respecting DateTimeKind during serialization of xs:time- 
    /// based elements. (Listing 9-54)
    ///</summary>
    ///<remarks>
    /// For this to work, the XmlTypeAttribute that the proxy generator places on this 
    /// class in the auto-generated .cs file must be removed
    /// E.g.
    ///<code> 
    ///     [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "…")]
    ///     [System.SerializableAttribute()]
    ///     [System.Diagnostics.DebuggerStepThroughAttribute()]
    ///     [System.ComponentModel.DesignerCategoryAttribute("code")]
    ///     //[System.Xml.Serialization.XmlTypeAttribute(Namespace="…")]
    ///     public partial class TimeChangeType {…}
    ///</code>
    ///</remarks>
    public partial class TimeChangeType : IXmlSerializable
    {
        /// <summary>
        /// Empty constructor, required for partial class implementations
        /// </summary>
        public TimeChangeType()
        { }

        /// <summary> 
        /// Returns an XmlSchema for the TimeChnageType that describes the
        /// XML representation of the output that is produced by the WriteXml
        /// method and consumed by the ReadXmlMethod 
        /// </summary>
        /// <returns>XmlSchema for the TimeChangeType</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            // This method must return
            //<xs:schema 
            //        id="types"
            //        elementFormDefault="qualified"
            //        version="Exchange2007" 
            //        xmlns:t="http://…/types"
            //        targetNamespace="http://…/types"
            //        xmlns:tns="http://…/types"
            //        xmlns:xs="http://www.w3.org/2001/XMLSchema">
            // <xs:complexType name="TimeChangeType">
            //  <xs:sequence>
            //    <xs:element name="Offset" type="xs:duration" />
            //    <xs:group ref="t:TimeChangePatternTypes" minOccurs="0"/>
            //    <xs:element name="Time" type="xs:time" />
            //  </xs:sequence>
            //  <xs:attribute name="TimeZoneName"
            //     type="xs:string" use="optional" />
            //</xs:complexType>
            //</xs:schema>

            string xsTypes =
                "http://schemas.microsoft.com/exchange/services/2006/types";
            string xsSchema = "http://www.w3.org/2001/XMLSchema";

            XmlSchema schema = new XmlSchema();
            schema.Id = "types";
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            schema.Version = "Exchange2007";
            schema.TargetNamespace = xsTypes;

            // <xs:complexType … >
            XmlSchemaComplexType xmlct1 = new XmlSchemaComplexType();
            schema.Items.Add(xmlct1);
            xmlct1.Name = "TimeChangeType";

            //  <xs:sequence … >
            XmlSchemaSequence xmlsq1 = new XmlSchemaSequence();
            xmlct1.Particle = xmlsq1;

            //   <xs:element … />
            XmlSchemaElement xmle1 = new XmlSchemaElement();
            xmlsq1.Items.Add(xmle1);
            xmle1.Name = "Offset";
            xmle1.SchemaTypeName = new XmlQualifiedName("duration", xsSchema);

            //   <xs:group … />
            XmlSchemaGroupRef xmlgr1 = new XmlSchemaGroupRef();
            xmlsq1.Items.Add(xmlgr1);
            xmlgr1.RefName =
                new XmlQualifiedName("TimeChangePatternTypes", xsTypes);
            xmlgr1.MinOccurs = 0;

            //   <xs:element … />
            XmlSchemaElement xmle2 = new XmlSchemaElement();
            xmlsq1.Items.Add(xmle2);
            xmle2.Name = "Time";
            xmle2.SchemaTypeName = new XmlQualifiedName("time", xsSchema);

            // <xs:attribute … />
            XmlSchemaAttribute xmla1 = new XmlSchemaAttribute();
            xmlct1.Attributes.Add(xmla1);
            xmla1.Name = "TimeZoneName";
            xmla1.Use = XmlSchemaUse.Optional;
            xmla1.SchemaTypeName = new XmlQualifiedName("string", xsSchema);

            return schema;
        }
        /// <summary>
        /// Generates a TimeChangeType object from it's XML representation
        /// </summary>
        /// <param name="reader">XmlReader posistioned at the start node
        /// of the TimeChangeType XML
        /// </param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            // Store the LocalName of the element we are currently at.
            // This should be either "Standard" or "Daylight".
            //
            // This also serves as our key to our position in the stream.
            // Once we reach an EndElement with this name, then we are done
            // with our portion of the XmlStream
            //
            string ruleSetName = reader.LocalName;

            // Value that indicates if the TimeChangePattern for us is a 
            // RelativeYearlyRecurrence or not.  If not, then it must
            // be an AbsoluteDate
            //
            bool isRelativeYearlyPattern = false;
            RelativeYearlyRecurrencePatternType relativeYearlyPattern =
                new RelativeYearlyRecurrencePatternType();
            while (true)
            {
                // Check to see if we are done processing
                if ((reader.NodeType == XmlNodeType.EndElement) &&
                    (0 == String.Compare(reader.LocalName, ruleSetName)))
                {
                    // We are done, consume this EndElement and stop processing
                    reader.Read();
                    break;
                }

                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    // This means we are at the closing tag of
                    // </RelativeYearlyRecurrence>
                    // No data here to process.
                    reader.Read();
                    continue;
                }

                // Consume TimeZoneName attribute
                //  e.g. <Standard|Daylight TimeZoneName="value">
                //
                if ((0 == String.Compare(reader.LocalName, "Standard")) ||
                    (0 == String.Compare(reader.LocalName, "Daylight")))
                {
                    if (reader.AttributeCount > 0)
                    {
                        reader.MoveToAttribute("TimeZoneName");
                        this.timeZoneNameField = reader.Value;
                    }
                    // We have consumed what we needed form this element
                    reader.Read();
                }

                // Consume Offset
                //  e.g <Offset>PT0M</Offset>
                if (0 == String.Compare(reader.LocalName, "Offset"))
                {
                    string value = reader.ReadElementContentAsString();
                    this.offsetField = value;
                }

                // Consume Time
                //  e.g. <Time>02:00:00</Time>
                if (0 == String.Compare(reader.LocalName, "Time"))
                {
                    this.timeField = reader.ReadElementContentAsDateTime();
                }

                // Consume the TimeChangePattern element if it is
                // an AbsoluteDate
                //
                if (0 == String.Compare(reader.LocalName, "AbsoluteDate"))
                {
                    isRelativeYearlyPattern = false;
                    this.itemField = reader.ReadElementContentAsDateTime();
                }

                // Consume the TimeChangePattern element if it is
                // an RelativeYearlyRecurrence
                //
                if (0 == String.Compare(reader.LocalName,
                    "RelativeYearlyRecurrence"))
                {
                    isRelativeYearlyPattern = true;
                    reader.Read();
                }

                // If the pattern is relative, then the next three checks 
                // will get the DayOfWeek, DayOfWeekIndex, and Month values 
                // accordingly.
                //
                if (0 == String.Compare(reader.LocalName, "DaysOfWeek"))
                {
                    string value = reader.ReadElementContentAsString();
                    relativeYearlyPattern.DaysOfWeek = value;
                }

                if (0 == String.Compare(reader.LocalName, "DayOfWeekIndex"))
                {
                    string value = reader.ReadElementContentAsString();
                    relativeYearlyPattern.DayOfWeekIndex =
                       (DayOfWeekIndexType)Enum.Parse(
                           typeof(DayOfWeekIndexType), value);
                }

                if (0 == String.Compare(reader.LocalName, "Month"))
                {
                    string value = reader.ReadElementContentAsString();
                    relativeYearlyPattern.Month =
                        (MonthNamesType)Enum.Parse(
                            typeof(MonthNamesType), value);
                }
            }

            // Before we leave, set the .itemField to our 
            // relativeYearlyPattern if necessary
            if (isRelativeYearlyPattern)
                this.itemField = relativeYearlyPattern;
        }

        /// <summary>
        /// Converts a TimeChangeType object into its XML representation
        /// </summary>
        /// <param name="writer">XmlWriter positioned at the point 
        /// to which the XML for this object is to be written to
        /// </param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            string xsTypes =
                "http://schemas.microsoft.com/exchange/services/2006/types";

            // Our position in the writer already includes a StartElement 
            // for our type, therefore, our job is to pickup writing to the 
            // stream all of our content starting with the attributes of
            // the StartElement.

            // Write TimeZoneName attribute
            if (!String.IsNullOrEmpty(this.timeZoneNameField))
            {
                writer.WriteAttributeString("TimeZoneName",
                    this.timeZoneNameField);
            }

            // Write Offset
            writer.WriteElementString("Offset", xsTypes, this.offsetField);

            // Write TimeChangeType, which can be either a 
            // RelativeYearlyRecurrencePattern or an AbsoluteDate
            //
            if (this.Item is RelativeYearlyRecurrencePatternType)
            {
                string innerNodeValues;

                // For the RelativeYearlyRecurrencePattern portion, we will 
                // simply create an XmlSerializer to do the work for us.  We 
                // will need a buffer to hold the XML, one 4k buffer should be
                // sufficient.
                //
                using (System.IO.MemoryStream buffer = new
                    System.IO.MemoryStream(4096))
                {
                    // Create a new Serializer. The .NET Framework internally 
                    // has a 'cache' of these (each XmlSerializer instance lives 
                    // in a dynamically generated assembly), so even though we 
                    // are requesting a 'new' one, the .NET Framework is 
                    // actually reusing instances for us.
                    //
                    XmlSerializer xmls = new
                         XmlSerializer(typeof(
                             RelativeYearlyRecurrencePatternType));
                    xmls.Serialize(buffer, this.Item);

                    // Reset the buffer position, and then hookup an XmlReader
                    buffer.Seek(0, System.IO.SeekOrigin.Begin);
                    XmlTextReader xmlrdr = new XmlTextReader(buffer);
                    xmlrdr.Read();

                    // The first node should always be the XmlDeclaration, and 
                    // we do not want that
                    if (xmlrdr.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        xmlrdr.Read();

                        // The XmlSerializer likes to put in some whitespace 
                        // as well.
                        //
                        if (xmlrdr.NodeType == XmlNodeType.Whitespace)
                        {
                            xmlrdr.Read();
                        }
                    }

                    // Here is the node we are interested in, however, we can't 
                    // just take the node 'as-is'.  The reason is that the 
                    // XmlSerializer named the outer node 
                    // "RelativeYearlyRecurrencePatternType" (the name of the 
                    // type) but our request must pass this as 
                    // "RelativeYearlyRecurrence".
                    //
                    // The InnerXml however, is all valid, so we'll save that 
                    // information and handle the outer-node part ourselves.
                    //
                    innerNodeValues = xmlrdr.ReadInnerXml();
                }

                // Write out our recurrence pattern node
                //
                writer.WriteStartElement("RelativeYearlyRecurrence", xsTypes);
                writer.WriteRaw(innerNodeValues);
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteElementString("AbsoluteDate",
                    System.Xml.XmlConvert.ToString((DateTime)this.Item,
                        XmlDateTimeSerializationMode.RoundtripKind));
            }

            // Write the Time Element
            //
            // This is the primary reason for implementing IXmlSerializable.  
            // For it is here where we control the XML output of the Time 
            // element of our type to include the 'correct' offset.
            //
            string correctXsTimeString = String.Empty;
            switch (this.timeField.Kind)
            {
                case DateTimeKind.Local:
                    correctXsTimeString =
                        this.timeField.ToString(@"HH"":""mm"":""sszzzzzz");
                    break;
                case DateTimeKind.Utc:
                    correctXsTimeString =
                        this.timeField.ToString(@"HH"":""mm"":""ssZ");
                    break;
                case DateTimeKind.Unspecified:
                default:
                    correctXsTimeString =
                        this.timeField.ToString(@"HH"":""mm"":""ss");
                    break;
            }

            writer.WriteElementString("Time", xsTypes, correctXsTimeString);

            // No need to write an "EndElement", simply exit.
        }
    }

}
