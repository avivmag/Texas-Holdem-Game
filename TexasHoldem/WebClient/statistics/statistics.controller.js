app.controller("statisticsCtrl", function ($rootScope, $scope, $http) {

    if ($rootScope.login) {
        $scope.getSystemUsers = $http({
                method: 'POST',
                url: 'https://localhost:4343/',
                data:
                    {
                        action: 'GetUsersDetails'
                    }
            }).then(function successCallback(response) {
                $scope.users = response.data;
            }, function errorCallback(response) {
                console.log(response);
                alert("Could not fetch users list.");
            });
    }
    else {
        alert("Could not fetch data. you are not logged in.");
    }
});