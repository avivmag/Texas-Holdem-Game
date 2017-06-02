var app = angular.module("texasHoldem", ['ui.router']);
app.config(function($stateProvider, $urlRouterProvider) {
    
    $stateProvider
		.state('login', {
			url : '/login',
			templateUrl: 'login/login.html',
			controller: 'loginCtrl'
			})

		.state('leaderboard', {
			url : '/leaderboard',
			templateUrl: 'leaderBoard/leaderboard.html',
			controller: 'leaderboardCtrl'
			})

        .state('dashboard', {
            url: '/dashboard',
            templateUrl: 'dashboard/dashboard.html',
    });
});