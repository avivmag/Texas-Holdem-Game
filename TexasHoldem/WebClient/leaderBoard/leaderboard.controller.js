app.controller("leaderboardCtrl", function ($rootScope, $scope, $http) {

    if ($rootScope.login) {
        $scope.totalGrossProfitQuery = $http({
            method: 'POST',
            url: 'https://localhost:4343/',
            data:
                {
                    action: 'LeaderBoard',
                    param: 'totalGrossProfit'
                }
        }).then(
              function successCallback(response) {
                  $scope.totalGrossleaders = response.data;
              },
              function errorCallback(response) {
                  alert('Could not fetch total gross profit leaders.');
              });

        $scope.highestCashInAGameQuery = $http({
            method: 'POST',
            url: 'https://localhost:4343/',
            data:
                {
                    action: 'LeaderBoard',
                    param: 'HighetsCashInAGame'
                }
        }).then(
              function successCallback(response) {
                  $scope.HighestCashleaders = response.data;
              },
              function errorCallback(response) {
                  alert('Could not fetch highest cash in a game leaders.');
              });

        $scope.totalGamesPlayedQuery = $http({
            method: 'POST',
            url: 'https://localhost:4343/',
            data:
                {
                    action: 'LeaderBoard',
                    param: 'gamesPlayed'
                }
        }).then(
              function successCallback(response) {
                  $scope.totalGamesPlayedleaders = response.data;
              },
              function errorCallback(response) {
                  alert('Could not fetch total gamesPlayed leaders.');
              });
    }
    else {
        alert("Could not fetch data. you are not logged in.");
    }
});