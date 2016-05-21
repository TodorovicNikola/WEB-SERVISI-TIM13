(function (angular) {
    var tasksControllerModule = angular.module('app.TasksCtrl', []);
    
    var tasksController = ['$scope', 'Tasks','$stateParams','$http','AuthenticationService', function($scope, Tasks,$stateParams,$http,AuthenticationService) {
        console.log('project id ' +$stateParams.id);
        console.log('task id ' +$stateParams.taskId);
        $scope.currentProject = $stateParams.id;
        $scope.currentTask = $stateParams.taskId;
       
       
        $scope.init = function () {
            
            
            Tasks.getTasks($scope.currentProject).success(function (data) {
                $scope.tasks = data;
                
            });

            
        }

        $scope.getTaskDetails = function () {
            
            Tasks.getTask($scope.currentProject,$scope.currentTask).success(function (data) {
                $scope.currentTask = data;
               
            });
        }

      
        $scope.sendComment = function () {
            var data = { "CommentContent": $scope.commentContent, "CommentCreated": "2016-05-21T00:00:00", "CommentUpdated": "2016-05-21T00:00:00", "TaskID": 1, "ProjectID": $scope.currentProject, "UserWroteID": 'admin' };
            $http.post(
                '/api/Comments',
                JSON.stringify(data),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
            ).success(function (data) {
                //console.log(data);
                //console.log($scope.currentTask);
                $scope.currentTask.comments.push(data);
            });
        }
       
        //if !==undefined it means that the address was /projects/smtg/tasks
        //and that detail preview was needed
        //else it means that we need to display list of tasks from that project
        if ($scope.currentTask !== undefined) {
            $scope.getTaskDetails();
        }
        else {
            $scope.init();
        }
    }];
    tasksControllerModule.controller('TasksCtrl', tasksController);
}(angular));