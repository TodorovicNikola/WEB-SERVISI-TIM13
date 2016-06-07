(function (angular) {
    angular.module('app.User.resource', ['ngResource'])
	.factory('User', function ($resource) {
	    var user = $resource('../../api/users/:userId',
			{
			    userId: '@userId',
			},
			{
			    update: { method: 'PUT' },
			});
	    return user;
	})
}(angular));