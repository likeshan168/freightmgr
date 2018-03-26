
namespace Allocation.Allot {

    @Serenity.Decorators.registerClass()
    @Serenity.Decorators.responsive()
    @Serenity.Decorators.panel()
    export class IdCardDialog extends Serenity.EntityDialog<IdCardRow, any> {
        protected getFormKey() { return IdCardForm.formKey; }
        protected getIdProperty() { return IdCardRow.idProperty; }
        protected getLocalTextPrefix() { return IdCardRow.localTextPrefix; }
        protected getNameProperty() { return IdCardRow.nameProperty; }
        protected getService() { return IdCardService.baseUrl; }

        protected form = new IdCardForm(this.idPrefix);

    }
}