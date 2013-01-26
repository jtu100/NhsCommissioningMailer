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
    /// TimeChangeInfoConverter can take a section of the "TZI" registry value and 
    /// convert it into a RelativeYearlyRecurrencePattern and DateTime objects 
    /// that represent the date and time of the time change information for a time
    /// zone region. Listing B-17
    /// </summary>
    internal class TimeChangeInfoConverter
    {
        private RelativeYearlyRecurrencePatternType tzYearlyPatternDesc;
        internal RelativeYearlyRecurrencePatternType YearlyPatternDescription
        { get { return this.tzYearlyPatternDesc; } }

        private DateTime time;
        internal DateTime Time { get { return this.time; } }
        internal string TimeAsXsTime
        { get { return this.time.ToString(@"HH"":""mm"":""ss"); } }

        private Int16 monthVal;
        internal Int16 Month { get { return this.monthVal; } }

        private Int16 dayOfWeekIndexVal;
        internal Int16 DayOrder { get { return this.dayOfWeekIndexVal; } }

        private Int16 dayOfWeekVal;
        private string dayOfWeek;
        internal string DayOfWeek { get { return this.dayOfWeek; } }

        private bool isValidTZChangeInfo;
        internal bool IsValidTZChangeInfo
        { get { return this.isValidTZChangeInfo; } }

        /// <summary>
        /// Creates a new TimeChangeInfoConverter object from the supplied byte 
        /// array and offset.  Sets the "isValidTZChangeInfo" flag to false if the 
        /// supplied bytes indicate that no time change is necessary for this 
        /// TimeZone (e.g. TimeZone does not support Daylight Savings Time.)
        /// </summary>
        /// <param name="dateTimeByteArray">
        /// Byte array from the reg value "TZI"</param>
        /// <param name="offsetIntoArray">Offset into the array where 
        /// Standard/Daylight time change information begins.</param>
        /// stored in 0 - 3 of the TZI registery byte array.</param>
        internal TimeChangeInfoConverter(
            Byte[] dateTimeByteArray,
            int offsetIntoArray)
        {
            // Bits 0 and 1 are the year bits - irrelivant to us
            // Bits 2 and 3 are the month bits
            monthVal = System.BitConverter.ToInt16(
                dateTimeByteArray,
                2 + offsetIntoArray);

            // According to the TIME_ZONE_INFORMATION documentation, if the month
            // is set to zero that means the zone doesn't ahdere to 
            // Daylight Savings Time.
            //  http://msdn2.microsoft.com/en-us/library/ms725481.aspx
            //
            if (monthVal == 0) { this.isValidTZChangeInfo = false; return; }

            // We have a time zone that can be described in a Relative Yearly 
            // Recurrence Pattern, begin to build that now
            //
            this.tzYearlyPatternDesc = new RelativeYearlyRecurrencePatternType();

            // The Month bit that we get is 1-based, however the MonthNamesType
            // enum is zero based
            //
            this.tzYearlyPatternDesc.Month = (MonthNamesType)(monthVal - 1);

            // Bits 4 and 5 are the day of the week indicator 
            //  (0 = Sunday, 6 = Saturday)
            //
            this.dayOfWeekVal = System.BitConverter.ToInt16(
                dateTimeByteArray,
                4 + offsetIntoArray);
            this.dayOfWeek = ((DayOfWeekType)dayOfWeekVal).ToString();
            this.tzYearlyPatternDesc.DaysOfWeek = this.dayOfWeek;


            // Bits 6 and 7 represent typically represent the day, however, in
            // this case they represent the weekly index of the month (1 = First,
            // 2 = Second, ... 5 = Last), note DayOfWeekIndexType
            // is zero-based however.
            //
            this.dayOfWeekIndexVal = System.BitConverter.ToInt16(
                dateTimeByteArray,
                6 + offsetIntoArray);
            this.tzYearlyPatternDesc.DayOfWeekIndex =
                (DayOfWeekIndexType)(dayOfWeekIndexVal - 1);

            // Bits 8-9, 10-11, 12-13, and 14-15 represent the hour, minute,
            // second, and millesecond values of when the time change should occur
            //
            Int16 hourVal = System.BitConverter.ToInt16(
                dateTimeByteArray,
                8 + offsetIntoArray);
            Int16 minVal = System.BitConverter.ToInt16(
                dateTimeByteArray,
                10 + offsetIntoArray);
            Int16 secVal = System.BitConverter.ToInt16(
                dateTimeByteArray,
                12 + offsetIntoArray);

            // Although only a time element is needed for the EWS proxy, the type 
            // still requires us to use a fully qualified DateTime object,
            // therefore, what we will do here is create one that represents the 
            // time specified for today's date.  The EWS proxy code will take care
            // of only using the Time portion of the string 
            //
            // Note: This method will require an IXmlSerializable interface
            // be defined on the TimeChangeType in order to ensure that
            // the .Time property values be serialized properly.  The code
            // for this interface can be found in Chapter 9 of 
            //  "Inside Microsoft Exchange Server 2007 Web Services."
            //
            //
            this.time = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                DateTime.UtcNow.Day,
                hourVal,
                minVal,
                secVal,
                DateTimeKind.Unspecified);

            this.isValidTZChangeInfo = true;
        }
    }

}
