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
    /// Proxy extension for Suggestion that implements IXmlSerializable.  
    /// The purpose of this extension is to control the XML for the MeetingTime 
    /// propery during de-serialization due to a problem in the Exchange Server 
    /// where the date/time string in this property incorrectly contains the UTC
    /// offset of the Client Access Server processing the 
    /// GetUserAvailabilityRequest. (Listing 21-54)
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
    ///     public partial class Suggestion {...}
    ///</code>   
    ///</remarks>
    public partial class Suggestion : IXmlSerializable
    {
        /// <summary>
        /// Empty constructor, required for partial class implementations
        /// </summary>
        public Suggestion()
        { }

        /// <summary>
        /// Returns an XmlSchema for the Suggestion that describes the
        /// XML representation of the output that is produced by the WriteXml
        /// method and consumed by the ReadXmlMethod 
        /// </summary>
        /// <returns>XmlSchema for the Suggestion</returns>
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
            // <xs:complexType name="Suggestion">
            //  <xs:sequence>
            //    <xs:element minOccurs="1" maxOccurs="1" name="MeetingTime"
            //                type="xs:dateTime" />
            //    <xs:element minOccurs="1" maxOccurs="1" name="IsWorkTime"
            //                type="xs:boolean" />
            //    <xs:element minOccurs="1" maxOccurs="1" 
            //                name="SuggestionQuality"
            //                type="t:SuggestionQuality" />
            //    <xs:element minOccurs="0" maxOccurs="1" 
            //                name="AttendeeConflictDataArray"
            //                type="t:ArrayOfAttendeeConflictData" />
            //  </xs:sequence>
            // </xs:complexType>

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
            xmlct1.Name = "Suggestion";

            //  <xs:sequence ... >
            XmlSchemaSequence xmlsq1 = new XmlSchemaSequence();
            xmlct1.Particle = xmlsq1;

            //    <xs:element ... name="MeetingTime" ... />
            XmlSchemaElement xmle1 = new XmlSchemaElement();
            xmlsq1.Items.Add(xmle1);
            xmle1.Name = "MeetingTime";
            xmle1.MinOccurs = 1;
            xmle1.MaxOccurs = 1;
            xmle1.SchemaTypeName = new XmlQualifiedName("dateTime", xsSchema);

            //    <xs:element ... name="IsWorkTime" ... />
            XmlSchemaElement xmle2 = new XmlSchemaElement();
            xmlsq1.Items.Add(xmle2);
            xmle2.Name = "IsWorkTime";
            xmle2.MinOccurs = 1;
            xmle2.MaxOccurs = 1;
            xmle2.SchemaTypeName = new XmlQualifiedName("boolean", xsSchema);

            //    <xs:element ... name="SuggestionQuality" ... />
            XmlSchemaElement xmle3 = new XmlSchemaElement();
            xmlsq1.Items.Add(xmle3);
            xmle3.Name = "SuggestionQuality";
            xmle3.MinOccurs = 1;
            xmle3.MaxOccurs = 1;
            xmle3.SchemaTypeName = new XmlQualifiedName(
                "SuggestionQuality", xsTypes);

            //    <xs:element ... name="AttendeeConflictDataArray" ... />
            XmlSchemaElement xmle4 = new XmlSchemaElement();
            xmlsq1.Items.Add(xmle4);
            xmle4.Name = "AttendeeConflictDataArray";
            xmle4.MinOccurs = 0;
            xmle4.MaxOccurs = 1;
            xmle4.SchemaTypeName = new XmlQualifiedName(
                "ArrayOfAttendeeConflictData", xsTypes);

            return schema;
        }

        /// <summary>
        /// Generates a Suggestion object from it's XML representation
        /// </summary>
        /// <param name="reader">XmlReader posistioned at the start node
        /// of the Suggestion XML</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            string xsTypes =
                "http://schemas.microsoft.com/exchange/services/2006/types";

            // Store the LocalName of the element we are currently at.
            // This should be "Suggestion".  
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
                    // This likely means we are at the closing tag of 
                    // </AttendeeConflictDataArray>
                    // No data here to process.
                    reader.Read();
                    continue;
                }

                // Consume MeetingTime
                if (0 == String.Compare(reader.LocalName, "MeetingTime"))
                {
                    // MeetingTime is the primary reason we needed to implement 
                    // IXmlSerializable, the server will always append a UTC offset
                    // to the date/time string in the MeetingTime element.  This
                    // offset can not be trusted.  The 'time' of the suggestion is
                    // always valid if treated as Local time.
                    //
                    // We will use a Regular Expression to extract whatever was
                    // supplied as a local time only
                    //
                    string meetingTimeValue = reader.ReadElementContentAsString();
                    System.Text.RegularExpressions.Regex regex =
                        new System.Text.RegularExpressions.Regex(
                            @"(?<untimezoned>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})",
                            System.Text.RegularExpressions.RegexOptions.Compiled);

                    string nonUTCOffsettingString =
                        regex.Match(meetingTimeValue).Result("${untimezoned}");
                    this.meetingTimeField = DateTime.Parse(nonUTCOffsettingString);
                }

                // Consume IsWorkTime
                if (0 == String.Compare(reader.LocalName, "IsWorkTime"))
                {
                    this.isWorkTimeField = reader.ReadElementContentAsBoolean();
                }

                // Consume SuggestionQuality
                if (0 == String.Compare(reader.LocalName, "SuggestionQuality"))
                {
                    string value = reader.ReadElementContentAsString();
                    this.suggestionQualityField =
                        (SuggestionQuality)Enum.Parse(typeof
                            (SuggestionQuality), value);
                }

                // Consume AttendeeConflictDataArray
                if (0 == String.Compare(reader.LocalName,
                    "AttendeeConflictDataArray"))
                {
                    // Unfortunately, the XmlSerializer can't just de-serialize an 
                    // array of items, therefore we need to look at the types of 
                    // each indivudal elements of the array and de-serialize them 
                    // based on their type.
                    XmlDocument xmld = new XmlDocument();
                    string outerXml = reader.ReadOuterXml();
                    xmld.LoadXml(outerXml);

                    if (!xmld.HasChildNodes)
                    {
                        // This an an empty AttendeeConflictDataArray, so were done.
                        this.attendeeConflictDataArrayField =
                            new AttendeeConflictData[0];
                        continue;
                    }

                    XmlNodeList attendeeConflictNodes = xmld.FirstChild.ChildNodes;
                    List<AttendeeConflictData> attendeeConflictDataList =
                        new List<AttendeeConflictData>(attendeeConflictNodes.Count);

                    foreach (XmlNode xmln in attendeeConflictNodes)
                    {
                        if (0 == String.Compare(xmln.Name,
                            "IndividualAttendeeConflictData"))
                        {
                            using (System.IO.StringReader strdr =
                                new System.IO.StringReader(xmln.OuterXml))
                            {
                                XmlSerializer xmls =
                                    new XmlSerializer(
                                        typeof(IndividualAttendeeConflictData),
                                        xsTypes);

                                attendeeConflictDataList.Add(
                                    (IndividualAttendeeConflictData)
                                    xmls.Deserialize(strdr));
                            }
                        }
                        if (0 == String.Compare(xmln.Name,
                            "GroupAttendeeConflictData"))
                        {
                            using (System.IO.StringReader strdr =
                                new System.IO.StringReader(xmln.OuterXml))
                            {
                                XmlSerializer xmls =
                                    new XmlSerializer(
                                        typeof(GroupAttendeeConflictData),
                                        xsTypes);

                                attendeeConflictDataList.Add(
                                    (GroupAttendeeConflictData)
                                    xmls.Deserialize(strdr));
                            }
                        }
                        if (0 == String.Compare(xmln.Name,
                            "UnknownAttendeeConflictData"))
                        {
                            using (System.IO.StringReader strdr =
                                new System.IO.StringReader(xmln.OuterXml))
                            {
                                XmlSerializer xmls =
                                    new XmlSerializer(
                                        typeof(UnknownAttendeeConflictData),
                                        xsTypes);

                                attendeeConflictDataList.Add(
                                    (UnknownAttendeeConflictData)
                                    xmls.Deserialize(strdr));
                            }
                        }
                        if (0 == String.Compare(xmln.Name,
                            "TooBigGroupAttendeeConflictData"))
                        {
                            using (System.IO.StringReader strdr =
                                new System.IO.StringReader(xmln.OuterXml))
                            {
                                XmlSerializer xmls =
                                    new XmlSerializer(
                                        typeof(TooBigGroupAttendeeConflictData),
                                        xsTypes);

                                attendeeConflictDataList.Add(
                                    (TooBigGroupAttendeeConflictData)
                                    xmls.Deserialize(strdr));
                            }
                        }
                    }

                    // Convert our list of AttendeeConflictData to an array
                    this.attendeeConflictDataArrayField =
                        attendeeConflictDataList.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts a Suggestion object into its XML representation
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

            // Write Meeting Time
            writer.WriteElementString("MeetingTime",
                System.Xml.XmlConvert.ToString(
                    (DateTime)this.meetingTimeField,
                    XmlDateTimeSerializationMode.RoundtripKind));

            // Write IsWorkTime
            writer.WriteElementString("IsWorkTime", xsTypes,
                this.isWorkTimeField.ToString());

            // Write Suggestion Quality
            writer.WriteElementString("SuggestionQuality", xsTypes,
                this.suggestionQualityField.ToString());

            // Write AttendeeConflictDataArray
            XmlSerializer xmls = new XmlSerializer(
                typeof(AttendeeConflictData[]), xsTypes);
            xmls.Serialize(writer, this.attendeeConflictDataArrayField);
        }
    }

}
