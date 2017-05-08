'use strict';
angular.module('smApp.controllers', [function () {}])

.controller('bodyController', ['$scope', 'exDialog', '$location', function ($scope, exDialog, $location) {
    //Close dialog if any when clicking broswer navigation buttons.
    $scope.$on('$locationChangeStart', function (event, newUrl, oldUrl) {
        if (newUrl != oldUrl && exDialog.hasOpenDialog()) {
            exDialog.closeAll();
            //event.preventDefault(); //Keep exiting page instead of leaving.
        }
    });
    //$scope.message = "ok";
}])

//Inject $location to controller that uses dialog to remove unwanted behaviors for browser navigation buttons.
.controller('sampleController', ['$scope', '$timeout', 'exDialog', '$location', function ($scope, $timeout, exDialog, $location) {

    $scope.openSimpleInfo = function () {
        exDialog.openMessage($scope, "This is called from a simple line of parameters.");
    };

    $scope.openSimpleWarning = function () {
        exDialog.openMessage($scope, "This is called from a simple line of parameters.", "Warning", "warning");
    };

    $scope.openSimpleError = function () {
        exDialog.openMessage($scope, "This is called from a simple line of parameters.", "Error", "error");
    };

    $scope.dialogNoIconAndGrayButWithBorder = function () {
        //Simple paramters can be used here if not set grayout to false.
        //exDialog.openMessage($scope, "This is called from a simple code line.", "", "none");        
        exDialog.openMessage({
            scope: $scope,
            title: "No Icon, No Gray",
            icon: "none",
            message: "This is called by passing a parameter object",
            grayBackground: false,
            dialogAddClass: 'border-to-dialog'
        });
    };

    $scope.dialogNoAnimationDrag = function () {
        exDialog.openMessage({
            scope: $scope,
            message: "Animation and drag-move disabled.",
            animation: false,
            draggable: false
        });
    };

    
    $scope.differentHeaderFooterStyles = function () {                
        exDialog.openMessage({
            scope: $scope,
            title: "New Look",
            icon: "none",
            message: "Show header and footer in other styles.",            
            headerAddClass: 'my-dialog-header',
            bodyAddClass: 'my-dialog-body',
            footerAddClass: 'my-dialog-footer'
        });
    };

    $scope.openSimpleConfirmation = function () {
        exDialog.openConfirm($scope, "Would you like to close the dialog and open another one?").then(function (value) {
            exDialog.openMessage($scope, "This is another dialog.");
        });
    };

    $scope.keepParentOpenAndNoCloseByOutside = function () {
        exDialog.openConfirm({
            scope: $scope,
            message: "Would you like to keep this dialog opening after showing the second?",
            keepOpenForAction: true,
            closeByClickOutside: false
        }).then(function (value) {
            exDialog.openMessage({
                //Pass a new child scope for second dialog if keeping the parent to open.
                scope: $scope.$new(),
                message: "This will close all opened dialogs.",
                closeAllDialogs: true,                
                closeByClickOutside: false
            });
        });
    };
    
    $scope.openDialogAfterClosingParentUsingPremise = function () {
        exDialog.openConfirm($scope, "Would you like to open a second dialog?").then(function (value) {
            exDialog.openMessage($scope, "This is the second dialog.");
        }, function (reason) {
            //This is for notification only.
            //Remove this function if nothing is needed before closing the dialog.
            exDialog.openMessage($scope, "The dialog has been closed.");
        });
    };

    $scope.openDialogBeforeClosingParentUsingCallback = function () {
        exDialog.openConfirm({
            scope: $scope,
            actionButtonLabel: "Continue",
            closeButtonLabel: "Cancel",
            message: "What next step would you like to take?",
            beforeCloseCallback: function (value) {
                var rtnPremise = exDialog.openConfirm({
                    scope: $scope,
                    message: "Do you really want to cancel it?"
                });
                return rtnPremise;
            }            
        }).then(function (value) {
            exDialog.openMessage($scope, "The action is continue...");
        }, function (reason) {
            //Click action button from callback dialog will pass handle here.
            exDialog.openMessage($scope, "The action has been cancelled.", "Notification");            
        });
    };

    //Action of clicking product name link.
    $scope.callType = {};
    //var pWidth = $window.innerWidth - 50;
    $scope.dataFormDialog = function (id) {
        $scope.callType.id = id;
        exDialog.openPrime({
            scope: $scope,
            template: 'Pages/_Product.html',
            controller: 'productController',
            width: '450px',//pWidth.toString() + 'px',//'450px',
            //animation: false,
            //grayBackground: false            
        });
    };    
}])

.controller('productController', ['$scope', '$rootScope', 'exDialog', 'localData', function ($scope, $rootScope, exDialog, localData) {
    $scope.model = {};
    
    //Disable click-on input field drag/move.
    $scope.setDrag = function (flag) {
        $rootScope.noDrag = flag;
    };

    //Get data from local service.
    $scope.model.Product = localData.getProduct();

    $scope.productDialogTitle = "Edit Product";

    $scope.saveProduct = function () {
        var ctId = $scope.callType.id;
        if (ctId == 0 || ctId == 2) {
            exDialog.openConfirm({
                scope: $scope,
                title: "Save Confirmation",
                message: "Are you sure to save the product?"
            }).then(function (value) {
                //Code to save product here...
                //Notification Message.
                exDialog.openMessage({
                    scope: $scope,
                    message: "The product has successfully been saved.",
                    closeAllDialogs: true
                });
            });
        }
        else if (ctId == 1) {
            exDialog.openConfirm({
                scope: $scope,
                title: "Save Confirmation",
                message: "Are you sure to save the product?",
                keepOpenForAction: true
            }).then(function (value) {
                //Code to save product here...
                //Notification Message.
                exDialog.openMessage({
                    scope: $scope,
                    message: "The product has successfully been saved.",
                    closeAllDialogs: true
                    //If leave original data form dialog there.
                    //closeImmediateParent: true
                });
            });
        }        
    };

    $scope.cancelEditing = function () {
        var ctId = $scope.callType.id;
        if (ctId == 0 || ctId == 1) {
            $scope.closeThisDialog("close");
        }
        else if (ctId == 2) {
            exDialog.openConfirm({
                scope: $scope,
                title: "Cancel Warning",
                icon: "warning",
                message: "Do you really want to cancel the data editing?",
                keepOpenForAction: true,
                keepOpenForClose: true
            }).then(function (value) {                
                exDialog.openMessage({
                    scope: $scope,
                    title: "Notification",
                    message: "The editing has been cancelled.",
                    closeAllDialogs: true
                });
            }, function (reason) {
                exDialog.openMessage({
                    scope: $scope,
                    title: "Notification",
                    message: "The editing will continue.",
                    closeImmediateParent: true
                });
            });
        }        
    };    
}])
//Not used after switching sampleSecond to component.
//.controller('sampleController2', ['$scope', '$timeout', 'exDialog', '$location', function ($scope, $timeout, exDialog, $location) {

//    $scope.openSimpleInfo = function () {
//        exDialog.openMessage($scope, "Open a dialog on second page.");
//    };

//    ////Close dialog if any when clicking broswer navigation buttons.
//    //$scope.$on('$locationChangeStart', function (event, newUrl, oldUrl) {
//    //    if (newUrl != oldUrl && exDialog.hasOpenDialog()) {
//    //        exDialog.closeAll();
//    //    }
//    //});
//}])
;


