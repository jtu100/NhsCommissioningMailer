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
using System.IO;
using System.Text.RegularExpressions;

namespace ProxyHelpers.EWS
{
    public static class DateTimeHelpers
    {
        private static XmlSerializer dateTimeSerializer = new XmlSerializer(typeof(DateTime));
        
        /// <summary>
        /// Converts a DateTime into an xs:dateTime string (Listing B-1)
        /// </summary>
        /// <param name="dateTime">DateTime to convert</param>
        /// <returns>xs:dateTime string</returns>
        /// 
        public static string DateTimeToXsDateTime(DateTime dateTime)
        {
            return XmlConvert.ToString(
                    dateTime,
                    XmlDateTimeSerializationMode.RoundtripKind);
        }

        /// <summary>
        /// Converts an xs:dateTime string into a DateTime instance (Listing B-2)
        /// </summary>
        /// <param name="xsDateTime">xs:DateTime string to convert</param>
        /// <returns>DateTime instance</returns>
        /// 
        public static DateTime XsDateTimeToDateTime(string xsDateTime)
        {
            string xsDateTimeWithTags = String.Format("<dateTime>{0}</dateTime", xsDateTime);
            return (DateTime)dateTimeSerializer.Deserialize(new StringReader(xsDateTimeWithTags));
        }

        /// <summary>
        /// Converts a DateTime instance into an xs:date string (Listing B-3)
        /// </summary>
        /// <param name="dateTime">DateTime to convert</param>
        /// <returns>xs:date string</returns>
        /// 
        public static string DateTimeToXsDate(DateTime dateTime)
        {
            return XmlConvert.ToString(dateTime.Date, "yyyy-MMM-ddzzzzzz");
        }

        /// <summary>
        /// Converts an xs:Date string into a DateTime (Listing B-4)
        /// </summary>
        /// <param name="xsDateString">xs:Date string</param>
        /// <returns>DateTime instance</returns>
        /// 
        public static DateTime XsDateToDateTime(string xsDateString)
        {
            string xsDateWithTags = String.Format("<dateTime>{0}</dateTime>", xsDateString);
            return (DateTime)dateTimeSerializer.Deserialize(new StringReader(xsDateWithTags));
        }

        /// <summary>
        /// Converts a DateTime into an xs:time string (Listing B-5)
        /// </summary>
        /// <param name="dateTime">DateTime to convert</param>
        /// <returns>xs:time string</returns>
        /// 
        public static string DateTimeToXsTime(DateTime dateTime)
        {
            return XmlConvert.ToString(dateTime, "HH:mm:sszzzzzz");
        }

        /// <summary>
        /// Converts an xs:time string into a DateTime (Listing B-6)
        /// </summary>
        /// <param name="xsTimeString">xs:string string to convert</param>
        /// <returns>DateTime instance</returns>
        /// 
        public static DateTime XsTimeToDateTime(string xsTimeString)
        {
            string xsTimeStringWithTags = String.Format("<dateTime>{0}</dateTime>", xsTimeString);
            return (DateTime)dateTimeSerializer.Deserialize(new StringReader(xsTimeStringWithTags));
        }

        /// <summary>
        /// Takes a System.TimeSpan structure and converts it into an xs:duration string as
        /// defined by the W3C Recommendation "XML Schema Part 2: Datatypes Second Edition", 
        /// http://www.w3.org/TR/xmlschema-2/#duration
        /// Listing B-7
        /// </summary>
        /// <param name="timeSpan">TimeSpan structure to convert</param>
        /// <returns>xs:duration formatted string</returns>
        ///
        public static string TimeSpanToXSDuration(TimeSpan timeSpan)
        {
            // The TimeSpan structure does not have a Year or Month property, therefore we
            // wouldn't be able to return an xs:duration string  containing nY or nM components.
            // This should not be an issue as the xs:duration format places no restriction on the
            // size of the nD component. 
            // 
            return String.Format("{0}P{1}DT{2}H{3}M{4}S",
                  (timeSpan.TotalSeconds < 0) ? "-" : "",  // {0} optional '-' offset
                  Math.Abs(timeSpan.Days),
                  Math.Abs(timeSpan.Hours),
                  Math.Abs(timeSpan.Minutes),
                  Math.Abs(timeSpan.Seconds) + "." + Math.Abs(timeSpan.Milliseconds));
        }

        /// <summary>
        /// Takes an xs:duration string as defined by the W3 Consortiums
        /// Recommendation "XML Schema Part 2: Datatypes Second Edition", 
        /// http://www.w3.org/TR/xmlschema-2/#duration, and converts it
        /// into a System.TimeSpan strcuture
        /// Listing B-8
        /// </summary>
        /// <remarks>
        /// This method uses the following approximations:
        ///     1 year = 365 days
        ///     1 month = 30 days
        /// 
        /// Additonally, it only allows for three decimal points of
        /// seconds precision.
        /// </remarks>
        /// <param name="xsDuration">xs:duration string to convert</param>
        /// <returns>System.TimeSpan structure</returns>
        public static TimeSpan XSDurationToTimeSpan(string xsDuration)
        {
            System.Text.RegularExpressions.Regex timeSpanParser =
                new System.Text.RegularExpressions.Regex(
                    "(?<pos>-)?" +
                    "P" +
                    "((?<year>[0-9]+)Y)?" +
                    "((?<month>[0-9]+)M)?" +
                    "((?<day>[0-9]+)D)?" +
                    "T" +
                    "((?<hour>[0-9]+)H)?" +
                    "((?<minute>[0-9]+)M)?" +
                    "((?<seconds>[0-9]+)(\\.(?<precision>[0-9]+))?S)?");

            Match m = timeSpanParser.Match(xsDuration);
            if (!m.Success)
            {
                throw new ArgumentException("the specified xsDuration argument " +
                    "could not be parsed.");
            }
            string token = m.Result("${pos}");
            bool negative = false;
            if (!String.IsNullOrEmpty(token))
            {
                negative = true;
            }

            // Year
            token = m.Result("${year}");
            int year = 0;
            if (!String.IsNullOrEmpty(token))
                year = Int32.Parse(token);

            // Month
            token = m.Result("${month}");
            int month = 0;
            if (!String.IsNullOrEmpty(token))
                month = Int32.Parse(token);

            // Day
            token = m.Result("${day}");
            int day = 0;
            if (!String.IsNullOrEmpty(token))
                day = Int32.Parse(token);

            // Hour
            token = m.Result("${hour}");
            int hour = 0;
            if (!String.IsNullOrEmpty(token))
                hour = Int32.Parse(token);

            // Minute
            token = m.Result("${minute}");
            int minute = 0;
            if (!String.IsNullOrEmpty(token))
                minute = Int32.Parse(token);

            // Seconds
            token = m.Result("${seconds}");
            int seconds = 0;
            if (!String.IsNullOrEmpty(token))
                seconds = Int32.Parse(token);

            int milliseconds = 0;
            token = m.Result("${precision}");

            // Only allowed 3 digits of precision
            if (token.Length > 3)
                token = token.Substring(0, 3);

            if (!String.IsNullOrEmpty(token))
                milliseconds = System.Convert.ToInt32(Double.Parse(token));

            // Apply conversions of year and months to days.
            // Year = 365 days
            // Month = 30 days
            day = day + (year * 365) + (month * 30);
            TimeSpan retval = new TimeSpan(day, hour, minute, seconds, milliseconds);

            if (negative)
            {
                retval = (-retval);
            }

            return retval;
        }
    }
}
