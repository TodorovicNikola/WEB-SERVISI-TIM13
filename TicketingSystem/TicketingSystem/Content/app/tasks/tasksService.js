(function (angular) {

    var appTasksModule = angular.module('app.Tasks', []);

    appTasksModule.factory('Tasks', function ($http) {
        'use strict';

        return {
            getTasks: function (id) {
                
                return $http.get('../../api/projects/'+id+'/tasks');
            },
            
        };
    });
}(angular));