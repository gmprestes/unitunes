(function (factory) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(['jquery'], factory);
    } else {
        // Browser globals
        factory(jQuery);
    }
}(function ($) {
    'use strict';

    function PaginarTable(el, options) {
        var that = this,
        totalItens = 0,
        maxPagina = 1,
        defaults = {
            tamPagina: 10,
            pagInicial: 1
        };

        that.options = $.extend({}, defaults, options);

        that.pagAtual = that.options.pagInicial;

        that.element = el;
        that.el = $(el);

        that.incializar();

        that.inChange = false;

        that.el.children().not('.paginador').bind("DOMNodeInserted DOMNodeRemoved", function () {
            if (!that.inChange) {
                that.inChange = true;
                that.reload();
                that.inChange = false;
            }
        });
    }

    $.PaginarTable = PaginarTable;

    PaginarTable.prototype = {

        incializar: function () {
            var that = this;

            if (!that.el.is('table')) {
                alert('Erro ao inicializar o paginador. O elemento deve ser do tipo "table"');
                return;
            }

            that.countItens();
            that.paginar();
            that.bindPaginador();
        },

        reload: function () {
            var that = this;
            that.countItens();
            that.paginar();
            that.bindPaginador();
        },

        countItens: function () {
            var that = this,
            el = that.el,
            tbody = el.children('tbody');

            this.totalItens = tbody.children('tr').not('.paginador').not('tr[item-ignore="true"]').length;
        },

        paginar: function () {
            var that = this,
            el = that.el,
            tbody = el.children('tbody'),
            trs = tbody.children('tr').not('.paginador').not('tr[item-ignore="true"]'),
            currentPag = 0,
            itensPorPag = that.options.tamPagina,
            pagInicial = that.options.pagInicial;

            tbody.children('tr[item-ignore="true"]').hide();

            $.each(trs, function (i, item) {
                var aux = i / itensPorPag;

                if (aux > currentPag && (i % itensPorPag) == 0) {
                    currentPag = aux;
                }

                var tr = $(item).attr('data-page', currentPag + 1);

                if (currentPag + 1 != that.pagAtual)
                    tr.hide();
                else
                    tr.show();
            });

            this.maxPagina = currentPag + 1;

            if (that.pagAtual > this.maxPagina) {
                that.pagAtual = this.maxPagina;
                tbody.children('tr[data-page="' + this.maxPagina + '"]').show();
            }
        },

        bindPaginador: function () {

            var that = this,
            el = that.el,
            tbody = el.children('tbody');

            try {
                var aux = tbody.children('tr.paginador');
                if (aux != null)
                    aux.remove();
            }
            catch (ERRO) {
            }

            var html = '';
            html += '<tr class="paginador">';
            html += '<td class="pagination"colspan="' + that.countColumns() + '" >';
            html += '<ul class="pageList">';

            //                    <li ng-show="CurrentPage.Number - PagerHalfRange > 0" class="paginadorctlPaginador btn btn-mini ng-hide" ng-click="GotoStart()">&lt;&lt;</li>
            //                    <li ng-show="CurrentPage.Number - PagerHalfRange > 0" class="paginadorctlPaginador btn btn-mini ng-hide" ng-click="StepBack()">&lt;</li>
            for (var i = 0; i < this.maxPagina; i++) {
                html += ' <li class="pagertable btn btn-mini';

                if (that.pagAtual == i + 1)
                    html += ' pageActive"';
                else
                    html += '"';

                html += 'data-itempage="' + (i + 1) + '" >' + (i + 1) + '</li>';
            }

            //                    <li ng-show="CurrentPage.Number + PagerHalfRange < Pages.length" class="paginadorctlPaginador btn btn-mini ng-hide" ng-click="StepForWard()">&gt;</li>
            //                    <li ng-show="CurrentPage.Number + PagerHalfRange < Pages.length" class="paginadorctlPaginador btn btn-mini ng-hide" ng-click="GotoEnd()">&gt;&gt;</li>
            html += '</ul>';
            html += '</td>';
            html += '</tr>';

            var tfoot = $(html);

            if (el.children('tfoot').length > 0)
                tfoot = $('<tfoot>' + html + '</tfoot>');

            $(el).append(tfoot);

            tfoot.children().children().children().on("click", function () {
                that.pageChange($(this).attr('data-itempage'));
            });

            el.attr('paginartable', '');
        },

        pageChange: function (index) {
            var that = this,
            el = that.el,
            tbody = el.children('tbody');

            tbody.children('tr').not('.paginador').hide();
            tbody.children('tr[data-page=' + index + ']').show();

            tbody.children('tr.paginador').children().children().children().removeClass('pageActive');
            tbody.children('tr.paginador').children().children().children('li[data-itempage=' + index + ']').addClass('pageActive');

            that.pagAtual = parseInt(index);
        },

        countColumns: function () {
            var that = this,
            el = that.el,
            tbody = el.children('tbody');
            var colCount = 0;

            tbody.children().eq(0).children().each(function () {
                if ($(this).attr('colspan')) {
                    colCount += +$(this).attr('colspan');
                } else {
                    colCount++;
                }
            });

            return colCount;
        }
    }


    $.fn.paginartable = function (options, args) {
        var dataKey = 'paginartable';
        //        // If function invoked without argument return
        //        // instance of the first matched element:
        if (arguments.length === 0) {
            return this.first().data(dataKey);
        }

        return this.each(function () {
            var inputElement = $(this),
            instance = inputElement.data(dataKey);

            if (typeof options === 'string') {
                if (instance && typeof instance[options] === 'function') {
                    instance[options](args);
                }
            } else {
                // Se já existe uma instancia pro mesmo elemento destroi ela
                if (instance && instance.dispose) {
                    instance.dispose();
                }
                instance = new PaginarTable(this, options);
                inputElement.data(dataKey, instance);
            }
        });
    };
}));
