namespace Allocation.Allot {
    export class IdCardForm extends Serenity.PrefixedContext {
        static formKey = 'Allot.IdCard';

    }

    export interface IdCardForm {
        AuthenticationType: Serenity.StringEditor;
        Name: Serenity.StringEditor;
        CardNo: Serenity.StringEditor;
        CheckResult: Serenity.StringEditor;
        RequestDate: Serenity.DateEditor;
        RequestIp: Serenity.StringEditor;
        Price: Serenity.DecimalEditor;
    }

    [['AuthenticationType', () => Serenity.StringEditor], ['Name', () => Serenity.StringEditor], ['CardNo', () => Serenity.StringEditor], ['CheckResult', () => Serenity.StringEditor], ['RequestDate', () => Serenity.DateEditor], ['RequestIp', () => Serenity.StringEditor], ['Price', () => Serenity.DecimalEditor]].forEach(x => Object.defineProperty(IdCardForm.prototype, <string>x[0], { get: function () { return this.w(x[0], (x[1] as any)()); }, enumerable: true, configurable: true }));
}

