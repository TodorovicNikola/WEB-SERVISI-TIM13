(function (angular) {
    var tasksControllerModule = angular.module('app.TasksCtrl', []);

    var tasksController = ['$scope', 'Tasks', function ($scope, Tasks) {
        $scope.init = function () {
            Tasks.getTasks().success(function (data) {
                $scope.tasks = data;
            });
        }
    $scope.init();
    }];
    tasksControllerModule.controller('TasksCtrl', tasksController);
}(angular));