
namespace Allocation.Administration {
    export interface TenantsRow {
        TenantId?: number;
        TenantName?: string;
    }

    export namespace TenantsRow {
        export const idProperty = 'TenantId';
        export const nameProperty = 'TenantName';
        export const localTextPrefix = 'Administration.Tenants';

        export namespace Fields {
            export declare const TenantId;
            export declare const TenantName;
        }

        [
            'TenantId',
            'TenantName'
        ].forEach(x => (<any>Fields)[x] = x);
    }
}