(function(angular) {
    /*global angular*/
    var appProjectsModule = angular.module('app.Projects', []);

    appProjectsModule.factory('Projects', function ($http) {
        'use strict';
        return {
            getProjects: function () {
                return $http.get('../../api/projects');
            },
            getTasksOfProject: function (id) {
                
                return $http.get('../../api/projects/'+id+'/tasks')
            }
            
        };
    });
}(angular));