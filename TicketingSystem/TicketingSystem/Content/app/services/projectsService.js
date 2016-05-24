(function(angular) {
    /*global angular*/
    var appProjectsModule = angular.module('app.Projects', []);

    appProjectsModule.factory('Projects', function ($http) {
        'use strict';
        var projectSelectedId;
        return {
            getProjects: function () {
                return $http.get('../../api/projects');
            },
            getTasksOfProject: function (id) {
                projectSelectedId = id;
                return $http.get('../../api/projects/' + id + '/tasks')

            },
            getCurrentProjectId:function()
            {
                return 5;
            }
            
        };
    });
}(angular));