(function (angular) {
    var projectModalControllerModule = angular.module('app.Project.projectModalController', ['angularModalService', 'app.Project.resource']);

    projectModalControllerModule.controller('projectModalController', function ($scope, ModalService, selectedproject, Project, close, $element) {
        $scope.selectedproject = selectedproject;
        $scope.message = '';

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

                newproject.$save(
                    function () {
                        $element.modal('hide');
                        $scope.close(newproject);
                    },
                    function (response) {
                        $scope.message = response.data.message;
                    }
                );
            } else {
                var newproject = {};
                newproject.projectName = $scope.projectName;
                newproject.projectCode = $scope.projectCode;
                newproject.projectDescription = $scope.projectDescription;
                newproject.projectID = $scope.projectID;

                Project.update({ projectId: $scope.projectID }, newproject,
                    function () {
                        $element.modal('hide');
                        $scope.close(newproject);
                    },
                    function (response) {
                        $scope.message = response.data.message;
                    }
                );
            }
        }

        $scope.validateForm = function () {

            if (!$scope.projectName || !$scope.projectCode) {
                return false;
            }
            return true;
        }

        $scope.close = function (result) {
            close(result, 200); // close, but give 500ms for bootstrap to animate
        };

    });
}(angular));