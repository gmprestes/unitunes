<%@ Page Title="" Language="C#" MasterPageFile="~/page.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="AET.Restricted.Admin.View.Associados.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/Scripts/utils/fileupload/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Scripts/utils/fileupload/jquery.fileupload.js"></script>
    <script type="text/javascript" src="/Scripts/utils/fileupload/jquery.iframe-transport.js"></script>

    <script type="text/javascript" src="/Scripts/view/admin/associados.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper" ng-controller="AssociadosEditCtrl">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Dados do Associado
                    <span id="msgSucesso" style="display: none;">- Dados salvos com sucesso
                    </span>
                </h1>

                <button id="bntSalvar" type="button" class="btn btn-success navbar-right" ng-click="salvar()" style="margin-bottom: 15px;">Salvar</button>
            </div>
            <!-- /.col-lg-12 -->
        </div>
        <!-- /.row -->
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        {{ pessoa.Nome }} - Inscricao Nº {{ pessoa.Codigo }}
                    </div>
                    <div class="panel-body">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs">
                            <li class="active"><a href="#dadosCadastrais" data-toggle="tab">Dados Cadastrais</a>
                            </li>
                            <li class=""><a href="#dadosDocs" data-toggle="tab">Documentos</a>
                            </li>
                            <li class=""><a href="#dadosSemestres" data-toggle="tab">Semestres</a>
                            </li>
                        </ul>
                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div class="tab-pane fade in active" id="dadosCadastrais">
                                <div class="row">
                                    <div class="form-group col-lg-3">
                                        <label>Não Associado</label>
                                        <input class="form-control" type="checkbox" ng-model="pessoa.NaoAssociado">
                                    </div>
                                    <div class="form-group col-lg-12">
                                        <label>Email</label>
                                        <input class="form-control" ng-model="pessoa.Email" />
                                        <%--<p class="help-block">Seu email de login nao pode ser alterado.</p>--%>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label>Nome</label>
                                        <input class="form-control" ng-model="pessoa.Nome">
                                        <%--<p class="help-block">Example block-level help text here.</p>--%>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label>Sobrenome</label>
                                        <input class="form-control" ng-model="pessoa.Sobrenome">
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label>CPF</label>
                                        <input class="form-control" maxlength="11" ng-model="pessoa.CPF">
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label>RG</label>
                                        <input class="form-control" ng-model="pessoa.RG">
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label>Titulo De Eleitor</label>
                                        <input class="form-control" ng-model="pessoa.TituloEleitoral">
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label>Data Nascimento</label>
                                        <input class="form-control data" ng-model="pessoa.DataNascimento">
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label>Telefone Contato</label>
                                        <input class="form-control numbers" maxlength="11" ng-model="pessoa.Telefone">
                                    </div>

                                </div>
                                <h3 class="page-header">Endereço
                                </h3>
                                <div class="row">
                                    <div class="form-group col-lg-5">
                                        <label>Logradouro</label>
                                        <input class="form-control" title="Aqui você deve informar o nome de sua rua, avenida, etc..." ng-model="pessoa.Logradouro">
                                    </div>
                                    <div class="form-group col-lg-1">
                                        <label>Numero</label>
                                        <input class="form-control" ng-model="pessoa.LogradouroNumero">
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label>Bairro</label>
                                        <input class="form-control" ng-model="pessoa.Bairro">
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label>Cidade</label>
                                        <input class="form-control" ng-model="pessoa.Cidade">
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label>CEP</label>
                                        <input class="form-control" ng-model="pessoa.CEP">
                                    </div>
                                    <div class="form-group col-lg-9">
                                        <label>Complemento</label>
                                        <input class="form-control" maxlength="11" ng-model="pessoa.Complemento">
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade" id="dadosDocs">
                                <h4 class="page-header">Selecione um Comprovante <span id="spanMsgSucessoFile" style="display: none;"><i class="fa fa-check"></i>Arquivo salvo com sucesso, aguarde alguns intantes enquanto atualizamos seus dados
                                </span></h4>
                                <div class="row">

                                    <div class="form-group col-lg-3">
                                        <label>Tipo de Comprovante</label>
                                        <select id="tipo" class="form-control" maxlength="11" ng-model="tipoComprovante" ng-change="tipoComprovanteChange()">
                                            <option value="identidade">Copia RG</option>
                                            <option value="cpf">Copia CPF</option>
                                            <option value="certidaonascimento">Copia Certidão Nascimento</option>
                                            <option value="endereco">Comprovante de Endereço</option>
                                            <option value="eleitor">Titulo de Eleitor</option>
                                        </select>
                                    </div>
                                    <div class="form-group col-lg-9">
                                        <input id="fileUploadArquivos" class="btn-default" title="Selecione o Comprovante" type="file" name="files[]" multiple style="margin-top: 30px;">
                                    </div>
                                </div>
                                <h3 class="page-header">Arquivos
                                </h3>
                                <div class="row">

                                    <div class="table-responsive">
                                        <table class="table">
                                            <thead>
                                                <tr>
                                                    <th>Nome Arquivo</th>
                                                    <th>Tipo de Comprovante</th>
                                                    <th>Data Upload</th>
                                                    <th>Verificado</th>
                                                    <th>Aceito</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr ng-repeat="item in arquivos">
                                                    <td>{{ item.Nome }}</td>
                                                    <td>{{ item.TipoArquivo == 'ComprovanteIdentidadePerfil' ? 'Copia RG' : (item.TipoArquivo == 'ComprovanteEnderecoPerfil' ? 'Comprovante de Endereço' : ( item.TipoArquivo == 'ComprovanteCPF' ? 'Copia CPF' : ( item.TipoArquivo == 'ComprovanteCertidaoNascimento' ? 'Copia Certidão Nascimento' : 'Titulo Eleitor'))) }}</td>

                                                    <td>{{ item.DataUpload }}</td>
                                                    <td>
                                                        <input type="checkbox" style="width: 20px; height: 20px;" ng-model="item.Verificado" ng-change="saveArquivo(item)" /></td>
                                                    <td>
                                                        <input type="checkbox" style="width: 20px; height: 20px;" ng-model="item.Aceito" ng-change="saveArquivo(item)" /></td>
                                                    <td><i style="cursor: pointer;" class="fa fa-cloud-download fa-fw" ng-click="baixarArquivo(item.Id)"></i></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade" id="dadosSemestres">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-2">
                                            <label>Semestre</label>
                                            <select class="form-control" ng-change="changeSemestre()" ng-model="auxilio.SemestreId" ng-options="o.Id as o.Nome for o in semestres">
                                            </select>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="panel panel-default">

                                                <div class="panel-body">
                                                    <!-- Nav tabs -->
                                                    <ul class="nav nav-tabs">
                                                        <li class="active"><a href="#dadosCadastraisSemestre" data-toggle="tab">Dados da solicitação de auxilio</a>
                                                        </li>
                                                        <li class=""><a href="#dadosDocsSemestre" data-toggle="tab">Documentos</a>
                                                        </li>
                                                    </ul>
                                                    <!-- Tab panes -->
                                                    <div class="tab-content">
                                                        <div class="tab-pane fade in active" id="dadosCadastraisSemestre">
                                                            <div class="row">
                                                                <div class="form-group col-lg-2">
                                                                    <label>Auxilio Concedido ?</label>
                                                                    <input type="checkbox" class="form-control" ng-model="auxilio.Concedido" />
                                                                </div>
                                                                <div class="form-group col-lg-3">
                                                                    <label>Instituição de Ensino</label>
                                                                    <select class="form-control" ng-disabled="auxilio.Concedido" ng-model="auxilio.InstituicaoId" ng-options="o.Id as o.Sigla for o in instituicoes">
                                                                    </select>
                                                                </div>
                                                                <div class="form-group col-lg-3">
                                                                    <label>Nº Matricula</label>
                                                                    <input class="form-control" maxlength="20" ng-disabled="auxilio.Concedido" title="Informe seu numero de matricula" ng-model="auxilio.NumMatricula" />
                                                                </div>
                                                                <div class="form-group col-lg-12">
                                                                    <label>Curso</label>
                                                                    <input class="form-control" ng-disabled="auxilio.Concedido" title="Informe o nome do seu curso" ng-model="auxilio.Curso" />
                                                                </div>

                                                            </div>
                                                            <h3 class="page-header">Disciplinas
                                                            </h3>
                                                            <button id="btnVoltarDisciplina" type="button" class="btn btn-danger navbar-right btnsTop" ng-click="voltarDisciplina()" style="margin-bottom: 15px; display: none;">Voltar</button>
                                                            <button id="btnSalvarDisciplina" type="button" class="btn btn-success navbar-right btnsTop" ng-click="saveDisciplina()" style="margin-bottom: 15px; display: none;">Salvar Disciplina</button>
                                                            <button id="btnNovaDisciplina" ng-hide="auxilio.Concedido" type="button" class="btn btn-info navbar-right btnsTop" ng-click="editDisciplina()" style="margin-bottom: 15px;">Nova Disciplina</button>
                                                            <div id="detalheDisciplina" style="display: none;">
                                                                <div class="row" ng-hide="auxilio.Concedido">
                                                                    <div class="form-group col-lg-12">
                                                                        <label>Nome da Disciplina</label>
                                                                        <input class="form-control" title="Informe o nome da disciplina" ng-model="disciplina.Nome" />
                                                                    </div>
                                                                    <div class="form-group col-lg-2">
                                                                        <label>Data Inicio</label>
                                                                        <input class="form-control data" ng-model="disciplina.DataInicio" />
                                                                    </div>
                                                                    <div class="form-group col-lg-2">
                                                                        <label>Data Termino</label>
                                                                        <input class="form-control data" ng-model="disciplina.DataTermino" />
                                                                    </div>
                                                                    <div class="form-group col-lg-2">
                                                                        <label>Turno Disciplina</label>
                                                                        <select class="form-control" ng-model="disciplina.Turno">
                                                                            <option>Matutino</option>
                                                                            <option>Tarde</option>
                                                                            <option>Vespertino</option>
                                                                            <option>Noite</option>
                                                                        </select>
                                                                    </div>
                                                                    <div class="form-group col-lg-1">
                                                                        <label>EAD</label>
                                                                        <input type="checkbox" class="form-control" ng-model="disciplina.EAD" />
                                                                    </div>
                                                                    <div class="form-group col-lg-1">
                                                                        <label>Transporte Ida</label>
                                                                        <input type="checkbox" class="form-control" ng-model="disciplina.TransporteIda" />
                                                                    </div>
                                                                    <div class="form-group col-lg-2">
                                                                        <label>Transporte Volta</label>
                                                                        <input type="checkbox" class="form-control" ng-model="disciplina.TransporteVolta" />
                                                                    </div>
                                                                    <div class="form-group col-lg-12">
                                                                        <h4>Indique os dias em que você tem aula nesta disciplina :</h4>
                                                                        <div class="table-responsive">
                                                                            <table class="table">
                                                                                <thead>
                                                                                    <tr>
                                                                                        <th></th>
                                                                                        <th style="text-align: center;">Segunda</th>
                                                                                        <th style="text-align: center;">Terça</th>
                                                                                        <th style="text-align: center;">Quarta</th>
                                                                                        <th style="text-align: center;">Quinta</th>
                                                                                        <th style="text-align: center;">Sexta</th>
                                                                                        <th style="text-align: center;">Sabado</th>
                                                                                    </tr>
                                                                                </thead>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="text-align: center;">Turno {{ disciplina.Turno }}</td>
                                                                                        <td>
                                                                                            <input type="checkbox" class="form-control" ng-model="disciplina.DiasSemana[0]"></td>
                                                                                        <td>
                                                                                            <input type="checkbox" class="form-control" ng-model="disciplina.DiasSemana[1]"></td>
                                                                                        <td>
                                                                                            <input type="checkbox" class="form-control" ng-model="disciplina.DiasSemana[2]"></td>
                                                                                        <td>
                                                                                            <input type="checkbox" class="form-control" ng-model="disciplina.DiasSemana[3]"></td>
                                                                                        <td>
                                                                                            <input type="checkbox" class="form-control" ng-model="disciplina.DiasSemana[4]"></td>
                                                                                        <td>
                                                                                            <input type="checkbox" class="form-control" ng-model="disciplina.DiasSemana[5]"></td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="form-group col-lg-12">
                                                                        <label>Observações</label>
                                                                        <textarea class="form-control" ng-model="disciplina.Observacoes"></textarea>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="table-responsive" id="listDisciplinas">
                                                                <table class="table">
                                                                    <thead>
                                                                        <tr>
                                                                            <th></th>
                                                                            <th style="text-align: center;">Disciplina</th>
                                                                            <th style="text-align: center;">Data Inicio</th>
                                                                            <th style="text-align: center;">Data Término</th>
                                                                            <th style="text-align: center;">EAD</th>
                                                                            <th style="text-align: center;">Turno</th>
                                                                            <th style="text-align: center;">Dias de Aula</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <tr ng-repeat="item in auxilio.Disciplinas">
                                                                            <td><i class="fa fa-times fa-fw editClick" title="Excluir item" ng-hide="auxilio.Concedido" ng-click="excluirDisciplina(item)"></i><i class="fa fa-pencil fa-fw editClick" title="Editar item" ng-click="editDisciplina(item)"></i></td>
                                                                            <td style="text-align: center;">{{ item.Nome }}</td>
                                                                            <td style="text-align: center;">{{ item.DataInicio }}</td>
                                                                            <td style="text-align: center;">{{ item.DataTermino }}</td>
                                                                            <td style="text-align: center;">{{ item.EAD?'Sim':'Não' }}</td>
                                                                            <td style="text-align: center;">{{ item.Turno }}</td>
                                                                            <td style="text-align: center;">{{ item.DiasSemana[0]?'Segunda;':'' }}
                                                    {{ item.DiasSemana[1]?' Terça;':'' }}
                                                    {{ item.DiasSemana[2]?' Quarta;':'' }}
                                                    {{ item.DiasSemana[3]?' Quinta;':'' }}
                                                    {{ item.DiasSemana[4]?' Sexta;':'' }}
                                                    {{ item.DiasSemana[5]?' Sabado':'' }}
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                                <h4 style="text-align: center;" ng-show="auxilio.Disciplinas.length == 0 || auxilio.Disciplinas == null">Sem disciplinas cadastradas</h4>
                                                            </div>
                                                        </div>
                                                        <div class="tab-pane fade" id="dadosDocsSemestre">
                                                            <h4 class="page-header" ng-hide="auxilio.Concedido">Selecione um Comprovante <span id="span1" style="display: none;"><i class="fa fa-check"></i>Arquivo salvo com sucesso, aguarde alguns intantes enquanto atualizamos seus dados
                                                            </span></h4>
                                                            <div class="row" ng-hide="auxilio.Concedido">
                                                                <div class="form-group col-lg-3">
                                                                    <label>Tipo de Comprovante</label>
                                                                    <select id="Select1" class="form-control" maxlength="11" ng-model="tipoComprovanteSemestre" ng-change="tipoComprovanteSemestreChange()">
                                                                        <option value="matricula">Comprovante de Matricula</option>
                                                                        <option value="notas">Notas Ultimo Semestre</option>
                                                                    </select>
                                                                </div>
                                                                <div class="form-group col-lg-9">
                                                                    <input id="fileUploadArquivosSemestre" class="btn-default" title="Selecione Comprovante de Endereço" type="file" name="files[]" multiple style="margin-top: 30px;">
                                                                </div>
                                                            </div>
                                                            <h3 class="page-header">Arquivos
                                                            </h3>
                                                            <div class="row">

                                                                <div class="table-responsive">
                                                                    <table class="table">
                                                                        <thead>
                                                                            <tr>
                                                                                <th>Nome Arquivo</th>
                                                                                <th>Tipo de Comprovante</th>
                                                                                <th>Data Upload</th>
                                                                                <th>Verificado</th>
                                                                                <th>Aceito</th>
                                                                                <th></th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr ng-repeat="item in arquivosSemestre">
                                                                                <td>{{ item.Nome }}</td>
                                                                                <td>{{ item.TipoArquivo == 'ComprovanteMatriculaSemestre' ? 'Comprovante de Maticula' : 'Notas Ultimo Semestre' }}</td>
                                                                                <td>{{ item.DataUpload }}</td>
                                                                                <td>
                                                                                    <input type="checkbox" style="width: 20px; height: 20px;" ng-model="item.Verificado" ng-change="saveArquivo(item)" /></td>
                                                                                <td>
                                                                                    <input type="checkbox" style="width: 20px; height: 20px;" ng-model="item.Aceito" ng-change="saveArquivo(item)" /></td>
                                                                                <td><i style="cursor: pointer;" class="fa fa-cloud-download fa-fw" ng-click="baixarArquivo(item.Id)"></i></td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <!-- /.col-lg-12 -->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
