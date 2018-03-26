
namespace Allocation.Allot {

    @Serenity.Decorators.registerClass()
    export class IdCardGrid extends Serenity.EntityGrid<IdCardRow, any> {
        protected getColumnsKey() { return 'Allot.IdCard'; }
        protected getDialogType() { return IdCardDialog; }
        protected getIdProperty() { return IdCardRow.idProperty; }
        protected getLocalTextPrefix() { return IdCardRow.localTextPrefix; }
        protected getService() { return IdCardService.baseUrl; }

        constructor(container: JQuery) {
            super(container);
        }

        protected getSlickOptions(): Slick.GridOptions {
            let opt = super.getSlickOptions();
            opt.enableTextSelectionOnCells = true;
            opt.selectedCellCssClass = "slick-row-selected";
            opt.enableCellNavigation = true;
            return opt;
        }

        protected createSlickGrid(): Slick.Grid {
            let grid = super.createSlickGrid();
            grid.setSelectionModel(new Slick.RowSelectionModel());
            return grid;
        }

        protected getButtons() {
            let buttons = super.getButtons();

            buttons.push({
                title: "导入Excel",
                cssClass: 'makeimport',
                onClick: () => {
                    var dialog = new IdCardExcelImportDialog();
                    dialog.element.on('dialogclose', () => {
                        this.refresh();
                        dialog = null;
                    });
                    dialog.dialogOpen();
                },
                separator: true
            });
            buttons.push(Common.ExcelExportHelper.createToolButton({
                title: '导出Excel',
                hint: "导出Excel",
                cssClass: "makeexport",
                grid: this,
                service: IdCardService.baseUrl + '/ListExcel',
                onViewSubmit: () => this.onViewSubmit(),
                separator: true
            }));
            buttons.push({
                title: '下载模板',
                cssClass: 'export-xlsx-button',
                onClick: () => {
                    Q.postToService({
                        service: IdCardService.baseUrl + '/ExcelTemplate',
                        request: null,
                        target: '_blank'
                    });
                },
                separator: true
            });
            return buttons;
        }
    }
}