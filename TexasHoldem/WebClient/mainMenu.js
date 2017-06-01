(function (angular) {
    'use strict';
    angular.module('formFactory', [])
      .controller('formFactoryController',['$http', function formFactoryController($http) {

          this.hello        = 'hello';
          this.inputType    = 'password';
          this.isPasswordShown = false;

          this.login =
            function login(username, password) {

                $http({
                    method: 'POST',
                    url: 'https://localhost:4343/',
                    data:
                        {
                            action:     'Login',
                            username:   'user',
                            password:   'password'
                        }
                }).then(function successCallback(response) {
                    console.log('success!');
                    console.log(response);
                }, function errorCallback(response) {
                    console.log('fail!');
                    console.log(response);
                });
            }

          this.hideShowPassword = 
              function hideShowPassword() {
                  this.isPasswordShown = !this.isPasswordShown;
                  if (this.inputType === 'password') {
                      this.inputType = 'text';
                  }
                  else {
                      this.inputType = 'password';
                  }
              }
      }]);
})(window.angular);