
// thanks a lot to Brainweber Inc. for sponsoring this feature and letting us share it with community

declare var Vue;

namespace Serenity {

    /**
     * A mixin that can be applied to a DataGrid for card view functionality
     */
    export class CardViewMixin<TItem> {

        private dataGrid: DataGrid<TItem, any>;
        private getId: (item: TItem) => any;
        private vm: any;
        private cardContainer: JQuery;

        constructor(private options: CardViewMixinOptions<TItem>) {
            var dg = this.dataGrid = options.grid;
            var idProperty = (dg as any).getIdProperty();
            var getId = this.getId = (item: TItem) => (item as any)[idProperty];

            var btnGroup = $(`
<div class="btn-group view-switch" data-toggle="buttons" style="float: right">
    <label class="btn btn-default active" title="List View">
        <i class="fa fa-th-list text-purple"></i>
        <input type="radio" name="${dg.element.attr('id')}_ViewType" value="list" checked />
    </label>
    <label class="btn btn-default" title="Card View">
        <i class="fa fa-th-large text-purple"></i>
        <input type="radio" name="${dg.element.attr('id')}_ViewType" value="card" />    
    </label>
</div>`)
                .prependTo(dg.element.find('.grid-title'));

            this.cardContainer = $('<div class="card-container" style="display: none;"><div class="card-items"></div></div>')
                .insertAfter(dg.element.children('.grid-container'));

            btnGroup.find('input').change((e) => this.switchView($(e.target).val()));

            this.resizeCardView();

            dg.element.bind('layout', () => this.resizeCardView());

            (dg.view as any).onDataChanged.subscribe(() => {
                if (this.vm)
                    this.updateCardItems();
            });

            var oldCurrentSettings = (dg as any).getCurrentSettings;
            (dg as any).getCurrentSettings = function (flag: GridPersistanceFlags) {
                var settings = oldCurrentSettings.apply(dg, [flag]);
                settings['viewType'] = btnGroup.find('input:checked').val();
                return settings;
            };

            var oldRestoreSettings = (dg as any).restoreSettings;
            (dg as any).restoreSettings = function (settings?: PersistedGridSettings, flags?: GridPersistanceFlags) {
                oldRestoreSettings.apply(dg, [settings, flags]);

                if (settings == null) {
                    var storage = this.getPersistanceStorage();
                    if (storage == null)
                        return;

                    var json = Q.trimToNull(storage.getItem(this.getPersistanceKey()));
                    if (!json)
                        return;

                    settings = JSON.parse(json);
                }
                var viewType = (settings as any).viewType || 'list';
                var currentViewType = btnGroup.find('input:checked').val() || 'list';
                if (viewType != currentViewType) {
                    btnGroup.find('input').eq(viewType == 'card' ? 1 : 0).click();
                }
            };
        }

        private switchView(viewType) {
            this.resizeCardView();
            var card = viewType == 'card';
            this.dataGrid.element.children('.card-container').toggle(card);
            this.dataGrid.element.children('.grid-container').toggle(!card);

            if (card)
                this.updateCardItems();

            (this.dataGrid as any).persistSettings();
        }

        private updateCardItems() {
            if (!this.vm) {
                this.vm = new Vue({
                    el: this.cardContainer.children()[0],
                    template: `<div class="card-items"><div class="card-item" v-for="(item, index) in items">` +
                    this.options.itemTemplate + `</div></div>`,
                    data: {
                        items: this.dataGrid.getItems()
                    },
                    methods: this.options.methods
                });
            }
            else
                this.vm.items = this.dataGrid.getItems()
        }

        private resizeCardView() {
            var gc = this.dataGrid.element.children('.grid-container');
            var width = this.dataGrid.element.width();
            var height = gc.height();
            this.dataGrid.element.children('.card-container').css({
                width: width + 'px',
                height: height + 'px'
            });
        }
    }

    export interface CardViewMixinOptions<TItem> {
        // data grid object
        grid: Serenity.DataGrid<TItem, any>;
        itemTemplate: string;
        methods?: any;
    }

    Q.initFullHeightGridPage = function (gridDiv) {
        $('body').addClass('full-height-page');
        gridDiv.addClass('responsive-height');
        var layout = function () {
            var inPageContent = gridDiv.parent().hasClass('page-content') ||
                gridDiv.parent().is('section.content');
            if (inPageContent) {
                gridDiv.css('height', '1px').css('overflow', 'hidden');
            }
            var cc = gridDiv.children('.card-container');
            if (cc.length && cc.is(':visible')) {
                cc.hide();
                gridDiv.children('.grid-container').show();
                try {
                    Q.layoutFillHeight(gridDiv);
                    gridDiv.triggerHandler('layout');
                }
                finally {
                    gridDiv.children('.grid-container').hide();
                    cc.show();
                }
            }
            else {
                Q.layoutFillHeight(gridDiv);
                if (inPageContent) {
                    gridDiv.css('overflow', '');
                }
                gridDiv.triggerHandler('layout');
            }
        };
        if ($('body').hasClass('has-layout-event')) {
            $('body').bind('layout', layout);
        }
        else if ((window as any).Metronic) {
            (window as any).Metronic.addResizeHandler(layout);
        }
        else {
            $(window).resize(layout);
        }
        layout();
    }
}

