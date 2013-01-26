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
    /// Partial class extension of the WeeklyRecurrencePatternType (Listing 9-31)
    /// </summary>
    public partial class WeeklyRecurrencePatternType
    {
        /// <summary>
        /// Default constructor (needed for XML serialization).
        /// </summary>
        public WeeklyRecurrencePatternType() { }

        /// <summary>
        /// Convenience constructor.
        /// </summary>
        /// <param name="interval"> Frequency of the weekly recurring pattern</param>
        /// <param name="oneOrMoreDaysOfTheWeek"> Group of DayOfWeek values</param>
        ///
        public WeeklyRecurrencePatternType(
                            int interval,
                            params DayOfWeek[] oneOrMoreDaysOfTheWeek)
        {
            foreach (DayOfWeek dayOfWeek in oneOrMoreDaysOfTheWeek)
            {
                this.DaysOfWeek = this.DaysOfWeek + dayOfWeek.ToString() + " ";
            }
            this.Interval = interval;
        }
    }
}
