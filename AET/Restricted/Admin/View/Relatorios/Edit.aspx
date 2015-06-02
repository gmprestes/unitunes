<%@ Page Title="" Language="C#" MasterPageFile="~/page.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="AET.Restricted.Admin.View.Relatorios.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/Scripts/view/admin/relatorios.js?v=1.4"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper" ng-controller="RelatoriosCtrl">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Relatorios
                </h1>

                <button id="bntSalvar" type="button" class="btn btn-success navbar-right" ng-click="baixarRelatorioAssociados()" style="margin-bottom: 15px;">Dias Uso Transporte</button>
            </div>
            <!-- /.col-lg-12 -->
        </div>
</asp:Content>
