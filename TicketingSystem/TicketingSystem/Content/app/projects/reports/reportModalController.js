(function (angular) {
    var reportModalControllerModule = angular.module('app.Project.reportModalController', ['angularModalService', 'chart.js', 'angular-timeline']);

    reportModalControllerModule.controller('reportModalController', function ($scope, ModalService, $http, selectedProject, reportType, close) {
        $scope.reportType = reportType;
        $scope.selectedProject = selectedProject;

        $scope.labels = [];
        $scope.series = [];
        $scope.data = [[]];
        $scope.events = [];

        $scope.init = function () {
            $scope.labels = [];

            $scope.data = [[]];

            if ($scope.reportType === 'assignedPercent' || $scope.reportType === 'finishedPercent') {
                $http.get('../../api/projects/' + $scope.selectedProject.projectID + '/' + $scope.reportType).then(function (response) {
                    for (var i in response.data.users) {
                        $scope.labels.push(response.data.users[i].item1.userName);
                        $scope.data[0].push(response.data.users[i].item2 * 100);
                    }

                    $scope.labels.push('unassigned');
                    $scope.data[0].push(response.data.unassigned * 100);
                });
            }

            if ($scope.reportType === 'created' || $scope.reportType === 'finished') {
                $http.get('../../api/projects/' + $scope.selectedProject.projectID + '/' + $scope.reportType).then(function (response) {
                    console.log(response.data);
                    $scope.events = [];
                    for (var i in response.data) {
                        var badgeCl = '';
                        var badgeIc = '';

                        switch(response.data[i].taskPriority) {
                            case 'Blocker':
                                badgeCl = 'warning';
                                badgeIc = 'glyphicon-warning-sign';
                                break;
                            case 'Critical':
                                badgeCl = 'danger';
                                badgeIc = 'glyphicon-exclamation-sign';
                                break;
                            case 'Major':
                                badgeCl = 'success';
                                badgeIc = 'glyphicon-star';
                                break;
                            case 'Minor':
                                badgeCl = 'primary';
                                badgeIc = 'glyphicon-warning-sign';
                                break;
                            case 'Trivial':
                                badgeCl = 'info';
                                badgeIc = 'glyphicon-warning-sign';
                                break;
                            default:
                                badgeCl = 'default';
                                badgeIc = 'glyphicon-warning-sign';
                        }

                        $scope.events.push({
                            badgeClass: badgeCl,
                            badgeIconClass: badgeIc,
                            time: new Date($scope.reportType === 'created' ? response.data[i].taskCreated : response.data[i].taskFinished),
                            title: response.data[i].taskName,
                            content: response.data[i].taskDescription,
                        });
                    };
                });
            }
        }

        $scope.init();

        $scope.close = function (result) {
            close(result, 200); // close, but give 500ms for bootstrap to animate
        };

    });
}(angular));