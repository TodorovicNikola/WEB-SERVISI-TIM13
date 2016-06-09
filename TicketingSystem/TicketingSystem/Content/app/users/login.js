(function (angular) {
    angular.module('login', ['authentication', 'ui.bootstrap'])
	.controller('loginCtrl', function ($scope, $log, AuthenticationService) {
	    $scope.user = {};
	    $scope.alert = null;
	    $scope.login = function () {
	        AuthenticationService.login($scope.user.name, $scope.user.password, loginCbck);
	    };
	    function loginCbck(success) {
	        if (success) {
	            $log.info('success!');
	        }
	        else {
	            $log.info('failure!');
	            $scope.alert = { msg: 'Invalid username or password', timeout: 3000 };
	            setTimeout(function () { $scope.alert = null; }, 3000);
	        }
	    }
	});
}(angular));