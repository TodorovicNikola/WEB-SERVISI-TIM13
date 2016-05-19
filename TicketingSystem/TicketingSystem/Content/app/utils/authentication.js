(function () {
    angular
        .module('authentication', ['ngStorage', 'ui.router', 'angular-jwt'])
        .factory('AuthenticationService', Service);

    function Service($http, $localStorage, $log, $state, jwtHelper) {
        var service = {};

        service.login = login;
        service.logout = logout;
        service.getCurrentUser = getCurrentUser;
        service.register = register;

        return service;

        function login(username, password, callback) {
            //$http.post('/../../oauth/token', { name: username, password: password })
            $http({
                method: 'POST',
                url: '/../../oauth/token',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                data: { grant_type: 'password', username: username, password: password }
            })
                .success(function (response) {
                    // ukoliko postoji token, prijava je uspecna
                    if (response.access_token) {
                        // korisnicko ime, token i rola (ako postoji) cuvaju se u lokalnom skladištu
                        var currentUser = { username: username, token: response.access_token }
                        var tokenPayload = jwtHelper.decodeToken(response.access_token);
                        if (tokenPayload.role) {
                            currentUser.role = tokenPayload.role;
                        }
                        $localStorage.currentUser = currentUser;
                        // jwt token dodajemo u to auth header za sve $http zahteve
                        $http.defaults.headers.common.Authorization = 'Bearer ' + response.access_token;
                        // callback za uspesan login
                        callback(true);
                        $state.go('dashboard');
                    } else {
                        // callback za neuspesan login
                        callback(false);
                    }
                });
        }

        function logout() {
            // uklonimo korisnika iz lokalnog skladišta
            delete $localStorage.currentUser;
            $http.defaults.headers.common.Authorization = '';
            $state.go('login');
        }

        function getCurrentUser() {
            return $localStorage.currentUser;
        }

        function register(username, password, callback) {
            $http.post('/api/account/register', { email: username, userName: username, password: password, confirmPassword: password })
                .success(function (response) {
                    callback(true);
                    $state.go('dashboard');
                })
                .error(function (response) {
                    callback(false);
                });
        }
    }
})();