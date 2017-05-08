'use strict';
var sm = angular.module('smApp.AppServices', []);
sm.service('localData', [function () {
    //Local data for product.
    this.getProduct = function () {
        return {
            ProductID: 2,
            ProductName: "Test Equipment 1",
            Category: "Office",
            UnitPrice: 67.89,
            AvailableSince: "04/28/2014",
            ProductStatus: "In Stock"
        }
    }    
}]);

