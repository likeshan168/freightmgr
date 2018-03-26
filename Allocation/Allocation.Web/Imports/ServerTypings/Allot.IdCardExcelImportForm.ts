namespace Allocation.Allot {
    export class IdCardExcelImportForm extends Serenity.PrefixedContext {
        static formKey = 'Allot.IdCardExcelImport';

    }

    export interface IdCardExcelImportForm {
        ApiId: Serenity.LookupEditor;
        FileName: Serenity.ImageUploadEditor;
    }

    [['ApiId', () => Serenity.LookupEditor], ['FileName', () => Serenity.ImageUploadEditor]].forEach(x => Object.defineProperty(IdCardExcelImportForm.prototype, <string>x[0], { get: function () { return this.w(x[0], (x[1] as any)()); }, enumerable: true, configurable: true }));
}

