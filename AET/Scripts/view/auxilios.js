function AuxiliosCtrl($scope, $http) {

    $scope.auxilio = new Object();
    $scope.arquivos = [];
    $scope.semestre = new Object();
    $scope.semestres = [];
    $scope.instituicoes = [];
    $scope.pessoa = new Object();

    $scope.tipoComprovante = 'matricula';

    $scope.init = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/pessoa/GetSemestresUser'
        }).success(function (data, status) {
            $scope.semestres = data;
            $scope.auxilio.SemestreId = data[0].Id;

            $scope.getInstituicoes();
            $scope.getPessoa();
            $scope.getSemestre();
            $scope.getAuxilio();

            // chamado dentro do getAuxilio();
            //$scope.getArquivos();


        });
    }

    $scope.tipoComprovanteChange = function () {
        $('#fileUploadArquivos').fileupload({
            url: _baseURL + '/ajaxarquivos/savearquivosemestre?semestreid=' + $scope.auxilio.SemestreId + '&' + $scope.tipoComprovante,
            dataType: 'json',
            pasteZone: null,
            done: function (e, data) {
                if (data.result == true) {
                    $scope.getArquivos();
                    $('#spanMsgSucessoFile').fadeIn(1500).delay(5000).fadeOut(500);
                }
                else
                    alert(data.result);
            },
        });
    }

    $scope.getSemestre = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/semestre/Get?semestreid=' + $scope.auxilio.SemestreId
        }).success(function (data, status) {
            $scope.semestre = data;

            $scope.novaDisciplina();

            $('#btnSalvarDisciplina').hide();
            $('#detalheDisciplina').hide();
            $('#btnNovaDisciplina').show();
            $('#listDisciplinas').show();
        });
    }

    $scope.getPessoa = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/pessoa/Get'
        }).success(function (data, status) {
            $scope.pessoa = data;
        });
    }

    $scope.getInstituicoes = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/instituicao/GetAllItensAtivos'
        }).success(function (data, status) {
            $scope.instituicoes = data;
            $scope.auxilio.InstituicaoId = data[0].Id;
        });
    }

    $scope.getAuxilio = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/auxilio/Get?id=' + $scope.auxilio.SemestreId
        }).success(function (data, status) {
            $scope.auxilio = data;

            $scope.getArquivos();
            $scope.tipoComprovanteChange();
        });
    }

    $scope.getArquivos = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/arquivo/GetAllFilesSemestre?id=' + $scope.auxilio.Id
        }).success(function (data, status) {
            $scope.arquivos = data;
        });
    }

    $scope.changeSemestre = function () {
        $scope.getSemestre();
        $scope.getAuxilio();
        $scope.getArquivos();
    }

    $scope.salvar = function () {
        $("#bntSalvar").prop("disabled", true);
        $("#bntSalvar").val("Salvando...");
        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/auxilio/Save',
            data: { auxilio: JSON.stringify($scope.auxilio).toString() }
        }).success(function (data, status) {
            $("#bntSalvar").prop("disabled", false);
            $("#bntSalvar").val("Salvar");
            $("#msgSucesso").fadeIn(1500).delay(3000).fadeOut(500);
        });
    }

    // DISCIPLINAS

    $scope.editDisciplina = function (item) {
        if (item == null)
            $scope.novaDisciplina();
        else
            $scope.disciplina = item;

        $('#btnNovaDisciplina').hide();
        $('#btnSalvarDisciplina').show();
        $('#btnVoltarDisciplina').show();

        $('#detalheDisciplina').show();
        $('#listDisciplinas').hide();



    }

    $scope.novaDisciplina = function () {
        $scope.disciplina = new Object();
        $scope.disciplina.DiasSemana = [false, false, false, false, false, false, false];
        $scope.disciplina.Nome = '';
        $scope.disciplina.DataInicio = $scope.semestre.DataInicio;
        $scope.disciplina.DataTermino = $scope.semestre.DataTermino;
        $scope.disciplina.Turno = 'Noite';
        $scope.disciplina.Observacoes = '';
        $scope.disciplina.TransporteIda = true;
        $scope.disciplina.TransporteVolta = true;
        $scope.disciplina.newItem = true;

    }

    $scope.saveDisciplina = function () {
        if ($scope.auxilio.Disciplinas == null) {
            $scope.auxilio.Disciplinas = [];
        }

        if (stringIsNullOrEmpty($scope.disciplina.Nome)) {
            alert("Um nome para a disciplina deve ser informado.");
            return;
        } else if (stringIsNullOrEmpty($scope.disciplina.DataInicio) || $scope.disciplina.DataInicio.length != 10) {
            alert("Uma data de inicio valida deve ser informada.");
            return;
        } else if (stringIsNullOrEmpty($scope.disciplina.DataTermino || $scope.disciplina.DataTermino.length != 10)) {
            alert("Uma data de término valida deve ser informada.");
            return;
        } else if (!($scope.disciplina.DiasSemana[0]
            || $scope.disciplina.DiasSemana[1]
            || $scope.disciplina.DiasSemana[2]
            || $scope.disciplina.DiasSemana[3]
            || $scope.disciplina.DiasSemana[4]
            || $scope.disciplina.DiasSemana[5])) {
            console.log($scope.disciplina.DiasSemana);
            alert("Ao informar uma disciplina ao menos um dia de aula deve ser informado.");
            return;
        }

        if ($scope.disciplina.newItem) {
            $scope.disciplina.newItem = false;
            $scope.auxilio.Disciplinas.unshift($scope.disciplina);
        }

        $('#btnSalvarDisciplina').hide();
        $('#btnVoltarDisciplina').hide();
        $('#detalheDisciplina').hide();
        $('#btnNovaDisciplina').show();
        $('#listDisciplinas').show();
    }

    $scope.voltarDisciplina = function () {

        $('#btnSalvarDisciplina').hide();
        $('#btnVoltarDisciplina').hide();
        $('#detalheDisciplina').hide();
        $('#btnNovaDisciplina').show();
        $('#listDisciplinas').show();
    }

    $scope.excluirDisciplina = function (item) {

        for (var i = 0, tamanho = $scope.auxilio.Disciplinas.length; i < tamanho; i++)
            if ($scope.auxilio.Disciplinas[i] == item)
                $scope.auxilio.Disciplinas.remove(i);
    }

    $scope.baixarArquivo = function (id) {
        var url = _baseURL + '/ajaxarquivos/getfile?id=' + id;
        downloadFile(guid(), url);
    }

    $scope.init();
}