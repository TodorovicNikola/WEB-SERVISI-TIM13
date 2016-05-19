(function(angular) {
    var app = angular.module('app', ['app.controllers', 'app.services', 'ui.router']);

	app.config(function($stateProvider, $urlRouterProvider) {
	    $urlRouterProvider.otherwise('/main');
	    $stateProvider
	    .state('main', {
			url: '/main',
			templateUrl: 'partials/main.html',
			controller: 'HelloWorldCtrl'

	    })
	    .state('dashboard', {
			url: '/dashboard',
			templateUrl: 'partials/dashboard.html',
			controller: 'DashboardCtrl'
	    })
        .state('tasks', {
            url: '/tasks',
            templateUrl: 'partials/tasksView.html',
            controller: 'TasksCtrl'
        });
  	});

}(angular));