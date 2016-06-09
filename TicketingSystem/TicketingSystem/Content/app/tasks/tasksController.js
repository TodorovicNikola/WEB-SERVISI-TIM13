(function (angular) {
    var tasksControllerModule = angular.module('app.Task.controller', ['app.Task.resource', 'app.Comment.resource', 'angularModalService', 'app.Project.resource']);

    var tasksController = ['$scope', 'Tasks', '$stateParams', '$http', 'AuthenticationService', 'Task', 'Comment', 'Project', 'ModalService', function ($scope, Tasks, $stateParams, $http, AuthenticationService, Task, Comment, Project, ModalService) {

        $scope.currentProject = $stateParams.id;
        $scope.dummyProject = {};
        $scope.dummyProject.projectID = $scope.currentProject;
        $scope.currentTask = $stateParams.taskId;
        $scope.projectData = {};

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


        $scope.show = function (creation, from) {
            if (from === 0) {
                $scope.selectedTaskIndex = $scope.selectedTaskIndex;
                $scope.selectedTask = $scope.tasks[$scope.selectedTaskIndex];
            }
            else {
                $scope.selectedTask = $scope.currentTask;
            }
            $scope.creation = creation;

            ModalService.showModal({
                scope: $scope,
                templateUrl: 'Content/app/tasks/views/addEditTask.html',
                controller: "ModalTicketController"
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                    if (result !== 'No' && result !== 'Error') {

                        if ($scope.creation) {

                            $scope.tasks.push(result);
                        }
                        else {
                            if (from === 0) {
                                $scope.tasks[$scope.selectedTaskIndex] = result.taskDto;
                            }
                            else {
                                var changes = angular.copy($scope.currentTask.changes);
                                var comments = angular.copy($scope.currentTask.comments);
                                $scope.currentTask = result.taskDto;
                                $scope.currentTask.userAssignedID = result.taskDto.userAssigned;
                                $scope.currentTask.userCreatedID = result.taskDto.userCreated;
                                $scope.currentTask.ticketID = result.taskDto.ticketId;
                                changes.push(result.changeDto);
                                $scope.currentTask.changes = changes;
                                $scope.currentTask.comments = comments;

                                console.log(result.ticketDto);

                            }

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
        $scope.getUserRole = function () {
            return AuthenticationService.getCurrentUser().role;
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
            $scope.commentEditing = false;
            $scope.unselectTask();

            loadTasks();
        }

        $scope.getTaskDetails = function () {


            Task.get({
                projectId: $scope.currentProject, taskId: $scope.currentTask
            }, function (data) {
                $scope.currentTask = data;
            }, function (error) {
                console.log('Error while getting tasks of specific project');
            }
            );
        }

        $scope.editComment = function (commentId, index, commentContent) {
            $scope.commentEditing = true;
            $scope.commentId = commentId;
            $scope.commentEditingIndex = index;
            $scope.commentContent = commentContent;

        }
        $scope.quitEditingComment = function () {
            $scope.commentId = null;
            $scope.commentEditing = false;
            $scope.commentEditingIndex = -1;
        }
        $scope.updateComment = function (commentContent) {
            var comments = $scope.currentTask.comments;
            var commentIndex = comments.length - $scope.commentEditingIndex - 1;
            comments[commentIndex].commentContent = commentContent;

            Comment.update({
                commentId: $scope.commentId
            }, comments[commentIndex],
                function (result) {
                    console.log('Comment updated');
                    comments[commentIndex] = result;
                    $scope.quitEditingComment();

                }, function () {
                    console.log('Error updating a comment');
                });

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

        $scope.deleteComment = function (commentId, commentIndex) {
            console.log(commentId + ' ' + commentIndex);
            $scope.quitEditingComment();
            var url = "/api/comments/" + commentId;

            Comment.delete({
                commentId: commentId
            },
               function () {
                   var comments = $scope.currentTask.comments;
                   $scope.currentTask.comments.splice(comments.length - commentIndex - 1, 1);
                   console.log('Deleted comment');
               }, function () {
                   console.log('Error deleting a comment');
               }
            );
        }

        $scope.sendComment = function () {
            var momentInTime = new Date();


            var comment = new Comment();
            comment.commentContent = $scope.commentContent;
            comment.commentCreated = momentInTime;
            comment.taskID = $scope.currentTask.ticketID;
            comment.projectID = $scope.currentProject;
            comment.userWroteID = $scope.getUserName();

            comment.$save(
            function (data) {
                $scope.currentTask.comments.push(data);
                console.log("Successfully added comment !");
            }, function (error) {

                console.log("Error adding comment !");
            });


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
        //if !==undefined it means that the address was /projects/smtg/tasks
        //and that detail preview was needed
        //else it means that we need to display list of tasks from that project
        if ($scope.currentTask !== undefined) {

            $scope.getTaskDetails();
        }
        else {
            $scope.init();
            $scope.getProjectDetails($scope.currentProject);

        }
    }];
    tasksControllerModule.controller('TasksCtrl', tasksController);
}(angular));