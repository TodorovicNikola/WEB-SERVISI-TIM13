(function (angular) {
    angular.module('app.Task', [
	    'app.Task.controller',
        'app.Task.modalTicketController',
        'app.Task.taskDetailController',
        'app.Task.deleteConfirmController'

    ]);
}(angular));