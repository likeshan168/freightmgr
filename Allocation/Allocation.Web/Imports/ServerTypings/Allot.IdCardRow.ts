namespace Allocation.Allot {
    export interface IdCardRow {
        Id?: number;
        AuthenticationType?: string;
        Name?: string;
        CardNo?: string;
        CheckResult?: string;
        RequestDate?: string;
        RequestIp?: string;
        Price?: number;
        ReusedCount?: number;
    }

    export namespace IdCardRow {
        export const idProperty = 'Id';
        export const nameProperty = 'AuthenticationType';
        export const localTextPrefix = 'Allot.IdCard';

        export namespace Fields {
            export declare const Id: string;
            export declare const AuthenticationType: string;
            export declare const Name: string;
            export declare const CardNo: string;
            export declare const CheckResult: string;
            export declare const RequestDate: string;
            export declare const RequestIp: string;
            export declare const Price: string;
            export declare const ReusedCount: string;
        }

        [
            'Id', 
            'AuthenticationType', 
            'Name', 
            'CardNo', 
            'CheckResult', 
            'RequestDate', 
            'RequestIp', 
            'Price', 
            'ReusedCount'
        ].forEach(x => (<any>Fields)[x] = x);
    }
}

