function LimparBasesController($scope, $http) {

    $scope.dbs = [];
    $scope.dbRegion = 'rsSIGE3';

    $scope.init = function () {
        $http({
            method: 'POST',
            url: "/api/limparbases/ListDbsExcluir",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ data: $scope.dbRegion })
        }).success(function (data, status) {
            $scope.dbs = data;
        });
    };

    $scope.excluirDbs = function () {
        $http({
            method: 'POST',
            url: "/api/limparbases/ExcluirDbs",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ data: JSON.stringify($scope.dbs), arg: $scope.dbRegion })
        }).success(function (data, status) {
            $scope.dbs = [];
            alert("Dbs Excluidos");

            $scope.init();
        });
    };

    $scope.init();
}