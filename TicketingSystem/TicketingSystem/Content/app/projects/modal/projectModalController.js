(function (angular) {
    var projectModalControllerModule = angular.module('app.Project.projectModalController', ['angularModalService', 'app.Project.resource']);

    projectModalControllerModule.controller('projectModalController', function ($scope, ModalService, selectedproject, Project, close) {
        $scope.selectedproject = selectedproject;

        if ($scope.selectedproject) {
            $scope.projectName = $scope.selectedproject.projectName;
            $scope.projectCode = $scope.selectedproject.projectCode;
            $scope.projectDescription = $scope.selectedproject.projectDescription;
            $scope.projectID = $scope.selectedproject.projectID;
        }

        $scope.save = function () {
            if (!$scope.selectedproject) {
                var newproject = new Project();
                newproject.projectName = $scope.projectName;
                newproject.projectCode = $scope.projectCode;
                newproject.projectDescription = $scope.projectDescription;

                newproject.$save(function () {
                    $scope.close(newproject);
                });
            } else {
                var newproject = {};
                newproject.projectName = $scope.projectName;
                newproject.projectCode = $scope.projectCode;
                newproject.projectDescription = $scope.projectDescription;
                newproject.projectID = $scope.projectID;

                Project.update({ projectId: $scope.projectID }, newproject, function () {
                    $scope.close(newproject);
                });
            }
        }

        $scope.validateForm = function () {

            if (!$scope.projectName || !$scope.projectCode || !$scope.projectDescription) {
                return false;
            }
            return true;
        }

        $scope.close = function (result) {
            close(result, 200); // close, but give 500ms for bootstrap to animate
        };

    });
}(angular));