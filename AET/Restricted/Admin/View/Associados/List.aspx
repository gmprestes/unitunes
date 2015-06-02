<%@ Page Title="" Language="C#" MasterPageFile="~/page.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="AET.Restricted.Admin.View.Associados.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Scripts/view/admin/associados.js?v1.2"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper" ng-controller="AssociadosListCtrl">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Listagem de Associados
                </h1>
                <div class="col-lg-3">
                    <label>Filtrar por nome :</label>
                    <input class="form-control" type="text" ng-model="nome">
                </div>
                 <div class="col-lg-3">
                    <label>Docs Pendentes</label>
                    <input class="form-control" type="checkbox" ng-model="somenteDocsPendentes">
                </div>
                 <div class="col-lg-3">
                    <label>Auxilio Não Concedido</label>
                    <input class="form-control" type="checkbox" ng-model="auxilioNaoConcedido">
                </div>
                <div class="col-lg-1">
                    <button id="btnFiltrar" style="margin-top: 25px;" type="button" class="btn btn-danger navbar-right btnsTop" ng-click="filtrar()">Filtrar</button>

                </div>
            </div>
            <!-- /.col-lg-12 -->

        </div>
        <!-- /.row -->
        <div class="row">
            <div class="table-responsive">
                <table class="table" id="tableAssociados">
                    <thead>
                        <tr>
                            <th></th>
                            <th>Código</th>
                            <th>Nome</th>
                            <th>Sobrenome</th>
                            <th>Telefone</th>
                            <th>Email</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="item in associados">
                            <td><i class="fa fa-pencil fa-fw editClick" title="Editar item" ng-click="editItem(item.Id)"></i></td>
                            <td>{{ item.Codigo }}</td>
                            <td>{{ item.Nome }}</td>
                            <td>{{ item.Sobrenome }}</td>
                            <td>{{ item.Telefone }}</td>
                            <td>{{ item.Email }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
</asp:Content>
