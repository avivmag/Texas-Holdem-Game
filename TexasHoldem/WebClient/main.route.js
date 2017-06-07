var app = angular.module("texasHoldem", ['ui.router']);
app.run(function ($rootScope) {
    $rootScope.login = false;
});

app.config(function($stateProvider, $urlRouterProvider) {
    
    $stateProvider
		.state('login', {
			url :           '/login',
			templateUrl:    'login/login.html',
			controller:     'loginCtrl'
			})

		.state('leaderboard', {
			url :           '/leaderboard',
			templateUrl:    'leaderBoard/leaderboard.html',
			controller:     'leaderboardCtrl'
		})
        .state('statistics', {
            url:            '/statistics',
            templateUrl:    'statistics/statistics.html',
            controller:     'statisticsCtrl'
        });
});