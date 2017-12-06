﻿using Serenity;
using Serenity.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Allocation.AdvancedSamples
{
    public partial class DataTableEditorAttribute : CustomEditorAttribute
    {
        public const string Key = "Allocation.AdvancedSamples.DataTableEditor";

        public DataTableEditorAttribute()
            : base(Key)
        {
        }

        public String CascadeField
        {
            get { return GetOption<String>("cascadeField"); }
            set { SetOption("cascadeField", value); }
        }

        public String CascadeFrom
        {
            get { return GetOption<String>("cascadeFrom"); }
            set { SetOption("cascadeFrom", value); }
        }

        public object CascadeValue
        {
            get { return GetOption<object>("cascadeValue"); }
            set { SetOption("cascadeValue", value); }
        }
    }
}

