namespace exDialog.components {

    interface ISecondSampleController {
        callerId: string;        
    }

    class SecondSampleController implements ISecondSampleController {
        //Not used here.
        callerId: string;         

        //Inject dependencies.
        static $inject = ['exDialog', '$scope']

        constructor(private exDialog, private $scope) { }

        //Not used here.
        $onInit() { }

        openSimpleInfo() {
            this.exDialog.openMessage(this.$scope, "Open a dialog on second page.");
        }        
    }

    class SecondSampleComponent implements ng.IComponentOptions {
        //TypeScript for AngularJS bindings bindings['callerId']: '<'.
        bindings: { [bindngs: string]: string } = {
            callerId: '<'
        }

        templateUrl = "/Pages/_sampleSecondTemplate.html";

        //If no this line, use default "$ctrl".
        controllerAs = 'vm';

        //Use this inject approach if not "static $inject" in controller class.
        //controller = ['commonQueryData', SecondSampleController];
        controller = SecondSampleController;
    }
    angular.module('smApp').component('sampleSecond', new SecondSampleComponent());
}

