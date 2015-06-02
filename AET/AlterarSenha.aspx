<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlterarSenha.aspx.cs" Inherits="AET.AlterarSenha" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <title>Alterar Sua Senha :</title>

    <!-- Core CSS - Include with every page -->
    <link href="/css/bootstrap.min.css" rel="stylesheet">
    <link href="/font-awesome/css/font-awesome.css" rel="stylesheet">
    <!-- SB Admin CSS - Include with every page -->
    <link href="/css/sb-admin.css" rel="stylesheet">
</head>
<body ng-app="aet">
    <div class="container" ng-controller="AlterarSenhaCtrl">

        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                <div class="login-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Informe sua senha atual e nova senha:</h3>
                    </div>
                    <div class="panel-body">
                        <form role="form">
                            <div class="form-group" id="Div1">
                                <input class="form-control" placeholder="Senha Atual" type="password" autofocus ng-model="atual">
                            </div>
                            <div class="form-group" id="txtNome">
                                <input class="form-control" placeholder="Nova Senha" type="password" ng-model="nova">
                            </div>
                            <!-- Change this to a button or input when using this as a form -->
                            <a class="btn btn-lg btn-success btn-block" ng-click="alterar()">Alterar</a>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <!-- Core Scripts - Include with every page -->
    <script src="/js/jquery-1.10.2.js"></script>
    <script src="/js/bootstrap.min.js"></script>
    <script src="/js/plugins/metisMenu/jquery.metisMenu.js"></script>

    <!-- SB Admin Scripts - Include with every page -->
    <script src="/js/sb-admin.js"></script>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.2.19/angular.min.js"></script>
    <script type="text/javascript" src="/Scripts/utils/helpers.js"></script>
    <script type="text/javascript" src="/Scripts/utils/app.js"></script>
    <!-- Page Script -->
    <script type="text/javascript" src="/Scripts/view/alterarsenha.js?v1.05"></script>


</body>
</html>
