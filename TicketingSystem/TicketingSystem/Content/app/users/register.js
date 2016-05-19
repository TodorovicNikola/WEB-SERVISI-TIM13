(function (angular) {
    angular.module('register', ['authentication'])
	.controller('registerCtrl', function ($scope, $log, AuthenticationService) {
	    $scope.user = {};
	    $scope.register = function () {
	        AuthenticationService.register($scope.user.name, $scope.user.password, registerCbck);
	    };
	    function registerCbck(success) {
	        if (success) {
	            $log.info('success!');
	        }
	        else {
	            $log.info('failure!');
	        }
	    }
	});
}(angular));