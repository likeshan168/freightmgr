namespace Allocation.Common {
    export enum MailQueuePriority {
        High = 1,
        Medium = 2,
        Low = 3
    }
    Serenity.Decorators.registerEnum(MailQueuePriority, 'Allocation.Common.MailQueuePriority');
}

