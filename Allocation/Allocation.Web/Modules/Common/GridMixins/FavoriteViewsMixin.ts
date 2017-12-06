
// thanks a lot to Brainweber Inc. for sponsoring this feature and letting us share it with community

declare var Vue;

namespace Serenity {

    /**
     * A mixin that can be applied to a DataGrid for favorite views functionality
     */
    export class FavoriteViewsMixin<TItem> {

        private dataGrid: DataGrid<TItem, any>;
        private getId: (item: TItem) => any;
        private ul: JQuery;

        constructor(private options: FavoriteViewsMixinOptions<TItem>) {
            var dg = this.dataGrid = options.grid;
            var idProperty = (dg as any).getIdProperty();
            var getId = this.getId = (item: TItem) => (item as any)[idProperty];

            var dropdown = $(`
<div class="dropdown favorite-views" style="float: right">
  <button class="btn btn-default dropdown-toggle" type="button" id="${dg.element.attr('id')}_Favorites" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
    <i class="fa fa-star text-blue"></i> Favorite Views
    <span class="caret"></span>
  </button>
  <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
    <li class="save">
        <div>Save view</div>
        <div><input type="text"></div>
        <div><button class="btn btn-primary save-button"><i class="fa fa-floppy-o"></i> Save</button></div>
    </li>
  </ul>
</div>
`)
                .prependTo(dg.element.find(".grid-title"));

            dropdown.on('shown.bs.dropdown', e => {
                dropdown.find('input[type=text]').focus();
            });

            var viewName = dropdown.find('li.save input[type=text]');

            dropdown.find('button.save-button').click(() => {
                var name = Q.trimToNull(viewName.val());
                if (!name) {
                    Q.notifyWarning("Please write a view name!");
                    return;
                }

                var favorites = this.getFavorites();
                favorites[name] = (dg as any).getCurrentSettings();
                this.saveFavorites(favorites);
                Q.notifySuccess("Current view is saved with name \"" + name + "\".");
                viewName.val('');
                this.populateFavorites();
            });

            this.ul = dropdown.children("ul");
            this.ul.on('click', 'i.fa-trash-o', e => {
                var name = $(e.target).closest('li').children('a').text();
                var favorites = this.getFavorites();
                delete favorites[name];
                this.saveFavorites(favorites);
                this.populateFavorites();
                Q.notifyWarning("View with name \"" + name + "\" is deleted.");
            });

            this.ul.on('click', 'a', e => {
                var name = $(e.target).text();
                var favorites = this.getFavorites();
                var settings = favorites[name];
                if (settings) {
                    (dg as any).restoreSettings(settings);
                    dg.refresh();
                    Q.notifySuccess("Loaded view: " + name);
                }
            });

            this.populateFavorites();
        }

        private populateFavorites() {
            var favorites = this.getFavorites();

            var saveLI = this.ul.children('.save');
            this.ul.children().not(saveLI).remove();

            var keys = Object.keys(favorites).sort();
            if (keys.length) {
                for (var k of keys) {
                    var li = $('<li class="fav"><a href="javascript:;"></a></li>')
                        .insertBefore(saveLI)
                        .children('a')
                        .text(k)
                        .end();

                    $('<i class="fa fa-trash-o pull-right"></i>').appendTo(li);
                }

                $('<li role="separator" class="divider"></li>').insertBefore(saveLI);
            }
        }

        private saveFavorites(favorites) {
            var storage = (this.dataGrid as any).getPersistanceStorage() as Serenity.SettingStorage;
            var key = "Views:" + (this.dataGrid as any).getPersistanceKey();
            storage.setItem(key, JSON.stringify(favorites));
        }

        private getFavorites() {
            var storage = (this.dataGrid as any).getPersistanceStorage() as Serenity.SettingStorage;
            var key = "Views:" + (this.dataGrid as any).getPersistanceKey();
            return JSON.parse(storage.getItem(key) || "{}") || {};
        }
    }

    export interface FavoriteViewsMixinOptions<TItem> {
        // data grid object
        grid: Serenity.DataGrid<TItem, any>;
    }
}