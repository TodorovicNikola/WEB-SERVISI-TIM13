(function (angular) {
    var assignUserModalControllerModule = angular.module('app.Project.assignUserModalController', ['angularModalService', 'app.Project.resource', 'app.UserProject.resource', 'app.User.resource']);

    assignUserModalControllerModule.controller('assignUserModalController', function ($scope, ModalService, project, Project, User, UserProject, close) {
        $scope.project = project;
       
        $scope.users = null;
        $scope.assignedUsers = null;

        $scope.init = function () {
            var users = User.query(function () {
                $scope.users = [];

                var mapUsers = {};
                for (var u in users) {
                    if (!isNaN(u)) {
                        mapUsers[users[u].userName] = users[u];
                        mapUsers[users[u].userName].assigned = false;
                    }
                }

                $scope.assignedUsers = UserProject.getAll({ projectId: $scope.project.projectID }, function () {
                    for (var au in $scope.assignedUsers) {
                        if (!isNaN(au)) {
                            mapUsers[$scope.assignedUsers[au].userName].assigned = true;
                        }
                    }

                    for (var i in mapUsers) {
                        $scope.users.push(mapUsers[i]);
                    }
                    console.log(users);
                });
            });
        }

        $scope.init();

        $scope.remove = function (username) {
            var up = new UserProject({ projectId: $scope.project.projectID, userId: username });
            up.$delete(function () {
                $scope.update(username, false);
            });
        };

        $scope.assign = function (username) {
            var up = new UserProject({ projectId: $scope.project.projectID, userId: username });
            up.$save(function () {
                $scope.update(username, true);
            });
        }

        $scope.close = function (result) {
            close(result, 200); // close, but give 500ms for bootstrap to animate
        };

        $scope.update = function (username, status) {
            for(var u in $scope.users) {
                if ($scope.users[u].userName === username) {
                    $scope.users[u].assigned = status;
                }
            }
        };

    });
}(angular));