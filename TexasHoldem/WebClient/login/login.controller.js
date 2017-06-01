app.controller("loginCtrl", function ($scope, $http) {
    $scope.inputType = 'password';
    $scope.isPasswordShown = false;
    $scope.login =
      function login(username, password) {

          $http({
              method: 'POST',
              url: 'https://localhost:4343/',
              data:
                  {
                      action: 'Login',
                      username: 'user',
                      password: 'password'
                  }
          }).then(function successCallback(response) {
              console.log('success!');
              console.log(response);
          }, function errorCallback(response) {
              console.log('fail!');
              console.log(response);
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