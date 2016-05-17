(function(angular) {
    /*global angular*/
    var appProjectsModule = angular.module('app.Projects', []);

    appProjectsModule.factory('Projects', function ($http) {
        'use strict';

        return {
            getProjects: function () {
                return $http.get('../../api/projects');
            },
        };
    });
}(angular));