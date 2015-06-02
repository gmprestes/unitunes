function CtrlArquivos($scope, $http) {

    var path = window.location.pathname.toString().getPath();
    var id = window.location.pathname.toString().getRotaID();
    $scope.init = function () {
        // Call the fileupload widget and set some parameters
        $('#fileUploadArquivos').fileupload({
            url: _baseURL + '/fileupload?path=' + path + '&id=' + id,
            dataType: 'json',
            pasteZone: null,
            done: function (e, data) {
                $('#barraProgresso, #progress').hide();
                if (data.result == true)
                    $scope.init();
                else
                    alert(data.result);
            },
            progressall: function (e, data) {
                $('#barraProgresso, #progress').show();

                // Update the progress bar while files are being uploaded
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $('#progress .bar').css('width', progress + '%');

            }
        });

        var httpRequest = $http({
            method: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: _baseURL + '/arquivosajax/getarquivos',
            data: JSON.stringify({ path: path, id: id })
        }).success(function (data, status) {
            $scope.arquivos = data;

            if ($scope.arquivos.length == 0)
                $("#contentFiles").hide();
            else
                $("#contentFiles").show();
        });
    }

    $scope.enviarEmailChange = function (id) {
        var httpRequest = $http({
            method: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: _baseURL + '/arquivosajax/anexaremail',
            data: JSON.stringify({ id: id })
        }).success(function (data, status) {
        });
    };

    $scope.excluirArquivo = function (id) {
        var httpRequest = $http({
            method: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: _baseURL + '/arquivosajax/excluir',
            data: JSON.stringify({ id: id })
        }).success(function (data, status) {
            $scope.init();
        });
    };

    $scope.enviarAoDrive = function (id) {
        var httpRequest = $http({
            method: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: _baseURL + '/arquivosajax/EnviarAoDrive',
            data: JSON.stringify({ id: id })
        }).success(function (data, status) {
            alert(data);
            $scope.init();
        });
    };

    $scope.visualizarNoDrive = function (url) {
        window.open(url);
    };

    $scope.baixarArquivo = function (id) {
        downloadFile(guid(), _baseURL + '/BaixarArquivo.ashx?Cod=' + id + '&type=File');
    };

    $scope.init();
}