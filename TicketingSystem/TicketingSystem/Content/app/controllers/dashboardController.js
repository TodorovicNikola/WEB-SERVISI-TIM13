(function(angular) {
	var dashboardCtrlModule = angular.module('app.DashboardCtrl', []);
	
	var dashboardController = ['$scope', 'Projects' , function($scope, Projects) {
		
	  
	    
	    $scope.init = function () {
	        
	        Projects.getProjects().success(function (data) {
	            $scope.projects = data
	        });
            
	    }
	    $scope.getTasks = function (id) {
	    
	        Projects.getTasksOfProject(id).success(function(data) {
	            $scope.selectedProjectTasks=data;
	        });

	    }

		$scope.init();
	}];

	dashboardCtrlModule.controller('DashboardCtrl', dashboardController);
}(angular));