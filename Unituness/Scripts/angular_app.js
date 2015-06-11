var appAngularSIGE = angular.module('app_sysadmin', []);

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

appAngularSIGE.directive('forcestring', function () {
    return {
        require: 'ngModel',
        link: function (scope, el, attr, ctrl) {
            el.bind('blur', function () {
                var value = ctrl.$modelValue;
                if (value) {
                    ctrl.$modelValue = value + '';
                    ctrl.$viewValue = value + '';
                    ctrl.$render();
                }
            });

            el.bind('focus', function () {
                var value = ctrl.$modelValue;
                if (value) {
                    ctrl.$modelValue = value + '';
                    ctrl.$viewValue = value + '';
                    ctrl.$render();
                }
            });
        }
    };
});

appAngularSIGE.config(function ($provide, $httpProvider) {
    $provide.factory('AjaxHttpInterceptor', function ($q, $injector) {
        return {
            // Não faça nada
            request: function (config) {
                return config || $q.when(config);
            },

            // Erro ao criar a request
            requestError: function (rejection) {
                console.log(rejection);
                return $q.reject(rejection);
            },

            //Não faça nada deu tudo certo
            response: function (response) {
                return response || $q.when(response);
            },

            // Aqui o bicho pega, se der erro refaz a conexão com um novo token
            responseError: function (rejection) {
                if (rejection.status == 403) {
                    try {
                        if ((rejection.data.Mensagem == 'Token informado não existe.' || rejection.data.Mensagem == 'Token invalido informado') && rejection.config.headers.Renew != 'true') {
                            var token = RenewAutenticationToken();

                            var config = rejection.config;
                            var req = {
                                method: config.method,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                url: config.url,
                                xhrFields: config.xhrFields,
                                headers: {
                                    'Authorization': 'Bearer ' + token,
                                    'Renew': 'true'
                                },
                                data: config.data
                            };

                            var $http = $injector.get('$http');
                            return $q.when($http(req));
                        }
                    }
                    catch (Erro) {
                        console.log(Erro);
                    }
                }

                // Se não for possivel refazer a conexão retorna o erro
                return $q.reject(rejection);
            }
        };
    });

    $httpProvider.interceptors.push('AjaxHttpInterceptor');

});