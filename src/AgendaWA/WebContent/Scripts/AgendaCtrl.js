var agendaApp = angular.module('agendaApp', ['ngRoute']);

var baseUrl = 'http://localhost:1032/AgendaService.svc/';

//  Force AngularJS to call our JSON Web Service with a 'GET' rather than an 'OPTION' 
//  Taken from: http://better-inter.net/enabling-cors-in-angular-js/
agendaApp.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
}]);

agendaApp.controller('AgendaCtrl', ['$scope', '$http', '$location',
    function ($scope, $http, $location) {

        $scope.estados = [
            { id: "1", descricao: "SP" },
            { id: "2", descricao: "RJ" },
            { id: "3", descricao: "RS" }
        ];

        $scope.listaContatos = null;
        $scope.selectedContato = null;

        $scope.novoContato = {};


        $http.get(baseUrl + 'getTodosContatos')
            .success(function (data) {

                $scope.listaContatos = data.GetTodosContatosResult;
                if ($scope.listaContatos.length > 0) {

                    $scope.selectedContato = $scope.listaContatos[0];
                }
            })
            .error(function (data, status, headers, config) {
                $scope.errorMessage = 'Não foi possível carregar a lista de contatos, erro # ' + status;
                $scope.dataResult = 'data = ' + data;
            });


        $scope.selectContato = function (contato) {
            $scope.selectedContato = contato;
        }

        $scope.criarContato = function () {

            //var data = angular.copy($scope.novoContato);
            //alert(JSON.stringify($scope.novoContato));

            $http({
                method: "POST",
                url: baseUrl + 'criarContato',
                data: $scope.novoContato,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
                }
            }).success(function (data, status, headers, config) {
                alert('Usuário criado com sucesso.');
            }).error(function (data, status, header, config) {
                    alert('Erro na criação do usuário, status = ' + status);
            });

        }

        $scope.salvarContato = function () {

            //var data = angular.copy($scope.novoContato);
            //alert(JSON.stringify($scope.selectedContato));

            $http({
                method: "POST",
                url: baseUrl + 'alterarContato',
                data: $scope.selectedContato,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
                }
            }).success(function (data, status, headers, config) {
                alert('Usuário salvo com sucesso.');
            }).error(function (data, status, header, config) {
                alert('Erro na gravação do usuário, status = ' + status);
            });

        }

        $scope.deleteContato = function () {

           $http.get(baseUrl + 'removerContato/' + $scope.selectedContato.id)
            .success(function (data) {
                alert('Contato excluído com sucesso.');
            })
            .error(function (data, status, headers, config) {
                alert('Não foi possível excluir o contato, erro # ' + status);
            });

           $location.path('/add');
            $scope.$apply();
            console.log($location.path());
        };

    }]);


/*** ROTAS */
agendaApp.config(['$routeProvider','$locationProvider',
    function ($routeProvider, $locationProvider) {

    $routeProvider
    .when("/", {
        templateUrl: "detalhesForm.htm"
    })
    .when("/add", {
        templateUrl: "incContatoForm.htm"
    })
    .when("/edit", {
        templateUrl: "editContatoForm.htm"
    }).
    otherwise({
        redirectTo: '/'
    });

}]);