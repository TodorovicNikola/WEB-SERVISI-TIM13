(function () {
    angular
        .module('authentication', [''])
        .factory('AuthenticationService', Service);

    function Service($http, $localStorage, $log, $state, jwtHelper) {
        var service = {};
        //not working yet, directly in controller
        service.postComment = postComment;
      
        return service;

        function postComment() {
        
            $http({
                method: 'POST',
                url: 'api/comments',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                data: {  }
            })
                .success(function (response) {
                  
                    if (s) {
                     
                    } else {
                        
                    }
                });
        }

       
        
    }
})();