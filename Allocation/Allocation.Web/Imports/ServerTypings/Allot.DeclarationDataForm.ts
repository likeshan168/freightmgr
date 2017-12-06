namespace Allocation.Allot {
    export class DeclarationDataForm extends Serenity.PrefixedContext {
        static formKey = 'Allot.DeclarationData';

    }

    export interface DeclarationDataForm {
        ApplicationUnit: Serenity.StringEditor;
        MasterAwb: Serenity.StringEditor;
        SubAwb: Serenity.StringEditor;
        Amount: Serenity.IntegerEditor;
        Weight: Serenity.DecimalEditor;
        Description: Serenity.StringEditor;
        IsChecked: Serenity.BooleanEditor;
    }

    [['ApplicationUnit', () => Serenity.StringEditor], ['MasterAwb', () => Serenity.StringEditor], ['SubAwb', () => Serenity.StringEditor], ['Amount', () => Serenity.IntegerEditor], ['Weight', () => Serenity.DecimalEditor], ['Description', () => Serenity.StringEditor], ['IsChecked', () => Serenity.BooleanEditor]].forEach(x => Object.defineProperty(DeclarationDataForm.prototype, <string>x[0], { get: function () { return this.w(x[0], (x[1] as any)()); }, enumerable: true, configurable: true }));
}

