(function (angular) {
    var modalTicketModule = angular.module('app.Task.modalTicketController', ['app.Task.resource', 'app.ProjectUsers.resource', 'ui.bootstrap.datetimepicker']);

    modalTicketModule.controller('ModalTicketController', function ($scope, $http, close, AuthenticationService, $stateParams, Task, ProjectUsers, selectedTask) {
        $scope.message = '';
        $scope.selectedTask = selectedTask;
        var userCreatedId = AuthenticationService.getCurrentUser().username;
        var currentProjectId = $stateParams.id;
        //var selectedTask = $scope.selectedTask;
        $scope.priorities = [{ value: "Blocker", name: "Blocker" }, { value: "Critical", name: "Critical" }, { value: "Major", name: "Major" }, { value: "Minor", name: "Minor" }, { value: "Trivial", name: "Trivial" }];
        $scope.statuses = [{ value: "To Do", name: "To Do" }, { value: "In Progress", name: "In Progress" }, { value: "Verify", name: "Verify" }, { value: "Done", name: "Done" }];


        if (selectedTask) {
            $scope.ticketName = selectedTask.taskName;
            if (selectedTask.ticketId != null) {
                $scope.ticketId = selectedTask.ticketId;
                $scope.ticketAssignedTo = selectedTask.userAssigned;
                $scope.ticketCreatedBy = selectedTask.userCreated;
            }
            else {
                $scope.ticketId = selectedTask.ticketID;
                $scope.ticketAssignedTo = selectedTask.userAssignedID;
                $scope.ticketCreatedBy = selectedTask.userCreatedID;
            }

            $scope.taskNumber = selectedTask.taskNumber;
            $scope.ticketDescription = selectedTask.taskDescription;
            $scope.ticketPriority = selectedTask.taskPriority;
            $scope.ticketStatus = selectedTask.taskStatus;

            $scope.ticketToBeFinishedOn = selectedTask.taskUntil;
            $scope.ticketCreatedOn = selectedTask.taskCreated;
        }

        $scope.getUsersOnThisTaskProject = function () {
            ProjectUsers.getUsersOnProject({ projectId: currentProjectId },
                function (response) {
                    $scope.usersOnProject = response;
                },
                function (error) {
                    alert('Unable to get users of project');
                }
            );
        }

        $scope.getUsersOnThisTaskProject();

        $scope.sendTicket = function () {
            var momentInTime = new Date();
            var ticket = new Task();

            ticket.projectId = currentProjectId;
            ticket.taskFrom = momentInTime;
            ticket.taskDescription = $scope.ticketDescription;
            ticket.taskPriority = $scope.ticketPriority;
            ticket.userCreatedId = userCreatedId;
            ticket.taskName = $scope.ticketName;
            ticket.userAssignedId = $scope.ticketAssignedTo;
            ticket.taskStatus = $scope.ticketStatus;
            ticket.taskUntil = $scope.ticketToBeFinishedOn;

            ticket.$save(
             function (data) {
                 $scope.close(data);
                 //alert("Success !");
             }, function (error) {
                 $scope.message = error.data.message;
                 //close('Error');
                 //alert("Error creating ticket !");
             });
        }

        $scope.validateTicketForm = function () {
            if (!$scope.ticketName || !$scope.ticketPriority || !$scope.ticketStatus || !$scope.ticketToBeFinishedOn) {
                return false;
            }
            return true;
        }


        $scope.updateTicket = function () {
            var dataUpdate = { "TicketID": $scope.ticketId, "TaskUntil": $scope.ticketToBeFinishedOn, "ProjectID": currentProjectId, "TaskCreated": $scope.ticketCreatedOn, "TaskPriority": $scope.ticketPriority, "TaskStatus": $scope.ticketStatus, "UserCreatedId": $scope.ticketCreatedBy, "TaskName": $scope.ticketName, "UserAssignedId": $scope.ticketAssignedTo, "TaskDescription": $scope.ticketDescription, "TaskNumber": $scope.taskNumber };

            Task.update({ projectId: currentProjectId, taskId: $scope.ticketId }, dataUpdate,
               function (data) {
                   $scope.close(data);
                   //alert('Updated !');
               }, function (error) {
                   $scope.message = error.data.message;
                   //close('Error');
                   //alert("Error updating");
               });
        }

        $scope.mytime = new Date();
        $scope.hstep = 1;
        $scope.mstep = 15;

        $scope.open = function () {
            $scope.popup = {};
            $scope.popup.opened = true;
        };

        $scope.close = function (result) {
            console.log(result);
            close(result, 200);
        };

    });
}(angular));