namespace Allocation.Allot {

    @Serenity.Decorators.registerFormatter()
    export class FlightFormatter implements Slick.Formatter {
        format(ctx: Slick.FormatterContext) {
            return "<span class='allot-flight'>" + Q.htmlEncode(ctx.value) + "</span>";
        }
    }
}