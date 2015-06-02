<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AET.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <title>Login</title>

    <!-- Core CSS - Include with every page -->
    <link href="/css/bootstrap.min.css" rel="stylesheet">
    <link href="/font-awesome/css/font-awesome.css" rel="stylesheet">
    <!-- SB Admin CSS - Include with every page -->
    <link href="/css/sb-admin.css" rel="stylesheet">
</head>
<body ng-app="aet">
    <div class="container" ng-controller="LoginCtrl">
        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                <div class="login-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Por favor faça login :</h3>
                    </div>
                    <div class="panel-body">
                        <form role="form">
                            <fieldset>
                                <div class="form-group" id="txtNome">
                                    <input class="form-control" placeholder="E-mail" name="email" type="text" autofocus ng-model="nome">
                                </div>
                                <div class="form-group" id="txtPass">
                                    <input class="form-control" placeholder="Password" name="password" type="password" value="" ng-model="senha">
                                </div>
                                <div class="form-group" id="Div1">
                                    <a href="/lembrarsenha">Esqueci a senha</a>
                                </div>


                                <!-- Change this to a button or input when using this as a form -->
                                <a id="btnLogin" class="btn btn-lg btn-success btn-block" ng-click="auth()">Login</a>
                                <br />
                                <h4 style="text-align: center;">OU</h4>
                                <br />
                                <a class="btn btn-sm btn-info btn-block" href="/cadastro">Criar Cadastro</a>
                            </fieldset>
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
    <script type="text/javascript" src="/Scripts/view/login.js"></script>


</body>
</html>
