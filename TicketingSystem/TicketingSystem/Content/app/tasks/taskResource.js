(function (angular) {
    angular.module('app.Task.resource', ['ngResource'])
	.factory('Task', function ($resource) {
	    var task = $resource('../../api/projects/:projectId/tasks/:taskId',
			{
			    projectId: '@projectId',
			    taskId: '@taskId',
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
	    return task;
	})
}(angular));