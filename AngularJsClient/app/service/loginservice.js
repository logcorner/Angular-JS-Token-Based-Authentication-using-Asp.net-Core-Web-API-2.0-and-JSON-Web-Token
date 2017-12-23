(function () {
    "use strict";
    angular
        .module("common.services")
        .factory("loginservice", ["$http", "appSettings",
            loginservice])
    function loginservice($http, appSettings) {
        this.register = function (userInfo) {
            var resp = $http({
                url: appSettings.serverPath + "/api/auth/Register",
                method: "POST",
                data: userInfo,
            });
            return resp;
        };
        this.login = function (userlogin) {
            var contentHeaders = [{ 'Content-Type': 'application/json' },
            { 'Accept': 'application/json' },
            { 'Content-Type': 'application/x-www-form-urlencoded' }
            ]

            var credentials = {
                grant_type: 'password',
                email: userlogin.username,
                password: userlogin.password
            };

            var resp = $http({
                url: appSettings.serverPath + "/api/auth/token",
                method: "POST",
                data: credentials,
                headers: contentHeaders,
            });
            return resp;
        };
        return {
            register: this.register,
            login: this.login
        }
    }
})();