namespace Allocation.Allot {
    export interface ApiRow {
        Id?: number;
        Title?: string;
        ApiUrl?: string;
        UserName?: string;
        Password?: string;
        AppKey?: string;
        Price?: number;
        IsEnabled?: boolean;
        ReuseCount?: number;
    }

    export namespace ApiRow {
        export const idProperty = 'Id';
        export const nameProperty = 'Title';
        export const localTextPrefix = 'Allot.Api';
        export const lookupKey = 'Allot.Api';

        export function getLookup(): Q.Lookup<ApiRow> {
            return Q.getLookup<ApiRow>('Allot.Api');
        }

        export namespace Fields {
            export declare const Id: string;
            export declare const Title: string;
            export declare const ApiUrl: string;
            export declare const UserName: string;
            export declare const Password: string;
            export declare const AppKey: string;
            export declare const Price: string;
            export declare const IsEnabled: string;
            export declare const ReuseCount: string;
        }

        [
            'Id', 
            'Title', 
            'ApiUrl', 
            'UserName', 
            'Password', 
            'AppKey', 
            'Price', 
            'IsEnabled', 
            'ReuseCount'
        ].forEach(x => (<any>Fields)[x] = x);
    }
}

