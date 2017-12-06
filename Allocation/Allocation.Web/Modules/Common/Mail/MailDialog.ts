
namespace Allocation.Common {

    @Serenity.Decorators.registerClass()
    @Serenity.Decorators.responsive()
    export class MailDialog extends Serenity.EntityDialog<MailRow, any> {
        protected getFormKey() { return MailForm.formKey; }
        protected getIdProperty() { return MailRow.idProperty; }
        protected getLocalTextPrefix() { return MailRow.localTextPrefix; }
        protected getNameProperty() { return MailRow.nameProperty; }
        protected getService() { return MailService.baseUrl; }

        protected form = new MailForm(this.idPrefix);

        constructor() {
            super();

            this.form.Status.changeSelect2(e => {
                if (this.form.Status.value == MailStatus.InQueue.toString()) {
                    this.form.RetryCount.value = 0;
                    this.form.LockExpiration.valueAsDate = new Date();
                }
            });
        }
    }
}