
namespace Allocation.Allot.Forms
{
    using Serenity.ComponentModel;
    using System;

    [FormScript("Allot.DeclarationData")]
    [BasedOnRow(typeof(Entities.DeclarationDataRow))]
    public class DeclarationDataForm
    {
        public String ApplicationUnit { get; set; }
        public String MasterAwb { get; set; }
        public String SubAwb { get; set; }
        public Int32 Amount { get; set; }
        public Double Weight { get; set; }
        public String Description { get; set; }
        public Boolean IsChecked { get; set; }
    }
}