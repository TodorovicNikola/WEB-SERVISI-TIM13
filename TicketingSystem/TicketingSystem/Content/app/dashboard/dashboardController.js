(function(angular) {
    var dashboardCtrlModule = angular.module('app.DashboardCtrl', ['app.Project.resource']);
	
	var dashboardController = ['$scope', '$http', 'Project' , function($scope, $http, Project) {
		
	    var loadProjects = function () {
	        $scope.projects = Project.query(function () {
	            $scope.tasks = [];

	            for (var i in $scope.projects) {
	                for (var j in $scope.projects[i].tasks) {
	                    $scope.tasks.push($scope.projects[i].tasks[j]);
	                }
	            }
	        });
	        $scope.project = new Project();  
	    }
	    
	    $scope.init = function ()  {
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
	            if (!$scope.$$phase) {
	                $scope.$apply();
	            }


	        }, function (error) {
	            alert('Error while filtering tasks!');
	        });

	    };

		$scope.init();
	}];

	dashboardCtrlModule.controller('DashboardCtrl', dashboardController);
}(angular));