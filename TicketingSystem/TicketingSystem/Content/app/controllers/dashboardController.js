(function(angular) {
	var dashboardCtrlModule = angular.module('app.DashboardCtrl', []);
	
	var dashboardController = ['$scope', 'Projects' , function($scope, Projects) {
		//$scope.projects = [
		//	{
		//		name : "projX",
		//		leader : "John Doe"
		//	},
		//	{
		//		name : "projY",
		//		leader : "Jane Doe"
		//	},
		//	{
		//		name : "projZ",
		//		leader : "John Doe"
		//	}
		//];

	    $scope.init = function () {
	        
	        Projects.getProjects().success(function (data) {
	            $scope.projects = data
	        });
            
	    }
	    $scope.getTasks = function (id) {
	        console.log('sssa'+id);
	        Projects.getTasksOfProject(id).success(function(data) {
	            $scope.selectedProjectTasks=data;
	        });

	    }

		$scope.init();
	}];

	dashboardCtrlModule.controller('DashboardCtrl', dashboardController);
}(angular));