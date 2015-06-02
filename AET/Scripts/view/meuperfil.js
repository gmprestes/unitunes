function MeuPerfilCtrl($scope, $http) {

    $scope.tipoComprovante = 'identidade';
    $scope.pessoa = new Object();
    $scope.arquivos = [];

    $scope.init = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/pessoa/Get'
        }).success(function (data, status) {
            $scope.pessoa = data;
            $scope.tipoComprovanteChange();
            $scope.buscaArquivos();

        });
    }

    $scope.salvar = function () {
        $("#bntSalvar").prop("disabled", true);
        $("#bntSalvar").val("Salvando...");
        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/pessoa/Save',
            data: { pessoa: JSON.stringify($scope.pessoa).toString() }
        }).success(function (data, status) {
            $("#bntSalvar").prop("disabled", false);
            $("#bntSalvar").val("Salvar");
            $("#msgSucesso").fadeIn(1500).delay(3000).fadeOut(500);
        });
    }

    $scope.buscaArquivos = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: _baseURL + '/request/arquivo/GetAllFilesPerfil'
        }).success(function (data, status) {
            $scope.arquivos = data;
        });
    }

    $scope.tipoComprovanteChange = function () {
        $('#fileUploadArquivos').fileupload({
            url: _baseURL + '/ajaxarquivos/savearquivoperfil?tipo=' + $scope.tipoComprovante,
            dataType: 'json',
            pasteZone: null,
            done: function (e, data) {
                if (data.result == true) {
                    $scope.buscaArquivos();
                    $('#spanMsgSucessoFile').fadeIn(1500).delay(5000).fadeOut(500);
                }
                else
                    alert(data.result);
            },
        });
    }

    $scope.baixarArquivo = function (id) {

        var url = _baseURL + '/ajaxarquivos/getfile?id=' + id;
        downloadFile(guid(), url);
    }

    $scope.init();
}