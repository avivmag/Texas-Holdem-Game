app.controller('mainController', ['$scope', '$http', '$state', function mainController($scope, $http, $state) {

    $state.go('login');
}]);