(function(angular) {
	var dashboardCtrlModule = angular.module('app.DashboardCtrl', []);
	
	var dashboardController = ['$scope', 'Projects' , function($scope, Projects) {
		
	  
	    
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
            
	    }
	    $scope.getTasks = function (id) {
	    
	        Projects.getTasksOfProject(id).success(function(data) {
	            $scope.selectedProjectTasks = data;

	            
	        });

	    }

		$scope.init();
	}];

	dashboardCtrlModule.controller('DashboardCtrl', dashboardController);
}(angular));