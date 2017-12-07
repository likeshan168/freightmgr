
namespace Allocation.Allot {

    @Serenity.Decorators.registerClass()
    @Serenity.Decorators.responsive()
    @Serenity.Decorators.panel()
    export class DeclarationDataDialog extends Serenity.EntityDialog<DeclarationDataRow, any> {
        protected getFormKey() { return DeclarationDataForm.formKey; }
        protected getIdProperty() { return DeclarationDataRow.idProperty; }
        protected getLocalTextPrefix() { return DeclarationDataRow.localTextPrefix; }
        protected getNameProperty() { return DeclarationDataRow.nameProperty; }
        protected getService() { return DeclarationDataService.baseUrl; }

        public form = new DeclarationDataForm(this.idPrefix);

    }
}