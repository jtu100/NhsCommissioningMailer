//  Inside Microsoft Exchange 2007 Web Services
//  ProxyHelpers
//
//  Copyright (c) 2007 David Sterling, Ben Spain, Mark Taylor, Huw Upshall, Michael Mainer.  
//  All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace ProxyHelpers.EWS
{
    public static class TimeZoneHelpers
    {
        // on Win9x machines this would be 
        // "Software\Microsoft\Windows\CurrentVersion\Time Zones" 
        // (Listing B-12)
        private const string _regTzCollection =
            @"Software\Microsoft\Windows NT\CurrentVersion\Time Zones";

        /// <summary>
        /// Attempts to access the registry and load in all TZ information from the 
        /// local OS.  Returns a TimeZoneRegistryInformation[] of those timezones,
        /// which can then be used to create TimeZone structures by the 
        /// other methods in this helper class. (Listing B-14)
        /// </summary>
        /// <returns>Array of TimeZoneRegistryInformation</returns>
        /// 
        public static TimeZoneRegistryInformation[] GetSupportedTimeZones()
        {
            using (RegistryKey tzCollection = Registry.LocalMachine.OpenSubKey(
                _regTzCollection))
            {
                // Create and Populate the Display Names collection
                //
                string[] tzSubKeyNames = tzCollection.GetSubKeyNames();
                List<TimeZoneRegistryInformation> timeZones =
                    new List<TimeZoneRegistryInformation>(
                        tzCollection.GetSubKeyNames().Length);

                foreach (string keyName in tzSubKeyNames)
                {
                    string tzDisplayName =
                        (string)tzCollection.OpenSubKey(keyName).GetValue("Display");
                    string standardName =
                        (string)tzCollection.OpenSubKey(keyName).GetValue("Std");
                    string daylightName =
                        (string)tzCollection.OpenSubKey(keyName).GetValue("Dlt");

                    // The description of time zone information is stored in the TZI
                    // regkey value as a byte array. For more information on the 
                    // format of this regkey value see
                    //   http://msdn2.microsoft.com/en-us/library/ms725481.aspx
                    //
                    Byte[] tzByteArray =
                        (Byte[])tzCollection.OpenSubKey(keyName).GetValue("TZI");

                    Int32 baseOffsetInMinutes =
                        System.BitConverter.ToInt32(tzByteArray, 0);
                    Int32 stdOffsetInMinutes =
                        System.BitConverter.ToInt32(tzByteArray, 4);
                    Int32 dltOffsetInMinutes =
                        System.BitConverter.ToInt32(tzByteArray, 8);

                    timeZones.Add(new TimeZoneRegistryInformation(
                        keyName,
                        tzDisplayName,
                        standardName,
                        daylightName,
                        tzByteArray,
                        baseOffsetInMinutes,
                        stdOffsetInMinutes,
                        dltOffsetInMinutes));
                }

                timeZones.Sort();
                return timeZones.ToArray();
            }
        }

        /// <summary>
        /// Performs a lookup against the local Windows Registry for time zone 
        /// information defined by the key name provided by the caller (Listing B-15)
        /// </summary>
        /// <param name="timeZoneKeyName">Name of the registry key to look for</param>
        /// <returns>Time Zone information structure</returns>
        /// 
        public static TimeZoneRegistryInformation
            GetTimeZoneRegistryInformationFromKeyName(string timeZoneKeyName)
        {
            TimeZoneRegistryInformation[] allTZRIs = GetSupportedTimeZones();
            foreach (TimeZoneRegistryInformation tzri in allTZRIs)
            {
                if (0 == String.Compare(tzri.KeyName, timeZoneKeyName))
                {
                    return tzri;
                }
            }
            throw new Exception("The specified keyname " + timeZoneKeyName +
                " could not be found.");
        }

        /// <summary>
        /// Performs a lookup against the local Windows Registry for the information
        /// about the the time zone of the current OS (specifcally, the current active
        /// Thread) (Listing B-16)
        /// </summary>
        /// <returns>Time Zone information structure</returns>
        /// 
        public static TimeZoneRegistryInformation GetCurrentOSTimeZone()
        {
            foreach (TimeZoneRegistryInformation tzri in GetSupportedTimeZones())
            {
                if ((0 == String.Compare(
                        tzri.StandardName,
                        TimeZone.CurrentTimeZone.StandardName)) &&
                    (0 == String.Compare(
                        tzri.DaylightName,
                        TimeZone.CurrentTimeZone.DaylightName)))
                {
                    return tzri;
                }
            }
            throw new Exception("Unable to find a TimeZone definition in the " +
                "registry that matches the current OS TimeZone.");
        }

        /// <summary>
        /// Creates a new TimeZoneType instance for the time zone defined by the 
        /// supplied time zone information (Listing B-18)
        /// </summary>
        /// <param name="timeZoneKeyName">Keyname of the time zone information</param>
        /// <returns>New TimeZoneType instance that can be set on the MeetingTimeZone 
        /// property of a CalendarItemType instance</returns>
        public static TimeZoneType
            CreateTimeZoneTypeFromRegistryInformation(string timeZoneKeyName)
        {
            return CreateTimeZoneTypeFromRegistryInformation(
                GetTimeZoneRegistryInformationFromKeyName(timeZoneKeyName));
        }

        /// <summary>
        /// Creates a new TimeZoneType instance for the time zone defined by the 
        /// supplied time zone information (Listing B-18)
        /// </summary>
        /// <param name="tzri">Time zone information in the form of a 
        /// TimeZoneRegistryInformation structure</param>
        /// <returns>New TimeZoneType instance that can be set on the MeetingTimeZone 
        /// property of a CalendarItemType instance</returns>
        public static TimeZoneType
            CreateTimeZoneTypeFromRegistryInformation(TimeZoneRegistryInformation tzri)
        {
            // Go to the registry and read the TZI value from the timeZoneName 
            // specified
            //
            Microsoft.Win32.RegistryKey tzCollection =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(_regTzCollection);

            // Verify the supplied timeZoneName is present
            //
            bool keyMatch = false;
            foreach (string keyName in tzCollection.GetSubKeyNames())
            {
                if (0 == String.Compare(tzri.KeyName, keyName))
                {
                    keyMatch = true;
                    break;
                }
            }
            if (!keyMatch)
            {
                throw new ArgumentException("The specified time zone key \"" +
                    tzri.KeyName + "\" was not found in the registry.");
            }

            // timeZoneName supplied is valid, starts constructing our TimeZoneType
            TimeZoneType tzReturn = new TimeZoneType();
            tzReturn.TimeZoneName = tzri.KeyName;

            // The description of time zone information is stored in the TZI regkey 
            // value as a byte array. For more information on the format of this regkey
            // value see
            //   http://msdn2.microsoft.com/en-us/library/ms725481.aspx
            //
            Byte[] tzByteArray = tzri.TimeZoneByteArray;

            // Populate the t:BaseOffset portion of the t:TimeZoneType
            //
            tzReturn.BaseOffset =
                DateTimeHelpers.TimeSpanToXSDuration(new TimeSpan(0, tzri.BaseOffsetInMinutes, 0));

            // Not all timezones require Standard and Daylight transitions times 
            // (Arizona Time Zone for example does not), the TimeChangeInfoConverter
            // class will indicate this via the isValidTZChangeInfo property, if that 
            // property is false, then there is no need to set the Daylight and 
            // Standard properties of our t:TimeZoneType
            //
            TimeChangeInfoConverter stdTimeZoneInfo =
                new TimeChangeInfoConverter(tzByteArray, 12);
            if (stdTimeZoneInfo.IsValidTZChangeInfo)
            {
                tzReturn.Standard = new TimeChangeType();
                tzReturn.Standard.Offset = DateTimeHelpers.TimeSpanToXSDuration(
                    new TimeSpan(0, tzri.StandardOffsetInMinutes, 0));
                tzReturn.Standard.Item = stdTimeZoneInfo.YearlyPatternDescription;
                tzReturn.Standard.Time = stdTimeZoneInfo.Time;
                tzReturn.Standard.TimeZoneName = tzri.StandardName;
            }

            TimeChangeInfoConverter dltTimeZoneInfo =
                new TimeChangeInfoConverter(tzByteArray, 28);
            if (dltTimeZoneInfo.IsValidTZChangeInfo)
            {
                tzReturn.Daylight = new TimeChangeType();
                tzReturn.Daylight.Offset = DateTimeHelpers.TimeSpanToXSDuration(
                    new TimeSpan(0, tzri.DaylightOffsetInMinutes, 0));
                tzReturn.Daylight.Item = dltTimeZoneInfo.YearlyPatternDescription;
                tzReturn.Daylight.Time = dltTimeZoneInfo.Time;
                tzReturn.Daylight.TimeZoneName = tzri.DaylightName;
            }

            // This return value can now be set as the MeetingTimeZone property 
            // value on a t:CalendarItem type
            //
            return tzReturn;
        }

        /// <summary>
        /// Creates a new SerializableTimeZone instance for the time zone defined by
        /// the supplied time zone information (Listing B-19)
        /// </summary>
        /// <param name="timeZoneKeyName">Keyname of the time zone information</param>
        /// <returns>New SerializableTimeZone instance that can be set on the TimeZone
        /// property of a GetUserAvailabilityRequestType instance</returns>
        /// 
        public static SerializableTimeZone
            CreateSerializableTimeZoneFromRegistryInformation(string timeZoneKeyName)
        {
            return CreateSerializableTimeZoneFromRegistryInformation(
                GetTimeZoneRegistryInformationFromKeyName(timeZoneKeyName));
        }

        /// <summary>
        /// Creates a new SerializableTimeZone instance for the time zone defined by 
        /// the supplied time zone information (Listing B-19)
        /// </summary>
        /// <param name="tzri">Time zone information in the form of a 
        /// TimeZoneRegistryInformation structure</param>
        /// <returns>New SerializableTimeZone instance that can be set on the TimeZone
        /// property of a GetUserAvailabilityRequestType instance</returns>
        /// 
        public static SerializableTimeZone
            CreateSerializableTimeZoneFromRegistryInformation(
                TimeZoneRegistryInformation tzri)
        {
            // Go to the registry and read the TZI value from the timeZoneName 
            // specified
            //
            Microsoft.Win32.RegistryKey tzCollection =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(_regTzCollection);

            // Verify the supplied timeZoneName is present
            //
            bool keyMatch = false;
            foreach (string keyName in tzCollection.GetSubKeyNames())
            {
                if (0 == String.Compare(tzri.KeyName, keyName))
                {
                    keyMatch = true;
                    break;
                }
            }
            if (!keyMatch)
            {
                throw new ArgumentException("The specified time zone key \"" +
                    tzri.KeyName + "\" was not found in the registry.");
            }

            // timeZoneName supplied is valid, starts constructing our TimeZoneType
            SerializableTimeZone tzReturn = new SerializableTimeZone();

            // The description of time zone information is stored in the TZI regkey 
            // value as a byte array. For more information on the format of this regkey
            // value see
            //   http://msdn2.microsoft.com/en-us/library/ms725481.aspx
            //
            Byte[] tzByteArray = tzri.TimeZoneByteArray;

            // Populate the t:Bias portion of the t:SerializableTimeZone, which is just
            // the BaseOffset in minutes
            //
            tzReturn.Bias = tzri.BaseOffsetInMinutes;

            TimeChangeInfoConverter stdTimeZoneInfo =
                new TimeChangeInfoConverter(tzByteArray, 12);
            tzReturn.StandardTime = new SerializableTimeZoneTime();
            tzReturn.DaylightTime = new SerializableTimeZoneTime();

            // Unlike the TimeZoneType (used for CalendarItems), the 
            // SerializableTimeZone type requires full descriptons of STD and DLT
            // structures, even if that Time Zone doesn't adhere to Daylight Savings
            // Time.  To accomidate this, we will set the Bias of each structure to
            // zero, and supply TZ info that defines DST as happening as little 
            // as possible.
            //
            if (stdTimeZoneInfo.IsValidTZChangeInfo)
            {
                tzReturn.StandardTime.Bias = tzri.StandardOffsetInMinutes;
                tzReturn.StandardTime.Time = stdTimeZoneInfo.TimeAsXsTime;
                tzReturn.StandardTime.DayOrder = stdTimeZoneInfo.DayOrder;
                tzReturn.StandardTime.Month = stdTimeZoneInfo.Month;
                tzReturn.StandardTime.DayOfWeek = stdTimeZoneInfo.DayOfWeek;
            }
            else
            {
                tzReturn.StandardTime.Bias = 0;
                tzReturn.StandardTime.Time = "00:00:00";
                tzReturn.StandardTime.DayOrder = 1;
                tzReturn.StandardTime.Month = 1;
                tzReturn.StandardTime.DayOfWeek = "Sunday";
            }

            TimeChangeInfoConverter dltTimeZoneInfo =
                new TimeChangeInfoConverter(tzByteArray, 28);
            if (dltTimeZoneInfo.IsValidTZChangeInfo)
            {
                tzReturn.DaylightTime.Bias = tzri.DaylightOffsetInMinutes;
                tzReturn.DaylightTime.Time = dltTimeZoneInfo.TimeAsXsTime;
                tzReturn.DaylightTime.DayOrder = dltTimeZoneInfo.DayOrder;
                tzReturn.DaylightTime.Month = dltTimeZoneInfo.Month;
                tzReturn.DaylightTime.DayOfWeek = dltTimeZoneInfo.DayOfWeek;
            }
            else
            {
                tzReturn.DaylightTime.Bias = 0;
                tzReturn.DaylightTime.Time = "23:59:59";
                tzReturn.DaylightTime.DayOrder = 5;
                tzReturn.DaylightTime.Month = 12;
                tzReturn.DaylightTime.DayOfWeek = "Sunday";
            }

            // This return value can now be set as the TimeZone property value
            // on a GetUserAvailabilityRequest instance
            //
            return tzReturn;
        }

    }
}
