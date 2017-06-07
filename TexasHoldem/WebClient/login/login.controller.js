app.controller("loginCtrl", function ($scope, $http, $state, $rootScope) {
    $scope.inputType        = 'password';
    $scope.isPasswordShown  = false;
    $scope.login =
      function login(username, password) {

          $http({
              method: 'POST',
              url: 'https://localhost:4343/',
              data:
                  {
                      action: 'Login',
                      username,
                      password
                  }
          }).then(function successCallback(response) {
              $rootScope.login = true;
              alert("You are now logged in!");
          }, function errorCallback(response) {
              alert("Could not log in. Try again later.");
          });
      }

    $scope.hideShowPassword =
        function hideShowPassword() {
            this.isPasswordShown = !this.isPasswordShown;
            if (this.inputType === 'password') {
                this.inputType = 'text';
            }
            else {
                this.inputType = 'password';
            }
        }
});