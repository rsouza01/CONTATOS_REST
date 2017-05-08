var exDialog;
(function (exDialog_1) {
    var components;
    (function (components) {
        var SecondSampleController = (function () {
            function SecondSampleController(exDialog, $scope) {
                this.exDialog = exDialog;
                this.$scope = $scope;
            }
            //Not used here.
            SecondSampleController.prototype.$onInit = function () { };
            SecondSampleController.prototype.openSimpleInfo = function () {
                this.exDialog.openMessage(this.$scope, "Open a dialog on second page.");
            };
            return SecondSampleController;
        }());
        //Inject dependencies.
        SecondSampleController.$inject = ['exDialog', '$scope'];
        var SecondSampleComponent = (function () {
            function SecondSampleComponent() {
                //TypeScript for AngularJS bindings bindings['callerId']: '<'.
                this.bindings = {
                    callerId: '<'
                };
                this.templateUrl = "/Pages/_sampleSecondTemplate.html";
                //If no this line, use default "$ctrl".
                this.controllerAs = 'vm';
                //Use this inject approach if not "static $inject" in controller class.
                //controller = ['commonQueryData', SecondSampleController];
                this.controller = SecondSampleController;
            }
            return SecondSampleComponent;
        }());
        angular.module('smApp').component('sampleSecond', new SecondSampleComponent());
    })(components = exDialog_1.components || (exDialog_1.components = {}));
})(exDialog || (exDialog = {}));
//# sourceMappingURL=secondSampleComponent.js.map