function midiacontroller($scope, $http) {
    var token = jQuery("#usertoken").val();
    $scope.transacao = {};
    $scope.transacao.ValorTransacao = 5;
    $scope.transacao.Cartao = {};

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
            data: JSON.stringify({ token: token, arg: JSON.stringify($scope.transacao) })
        }).success(function (data, status) {
            if (data[0] == true) {
                alert("Transação realizada com sucesso. Você já pode aproveitar seus novos creditos.");
                window.location = "/pagamento/recibo/" + data[1];
            }
            else {
                alert("A transação não foi efetuada, verifique os dados informados e certifique-se que o cartão possui saldo para a transação.");
            }
        });
    }

    $scope.init();
}