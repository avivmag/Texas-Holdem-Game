app.controller('mainController', ['$rootScope', '$scope', '$http', '$state', function mainController($rootScope, $scope, $http, $state) {

    $state.go('login');

    $scope.logout = function () {
        $rootScope.login = false;
        $state.go('login');
    }
}]);