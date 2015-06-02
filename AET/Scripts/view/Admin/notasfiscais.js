
function NotasFiscaisCtrl($scope, $http) {

    $scope.filtro = new Object();
    $scope.notas = [];

    $scope.init = function () {

        var mes = $.cookie("mesListNotasFicais");
        if (typeof mes != 'undefined')
            $scope.filtro.Mes = mes;

        var ano = $.cookie("anoListNotasFiscais");
        if (typeof ano != 'undefined')
            $scope.filtro.ano = ano;

        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/notasfiscais/Get',
            data: { mes: $scope.filtro.Mes, ano: $scope.filtro.Ano }
        }).success(function (data, status) {
            
            $scope.notas = data;

            console.log(data);

            $('#tableAssociados').paginartable({ tamPagina: 10 });
        });
    }

    $scope.filtrar = function () {

        $('#btnFiltrar').text("Filtrando...");
        $('#btnFiltrar').prop("disabled", true);

        $.cookie("anoListNotasFiscais", $scope.filtro.Ano);
        $.cookie("mesListNotasFicais", $scope.filtro.Mes);

        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/notasfiscais/Get',
            data: { mes: $scope.filtro.Mes, ano: $scope.filtro.Ano }
        }).success(function (data, status) {
            $scope.notas = data;

            console.log(data);

            $('#btnFiltrar').text("Filtrar");
            $('#btnFiltrar').prop("disabled", false);

            $('#tableAssociados').paginartable({ tamPagina: 10 });
        });
    }
}

