function MasterCtrl($scope, $http) {

    $scope.username = '';
    $scope.isAdmin = false;

    $scope.init = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/login/GetNomePessoaCurrentUser'
        }).success(function (data, status) {
            $scope.username = data;

            var httpRequest = $http({
                method: 'GET',
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                url: _baseURL + '/request/login/CurrentUserIsAdmin'
            }).success(function (data, status) {
                if (data == 'True')
                    $scope.isAdmin = true;
            });
        });
    }

    $scope.logout = function () {
        var httpRequest = $http({
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: _baseURL + '/request/login/Logout'
        }).success(function (data, status) {
            window.location = _baseURL + "/meuperfil";
        });
    }

    $scope.init();
}