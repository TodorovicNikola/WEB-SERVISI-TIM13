(function (angular) {
    var tasksControllerModule = angular.module('app.TasksCtrl', []);
    
    var tasksController = ['$scope', 'Tasks','$stateParams', function($scope, Tasks,$stateParams) {
        console.log('project id ' +$stateParams.id);
        console.log('task id ' +$stateParams.taskId);
        $scope.currentProject = $stateParams.id;
        $scope.currentTask = $stateParams.taskId;
        $scope.init = function () {
            
            Tasks.getTasks($scope.currentProject).success(function (data) {
                $scope.tasks = data;
                
            });

            
        }

        $scope.getTaskDetails = function () {
            
            Tasks.getTask($scope.currentProject,$scope.currentTask).success(function (data) {
                $scope.currentTask = data;
               
            });
        }
       
        //if !==undefined it means that the address was /projects/smtg/tasks
        //and that detail preview was needed
        //else it means that we need to display list of tasks from that project
        if ($scope.currentTask !== undefined) {
            $scope.getTaskDetails();
        }
        else {
            $scope.init();
        }
    }];
    tasksControllerModule.controller('TasksCtrl', tasksController);
}(angular));