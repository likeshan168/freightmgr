using Serenity;
using Serenity.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Allocation.AdvancedSamples
{
    public partial class InlineButtonsEditorAttribute : CustomEditorAttribute
    {
        public const string Key = "Allocation.AdvancedSamples.InlineButtonsEditor";

        public InlineButtonsEditorAttribute()
            : base(Key)
        {
        }
    }
}

