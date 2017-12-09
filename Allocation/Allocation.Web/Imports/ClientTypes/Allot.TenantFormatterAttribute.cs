using Serenity;
using Serenity.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Allocation.Allot
{
    public partial class TenantFormatterAttribute : CustomFormatterAttribute
    {
        public const string Key = "Allocation.Allot.TenantFormatter";

        public TenantFormatterAttribute()
            : base(Key)
        {
        }
    }
}

