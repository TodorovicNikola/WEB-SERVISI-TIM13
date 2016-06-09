(function(angular) {
    var dashboardCtrlModule = angular.module('app.DashboardCtrl', ['app.Project.resource']);
	
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
           
	        $scope.filterSelectModel = [{ "id": "Blocker" }, { "id": "Critical" }, { "id": "Major" }, { "id": "Minor" }, { "id": "Trivial" }];
	        $scope.filterSelectSetings = {
	            scrollableHeight: '200px',
	            scrollable: true,
	            enableSearch: true,
	            showUncheckAll: false
                
	        };

	        $scope.filterSelectTexts = {
                buttonDefaultText: 'Filter'
	        };

	        $scope.filterSelectdata = [
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

	    }

	    $scope.SubmitFilter = function () {
	        var filterIDs = [];
	        angular.forEach($scope.filterSelectModel, function (value, index) {
	            filterIDs.push(value.id);
	        });
            
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
	                title: "Sample 02",
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
	                    content: "# " + task.taskName + ' - ' + task.taskPriority + "\n\n> Dashboard framework with Angular.js and Twitter Bootstrap.\n\nThe api of angular-dashboard-framework (adf) is documented [here](http://angular-dashboard-framework.github.io/angular-dashboard-framework/docs/). A getting\nstarted guide can be found [here](https://github.com/angular-dashboard-framework/angular-dashboard-framework/wiki/Getting-started).\nFollow me ([@ssdorra](https://twitter.com/ssdorra)) on twitter for latest updates and news about adf.\n\n## Demo\n\nA live demo of the adf can be viewed [here](http://angular-dashboard-framework.github.io/angular-dashboard-framework/). The demo uses html5 localStorage to store the state of the dashboard. The source of the demo can be found [here](https://github.com/angular-dashboard-framework/angular-dashboard-framework/tree/master/sample).\n\nA more dynamic example can be found [here](https://github.com/angular-dashboard-framework/adf-dynamic-example).\n\n## Build from source\n\nInstall bower and gulp:\n\n```bash\nnpm install -g bower\nnpm install -g gulp\n```\n\nClone the repository:\n\n```bash\ngit clone https://github.com/angular-dashboard-framework/angular-dashboard-framework\ncd angular-dashboard-framework\n```\n\nInstall npm and bower dependencies:\n\n```bash\nnpm install\nbower install\n```\n\nCheckout git submodule widgets:\n\n```bash\ngit submodule init\ngit submodule update\n```\n\nYou can start the sample dashboard, by using the serve gulp task:\n\n```bash\ngulp serve\n```\n\nNow you open the sample in your browser at http://localhost:9001/sample\n\nOr you can create a release build of angular-dashboard-framework and the samples:\n\n```bash\ngulp all\n```\nThe sample and the final build of angular-dashboard-framework are now in the dist directory."
	                },
	                title: "Task"
	            }]
	        };
	    }

	    $scope.name = 'widget';
	    $scope.model = {
	        title: "Sample 02",
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