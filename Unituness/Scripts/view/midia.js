function midiacontroller($scope, $http) {
    var token = jQuery("#usertoken").val();
    var id = window.location.pathname.toString().getRotaID();

    $scope.midia = {};

    $scope.init = function () {
        $http({
            method: 'POST',
            url: "/api/midia/iniciarcompra",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: token, arg: id })
        }).success(function (data, status) {
            $scope.midia = data[0];
            $scope.podePagar = data[1];
            $scope.credito = data[2];
        });
    };

    $scope.comprar = function () {
        $http({
            method: 'POST',
            url: "/api/mida/comprar",
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