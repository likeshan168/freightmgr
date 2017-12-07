namespace Allocation.Allot  {

    @Serenity.Decorators.registerFormatter()
    export class IsCheckedFormatter implements Slick.Formatter {
        format(ctx: Slick.FormatterContext) {
            //return "<span class='shipper-symbol shipper-" +
            //    Q.replaceAll((ctx.value || '').toString(), ' ', '') +
            //    "'>" + Q.htmlEncode(ctx.value) + '</span>';
            if (ctx.value) {
                return "<span class='allot-checked'/>";
            } else {
                return "<span class='allot-nochecked'/>";
            }
        }
    }
}