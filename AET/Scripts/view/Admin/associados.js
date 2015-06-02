
function AuxiliosAssociadoCtrl($scope, $http) {

    $scope.auxilio = new Object();
    $scope.arquivosSemestre = [];
    $scope.semestre = new Object();
    $scope.semestres = [];
    $scope.instituicoes = [];

    $scope.tipoComprovanteSemestre = 'matricula';

    $scope.initSemestres = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/pessoa/GetSemestresAssociado/' + _id
        }).success(function (data, status) {
            $scope.semestres = data;
            $scope.auxilio.SemestreId = data[0].Id;

            $scope.getInstituicoes();
            $scope.getSemestre();
            $scope.getAuxilio();

            // chamado dentro do getAuxilio();
            //$scope.getArquivos();
        });
    }

    $scope.tipoComprovanteSemestreChange = function () {
        $('#fileUploadArquivosSemestre').fileupload({
            url: _baseURL + '/ajaxarquivos/savearquivosemestreassociado?semestreid=' + $scope.auxilio.SemestreId + '&' + $scope.tipoComprovanteSemestre + '&pessoaid=' + _id,
            dataType: 'json',
            pasteZone: null,
            done: function (e, data) {
                if (data.result == true) {
                    $scope.getArquivosSemestre();
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
            url: _baseURL + '/request/auxilio/GetAssociado?semestreid=' + $scope.auxilio.SemestreId + '&pessoaid=' + _id
        }).success(function (data, status) {
            $scope.auxilio = data;

            $scope.getArquivosSemestre();
            $scope.tipoComprovanteChange();
        });
    }

    $scope.getArquivosSemestre = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/arquivo/GetAllFilesSemestreAssociado?id=' + $scope.auxilio.Id + '&pessoaid=' + _id
        }).success(function (data, status) {
            $scope.arquivosSemestre = data;
        });
    }

    $scope.changeSemestre = function () {
        $scope.getSemestre();
        $scope.getAuxilio();
        //$scope.getArquivosSemestre();
    }

    $scope.salvarSemestre = function () {
        $("#bntSalvar").prop("disabled", true);
        $("#bntSalvar").val("Salvando...");
        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/auxilio/Save',
            data: { auxilio: JSON.stringify($scope.auxilio).toString() }
        }).success(function (data, status) {
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

    $scope.initSemestres();
}

function AssociadosListCtrl($scope, $http) {
    $scope.nome = '';
    $scope.somenteDocsPendentes = '';

    $scope.associados = [];



    $scope.init = function () {

        var nome = $.cookie("nomeListAssociado");
        if (typeof nome != 'undefined')
            $scope.nome = nome;

        var docs = $.cookie("somenteDocsPendentesListAssociado");
        if (typeof docs != 'undefined')
            $scope.somenteDocsPendentes = docs == 'true';

        var auxilio = $.cookie("AuxilioNaoConcedidoListAssociado");
        if (typeof docs != 'undefined')
            $scope.auxilioNaoConcedido = auxilio == 'true';

        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/pessoa/GetAll',
            data: { nome: $scope.nome, docspendentes: $scope.somenteDocsPendentes, auxilionaoconcedido: $scope.somenteDocsPendentes }
        }).success(function (data, status) {
            $scope.associados = data;

            $('#tableAssociados').paginartable({ tamPagina: 10 });
        });
    }

    $scope.editItem = function (id) {
        window.location = _baseURL + "/associado/edit/" + id;
    }

    $scope.init();

    $scope.filtrar = function () {

        $('#btnFiltrar').text("Filtrando...");
        $('#btnFiltrar').prop("disabled", true);

        $.cookie("nomeListAssociado", $scope.nome);
        $.cookie("somenteDocsPendentesListAssociado", $scope.somenteDocsPendentes);
        $.cookie("AuxilioNaoConcedidoListAssociado", $scope.auxilioNaoConcedido);



        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/pessoa/GetAll',
            data: { nome: $scope.nome, docspendentes: $scope.somenteDocsPendentes, auxilionaoconcedido: $scope.somenteDocsPendentes }
        }).success(function (data, status) {
            $scope.associados = data;

            $('#btnFiltrar').text("Filtrar");
            $('#btnFiltrar').prop("disabled", false);

            $('#tableAssociados').paginartable({ tamPagina: 10 });
        });
    }
}

function AssociadosEditCtrl($scope, $http) {

    $scope.tipoComprovante = 'identidade';
    $scope.pessoa = new Object();
    $scope.arquivos = [];

    $scope.init = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/pessoa/GetById/' + _id
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

            $scope.salvarSemestre();

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
            url: _baseURL + '/request/arquivo/GetAllFilesPessoa/' + $scope.pessoa.Id
        }).success(function (data, status) {
            $scope.arquivos = data;
        });
    }

    $scope.tipoComprovanteChange = function () {
        $('#fileUploadArquivos').fileupload({
            url: _baseURL + '/ajaxarquivos/savearquivoassociado?tipo=' + $scope.tipoComprovante + '&id=' + $scope.pessoa.Id,
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

    $scope.saveArquivo = function (item) {
        var httpRequest = $http({
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            data: { arquivo: JSON.stringify(item).toString() },
            url: _baseURL + '/request/arquivo/SaveArquivo'
        }).success(function (data, status) {
            if (data == false)
                alert("Não foi possivel salvar esta alteração. Possivelmente o arquivo não existe mais");
            else {
                $scope.buscaArquivos();
                $scope.getArquivosSemestre();
            }
        });

    }

    $scope.init();

    AuxiliosAssociadoCtrl($scope, $http);
}

