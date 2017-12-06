
namespace Allocation.Allot.Forms
{
    using Serenity.ComponentModel;
    using Serenity.Web;
    using System;
    using System.ComponentModel;

    [FormScript("Allot.DeclarationDataExcelImport")]
    public class DeclarationDataExcelImportForm
    {
        [DisplayName("Excel文件"), FileUploadEditor, Required]
        public String FileName { get; set; }
    }
}
