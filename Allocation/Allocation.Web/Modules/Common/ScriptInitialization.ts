/// <reference path="../Common/Helpers/LanguageList.ts" />

namespace Allocation.ScriptInitialization {
    Q.Config.responsiveDialogs = true;
    Q.Config.rootNamespaces.push('Allocation');
    Serenity.EntityDialog.defaultLanguageList = LanguageList.getValue;
}