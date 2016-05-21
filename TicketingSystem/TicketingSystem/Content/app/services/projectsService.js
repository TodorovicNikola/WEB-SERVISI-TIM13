(function(angular) {
    /*global angular*/
    var appProjectsModule = angular.module('app.Projects', []);

    appProjectsModule.factory('Projects', function ($http) {
        'use strict';
        var a = 5;
        return {
            getProjects: function () {
                return $http.get('../../api/projects');
            },
            getTasksOfProject: function (id) {
                
                return $http.get('../../api/projects/'+id+'/tasks')
            },
            getBla:function()
            {
                return 'sada';
            }
        };
    });
}(angular));