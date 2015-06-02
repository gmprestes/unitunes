<%@ Page Title="" Language="C#" MasterPageFile="~/page.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="AET.Restricted.Admin.View.InstituicoesEnsino.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Scripts/view/admin/instituicoes.js?1.1"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper" ng-controller="InstituicoesListCtrl">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Listagem de Instituicoes
                </h1>
            </div>
            <!-- /.col-lg-12 -->
            <button id="btnNovo" type="button" class="btn btn-primary navbar-right btnsTop" ng-click="novo()">Novo</button>
        </div>
        <!-- /.row -->
        <div class="row">
            <div class="table-responsive">
                <table class="table" id="tableInstituicoes">
                    <thead>
                        <tr>
                            <th></th>
                            <th>Ativa</th>
                            <th>Instituição</th>
                            <th>Sigla</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="item in instituicoes">
                            <td><i class="fa fa-times fa-fw editClick" title="Excluir item" ng-click="excluirItem(item.Id)"></i><i class="fa fa-pencil fa-fw editClick" title="Editar item" ng-click="editItem(item.Id)"></i></td>
                            <td>{{ item.Ativo ? 'Sim':'Não' }}</td>
                            <td>{{ item.Nome }}</td>
                            <td>{{ item.Sigla }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
</asp:Content>
