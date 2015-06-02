
function RelatoriosCtrl($scope, $http) {

    $scope.baixarRelatorioAssociados = function (id) {

        console.log("hehe");

        var url = _baseURL + '/request/Relatorio/RelatorioAlunosDia/hehe';
        downloadFile(guid(), url);

    }

}

