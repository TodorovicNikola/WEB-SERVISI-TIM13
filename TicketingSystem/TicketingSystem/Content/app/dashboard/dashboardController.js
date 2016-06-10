(function(angular) {
    var dashboardCtrlModule = angular.module('app.Dashboard', ['app.Project.resource', 'ui.sortable', 'adf', 'adf.structures.base', 'adf.widget.markdown', 'angularjs-dropdown-multiselect']);
	
	var dashboardController = ['$scope', '$http', 'Project' , function($scope, $http, Project) {

	    $scope.taskWidgetArray = [];

	    var loadProjects = function () {
	        $scope.projects = Project.query(function () {
	            $scope.tasks = [];
	            for (var i in $scope.projects) {
	                for (var j in $scope.projects[i].tasks) {
	                    $scope.tasks.push($scope.projects[i].tasks[j]);
	                    $scope.taskWidgetArray.push(addTaskToModel($scope.projects[i].tasks[j]));
	                }
	            }
	        });
	        $scope.project = new Project();

	    }
	    
	    $scope.init = function () {
	        loadProjects();
           
	        $scope.filterSelectPriorityModel = [{ "id": "Blocker" }, { "id": "Critical" }, { "id": "Major" }, { "id": "Minor" }, { "id": "Trivial" }];
	        $scope.filterSelectSetings = {
	            scrollableHeight: '200px',
	            scrollable: true,
	            enableSearch: true,
	            showUncheckAll: false
                
	        };
	        $scope.filterSelectStatusModel = [{ "id": "To Do" }, { "id": "In Progress" }, { "id": "Verify" }, { "id": "Done" }];
	        $scope.filterSelectTextPriority = {
                buttonDefaultText: 'Priority'
	        };

	        $scope.filterSelectTextStatus = {
	            buttonDefaultText: 'Status'
	        };

	        $scope.filterSelectPrioritydata = [
                {
                    "label": "Blocker",
                    "id": "Blocker"
	            },
                {
                    "label": "Critical",
                    "id": "Critical"
                },
                {
                    "label": "Major",
                    "id": "Major"
                },
                {
                    "label": "Minor",
                    "id": "Minor"
                },
                {
                    "label": "Trivial",
                    "id": "Trivial"
                }

	        ];

	        $scope.filterSelectStatusdata = [
               {
                   "label": "To Do",
                   "id": "To Do"
               },
               {
                   "label": "In Progress",
                   "id": "In Progress"
               },
               {
                   "label": "Verify",
                   "id": "Verify"
               },
               {
                   "label": "Done",
                   "id": "Done"
               }
	        ];

	    }

	    $scope.SubmitFilter = function () {
	        var filterIDs = [];
	        angular.forEach($scope.filterSelectPriorityModel, function (value, index) {
	            filterIDs.push(value.id);
	        });

	        angular.forEach($scope.filterSelectStatusModel, function (value, index) {
	            filterIDs.push(value.id);
	        });
            
	        console.log(filterIDs);
	        var data = {
                filterIDs : filterIDs
	        };

	        $http({
	            method: "Post",
	            url: "api/Filter/PostFilter",
	            data: filterIDs,
	            datatype: "json",
                traditional:true
	        }).then(function (data) {
	            $scope.tasks = data.data;
	            $scope.taskWidgetArray = [];

	            for (var i in data.data) {
	                $scope.taskWidgetArray.push(addTaskToModel(data.data[i]));
	            }

	            $scope.model = {
	                title: "Tasks",
	                structure: "6-6",
	                rows: [{
	                    columns: $scope.taskWidgetArray
	                }]
	            };

	            if (!$scope.$$phase) {
	                $scope.$apply();
	            }


	        }, function (error) {
	            alert('Error while filtering tasks!');
	        });

	    };

	    function addTaskToModel(task) {
	        return {
	            styleClass: "col-md-6",
	            widgets: [{
	                fullScreen: false,
	                modalSize: 'lg',
	                type: "markdown",
	                config: {
	                    content: "###" + task.projectCode + ' - ' + task.taskName + "\n\n>Status: " + task.taskStatus + "\n\n>Until: " + task.taskUntil + "\n\n>Created: " + task.taskCreated + "\n\n Description: " +  task.taskDescription
	                },
	                title: task.taskPriority
	            }]
	        };
	    }

	    $scope.name = 'widget';
	    $scope.model = {
	        title: "Tasks",
	        structure: "6-6",
	        rows: [{
	            columns: $scope.taskWidgetArray
	        }]
	    };
	    $scope.collapsible = false;
	    $scope.maximizable = false;
	    $scope.categories = true;

		$scope.init();
	}];

	dashboardCtrlModule.controller('DashboardCtrl', dashboardController);
}(angular));