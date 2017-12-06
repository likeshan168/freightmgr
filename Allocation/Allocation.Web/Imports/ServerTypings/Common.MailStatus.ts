namespace Allocation.Common {
    export enum MailStatus {
        InQueue = 0,
        Sent = 1,
        Failed = -1
    }
    Serenity.Decorators.registerEnum(MailStatus, 'Allocation.Common.MailStatus');
}

