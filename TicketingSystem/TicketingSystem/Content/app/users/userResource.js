(function (angular) {
    angular.module('app.User.resource', ['ngResource'])
	.factory('User', function ($resource) {
	    var user = $resource('../../api/projects/:projectId/users/:userId',
			{
			    projectId: '@projectId',
			    userId: '@userId',
			},
			{
			    update: { method: 'PUT' },
			    getAll: {
			        method: 'GET',
			        params: {
			            projectId: '@projectId',
			        },
			        isArray: true
			    }
			});
	    return user;
	})
}(angular));