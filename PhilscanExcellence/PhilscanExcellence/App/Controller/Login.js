﻿angular.module("login", ["angular-growl"])

.controller("loginController", ["$scope", "$location", "$http", "growl", function ($scope, $location,
    $http, growl) {

    var vm = this;

    $scope.TryLogin = function (username, password) {
        var error = "N";

        $http({
            method: "POST",
            url: "/Home/TryLogin",
            data: {
                _username: username,
                _password: password
            }
        }).then(function (data) {
            if (data.data != "") {
                growl.error(data.data, { ttl: 3000 });
            }
            else {
                GetUser();
            }
        });
    }

    GetUser = function () {
        $http({
            method: "POST",
            url: "/Home/GetCurrentUser",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data != null) {
                window.location.href = "/Home/Index";
            }
        });
    }
}]);