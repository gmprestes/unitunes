function CadastroCtrl($scope, $http) {

    $scope.nome = "";
    $scope.cpf = "";
    $scope.pass = "";
    $scope.pass2 = "";


    $scope.msgerro = '';

    $scope.cadastro = function () {

        $('#btnCad').prop('disabled', true);
        $('#btnCad').text('Processando...');

        console.log($scope.pass == $scope.pass2);

        if (stringIsNullOrEmpty($scope.nome)) {
            $("#txtNome").addClass("has-error");
            $scope.msgerro = 'Email não informado';
        }
        else if (stringIsNullOrEmpty($scope.cpf) || $scope.cpf.length != 11) {
            $("#txtCPF").addClass("has-error");
            $scope.msgerro = 'CPF Invalido';
        }
        else if (stringIsNullOrEmpty($scope.pass) || $scope.pass.length < 6) {
            $("#txtPass").addClass("has-error");
            $scope.msgerro = 'Informe uma senha com no minimo 6 caracteres';
        }
        else if ($scope.pass != $scope.pass2) {
            $("#txtPass2").addClass("has-error");
            $scope.msgerro = 'As senhas não conferem';
        }

        if (!stringIsNullOrEmpty($scope.msgerro)) {

            $('#msgErro').html($scope.msgerro);
            $('#msgErro').show();

            $('#btnCad').prop('disabled', false);
            $('#btnCad').text('Cadastrar');

            return;
        }

        $('#msgErro').hide();
        //$('#msgErro').hide();


        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/cadastro/EfetuaCadastro',
            data: { usuario: $scope.nome, cpf: $scope.cpf, pass: $scope.pass }
        }).success(function (data, status) {
            console.log(data);
            if (data[0] == true) {

                $('#btnCad').prop('disabled', false);
                $('#btnCad').text('Cadastrar');

                alert(data[1]);
                window.location = _baseURL + '/login.aspx?n=' + $scope.nome;
            }
            else {
                $('#msgErro').html(data[1]);
                $('#msgErro').show();

                $('#btnCad').prop('disabled', false);
                $('#btnCad').text('Cadastrar');
            }
        });
    }

}