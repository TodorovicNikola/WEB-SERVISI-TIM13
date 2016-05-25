(function(angular) {
	var dashboardCtrlModule = angular.module('app.DashboardCtrl', []);
	
	var dashboardController = ['$scope', 'Projects', '$http' , function($scope, Projects, $http) {
		
	  
	    
	    $scope.init = function () {
	        
	        Projects.getProjects().success(function (data) {
	            $scope.projects = data;

	            var tasks = new Array();

	            for (var i = 0; i < $scope.projects.length; i++) {
	                for (var j = 0; j < $scope.projects[i].tasks.length; j++) {
	                    tasks.push($scope.projects[i].tasks[j]);
	                }
	            }

	            $scope.tasks = tasks;

	        });
           
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
	        var filterIDs = new Array();
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
	            if (!$scope.$$phase) {
	                $scope.$apply();
	            }


	        }, function (error) {
	            alert('Error while filtering tasks!');
	        });

	    };

	    $scope.getTasks = function (id) {
	    
	        Projects.getTasksOfProject(id).success(function(data) {
	            $scope.selectedProjectTasks = data;

	            
	        });

	    }

		$scope.init();
	}];

	dashboardCtrlModule.controller('DashboardCtrl', dashboardController);
}(angular));