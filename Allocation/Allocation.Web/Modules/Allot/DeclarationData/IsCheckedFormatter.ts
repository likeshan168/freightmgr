namespace Allocation.Allot {

    @Serenity.Decorators.registerFormatter()
    export class IsCheckedFormatter implements Slick.Formatter {
        format(ctx: Slick.FormatterContext) {
            //return "<span class='shipper-symbol shipper-" +
            //    Q.replaceAll((ctx.value || '').toString(), ' ', '') +
            //    "'>" + Q.htmlEncode(ctx.value) + '</span>';
            if (ctx.value === 2) {
                return "<span title='已收货' class='allot-checked'/>";
            } else if (ctx.value === 1) {
                return "<span title='未收货' class='allot-nochecked'/>";
            } else if (ctx.value === 3) {
                return "<span title='溢装收货' class='allot-overchecked'/>";
            }
        }
    }
}