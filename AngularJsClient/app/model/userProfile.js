(function () {
    "use strict";
    angular
        .module("common.services")
        .factory("userProfile",
        userProfile)

    function userProfile() {
        var setProfile = function (data) {
            sessionStorage.setItem('accessToken', data.token);
            sessionStorage.setItem('claims', data.claims);
            sessionStorage.setItem('expiration', data.expiration);
        };

        var getProfile = function () {
            var profile = {
                isLoggedIn: sessionStorage.getItem('accessToken') != null,
                token: sessionStorage.getItem('accessToken'),
                claims: sessionStorage.getItem('claims'),
                expiration: sessionStorage.getItem('expiration')
            };
            return profile;
        };

        var getToken = function () {
            return sessionStorage.getItem('accessToken');
        };

        var getAuthHeaders = function () {
            var accesstoken = sessionStorage.getItem('accessToken');
            var authHeaders = {};
            if (accesstoken) {
                authHeaders.Authorization = 'Bearer ' + accesstoken;
            }
            return authHeaders;
        };
        var logout = function () {
            sessionStorage.removeItem('accessToken');
        };

        return {
            setProfile: setProfile,
            getProfile: getProfile,
            getToken: getToken,
            getAuthHeaders: getAuthHeaders,
            logout: logout
        }
    }
})();