using Serenity.Navigation;
using MyPages = Allocation.Allot.Pages;

[assembly: NavigationMenu(8000, "Allot", "iconfont icon-pandian")]
[assembly: NavigationLink(8100, "Allot/Declaration Data", typeof(MyPages.DeclarationDataController), "iconfont icon-yundangenzong")]
[assembly: NavigationLink(8200, "Allot/IdCard", typeof(MyPages.IdCardController), "iconfont icon-shenfenzheng")]
[assembly: NavigationLink(8300, "Allot/Api", typeof(MyPages.ApiController), "iconfont icon-api")]
