<%@ Page Title="" Language="C#" MasterPageFile="~/page.Master" AutoEventWireup="true" CodeBehind="Aulixios.aspx.cs" Inherits="AET.Aulixios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Page Script -->
    <%--<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js"></script>--%>
    <script type="text/javascript" src="/Scripts/utils/fileupload/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Scripts/utils/fileupload/jquery.fileupload.js"></script>
    <script type="text/javascript" src="/Scripts/utils/fileupload/jquery.iframe-transport.js"></script>

    <script type="text/javascript" src="/Scripts/view/auxilios.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper" ng-controller="AuxiliosCtrl">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Meus Auxílios
                    <span id="msgSucesso" style="display: none;">- Dados salvos com sucesso
                    </span>
                </h1>
                <button id="bntSalvar" type="button" class="btn btn-success navbar-right btnsTop" ng-click="salvar()" style="margin-bottom: 15px;">Salvar</button>
                <div class="form-group col-lg-2">
                    <label>Semestre</label>
                    <select class="form-control" ng-change="changeSemestre()" ng-model="auxilio.SemestreId" ng-options="o.Id as o.Nome for o in semestres">
                    </select>
                </div>
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
                            <li class="active"><a href="#dadosCadastrais" data-toggle="tab">Dados da solicitação de auxilio</a>
                            </li>
                            <li class=""><a href="#dadosDocs" data-toggle="tab">Documentos</a>
                            </li>
                        </ul>
                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div class="tab-pane fade in active" id="dadosCadastrais">
                                <div class="row">
                                    <div class="form-group col-lg-2">
                                        <label>Auxilio Concedido ?</label>
                                        <input type="checkbox" class="form-control" ng-model="auxilio.Concedido" disabled />
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
                                <button id="btnSalvarDisciplina" type="button" class="btn btn-success navbar-right btnsTop" ng-click="saveDisciplina()" style="margin-bottom: 15px; display: none;">OK</button>
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
                                                <td><i class="fa fa-times fa-fw editClick" title="Excluir item" ng-click="excluirDisciplina(item)" ng-hide="auxilio.Concedido"></i><i class="fa fa-pencil fa-fw editClick" title="Editar item" ng-click="editDisciplina(item)"></i></td>
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
                            <div class="tab-pane fade" id="dadosDocs">
                                <h4 class="page-header" ng-hide="auxilio.Concedido">Selecione um Comprovante <span id="spanMsgSucessoFile" style="display: none;"><i class="fa fa-check"></i>Arquivo salvo com sucesso, aguarde alguns intantes enquanto atualizamos seus dados
                                </span></h4>
                                <div class="row" ng-hide="auxilio.Concedido">
                                    <div class="form-group col-lg-3">
                                        <label>Tipo de Comprovante</label>
                                        <select id="tipo" class="form-control" maxlength="11" ng-model="tipoComprovante" ng-change="tipoComprovanteChange()">
                                            <option value="matricula">Comprovante de Matricula</option>
                                            <option value="notas">Notas Ultimo Semestre</option>
                                        </select>
                                    </div>
                                    <div class="form-group col-lg-9">
                                        <input id="fileUploadArquivos" class="btn-default" title="Selecione Comprovante de Endereço" type="file" name="files[]" multiple style="margin-top: 30px;">
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
                                                    <td>{{ item.TipoArquivo == 'ComprovanteMatriculaSemestre' ? 'Comprovante de Maticula' : 'Notas Ultimo Semestre' }}</td>
                                                    <td>{{ item.DataUpload }}</td>
                                                    <td>{{ item.Verificado ? 'Sim':'Não' }}</td>
                                                    <td>{{ item.Aceito ? 'Sim':'Não' }}</td>
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
</asp:Content>
