namespace Allocation.Common {
    export class MailForm extends Serenity.PrefixedContext {
        static formKey = 'Common.Mail';

    }

    export interface MailForm {
        Uid: Serenity.StringEditor;
        Subject: Serenity.StringEditor;
        Body: Serenity.HtmlNoteContentEditor;
        MailFrom: Serenity.StringEditor;
        MailTo: Serenity.StringEditor;
        ReplyTo: Serenity.StringEditor;
        Cc: Serenity.StringEditor;
        Bcc: Serenity.StringEditor;
        Priority: Serenity.EnumEditor;
        Status: Serenity.EnumEditor;
        RetryCount: Serenity.IntegerEditor;
        LockExpiration: Serenity.DateTimeEditor;
        ErrorMessage: Serenity.StringEditor;
        SentDate: Serenity.DateTimeEditor;
        InsertUser: Serenity.IntegerEditor;
        InsertDate: Serenity.DateTimeEditor;
    }

    [['Uid', () => Serenity.StringEditor], ['Subject', () => Serenity.StringEditor], ['Body', () => Serenity.HtmlNoteContentEditor], ['MailFrom', () => Serenity.StringEditor], ['MailTo', () => Serenity.StringEditor], ['ReplyTo', () => Serenity.StringEditor], ['Cc', () => Serenity.StringEditor], ['Bcc', () => Serenity.StringEditor], ['Priority', () => Serenity.EnumEditor], ['Status', () => Serenity.EnumEditor], ['RetryCount', () => Serenity.IntegerEditor], ['LockExpiration', () => Serenity.DateTimeEditor], ['ErrorMessage', () => Serenity.StringEditor], ['SentDate', () => Serenity.DateTimeEditor], ['InsertUser', () => Serenity.IntegerEditor], ['InsertDate', () => Serenity.DateTimeEditor]].forEach(x => Object.defineProperty(MailForm.prototype, <string>x[0], { get: function () { return this.w(x[0], (x[1] as any)()); }, enumerable: true, configurable: true }));
}

