(function (angular) {
    var tasksControllerModule = angular.module('app.TasksCtrl', []);
    
    var tasksController = ['$scope', 'Tasks','$stateParams','$http','AuthenticationService', function($scope, Tasks,$stateParams,$http,AuthenticationService) {
        console.log('project id ' +$stateParams.id);
        console.log('task id ' +$stateParams.taskId);
        $scope.currentProject = $stateParams.id;
        $scope.currentTask = $stateParams.taskId;
        //$scope.userId = AuthenticationService.getCurrentUserId;
        $scope.getUserName=function()
        {
            return AuthenticationService.getCurrentUser().username;
        }
       
        $scope.init = function () {
            $scope.commentEditing = false;
            
            Tasks.getTasks($scope.currentProject).success(function (data) {
                $scope.tasks = data;
                
            });

            
        }

        $scope.getTaskDetails = function () {
            
            Tasks.getTask($scope.currentProject,$scope.currentTask).success(function (data) {
                $scope.currentTask = data;
               
            });
        }

        $scope.editComment=function(commentId,commentContent)
        {
            $scope.commentEditing=true;
            $scope.commentId=commentId;
            $scope.commentContent = commentContent;
            console.log(commentContent);
        }
        $scope.quitEditingComment=function()
        {
            $scope.commentId = null;
            $scope.commentEditing = false;
        }
        $scope.updateComment = function (commentId, commentContent) {
            $scope.quitEditingComment();
           
        }

        $scope.sendComment = function () {
  
            var data = { "CommentContent": $scope.commentContent, "CommentCreated": "2016-05-21T00:00:00", "CommentUpdated": "2016-05-21T00:00:00", "TaskID": 1, "ProjectID": $scope.currentProject, "UserWroteID": $scope.userId };
            $http.post(
                '/api/Comments',
                JSON.stringify(data),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
            ).success(function (data) {
             
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