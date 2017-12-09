using Serenity.Extensibility;
using System.ComponentModel;

namespace Allocation.Allot
{

    [NestedPermissionKeys]
    [DisplayName("Allot")]
    public class AllotPermissionKeys
    {
        [DisplayName("DeclarationData")]
        public class DeclarationData
        {
            public const string Delete = "Allot:DeclarationData:Delete";
            public const string Modify = "Allot:DeclarationData:Modify";
            public const string View = "Allot:DeclarationData:View";
        }
    }
}