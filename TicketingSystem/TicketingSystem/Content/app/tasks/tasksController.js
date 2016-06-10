(function (angular) {
    var tasksControllerModule = angular.module('app.Task.controller', ['app.Task.resource', 'app.Comment.resource', 'angularModalService', 'app.Project.resource']);

    var tasksController = ['$scope', '$stateParams', '$http', 'AuthenticationService', 'Task', 'Comment', 'Project', 'ModalService', function ($scope, $stateParams, $http, AuthenticationService, Task, Comment, Project, ModalService) {

        $scope.currentProject = $stateParams.id;
        $scope.dummyProject = {};
        $scope.dummyProject.projectID = $scope.currentProject;
        $scope.currentTask = $stateParams.taskId;
        $scope.projectData = {};

        $scope.deleteTask = function () {
            ModalService.showModal({
                templateUrl: 'Content/app/tasks/modal/deleteConfirm.html',
                controller: "deleteConfirmController"
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                    $('body').removeClass('modal-open');
                    if (result) {
                        $scope.selectedTask = $scope.tasks[$scope.selectedTaskIndex];
                        Task.delete({
                            taskId: $scope.selectedTask.ticketId,
                            projectId: $scope.currentProject
                        },
                           function () {
                               $scope.tasks.splice($scope.selectedTaskIndex, 1)
                               console.log($scope.selectedTaskIndex);
                               console.log('Deleted task');
                           }, function () {
                               console.log('Error deleting a task');
                           }
                        );
                    }
                })
            });
        }
        $scope.unselectTask = function () {
            $scope.isSelectedTask = false;
            $scope.selectedTaskIndex = -1;

        }

        $scope.selectTask = function (index) {
            if ($scope.selectedTaskIndex == index && $scope.isSelectedTask) {
                $scope.unselectTask();
            }
            else {
                $scope.isSelectedTask = true;
                $scope.selectedTaskIndex = index;
            }
        }


        $scope.show = function (creation) {

            $scope.selectedTaskIndex = $scope.selectedTaskIndex;
            $scope.selectedTask = $scope.tasks[$scope.selectedTaskIndex];
            $scope.creation = creation;

            ModalService.showModal({
                templateUrl: 'Content/app/tasks/modal/addEditTask.html',
                controller: "ModalTicketController",
                inputs: {
                    selectedTask: creation ? null : $scope.selectedTask
                }
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                    $('body').removeClass('modal-open');
                    if (result !== 'No' && result !== 'Error' && result != 'Cancel') {

                        if ($scope.creation) {
                            $scope.tasks.push(result);
                        }
                        else {
                            $scope.tasks[$scope.selectedTaskIndex] = result.taskDto;
                        }
                    }
                });
            });

        };



        $scope.openUsersModal = function () {
            ModalService.showModal({
                templateUrl: 'Content/app/projects/modal/assignUserModal.html',
                controller: 'assignUserModalController',
                inputs: {
                    project: $scope.dummyProject
                }
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                });
            });
        }

        $scope.getUserName = function () {
            return AuthenticationService.getCurrentUser().username;
        }

        var loadTasks = function () {
            Task.getAll({ projectId: $scope.currentProject },
             function (data) {
                 $scope.tasks = data;
             }, function (error) {
                 console.log('Error while fetching tasks');
             });
        }

        $scope.init = function () {
            $scope.priorities = [{ value: "Blocker", name: "Blocker" }, { value: "Critical", name: "Critical" }, { value: "Major", name: "Major" }, { value: "Minor", name: "Minor" }, {
                value: "Trivial", name: "Trivial"
            }];
            $scope.statuses = [{ value: "To do", name: "To do" }, { value: "In progress", name: "In progress" }, { value: "Verify", name: "Verify" }, {
                value: "Done", name: "Done"
            }];
            $scope.unselectTask();

            loadTasks();
        }

        $scope.getTimeString = function (stringTime) {
            if (stringTime === null || stringTime === undefined) {
                return null;
            }
            var momentInTime = new Date(stringTime);
            var year = momentInTime.getYear() + 1900;
            var month = momentInTime.getMonth() + 1;
            var day = momentInTime.getDate();
            var hours = momentInTime.getHours();
            var minutes = momentInTime.getMinutes();
            var seconds = momentInTime.getSeconds();
            var dateString = year + '/' + month + '/' + day;
            var timeString = hours + ':' + minutes + ':' + seconds;
            return dateString + ' , ' + timeString;
        }

        $scope.getProjectDetails = function (projectId) {

            Project.get({
                projectId: projectId
            },
            function (data) {
                $scope.projectData = data;
            },
            function (error) {
                console.log('Error while getting project details');
            }
            )
        }

        $scope.init();
        $scope.getProjectDetails($scope.currentProject);


    }];
    tasksControllerModule.controller('TasksCtrl', tasksController);
}(angular));