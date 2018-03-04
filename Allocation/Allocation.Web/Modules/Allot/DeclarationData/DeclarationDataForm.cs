
using Allocation.Allot.Entities;

namespace Allocation.Allot.Forms
{
    using Serenity.ComponentModel;
    using System;

    [FormScript("Allot.DeclarationData")]
    [BasedOnRow(typeof(Entities.DeclarationDataRow))]
    public class DeclarationDataForm
    {
        //public String ApplicationUnit { get; set; }
        public String Flight { get; set; }
        public String MasterAwb { get; set; }
        public String SubAwb { get; set; }
        public Int32 Amount { get; set; }
        public Double Weight { get; set; }
        public String Description { get; set; }
        public String Status { get; set; }
        public StateKind IsChecked { get; set; }
    }
}