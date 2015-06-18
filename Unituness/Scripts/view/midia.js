function midiahomecontroller($scope, $http) {
    $scope.midias = [];

    $scope.init = function () {
        $http({
            method: 'POST',
            url: "/api/midia/all",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: "" })
        }).success(function (data, status) {
            $scope.midias = data;
        });
    }

    $scope.init();
}

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
            url: "/api/midia/comprar",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: token, arg: $scope.midia.Id })
        }).success(function (data, status) {
            if (data[0] == true) {
                alert("Transação realizada com sucesso. Você já pode aproveitar sua nova midia.");
                window.location = "/pagamento/recibo_midia/" + data[1];
            }
            else {
                alert(data);
            }
        });
    }

    $scope.init();
}

function midia_list_controller($scope, $http) {
    var token = jQuery("#usertoken").val();

    $scope.midias = [];

    $scope.init = function () {
        $http({
            method: 'POST',
            url: "/api/midia/getall",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: token })
        }).success(function (data, status) {
            $scope.midias = data;
        });
    }

    $scope.delete = function (id) {

        $http({
            method: 'POST',
            url: "/api/midia/delete",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: token, arg: id })
        }).success(function (data, status) {
            console.log(data);
            if (data == "") {
                alert("Ocorreu um erro ao deletar a midia");
            }
            else {
                alert("Deletado com sucesso");
                $scope.init();
            }
        });

    }

    $scope.init();
}

function midia_edit_controller($scope, $http) {
    var token = jQuery("#usertoken").val();
    var id = window.location.pathname.toString().getRotaID();

    $scope.midia = {};

    $http({
        method: 'POST',
        url: "/api/midia/get",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ token: token, arg: id })
    }).success(function (data, status) {
        $scope.midia = data;
    });

    $scope.save = function () {
        $http({
            method: 'POST',
            url: "/api/midia/save",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ token: token, arg: JSON.stringify($scope.midia) })
        }).success(function (data, status) {
            console.log(data);
            if (data == "") {
                alert("Ocorreu um erro ao salvar a midia");
            }
            else {
                alert("Salvo com sucesso");
                window.location = "/midia/edit/" + data;
            }
        });
    }

    $scope.calcular = function () {
        var acressimo = $scope.midia.Preco * .1;
        $scope.midia.TaxaUnitunes = acressimo;
        $scope.midia.PrecoVenda = $scope.midia.Preco + $scope.midia.TaxaUnitunes;

    }
}

