
namespace Allocation.Administration {

    @Serenity.Decorators.registerClass()
    @Serenity.Decorators.responsive()
    @Serenity.Decorators.panel()
    export class TenantsDialog extends Serenity.EntityDialog<TenantsRow, any> {
        protected getFormKey() { return TenantsForm.formKey; }
        protected getIdProperty() { return TenantsRow.idProperty; }
        protected getLocalTextPrefix() { return TenantsRow.localTextPrefix; }
        protected getNameProperty() { return TenantsRow.nameProperty; }
        protected getService() { return TenantsService.baseUrl; }

        protected form = new TenantsForm(this.idPrefix);

    }
}