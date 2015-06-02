<%@ Page Title="" Language="C#" MasterPageFile="~/page.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="AET.Restricted.Admin.View.Semestres.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Scripts/view/admin/semestres.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper" ng-controller="SemestresListCtrl">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Listagem de Semestres
                </h1>
            </div>
            <!-- /.col-lg-12 -->
             <button id="btnNovo" type="button" class="btn btn-primary navbar-right btnsTop" ng-click="novo()">Novo</button>
        </div>
        <!-- /.row -->
        <div class="row" >
            <div class="table-responsive">
                <table class="table" id="tableSemestres">
                    <thead>
                        <tr>
                            <th></th>
                            <th>Ativo</th>
                            <th>Semestre</th>
                            <th>Inicio</th>
                            <th>Fim</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="item in semestres">
                            <td><i class="fa fa-times fa-fw editClick" title="Excluir item" ng-click="excluirItem(item.Id)"></i><i class="fa fa-pencil fa-fw editClick" title="Editar item" ng-click="editItem(item.Id)"></i></td>
                            <td>{{ item.Ativo ? 'Sim':'Não' }}</td>
                            <td>{{ item.Nome }}</td>
                            <td>{{ item.DataInicio }}</td>
                            <td>{{ item.DataTermino }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
</asp:Content>
