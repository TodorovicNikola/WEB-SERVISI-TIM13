(function (angular) {
    var app = angular.module('app', ['app.controllers', 'app.services', 'ui.router', 'login', 'register', 'angularModalService', 'angularjs-dropdown-multiselect']);
    app.controller('TicketAddingController', function ($scope, ModalService) {
        $scope.show = function (creation) {
            $scope.selectedTaskIndex = $scope.$parent.selectedTaskIndex;
            $scope.selectedTask = $scope.tasks[$scope.selectedTaskIndex];
            $scope.creation = creation;
            ModalService.showModal({
                scope: $scope,
                templateUrl: 'addEditTask.html',
                controller: "ModalTicketController"
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                    console.log(result);
                    if ($scope.creation)
                    {
                        $scope.$parent.tasks.push(result);
                    }
                    else
                    {
                        $scope.$parent.tasks[$scope.selectedTaskIndex] = result;
                    }
                });
            });
        };

    });
    app.controller('ModalTicketController', function ($scope, $http, close, AuthenticationService, $stateParams) {
        var userCreatedId = AuthenticationService.getCurrentUser().username;
        var currentProjectId = $stateParams.id;
        var selectedTask = $scope.selectedTask;
        if (!$scope.creation) {

            $scope.ticketName = selectedTask.taskName;
            $scope.ticketId = selectedTask.ticketId;
            $scope.ticketDescription = selectedTask.taskDescription;
            $scope.ticketPriority = selectedTask.taskPriority;
            $scope.ticketStatus = selectedTask.taskStatus;
            $scope.ticketAssignedTo = selectedTask.userAssigned;
            $scope.ticketToBeFinishedOn = selectedTask.taskUntil;
            $scope.ticketCreatedOn=selectedTask.taskFrom;
            $scope.ticketCreatedBy = selectedTask.userCreated;


        }

        $scope.sendTicket = function () {
         
            var momentInTime = new Date();
            var data = { "TaskUntil": momentInTime, "ProjectID": currentProjectId, "TaskFrom": momentInTime, "TaskPriority": $scope.ticketPriority, "TaskStatus": $scope.ticketStatus, "UserCreatedId": userCreatedId, "TaskName": $scope.ticketName, "UserAssignedId": $scope.ticketAssignedTo, "TaskDescription": $scope.ticketDescription };
            $http.post(
                'api/Projects/' + currentProjectId + '/Tasks',
                JSON.stringify(data),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
            ).success(function (data) {
                close(data);
                alert("Success");
            }).error(function (error) {
                alert("Error");
            });

        }



        $scope.updateTicket = function () {
           
            var momentInTime = new Date();
            var dataUpdate = { "TicketID":$scope.ticketId,"TaskUntil": momentInTime, "ProjectID": currentProjectId, "TaskFrom": momentInTime, "TaskPriority": $scope.ticketPriority, "TaskStatus": $scope.ticketStatus, "UserCreatedId": userCreatedId, "TaskName": $scope.ticketName, "UserAssignedId": $scope.ticketAssignedTo, "TaskDescription": $scope.ticketDescription };
            $http.put(
                'api/Projects/' + currentProjectId + '/Tasks/' + $scope.ticketId,
                JSON.stringify(dataUpdate),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
            ).success(function (data) {
                close(data);
                
            }).error(function (error) {
                alert("Error");
            });

        }



        $scope.close = function (result) {
            close(result, 500); // close, but give 500ms for bootstrap to animate
        };

    });
    app.config(function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/dashboard');
        $stateProvider
	    .state('dashboard', {
	        url: '/dashboard',
	        templateUrl: 'Content/app/partials/dashboard.html',
	        controller: 'DashboardCtrl'
	    })
	    .state('login', {
	        url: '/login',
	        templateUrl: 'Content/app/users/login.html',
	        controller: 'loginCtrl'
	    })
        .state('register', {
            url: '/register',
            templateUrl: 'Content/app/users/register.html',
            controller: 'registerCtrl'
        })
	    .state('tasks', {
	        url: '/projects/:id/tasks',
	        templateUrl: 'Content/app/tasks/views/tasksView.html',
	        controller: 'TasksCtrl'
	    })
        .state('taskDetails', {
            url: '/projects/:id/tasks/:taskId',
            templateUrl: 'Content/app/tasks/views/taskDetailsView.html',
            controller: 'TasksCtrl'
        });
    })
    .run(run);;

    function run($rootScope, $http, $location, $localStorage, AuthenticationService, $state) {
        //postavljanje tokena nakon refresh
        if ($localStorage.currentUser) {
            $http.defaults.headers.common.Authorization = $localStorage.currentUser.token;
        }

        // ukoliko pokušamo da odemo na stranicu za koju nemamo prava, redirektujemo se na login
        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            var publicStates = ['login', 'dashboard', 'register'];
            var restrictedState = publicStates.indexOf(toState.name) === -1;
            if (restrictedState && !AuthenticationService.getCurrentUser()) {
                $state.go('login');
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