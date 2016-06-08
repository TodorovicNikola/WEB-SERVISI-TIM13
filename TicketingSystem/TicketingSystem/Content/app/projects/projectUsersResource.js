(function (angular) {
    angular.module('app.ProjectUsers.resource', ['ngResource'])
	.factory('ProjectUsers', function ($resource) {
	    var projectUsers = $resource('../../api/projects/:projectId/users',
			{
			    projectId: '@projectId',
			},
			{
			    getUsersOnProject: {
			        method: 'GET',
			        params: {
			            projectId: '@projectId',
			        },
			        isArray: true
			    },
			});
	    return projectUsers;
	})
}(angular));

