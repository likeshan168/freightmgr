using Serenity;
using Serenity.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Allocation.Allot
{
    public partial class FlightFormatterAttribute : CustomFormatterAttribute
    {
        public const string Key = "Allocation.Allot.FlightFormatter";

        public FlightFormatterAttribute()
            : base(Key)
        {
        }
    }
}

