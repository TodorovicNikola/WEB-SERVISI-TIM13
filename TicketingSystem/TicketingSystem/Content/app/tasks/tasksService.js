(function (angular) {

    var appTasksModule = angular.module('app.Tasks', []);

    appTasksModule.factory('Tasks', function ($http) {
        'use strict';
        var creation = false;

        
        return {
            getTasks: function (id) {
                
                return $http.get('../../api/projects/'+id+'/tasks');
            },
            getTask: function (id,taskId) {

                return $http.get('../../api/projects/' + id + '/tasks/'+taskId);
            },
            getCreation:function()
            {
                return creation;
            },
            setCreation: function (creationParam) {
                creation = creationParam;
            }
            
        };
    });
}(angular));