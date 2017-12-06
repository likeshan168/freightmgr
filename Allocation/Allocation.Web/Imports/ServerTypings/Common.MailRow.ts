namespace Allocation.Common {
    export interface MailRow {
        MailId?: number;
        Uid?: string;
        Subject?: string;
        Body?: string;
        MailFrom?: string;
        MailTo?: string;
        ReplyTo?: string;
        Cc?: string;
        Bcc?: string;
        Priority?: MailQueuePriority;
        Status?: MailStatus;
        RetryCount?: number;
        ErrorMessage?: string;
        LockExpiration?: string;
        SentDate?: string;
        InsertUserId?: number;
        InsertDate?: string;
        InsertUser?: string;
    }

    export namespace MailRow {
        export const idProperty = 'MailId';
        export const nameProperty = 'Subject';
        export const localTextPrefix = 'Common.Mail';

        export namespace Fields {
            export declare const MailId: string;
            export declare const Uid: string;
            export declare const Subject: string;
            export declare const Body: string;
            export declare const MailFrom: string;
            export declare const MailTo: string;
            export declare const ReplyTo: string;
            export declare const Cc: string;
            export declare const Bcc: string;
            export declare const Priority: string;
            export declare const Status: string;
            export declare const RetryCount: string;
            export declare const ErrorMessage: string;
            export declare const LockExpiration: string;
            export declare const SentDate: string;
            export declare const InsertUserId: string;
            export declare const InsertDate: string;
            export declare const InsertUser: string;
        }

        [
            'MailId', 
            'Uid', 
            'Subject', 
            'Body', 
            'MailFrom', 
            'MailTo', 
            'ReplyTo', 
            'Cc', 
            'Bcc', 
            'Priority', 
            'Status', 
            'RetryCount', 
            'ErrorMessage', 
            'LockExpiration', 
            'SentDate', 
            'InsertUserId', 
            'InsertDate', 
            'InsertUser'
        ].forEach(x => (<any>Fields)[x] = x);
    }
}

