(function () {
    "use strict";

    angular
        .module("appmodule")
        .controller("userCtrl",
        ["$scope", "loginservice", "userProfile",
            userCtrl]);

    function userCtrl($scope, loginservice, userProfile) {
        var vm = this;
        vm.registerErrorData = "";
        vm.loginErrorData = "";
        vm.responseData = "";
        vm.userName = "";
        vm.userEmail = "";
        vm.userPassword = "";

        vm.newUserEmail = "";
        vm.newUserPassword = "";
        vm.confirmUserPassword = "";

        vm.accessToken = "";
        vm.refreshToken = "";

        vm.isLoggedIn = function () {
            var result = userProfile.getProfile().isLoggedIn;
            return result;
        };

        vm.registerUser = function () {
            vm.responseData = '';
            vm.registerErrorData = '';
            vm.loginErrorData = '';
            var userRegistrationInfo = {
                Email: vm.newUserEmail,
                Password: vm.newUserPassword,
                ConfirmPassword: vm.confirmUserPassword
            };

            var registerResult = loginservice.register(userRegistrationInfo);

            registerResult.then(function (data) {
                vm.responseData = "User Registration successful";
                vm.newUserPassword = "";
                vm.confirmUserPassword = "";
            }, function (response) {
                vm.registerErrorData = response.statusText + "\r\n";

                if (response.data) {
                    for (var key in response.data) {
                        vm.registerErrorData += response.data[key] + "\r\n";
                    }
                    if (response.data.exceptionMessage)
                        vm.registerErrorData += response.data.exceptionMessage;
                }
            });
        };

        vm.redirect = function (url) {
            window.location.href = url;
        };

        vm.login = function () {
            var userLogin = {
                grant_type: 'password',
                username: vm.userEmail,
                password: vm.userPassword
            };
            vm.responseData = '';
            vm.registerErrorData = '';
            vm.loginErrorData = '';
            var loginResult = loginservice.login(userLogin);

            loginResult.then(function (resp) {
                var result = resp.data.claims.filter(function (o) {
                    return o.type == 'sub';
                });
                vm.userName = result ? result[0].value : null;
                userProfile.setProfile(resp.data);
            }, function (response) {
                vm.loginErrorData = response.statusText + " : \r\n";
                if (response.data) {
                    for (var key in response.data) {
                        vm.loginErrorData += response.data[key] + "\r\n";
                    }
                }
            });
        };

        vm.logout = function () {
            userProfile.logout();
        };
    }
})();