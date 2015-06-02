var appAngularSIGE = angular.module('sige', []);

appAngularSIGE.directive('repeatDone', function () {
    return function (scope, element, attrs) {
        if (scope.$last) { // all are rendered
            eval(attrs.repeatDone);
        }
    }
});

// formata dinheiro -> é necessario a tag ng-model para que funcione
appAngularSIGE.directive('money', function () {
    'use strict';

    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, el, attr, ctrl) {

            // pra que ele não loqueie quando coloca as casas decimais
            function round(num, decimals) {
                var module = "1";
                for (var i = 0; i < decimals; i++) {
                    module += "0";
                }
                module = module.parseNumber();
                return Math.round(num * module) / module;
            }

            // faz meu parse especial que esta no helpers.js
            // Com este parse funciona com virgulas ou pontos
            ctrl.$parsers.push(function (value) {
                var decimals = 2;
                if (attr.$$element[0].attributes["ng-decimals"]) decimals = attr.$$element[0].attributes["ng-decimals"].value.parseNumber();

                if (typeof (value) != "undefined")
                    return value ? round(value.parseNumber(), decimals) : value.parseNumber();
                else
                    return 0;
            });


            el.bind('blur', function () {
                var decimals = 2;
                if (this.attributes["ng-decimals"]) decimals = this.attributes["ng-decimals"].value.parseNumber();

                var value = ctrl.$modelValue;
                if (value) {
                    // metodo formata dinheiro se encontra no helpers.js
                    ctrl.$viewValue = round(value.parseNumber(), decimals).formataDinheiro(decimals, 3);
                    ctrl.$render();
                }
            });

            el.bind('focus', function () {
                var decimals = 2;
                if (this.attributes["ng-decimals"]) decimals = this.attributes["ng-decimals"].value.parseNumber();

                var value = ctrl.$modelValue;
                if (value) {
                    // metodo formata dinheiro se encontra no helpers.js
                    ctrl.$viewValue = round(value.parseNumber(), decimals).formataDinheiro(decimals, 3);
                    ctrl.$render();
                }
            });


            // manha pro angular js formata os campos corretamente na hora
            //            el.click();
        }
    };
});