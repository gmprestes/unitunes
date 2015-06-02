function SemestresListCtrl($scope, $http) {
    $scope.init = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/semestre/GetAllItens'
        }).success(function (data, status) {
            $scope.semestres = data;

            $('#tableSemestres').paginartable({ tamPagina: 10 });
        });
    }

    $scope.editItem = function (id) {
        window.location = _baseURL + "/semestres/edit/" + id;
    }

    $scope.novo = function () {
        window.location = _baseURL + "/semestres/edit/";
    }

    $scope.excluirItem = function (id) {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/semestre/Excluir/' + id
        }).success(function (data, status) {
            if (data != "true")
                alert(data);
        });
    }

    $scope.init();

}

function SemestresEditCtrl($scope, $http) {

    $scope.semestre = new Object();

    $scope.init = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/semestre/Get/' + _id
        }).success(function (data, status) {
            $scope.semestre = data;
        });
    }


    $scope.saveSemestre = function () {
        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/semestre/Save',
            data: { item: JSON.stringify($scope.semestre).toString() }
        }).success(function (data, status) {
            if (data == "ERRO")
                alert("Ocorreu um erro ao salvar este item, tente novamente em alguns instantes.");
            else
                window.location = "/semestres/edit/" + data;
        });
    }

    $scope.voltar = function () {
        window.location = "/semestres/list";
    }

    $scope.init();
}