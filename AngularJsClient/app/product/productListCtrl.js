(function () {
    "use strict";
    angular
        .module("appmodule")
        .controller("ProductListCtrl",
        ["$scope", "productService",
            ProductListCtrl]);

    function ProductListCtrl($scope, productService) {
        var vm = this;
        vm.products = [];
        vm.Message = "";
        GetProducts();
        function GetProducts() {
            var groupResult = productService.get();
            groupResult.then(function (resp) {
                vm.products = resp.data;
                vm.Message = "Call Completed Successfully";
            }, function (err) {
                vm.Message = "Error!!! " + err.status
            });
        };
    }
}());