(function (angular) {
    var deleteConfirmControllerModule = angular.module('app.Task.deleteConfirmController', ['angularModalService']);

    deleteConfirmControllerModule.controller('deleteConfirmController', function ($scope, close) {
        $scope.close = function (result) {
            close(result, 200); // close, but give 500ms for bootstrap to animate
        };

    });
}(angular));