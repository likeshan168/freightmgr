namespace Allocation.Allot {

    @Serenity.Decorators.registerFormatter()
    export class TenantFormatter implements Slick.Formatter {
        format(ctx: Slick.FormatterContext) {
            return "<span class='allot-tenants'>" + Q.htmlEncode(ctx.value) + "</span>";
        }
    }
}