(function (angular) {
    var tasksControllerModule = angular.module('app.TasksCtrl', []);
    
    var tasksController = ['$scope', 'Tasks','$stateParams', function($scope, Tasks,$stateParams) {
        console.log($stateParams.id);
        $scope.currentProject = $stateParams.id;
        $scope.init = function () {
            console.log($scope.currentProject);
            Tasks.getTasks($scope.currentProject).success(function (data) {
                $scope.tasks = data;
                console.log($stateParams.id);
            });
        }
       
        $scope.init();
    }];
    tasksControllerModule.controller('TasksCtrl', tasksController);
}(angular));