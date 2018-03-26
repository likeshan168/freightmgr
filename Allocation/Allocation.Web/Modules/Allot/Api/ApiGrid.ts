
namespace Allocation.Allot {

    @Serenity.Decorators.registerClass()
    export class ApiGrid extends Serenity.EntityGrid<ApiRow, any> {
        protected getColumnsKey() { return 'Allot.Api'; }
        protected getDialogType() { return ApiDialog; }
        protected getIdProperty() { return ApiRow.idProperty; }
        protected getLocalTextPrefix() { return ApiRow.localTextPrefix; }
        protected getService() { return ApiService.baseUrl; }

        constructor(container: JQuery) {
            super(container);
        }

        protected addButtonClick() {
            this.editItem(<ApiRow>{
                IsEnabled: true,
                ReuseCount: 10
            });
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
    }
}