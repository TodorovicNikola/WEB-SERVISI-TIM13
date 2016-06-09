(function (angular) {
    var app = angular.module('app', ['ui.router', 'login', 'angularjs-dropdown-multiselect', 'app.User', 'app.Project', 'app.Task','app.Dashboard', 'ui.bootstrap']);

    app.config(function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/dashboard');
        $stateProvider
	    .state('dashboard', {
	        url: '/dashboard',
	        templateUrl: 'Content/app/dashboard/dashboard.html',
	        controller: 'DashboardCtrl'
	    })
	    .state('login', {
	        url: '/login',
	        templateUrl: 'Content/app/users/login.html',
	        controller: 'loginCtrl'
	    })
	    .state('tasks', {
	        url: '/projects/:id/tasks',
	        templateUrl: 'Content/app/tasks/views/tasksView.html',
	        controller: 'TasksCtrl'
	    })
        .state('taskDetails', {
            url: '/projects/:id/tasks/:taskId',
            templateUrl: 'Content/app/tasks/views/taskDetailsView.html',
            controller: 'TaskDetailCtrl'
        })
        .state('users', {
            url: '/users',
            templateUrl: 'Content/app/users/usersView.html',
            controller: 'usersCtrl'
        })
        .state('projects', {
            url: '/projects',
            templateUrl: 'Content/app/projects/projectsView.html',
            controller: 'projectsCtrl'
        })
        ;
    })
    .run(run);;

    function run($rootScope, $http, $location, $localStorage, AuthenticationService, $state) {
        //postavljanje tokena nakon refresh
        if ($localStorage.currentUser) {
            $http.defaults.headers.common.Authorization = 'Bearer ' + $localStorage.currentUser.token;
        }

        // ukoliko pokušamo da odemo na stranicu za koju nemamo prava, redirektujemo se na login
        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            var publicStates = ['login'];//, 'register'];
            var restrictedState = publicStates.indexOf(toState.name) === -1;
            if (restrictedState && !AuthenticationService.getCurrentUser()) {
                $state.go('login');
            }

            var adminStates = ['projects', 'users'];
            restrictedState = adminStates.indexOf(toState.name) !== -1;
            if (restrictedState && AuthenticationService.getCurrentUser() && $rootScope.getCurrentUserRole() != 'Admin') {
                $state.go('dashboard');
            }
        });

        $rootScope.logout = function () {
            AuthenticationService.logout();
        }

        $rootScope.getCurrentUserRole = function () {
            if (!AuthenticationService.getCurrentUser()) {
                return undefined;
            }
            else {
                return AuthenticationService.getCurrentUser().role;
            }
        }
        $rootScope.isLoggedIn = function () {
            if (AuthenticationService.getCurrentUser()) {
                return true;
            }
            else {
                return false;
            }
        }
        $rootScope.getCurrentState = function () {
            return $state.current.name;
        }
    }


}(angular));