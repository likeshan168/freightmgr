namespace Allocation.Administration {
    export enum TwoFactorAuthType {
        Email = 1,
        SMS = 2
    }
    Serenity.Decorators.registerEnum(TwoFactorAuthType, 'Allocation.Administration.TwoFactorAuthType');
}

