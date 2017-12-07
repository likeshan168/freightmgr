namespace Allocation.Allot {
    import ListResponse = Serenity.ListResponse;

    @Serenity.Decorators.registerClass()
    export class DeclarationDataGrid extends Serenity.EntityGrid<DeclarationDataRow, any> {
        protected getColumnsKey() { return 'Allot.DeclarationData'; }
        protected getDialogType() { return DeclarationDataDialog; }
        protected getIdProperty() { return DeclarationDataRow.idProperty; }
        protected getLocalTextPrefix() { return DeclarationDataRow.localTextPrefix; }
        protected getService() { return DeclarationDataService.baseUrl; }

        private subawbEvent: JQueryEventObject;
        public rowSelection: Serenity.GridRowSelectionMixin;
        constructor(container: JQuery) {
            super(container);
            $("input.is-subawb").on("change", (e) => this.subawbEvent = e);
        }

        protected createToolbarExtensions() {
            super.createToolbarExtensions();
            this.rowSelection = new Serenity.GridRowSelectionMixin(this);
        }

        protected getButtons(): Serenity.ToolButton[] {

            // call base method to get list of buttons
            let buttons = super.getButtons();
            // add our export button
            buttons.push(Common.ExcelExportHelper.createToolButton({
                title: '导出Excel',
                hint: '导出Excel',
                grid: this,
                service: DeclarationDataService.baseUrl + '/ListExcel',
                onViewSubmit: () => this.onViewSubmit(),
                separator: true
            }));
            // add our import button

            buttons.push({
                title: '导入运单信息',
                cssClass: 'export-xlsx-button',
                onClick: () => {
                    // open import dialog, let it handle rest
                    var dialog = new DeclarationDataExcelImportDialog();
                    dialog.element.on('dialogclose', () => {
                        this.refresh();
                        dialog = null;
                    });
                    dialog.dialogOpen();
                },
                separator: true
            });
            buttons.push({
                title: '删除选中项',
                cssClass: 'delete-button',
                onClick: () => {
                    let selectedKeys = this.rowSelection.getSelectedKeys();
                    console.log(selectedKeys);
                    if (selectedKeys.length == 0) {
                        Q.notifyWarning("请选择需要删除的行");
                    } else {
                        Q.confirm("确认删除吗？删除之后相关的信息会受影响", () => {
                            Allot.DeclarationDataService.BatchDelete({
                                EntityIds: selectedKeys
                            }, response => {
                                this.refresh();
                            });
                        });
                    }
                },
                separator: true
            });
            return buttons;
        }
        protected getColumns() {
            let columns = super.getColumns();

            columns.unshift({
                field: 'Delete Row',
                name: '',
                format: ctx => '<a class="inline-action delete-row" title="delete">' +
                    '<i class="fa fa-trash-o text-red"></i></a>',
                width: 24,
                minWidth: 24,
                maxWidth: 24
            });

            columns.splice(0, 0, Serenity.GridRowSelectionMixin.createSelectColumn(() => this.rowSelection));

            return columns;
        }

        protected onClick(e: JQueryEventObject, row: number, cell: number) {
            super.onClick(e, row, cell);

            if (e.isDefaultPrevented())
                return;

            let item = this.itemAt(row);
            let target = $(e.target);

            // if user clicks "i" element, e.g. icon
            if (target.parent().hasClass('inline-action'))
                target = target.parent();

            if (target.hasClass('inline-action')) {
                e.preventDefault();

                if (target.hasClass('delete-row')) {
                    Q.confirm('确认删除吗？删除之后相关的信息会受影响', () => {
                        Allot.DeclarationDataService.Delete({
                            EntityId: item.Id,
                        }, response => {
                            this.refresh();
                        });
                    });
                }
            }
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

        //protected quickFilterChange(e: JQueryEventObject): void {
        //    super.quickFilterChange(e);
        //    console.log(e);
        //    if (e.target.id === "Allocation_Allot_DeclarationDataGrid0_QuickFilter_SubAwb") {
        //        let rows = this.view.getItems();
        //        console.log(rows);
        //        if (rows.length === 1) {
        //            //提交更新的操作
        //            let item = rows[0];
        //            item.IsChecked = true;
        //            DeclarationDataService.Update({
        //                EntityId: rows[0].Id,
        //                Entity: item
        //            },
        //                (): void => {
        //                    Q.notifySuccess("已经到货");
        //                });
        //        } else if (rows.length === 0) {
        //            Q.notifySuccess("溢装到货");
        //        }
        //    }
        //}

        //private onSubAwbChange(e: JQueryEventObject): void {
        //    let rows = this.getItems();
        //    console.log(rows);
        //    if (rows.length === 1) {
        //        //提交更新的操作
        //        let item = rows[0];
        //        item.IsChecked = true;
        //        DeclarationDataService.Update({
        //            EntityId: rows[0].Id,
        //            Entity: item
        //        },
        //            (): void => {
        //                Q.notifySuccess("已经到货");
        //            });
        //    } else if (rows.length === 0) {
        //        Q.notifyWarning("溢装到货");
        //    }
        //}

        protected getQuickFilters(): Serenity.QuickFilter<Serenity.Widget<any>, any>[] {
            // get quick filter list from base class, e.g. columns
            let filters = super.getQuickFilters();
            let fld = Allot.DeclarationDataRow.Fields;
            let filter = Q.first(filters, x => x.field === fld.SubAwb);
            filter.title = "扫分单号点货";
            filter.element = e => {
                e.addClass("is-subawb");
            }

            return filters;
        }


        protected onViewProcessData(response: Serenity.ListResponse<Allot.DeclarationDataRow>): ListResponse<DeclarationDataRow> {

            if (this.subawbEvent) {
                if (response.Entities.length === 1) {
                    //提交更新的操作
                    let item = response.Entities[0];
                    item.IsChecked = true;
                    DeclarationDataService.Update({
                        EntityId: item.Id,
                        Entity: item
                    },
                        (): void => {
                            Q.notifySuccess("已经到货");
                        });
                } else if (response.Entities.length === 0) {
                    Q.notifyWarning("溢装到货");
                }
            }
            this.subawbEvent = null;
            return response;
        }

        //protected getItemCssClass(item: Allot.DeclarationDataRow, index: number): string {
        //    let klass: string = "";
        //    if (item.IsChecked) {
        //        klass += " is-checked";
        //    } else {
        //        klass += " is-nochecked";
        //    }
        //    return Q.trimToNull(klass);
        //}
    }
}