<%@ Page Title="" Language="C#" MasterPageFile="~/page.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="AET.Restricted.Admin.View.Semestres.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Scripts/view/admin/semestres.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper" ng-controller="SemestresEditCtrl">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Cadastro/Edição de Semestre
                    <span id="msgSucesso" style="display: none;">- Dados salvos com sucesso
                    </span>
                </h1>
                <button id="bntSalvar" type="button" class="btn btn-success navbar-right btnsTop" ng-click="saveSemestre()">Salvar</button>
                <button id="btnVoltar" type="button" class="btn btn-danger navbar-right btnsTop" ng-click="voltar()">Voltar</button>
            </div>
            <!-- /.col-lg-12 -->
        </div>
        <!-- /.row -->
        <div class="row">
            <div class="row">
                <div class="form-group col-lg-3">
                    <label>Nome</label>
                    <input class="form-control" type="text" ng-model="semestre.Nome">
                </div>
                <div class="form-group col-lg-1">
                    <label>Ativo</label>
                    <input class="form-control" type="checkbox" ng-model="semestre.Ativo">
                </div>
                <div class="form-group col-lg-1">
                    <label>Data Inicio</label>
                    <input class="form-control data" type="text" ng-model="semestre.DataInicio">
                </div>
                <div class="form-group col-lg-1">
                    <label>Data Termino</label>
                    <input class="form-control data" type="text" ng-model="semestre.DataTermino">
                </div>
            </div>
        </div>
</asp:Content>
