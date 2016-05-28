(function (angular) {
    var modalTicketModule = angular.module('ModalTicketModule', []);

    modalTicketModule.controller('ModalTicketController', function ($scope, $http, close, AuthenticationService, $stateParams) {
        
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
            $scope.ticketCreatedOn = selectedTask.taskFrom;
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
                close('Error');
                alert("Error creating");
            });

        }



        $scope.updateTicket = function () {

            var momentInTime = new Date();
            var dataUpdate = { "TicketID": $scope.ticketId, "TaskUntil": momentInTime, "ProjectID": currentProjectId, "TaskFrom": momentInTime, "TaskPriority": $scope.ticketPriority, "TaskStatus": $scope.ticketStatus, "UserCreatedId": userCreatedId, "TaskName": $scope.ticketName, "UserAssignedId": $scope.ticketAssignedTo, "TaskDescription": $scope.ticketDescription };
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
                alert('Updated');

            }).error(function (error) {
                close('Error');
                alert("Error updating");
            });

        }



        $scope.close = function (result) {
            close(result, 500); // close, but give 500ms for bootstrap to animate
        };

    });
}(angular));