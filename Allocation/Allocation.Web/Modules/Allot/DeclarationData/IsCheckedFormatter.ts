namespace Allocation.Allot  {

    @Serenity.Decorators.registerFormatter()
    export class IsCheckedFormatter implements Slick.Formatter {
        format(ctx: Slick.FormatterContext) {
            //return "<span class='shipper-symbol shipper-" +
            //    Q.replaceAll((ctx.value || '').toString(), ' ', '') +
            //    "'>" + Q.htmlEncode(ctx.value) + '</span>';
            if (ctx.value) {
                return "<span class='iconfont icon-checked'/>";
            } else {
                return "<span class='iconfont icon-nochecked'/>";
            }
        }
    }
}