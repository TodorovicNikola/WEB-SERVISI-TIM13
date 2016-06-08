(function (angular) {
    var projectsControllerModule = angular.module('app.Project.controller', ['angularModalService', 'app.Project.resource']);
    
    var projectsController = ['$scope', 'Project', 'ModalService', function ($scope, Project, ModalService) {
        $scope.projects = {}

        $scope.selected = null;
        $scope.selectedIndex = null;

        $scope.unselect = function () {
            $scope.selected = null;
            $scope.selectedIndex = null;
        }

        $scope.selectProject = function (index) {
            if (index != $scope.selectedIndex) {
                $scope.selected = $scope.projects[index];
                $scope.selectedIndex = index;
            } else {
                $scope.unselect();
            }
        }

        $scope.openModal = function (update) {
            ModalService.showModal({
                templateUrl: 'Content/app/projects/projectModal.html',
                controller: 'projectModalController',
                inputs: {
                    selectedproject: update ? $scope.selected : null
                }
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                    if (result !== 'No' && result !== 'Error') {
                        if (!update) {
                            $scope.projects.push(result);
                        } else {
                            $scope.projects[$scope.selectedIndex] = result;
                        }
                    }
                });
            });
        }

        $scope.openUsersModal = function (project) {
            ModalService.showModal({
                templateUrl: 'Content/app/projects/assignUserModal.html',
                controller: 'assignUserModalController',
                inputs: {
                    project: project
                }
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {

                });
            });
        }

        $scope.openReportModal = function (selectedProject, reportType) {
            ModalService.showModal({
                templateUrl: 'Content/app/projects/reports/projectReportModal.html',
                controller: 'reportModalController',
                inputs: {
                    reportType: reportType,
                    selectedProject: selectedProject
                }
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {

                });
            });
        }

        $scope.init = function () {
            var projects = Project.query(function () {
                $scope.projects = projects;
            });
        }
        
        $scope.init();
    }];

    projectsControllerModule.controller('projectsCtrl', projectsController);
})(angular);