(function(angular) {
	var helloWorldCtrlModule = angular.module('app.HelloWorldCtrl', []);

	var helloWorldController = ['$scope', function($scope) {
		$scope.temp = "qwerty";
	}];

	helloWorldCtrlModule.controller('HelloWorldCtrl', helloWorldController);
}(angular));