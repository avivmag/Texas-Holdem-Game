app.controller("leaderboardCtrl", function ($scope, $http) {

    $scope.leadersQuery = $http({
              method: 'POST',
              url: 'https://localhost:4343/',
              data:
                  {
                      action: 'LeaderBoard',
                  }
    }).then(
          function successCallback(response) {
              $scope.leaders = response.data;
              console.log($scope.leaders);
          }, 
          function errorCallback(response) {
              console.log('fail!');
              $scope.leaders = 'fail';
          });
});