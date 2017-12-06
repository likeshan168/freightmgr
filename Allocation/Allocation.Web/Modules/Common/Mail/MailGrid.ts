
namespace Allocation.Common {
    
    @Serenity.Decorators.registerClass()
    export class MailGrid extends Serenity.EntityGrid<MailRow, any> {
        protected getColumnsKey() { return 'Common.Mail'; }
        protected getDialogType() { return MailDialog; }
        protected getIdProperty() { return MailRow.idProperty; }
        protected getLocalTextPrefix() { return MailRow.localTextPrefix; }
        protected getService() { return MailService.baseUrl; }

        constructor(container: JQuery) {
            super(container);
        }

        protected getButtons(): Serenity.ToolButton[] {

            var buttons = super.getButtons();
            buttons.splice(Q.indexOf(buttons, x => x.cssClass == "add-button"), 1);
            return buttons;
        }
    }
}