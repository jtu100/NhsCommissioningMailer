//  Inside Microsoft Exchange 2007 Web Services
//  TimeZoneRegistryInformation
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
    /// The TimeZoneRegistryInformation is meant to hold all of the information 
    /// about a time zone as it is stored in the Windows Registry. (Listing B-13)
    /// </summary>
    public struct TimeZoneRegistryInformation :
        IComparable<TimeZoneRegistryInformation>,
        IEquatable<TimeZoneRegistryInformation>
    {
        public string KeyName;
        public string DisplayName;
        public string StandardName;
        public string DaylightName;

        public Byte[] TimeZoneByteArray;
        public Int32 BaseOffsetInMinutes;
        public Int32 StandardOffsetInMinutes;
        public Int32 DaylightOffsetInMinutes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keyName">KeyName</param>
        /// <param name="displayName">Display Name</param>
        /// <param name="standardName">Standard time name</param>
        /// <param name="daylightName">daylight time name</param>
        /// <param name="tzByteArray">time zone byte array</param>
        /// <param name="baseOffset">Base offset</param>
        /// <param name="stdOffset">Standard time offset</param>
        /// <param name="dltOffset">Daylight time offset</param>
        /// 
        public TimeZoneRegistryInformation(
            string keyName,
            string displayName,
            string standardName,
            string daylightName,
            Byte[] tzByteArray,
            Int32 baseOffset,
            Int32 stdOffset,
            Int32 dltOffset)
        {
            this.KeyName = keyName;
            this.DisplayName = displayName;
            this.StandardName = standardName;
            this.DaylightName = daylightName;
            this.TimeZoneByteArray = tzByteArray;
            this.BaseOffsetInMinutes = baseOffset;
            this.StandardOffsetInMinutes = stdOffset;
            this.DaylightOffsetInMinutes = dltOffset;
        }

        /// <summary>
        /// Compares this instance to the passed in time zone registry info
        /// </summary>
        /// <param name="tzi">instance to compare this to</param>
        /// <returns>less than zero if this is less than passed in instance, 0 if equal, 
        /// greater than zero if this is greater than passed in instance</returns>
        /// 
        public int CompareTo(TimeZoneRegistryInformation tzi)
        {
            return String.Compare(this.DisplayName, tzi.DisplayName);
        }

        /// <summary>
        /// Returns true if the passed in instance is the same as this
        /// </summary>
        /// <param name="tzi">instance to compare to</param>
        /// <returns>Returns true if they are equal</returns>
        /// 
        public bool Equals(TimeZoneRegistryInformation tzi)
        {
            return (0 == String.Compare(this.DisplayName, tzi.DisplayName));
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>Human readable representation of this instance</returns>
        /// 
        public override string ToString()
        {
            return String.Format("'{0}' - {1}",
                this.DisplayName,
                this.KeyName);
        }
    }
}
