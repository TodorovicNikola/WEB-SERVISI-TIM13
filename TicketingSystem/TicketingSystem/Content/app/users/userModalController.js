(function (angular) {
    var userModalControllerModule = angular.module('app.User.userModalController', ['angularModalService', 'app.User.resource']);

    userModalControllerModule.controller('userModalController', function ($scope, ModalService, selectedUser, User, close) {
        $scope.selectedUser = selectedUser;

        if ($scope.selectedUser) {
            $scope.userName = $scope.selectedUser.userName;
            $scope.password = $scope.selectedUser.password;
            $scope.repeatedPassword = $scope.selectedUser.password;
            $scope.firstName = $scope.selectedUser.firstName;
            $scope.lastName = $scope.selectedUser.lastName;
            $scope.email = $scope.selectedUser.email;
            $scope.selectedUser.disable = true;
        }

        $scope.save = function () {
            if (!$scope.selectedUser) {
                var newUser = new User({ userId: $scope.userName });
                newUser.userName = $scope.userName;
                newUser.password = $scope.password;
                newUser.firstName = $scope.firstName;
                newUser.lastName = $scope.lastName;
                newUser.email = $scope.email;

                newUser.$save(function () {
                    $scope.close(newUser);
                });
            } else {
                var newUser = {};
                newUser.userName = $scope.userName;
                newUser.password = $scope.password;
                newUser.firstName = $scope.firstName;
                newUser.lastName = $scope.lastName;
                newUser.email = $scope.email;

                User.update({ userId: $scope.userName }, newUser, function () {
                    $scope.close(newUser);
                });
            }
        }

        $scope.validateForm = function () {

            if (!$scope.userName || !$scope.password || !$scope.repeatedPassword || !$scope.firstName
                || !$scope.lastName || !$scope.email || ($scope.password === !$scope.repeatedPassword)) {
                return false;
            }
            return true;
        }

        $scope.close = function (result) {
            close(result, 500); // close, but give 500ms for bootstrap to animate
        };

    });
}(angular));