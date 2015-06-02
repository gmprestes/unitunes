function AlterarSenhaCtrl($scope, $http) {

    $scope.atual = "";
    $scope.nova = "";
    $scope.msgerro = '';

    $scope.alterar = function () {

        if (stringIsNullOrEmpty($scope.atual)) {
            $("#txtNome").addClass("has-error");
            return;
        }

        if (stringIsNullOrEmpty($scope.nova) || $scope.nova.length < 4) {
            $("#txtPass").addClass("has-error");
            alert("Sua senha deve ter ao menos 4 caracteres.");
            return;
        }

        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/login/AlterarSenha',
            data: { atual: $scope.atual, nova: $scope.nova }
        }).success(function (data, status) {
            console.log(data);
            if (data[0] == true) {
                alert('Senha alterada com sucesso. Você será redirecionada a pagina de login.');
                window.location = _baseURL + '/login?n=' + data[1];
            }
            else {
                alert('Ocorreu um erro ao alterar sua senha. Possivelmente sua senha atual esta errada.');
            }
        });
    }

}

function AlterarSenha(senha) {
    $.ajax({
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        url: _baseURL + '/request/login/AlterarSenha2?nova=' + senha
        // data: { nova: JSON.stringify(senha) }
    }).done(function (msg) {
        console.log(msg);
        alert("OK");
    });
}