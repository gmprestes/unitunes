function InstituicoesListCtrl($scope, $http) {
    $scope.init = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/instituicao/GetAllItens'
        }).success(function (data, status) {
            $scope.instituicoes = data;

            $('#tableInstituicoes').paginartable({ tamPagina: 10 });
        });
    }

    $scope.editItem = function (id) {
        window.location = _baseURL + "/instituicoes/edit/" + id;
    }

    $scope.novo = function () {
        window.location = _baseURL + "/instituicoes/edit/new";
    }

    $scope.excluirItem = function (id) {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/instituicao/Excluir/' + id
        }).success(function (data, status) {
            if (data != "true")
                alert(data);
        });
    }

    $scope.init();

}

function InstituicoesEditCtrl($scope, $http) {

    $scope.instituicao = new Object();

    $scope.init = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/instituicao/Get/' + _id
        }).success(function (data, status) {
            $scope.instituicao = data;
        });
    }


    $scope.save = function () {
        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/instituicao/Save',
            data: { item: JSON.stringify($scope.instituicao).toString() }
        }).success(function (data, status) {
            if (data == "ERRO")
                alert("Ocorreu um erro ao salvar este item, tente novamente em alguns instantes.");
            else
                window.location = "/instituicoes/edit/" + data;
        });
    }

    $scope.voltar = function () {
        window.location = "/instituicoes/list";
    }

    $scope.init();
}