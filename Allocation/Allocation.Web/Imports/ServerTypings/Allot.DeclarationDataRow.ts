namespace Allocation.Allot {
    export interface DeclarationDataRow {
        Id?: number;
        ApplicationUnit?: string;
        MasterAwb?: string;
        SubAwb?: string;
        Amount?: number;
        Weight?: number;
        Description?: string;
        Flight?: string;
        IsChecked?: StateKind;
        TenantId?: number;
        TenantName?: string;
    }

    export namespace DeclarationDataRow {
        export const idProperty = 'Id';
        export const nameProperty = 'ApplicationUnit';
        export const localTextPrefix = 'Allot.DeclarationData';
        export const lookupKey = 'Allocation.DeclarationDataMasterAwb';

        export function getLookup(): Q.Lookup<DeclarationDataRow> {
            return Q.getLookup<DeclarationDataRow>('Allocation.DeclarationDataMasterAwb');
        }

        export namespace Fields {
            export declare const Id: string;
            export declare const ApplicationUnit: string;
            export declare const MasterAwb: string;
            export declare const SubAwb: string;
            export declare const Amount: string;
            export declare const Weight: string;
            export declare const Description: string;
            export declare const Flight: string;
            export declare const IsChecked: string;
            export declare const TenantId: string;
            export declare const TenantName: string;
        }

        [
            'Id', 
            'ApplicationUnit', 
            'MasterAwb', 
            'SubAwb', 
            'Amount', 
            'Weight', 
            'Description', 
            'Flight', 
            'IsChecked', 
            'TenantId', 
            'TenantName'
        ].forEach(x => (<any>Fields)[x] = x);
    }
}

