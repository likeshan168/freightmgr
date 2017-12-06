namespace Allocation.Common {
    export interface BackgroundTaskLogRow {
        LogId?: number;
        TaskKey?: string;
        RunAt?: string;
        Status?: number;
        Server?: string;
        Message?: string;
    }

    export namespace BackgroundTaskLogRow {
        export const idProperty = 'LogId';
        export const nameProperty = 'TaskKey';
        export const localTextPrefix = 'Common.BackgroundTask';

        export namespace Fields {
            export declare const LogId: string;
            export declare const TaskKey: string;
            export declare const RunAt: string;
            export declare const Status: string;
            export declare const Server: string;
            export declare const Message: string;
        }

        [
            'LogId', 
            'TaskKey', 
            'RunAt', 
            'Status', 
            'Server', 
            'Message'
        ].forEach(x => (<any>Fields)[x] = x);
    }
}

