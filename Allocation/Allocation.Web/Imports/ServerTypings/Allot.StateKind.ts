namespace Allocation.Allot {
    export enum StateKind {
        NoChecked = 1,
        Checked = 2,
        OverChecked = 3
    }
    Serenity.Decorators.registerEnum(StateKind, 'Allocation.StateKind');
}

