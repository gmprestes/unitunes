<%@ Page Title="" Language="C#" MasterPageFile="~/page.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="AET.Restricted.Admin.View.InstituicoesEnsino.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Scripts/view/admin/instituicoes.js?1.1"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper" ng-controller="InstituicoesEditCtrl">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Cadastro/Edição de Instituições de Ensino
                    <span id="msgSucesso" style="display: none;">- Dados salvos com sucesso
                    </span>
                </h1>
                <button id="bntSalvar" type="button" class="btn btn-success navbar-right btnsTop" ng-click="save()">Salvar</button>
                <button id="btnVoltar" type="button" class="btn btn-danger navbar-right btnsTop" ng-click="voltar()">Voltar</button>
            </div>
            <!-- /.col-lg-12 -->
        </div>
        <!-- /.row -->
        <div class="row">
            <div class="row">
                <div class="form-group col-lg-1">
                    <label>Habilitada</label>
                    <input class="form-control" type="checkbox" ng-model="instituicao.Ativo">
                </div>
                <div class="form-group col-lg-3">
                    <label>Nome</label>
                    <input class="form-control" type="text" ng-model="instituicao.Nome">
                </div>
                <div class="form-group col-lg-3">
                    <label>Sigla da Instituição</label>
                    <input class="form-control" type="text" ng-model="instituicao.Sigla">
                </div>

                <div class="form-group col-lg-12">
                    <label>Breve Descrição</label>
                    <input class="form-control" type="text" ng-model="instituicao.Descricao">
                </div>
            </div>
        </div>
</asp:Content>
