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

        [DisplayName("Api")]
        public class Api
        {
            public const string Delete = "Allot:Api:Delete";
            public const string Modify = "Allot:Api:Modify";
            public const string View = "Allot:Api:View";
        }
    }
}