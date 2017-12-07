using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Serenity.ComponentModel;

namespace Allocation.Allot.Entities
{
    [EnumKey("Allocation.StateKind")]
    public enum StateKind
    {
        [Description("NoChecked")]
        NoChecked = 1,
        [Description("Checked")]
        Checked = 2,
        [Description("OverChecked")]
        OverChecked = 3
    }
}