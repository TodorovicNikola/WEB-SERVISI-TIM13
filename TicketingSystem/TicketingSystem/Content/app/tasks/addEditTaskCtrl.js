(function (angular){
    var tasksUpdatingModule = angular.module('UpdatingTasksModule', ['angularModalService', 'angularjs-datetime-picker']);

tasksUpdatingModule.controller('TicketAddingController', function ($scope, ModalService) {
   
   
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
                if (result !== 'No' && result !== 'Error') {
                    if ($scope.creation) {
                        $scope.$parent.tasks.push(result);
                    }
                    else {
                        $scope.$parent.tasks[$scope.selectedTaskIndex] = result;
                    }
                }
            });
        });



    };
});
}(angular));