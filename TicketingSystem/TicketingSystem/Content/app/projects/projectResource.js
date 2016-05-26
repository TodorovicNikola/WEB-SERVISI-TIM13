(function (angular) {
    angular.module('app.Project.resource', ['ngResource'])
	.factory('Project', function ($resource) {
	    var project = $resource('../../api/projects/:projectId',
			{
			    projectId: '@projectId',
			},
			{ update: { method: 'PUT' } });
	    return project;
	})
}(angular));