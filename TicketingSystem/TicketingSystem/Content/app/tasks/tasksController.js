(function (angular) {
    var tasksControllerModule = angular.module('app.TasksCtrl', ['app.Task.resource','app.Comment.resource']);
    
    var tasksController = ['$scope', 'Tasks', '$stateParams', '$http', 'AuthenticationService', 'Task','Comment', function ($scope, Tasks, $stateParams, $http, AuthenticationService, Task,Comment) {
    
        console.log('project id ' + $stateParams.id);
        console.log('task id ' + $stateParams.taskId);
        $scope.currentProject = $stateParams.id;
        $scope.currentTask = $stateParams.taskId;
        $scope.unselectTask=function()
        {
            $scope.isSelectedTask = false;
            $scope.selectedTaskIndex = -1;

        }

        $scope.selectTask=function(index)
        {
            if ($scope.selectedTaskIndex == index && $scope.isSelectedTask) {
                $scope.unselectTask();
            }
            else {
                $scope.isSelectedTask = true;
                $scope.selectedTaskIndex = index;
               
            }
        }
       

        $scope.getUserName = function () {
            return AuthenticationService.getCurrentUser().username;
        }
        $scope.getUserRole = function () {
            return AuthenticationService.getCurrentUser().role;
        }

        var loadTasks = function () {
            $scope.tasks = Task.getAll({ projectId: $scope.currentProject })
        }

       

        $scope.init = function () {
            $scope.priorities = [{ value: "Blocker", name: "Blocker" }, { value: "Critical", name: "Critical" }, { value: "Major", name: "Major" }, { value: "Minor", name: "Minor" }, { value: "Trivial", name: "Trivial" }];
            $scope.statuses = [{ value: "To do", name: "To do" }, { value: "In progress", name: "In progress" }, { value: "Verify", name: "Verify" }, { value: "Done", name: "Done" }];
            $scope.commentEditing = false;
            $scope.unselectTask();

            loadTasks();
        }

        $scope.getTaskDetails = function () {

            /*Tasks.getTask($scope.currentProject, $scope.currentTask).success(function (data) {
                $scope.currentTask = data;

            });
            */
            $scope.currentTask = Task.get({ projectId: $scope.currentProject, taskId: $scope.currentTask });
        }

        $scope.editComment = function (commentId, index, commentContent) {
            $scope.commentEditing = true;
            $scope.commentId = commentId;
            $scope.commentEditingIndex = index;
            $scope.commentContent = commentContent;

        }
        $scope.quitEditingComment = function () {
            $scope.commentId = null;
            $scope.commentEditing = false;
            $scope.commentEditingIndex = -1;
        }
        $scope.updateComment = function (commentContent) {
            var comments = $scope.currentTask.comments;
            var commentIndex = comments.length - $scope.commentEditingIndex - 1;
            comments[commentIndex].commentContent = commentContent;
            //console.log($scope.commentId + ' ' + $scope.commentEditingIndex + ' ' + commentContent);
            /*var url = "/api/comments/" + $scope.commentId;
            $http.put(url,
                JSON.stringify(comments[commentIndex]),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
                    )
                .success(function (result) {
                    //console.log(result);
                    comments[commentIndex] = result;
                    $scope.quitEditingComment();

                })
                    .error(function () {
                        alert("Error updating");
                    })
                    .then(function () {
                    });
                    */
            Comment.update({ commentId: $scope.commentId }, comments[commentIndex],
                function (result) {
                    console.log('Comment updated');
                    comments[commentIndex] = result;
                    $scope.quitEditingComment();

                }, function () {
                    console.log('Error updating a comment');
                });

        }
        $scope.getTimeString = function (stringTime) {
            if (stringTime === null || stringTime === undefined)
            {
                return null;
            }
            var momentInTime = new Date(stringTime);
            var year = momentInTime.getYear() + 1900;
            var month = momentInTime.getMonth() + 1;
            var day = momentInTime.getDate();
            var hours = momentInTime.getHours();
            var minutes = momentInTime.getMinutes();
            var seconds = momentInTime.getSeconds();
            var dateString = year + '/' + month + '/' + day;
            var timeString = hours + ':' + minutes + ':' + seconds;
            return dateString + ' , ' + timeString;
        }

        $scope.deleteComment = function (commentId, commentIndex) {
            console.log(commentId + ' ' + commentIndex);
            $scope.quitEditingComment();
            var url = "/api/comments/" + commentId;
            /*$http.delete(url)
                .success(function (result) {
                    var comments = $scope.currentTask.comments;
                    $scope.currentTask.comments.splice(comments.length - commentIndex - 1, 1);
                    alert("Delete Successfull");
                })
                .error(function () {
                    alert("error");
                })
                .then(function () {
                    //$window.location = "#/";
                });
                */
            Comment.delete({ commentId: commentId },
               function () {
                   var comments = $scope.currentTask.comments;
                   $scope.currentTask.comments.splice(comments.length - commentIndex - 1, 1);
                   console.log('Deleted comment');
               }, function () {
                   console.log('Error deleting a comment');
               }
            );
        }

            $scope.sendComment = function () {
                var momentInTime = new Date();
                //var data = { "CommentContent": $scope.commentContent, "CommentCreated": momentInTime, "TaskID": $scope.currentTask.ticketID, "ProjectID": $scope.currentProject, "UserWroteID": $scope.getUserName() };
                /*$http.post(
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
                */
            
                var comment = new Comment();
                comment.commentContent = $scope.commentContent;
                comment.commentCreated=momentInTime;
                comment.taskID=$scope.currentTask.ticketID;
                comment.projectID=$scope.currentProject;
                comment.userWroteID = $scope.getUserName();

                comment.$save(
                function (data) {
                    $scope.currentTask.comments.push(data);
                    console.log("Successfully added comment !");
                }, function (error) {

                    console.log("Error adding comment !");
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