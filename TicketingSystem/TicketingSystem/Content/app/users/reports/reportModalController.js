(function (angular) {
    var userReportModalControllerModule = angular.module('app.User.reportModalController', ['angularModalService', 'angular-timeline']);

    userReportModalControllerModule.controller('userReportModalController', function ($scope, ModalService, $http, selectedUser, close) {
        $scope.selectedUser = selectedUser;

        $scope.events = [];

        $scope.init = function () {

            $http.get('../../api/users/' + $scope.selectedUser.userName + '/finished').then(function (response) {
                console.log(response.data);
                $scope.events = [];
                for (var i in response.data) {
                    var badgeCl = '';
                    var badgeIc = '';

                    switch (response.data[i].taskPriority) {
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
                        time: new Date(response.data[i].taskFinished),
                        title: response.data[i].taskName,
                        content: response.data[i].taskDescription,
                    });
                };
            });

        }

        $scope.init();

        $scope.close = function (result) {
            close(result, 200); // close, but give 500ms for bootstrap to animate
        };

    });
}(angular));