function pagamentocontroller($scope, $http) {
    var token = jQuery("#usertoken").val();
    $scope.transacao = {};
    $scope.transacao.ValorTransacao = 5;
    $scope.transacao.Cartao = {};
    $scope.user = {};
    $scope.user.Credito = 0;


    $scope.init = function () {
        $http({
            method: 'POST',
            url: "/api/pagamento/novatransacao",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: token })
        }).success(function (data, status) {
            $scope.transacao = data[0];
            $scope.user = data[1];
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


function pagamentorecibocontroller($scope, $http) {
    var token = jQuery("#usertoken").val();

    var id = window.location.pathname.toString().getRotaID();

    $scope.transacao = {};

    $scope.init = function () {
        $http({
            method: 'POST',
            url: "/api/pagamento/get",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: token, arg: id })
        }).success(function (data, status) {
            $scope.transacao = data;
        });
    };

    $scope.baixar = function () {
        window.print();
    }

    $scope.init();
}

function pagamentorecibovendacontroller($scope, $http) {
    var token = jQuery("#usertoken").val();
    var id = window.location.pathname.toString().getRotaID();

    $scope.transacao = {};

    $scope.init = function () {
        $http({
            method: 'POST',
            url: "/api/pagamento/getvenda",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: token, arg: id })
        }).success(function (data, status) {
            $scope.venda = data[0];
            $scope.midia = data[1];
        });
    };

    $scope.baixar = function () {
        window.print();
    }

    $scope.init();
}

function vendacontroller($scope, $http) {
    var token = jQuery("#usertoken").val();

    $scope.vendas = [];

    $scope.init = function () {
        $http({
            method: 'POST',
            url: "/api/pagamento/allvendas",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: token })
        }).success(function (data, status) {
            $scope.vendas = data;
        });
    };

    $scope.init();
}