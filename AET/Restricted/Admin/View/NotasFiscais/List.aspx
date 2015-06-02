<%@ Page Title="" Language="C#" MasterPageFile="~/page.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="AET.Restricted.Admin.View.NotasFiscais.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Scripts/view/admin/notasfiscais.js?v1.2"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper" ng-controller="NotasFiscaisCtrl">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Listagem de Notas Fiscais/Semestre
                </h1>
                <div class="col-lg-3">
                    <label>Mês</label>
                    <select ng-model="filtro.Mes">
                        <option value="01">Janeiro</option>
                        <option value="02">Fevereiro</option>
                        <option value="03">Março</option>
                        <option value="04">Maio</option>
                        <option value="05">Abril</option>
                        <option value="06">Junho</option>
                        <option value="07">Julho</option>
                        <option value="08">Agosto</option>
                        <option value="09">Setembro</option>
                        <option value="10">Outubro</option>
                        <option value="11">Novembro</option>
                        <option value="12">Dezembro</option>
                    </select>
                </div>
                <div class="col-lg-3">
                    <label>Ano</label>
                    <select ng-model="filtro.Ano">
                        <option value="2015">2015</option>
                        <option value="2016">2016</option>
                    </select>
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
