<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="AET.Cadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <title>Cadastro de novo usuario :</title>

    <!-- Core CSS - Include with every page -->
    <link href="/css/bootstrap.min.css" rel="stylesheet">
    <link href="/font-awesome/css/font-awesome.css" rel="stylesheet">
    <!-- SB Admin CSS - Include with every page -->
    <link href="/css/sb-admin.css" rel="stylesheet">
</head>
<body ng-app="aet">
    <div class="container" ng-controller="CadastroCtrl">

        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                <div class="login-panel panel panel-default">
                    <div class="alert alert-danger" id="msgErro" style="display: none;">
                    </div>
                    <div class="panel-heading">
                        <h3 class="panel-title">Informe seu email e CPF:</h3>
                    </div>
                    <div class="panel-body">
                        <form role="form">
                            <div class="form-group" id="txtNome">
                                <input class="form-control" placeholder="E-mail" name="email" type="text" autofocus ng-model="nome">
                            </div>
                            <div class="form-group" id="txtCPF">
                                <input class="form-control numbers" maxlength="11" placeholder="CPF" name="cpf" type="text" value="" ng-model="cpf">
                            </div>
                            <div class="form-group" id="txtPass">
                                <input class="form-control" maxlength="11" placeholder="Senha com ao menos 6 caracteres" type="password" value="" ng-model="pass">
                            </div>
                            <div class="form-group" id="txtPass2">
                                <input class="form-control" maxlength="11" placeholder="Repita a senha" type="password" value="" ng-model="pass2">
                            </div>

                            <!-- Change this to a button or input when using this as a form -->
                            <a id="btnCad" class="btn btn-lg btn-success btn-block" ng-click="cadastro()">Cadastrar</a>
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
    <script type="text/javascript" src="/Scripts/view/cadastro.js"></script>


</body>
</html>
