﻿namespace Allocation.Allot {

    @Serenity.Decorators.registerClass()
    export class IdCardExcelImportDialog extends Serenity.PropertyDialog<any, any> {

        private form: IdCardExcelImportForm;

        constructor() {
            super();

            this.form = new IdCardExcelImportForm(this.idPrefix);
        }

        protected getDialogTitle(): string {
            return "导入Excel";
        }

        protected getDialogButtons(): Serenity.DialogButton[] {
            return [
                {
                    text: '导入',
                    click: () => {
                        if (!this.validateBeforeSave())
                            return;
                        if (Q.isEmptyOrNull(this.form.ApiId.value)) {
                            Q.notifyError("请选择验证的接口!");
                            return;
                        }

                        if (this.form.FileName.value == null ||
                            Q.isEmptyOrNull(this.form.FileName.value.Filename)) {
                            Q.notifyError("Please select a file!");
                            return;
                        }

                        if (this.form.FileName.value.Filename.substr(this.form.FileName.value.Filename.lastIndexOf(".")) !== ".xlsx") {
                            Q.notifyError("The only support excel file is above the excel 2007!");
                            return;
                        }
                        //IdCardExcelImportService.baseUrl = "Allot/DeclarationDataImport";
                        IdCardInfoExcelImportService.ExcelImport({
                            ApiId: parseInt(this.form.ApiId.value),
                            FileName: this.form.FileName.value.Filename
                        }, response => {
                            Q.notifyInfo(
                                '新增: ' + (response.Inserted || 0) +
                                ', 更新: ' + (response.Updated || 0));

                            if (response.ErrorList != null && response.ErrorList.length > 0) {
                                Q.notifyError(response.ErrorList.join(',\r\n '));
                            }

                            this.dialogClose();
                        });
                    },
                },
                {
                    text: '取消',
                    click: () => this.dialogClose()
                }
            ];
        }
    }
}