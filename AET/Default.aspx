<%@ Page Title="" Language="C#" MasterPageFile="~/page.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AET.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Page Script -->
    <%--<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js"></script>--%>
    <script type="text/javascript" src="/Scripts/utils/fileupload/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Scripts/utils/fileupload/jquery.fileupload.js"></script>
    <script type="text/javascript" src="/Scripts/utils/fileupload/jquery.iframe-transport.js"></script>

    <script type="text/javascript" src="/Scripts/view/meuperfil.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper" ng-controller="MeuPerfilCtrl">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Meus Dados
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
                            <li class="active"><a href="#dadosCadastrais" data-toggle="tab">Meus Dados</a>
                            </li>
                            <li class=""><a href="#dadosDocs" data-toggle="tab">Documentos</a>
                            </li>
                        </ul>
                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div class="tab-pane fade in active" id="dadosCadastrais">
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <label>Email</label>
                                        <p class="form-control-static">{{ pessoa.Email }}</p>
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
