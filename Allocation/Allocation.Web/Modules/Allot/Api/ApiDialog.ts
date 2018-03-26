
namespace Allocation.Allot {

    @Serenity.Decorators.registerClass()
    @Serenity.Decorators.responsive()
    @Serenity.Decorators.panel()
    export class ApiDialog extends Serenity.EntityDialog<ApiRow, any> {
        protected getFormKey() { return ApiForm.formKey; }
        protected getIdProperty() { return ApiRow.idProperty; }
        protected getLocalTextPrefix() { return ApiRow.localTextPrefix; }
        protected getNameProperty() { return ApiRow.nameProperty; }
        protected getService() { return ApiService.baseUrl; }

        protected form: ApiForm;
        constructor() {
            super();
            this.form = new ApiForm(this.idPrefix);
        }
    }
}