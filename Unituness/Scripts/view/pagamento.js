function pagamentocontroller($scope, $http) {
    var token = jQuery("#usertoken").val();
    $scope.transacao = {};
    $scope.transacao.ValorTransacao = 5;
    $scope.transacao.Cartao = {};

    console.log(token);

    $scope.init = function () {
        $http({
            method: 'POST',
            url: "/api/pagamento/novatransacao",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: token })
        }).success(function (data, status) {
            $scope.transacao = data;
        });
    };

    $scope.pagar = function () {
        $http({
            method: 'POST',
            url: "/api/pagamento/realizartransacao",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ arg: JSON.stringify($scope.transacao) })
        }).success(function (data, status) {
            $scope.transacao = data;
        });
    }

    $scope.init();
}