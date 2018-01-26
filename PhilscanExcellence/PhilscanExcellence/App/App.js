var app = angular.module("app", ["elearning", "angular-growl", "ngFileUpload"])

.controller("mainController", ["$scope", "$location", "$http", "growl", function(
    $scope, $location, $http, growl){

    SucessMessage = function (message) {
        growl.success(message, { ttl: 3000 });
    }

    ErrorMessage = function (message) {
        growl.error(message, { ttl: 3000 });
    }

}]);