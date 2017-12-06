using Serenity;
using Serenity.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Allocation.AdvancedSamples
{
    public partial class PickerOrderDetailsEditorAttribute : CustomEditorAttribute
    {
        public const string Key = "Allocation.AdvancedSamples.PickerOrderDetailsEditor";

        public PickerOrderDetailsEditorAttribute()
            : base(Key)
        {
        }
    }
}

