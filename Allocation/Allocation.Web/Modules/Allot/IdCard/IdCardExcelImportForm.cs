

namespace Allocation.Allot.Forms
{
    using System;
    using System.ComponentModel;
    using Allocation.Allot.Entities;
    using Serenity.ComponentModel;

    [FormScript("Allot.IdCardExcelImport")]
    public class IdCardExcelImportForm
    {
        [DisplayName("验证接口"), LookupEditor(typeof(ApiRow)), Required]
        public int ApiId { get; set; }
        [DisplayName("Excel文件"), FileUploadEditor, Required]
        public String FileName { get; set; }
    }
}