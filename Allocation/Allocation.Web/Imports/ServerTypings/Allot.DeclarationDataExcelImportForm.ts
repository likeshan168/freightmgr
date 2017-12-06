namespace Allocation.Allot {
    export class DeclarationDataExcelImportForm extends Serenity.PrefixedContext {
        static formKey = 'Allot.DeclarationDataExcelImport';

    }

    export interface DeclarationDataExcelImportForm {
        FileName: Serenity.ImageUploadEditor;
    }

    [['FileName', () => Serenity.ImageUploadEditor]].forEach(x => Object.defineProperty(DeclarationDataExcelImportForm.prototype, <string>x[0], { get: function () { return this.w(x[0], (x[1] as any)()); }, enumerable: true, configurable: true }));
}

