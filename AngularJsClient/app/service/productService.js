(function () {
    "use strict";
    angular
        .module("common.services")
        .factory("productService", ["$http", "appSettings", "userProfile",
            productService])

    function productService($http, appSettings, userProfile) {
        this.get = function () {
            var authHeaders = userProfile.getAuthHeaders();
            var response = $http({
                url: appSettings.serverPath + "/api/product",
                method: "GET",
                headers: authHeaders
            });
            return response;
        };

        return {
            get: this.get
        }
    }
})();