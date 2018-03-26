namespace Allocation.Allot {
    export class ApiForm extends Serenity.PrefixedContext {
        static formKey = 'Allot.Api';

    }

    export interface ApiForm {
        Title: Serenity.StringEditor;
        ApiUrl: Serenity.StringEditor;
        UserName: Serenity.StringEditor;
        Password: Serenity.StringEditor;
        AppKey: Serenity.StringEditor;
        Price: Serenity.DecimalEditor;
        IsEnabled: Serenity.BooleanEditor;
        ReuseCount: Serenity.IntegerEditor;
    }

    [['Title', () => Serenity.StringEditor], ['ApiUrl', () => Serenity.StringEditor], ['UserName', () => Serenity.StringEditor], ['Password', () => Serenity.StringEditor], ['AppKey', () => Serenity.StringEditor], ['Price', () => Serenity.DecimalEditor], ['IsEnabled', () => Serenity.BooleanEditor], ['ReuseCount', () => Serenity.IntegerEditor]].forEach(x => Object.defineProperty(ApiForm.prototype, <string>x[0], { get: function () { return this.w(x[0], (x[1] as any)()); }, enumerable: true, configurable: true }));
}

