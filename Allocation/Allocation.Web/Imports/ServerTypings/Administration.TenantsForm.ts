namespace Allocation.Administration {
    export class TenantsForm extends Serenity.PrefixedContext {
        static formKey = 'Administration.Tenants';

    }

    export interface TenantsForm {
        TenantName: Serenity.StringEditor;
    }

    [['TenantName', () => Serenity.StringEditor]].forEach(x => Object.defineProperty(TenantsForm.prototype, <string>x[0], { get: function () { return this.w(x[0], (x[1] as any)()); }, enumerable: true, configurable: true }));
}

