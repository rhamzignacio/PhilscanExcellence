angular.module("elearning", [])

.controller("eLearningController", function ($scope, $location, $http, growl, Upload) {

    var vm = this;

    vm.HeaderID = "";

    vm.FileUpload = {};

    SucessMessage = function (message) {
        growl.success(message, { ttl: 3000 });
    }

    ErrorMessage = function (message) {
        growl.error(message, { ttl: 3000 });
    }

    $scope.AnswerDropdown = [
        { value: "A", label: "A" },
        { value: "B", label: "B" },
        { value: "C", label: "C" },
        { value: "D", label: "D" },
        { value: "E", label: "E" }
    ];

    $scope.InitHeader = function () {
        $http({
            method: "POST",
            url: "/ELearning/GetAllActiveHeader",
            arguments: { "Content-Type": "application/json" }
        }).then(function (data) {
            if (data.data.error != "") {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Header = data.data.header;
            }
        });
    }

    $scope.ClearModal = function () {
        vm.Modal = {};
    }

    $scope.TakeExam = function (value) {
        window.location = "/ELearning/TakeExam?ID=" + value.ID;
    }

    $scope.OpenHeader = function (value) {
        vm.Modal = value;
    }


    $scope.SaveHeader = function (value) {
        $http({
            method: "POST",
            url: "/ELearning/SaveHeader",
            data: {
                _header: value
            }
        }).then(function (data) {
            if (data.data !== "") {
                ErrorMessage(data.data);
            }
            else {
                SucessMessage("Sucessfully Saved");

                $("#HeaderModal").modal('hide');

                vm.Modal = {};

                $scope.InitHeader();
            }
        });
    }

    //=====ELEARNING ITEM=====
    $scope.InitItem = function () {
        $http({
            method: "POST",
            url: "/ELearning/GetItems",
            data: {
                _headerID: vm.HeaderID
            }
        }).then(function (data) {
            if (data.data.error !== "") {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Items = data.data.items;
            }
        });
    }

    $scope.AssignItem = function (value) {
        vm.ItemModal = value;
    }

    $scope.AssignDelete = function (value) {
        vm.ItemDelete = value;
    }

    $scope.DeleteItem = function () {
        $http({
            method: "POST",
            url: "/ELearning/DeleteItem",
            data: { _item: vm.ItemDelete }
        }).then(function (data) {
            if (data.data !== "") {
                ErrorMessage(data.data);
            }
            else {
                SucessMessage("Sucesfully Deleted");

                $("#deleteModal").modal('hide');
            }
        });
    }

    $scope.Redirection = function (value) {
        window.location = "/ELearning/Items?ID=" + value;
    }

    $scope.UploadClick = function () {
        $("#Files").click();
    }

    $scope.uploadClick = function () {
        $("#Files").click();
    }

    $scope.uploadFiles = function (files, errFiles) {
        var ID = "";

        $scope.files = files;

        $scope.errFiles = errFiles;

        angular.forEach(files, function (file) {
            if (file.size >= 2560000) {
                ErrorMessage("Maximum file upload is 25MB");
            }
            else {
                if (file.Status != "X") {
                    file.upload = Upload.upload({
                        url: "/ELearning/FileUpload",
                        data: {
                            file: file
                        },
                        async: true
                    }).then(function (data) {
                        vm.FileUploaded = data.data.uploadFile;
                    })
                }
            }
        });
    }

    $scope.ClearItemModal = function () {
        vm.ItemModal = {};
    }

    $scope.InitItem = function () {
        var ID = "";

        ID = window.location.href.substr(window.location.href.indexOf('=') + 1, 36);

        $http({
            method: "POST",
            url: "/ELearning/GetItems",
            data: {
                _headerID: ID
            }
        }).then(function (data) {
            if (data.data.error != "") {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Items = data.data.items;
            }
        });
    }

    $scope.SaveItem = function (value) {
        var ID = "";

        var attachmentID = "";

        if (vm.FileUpload !== null || vm.FileUpload !== {}) {
            attachmentID = vm.FileUpload.ID;
        }

        ID = window.location.href.substr(window.location.href.indexOf('=') + 1, 36);


        $http({
            method: "POST",
            url: "/ELearning/SaveItem",
            data: {
                _item: value,
                _attachmentID: attachmentID,
                _headerID: ID
            }
        }).then(function(data){
            if (data.data != "") {
                ErrorMessage(data.data);
            }
            else { //Sucessfully Saved
                $("#itemModal").modal('hide');

                SucessMessage("Sucessfully Saved");

                $scope.ClearItemModal();
            }
        });
    }

    //=======TAKE EXAM=========
    $scope.InitTakeExam = function () {
        var ID = "";

        ID = window.location.href.substr(window.location.href.indexOf('=') + 1, 36);

        $http({
            method: "POST",
            url: "/ELearning/GetRandomQuestion",
            data: {
                _headerID: ID
            }
        }).then(function (data) {
            if (data.data.error !== "") {
                ErrorMessage(data.data.error);
            }
            else {
                vm.Question = data.data.question;

                vm.ExamCount = data.data.examCount;
            }
        });
    }

    $scope.SubmitAnswer = function () {
        $http({
            method: "POST",
            url: "/ELearning/Submit",
            data: {

            }
        }).then(function (data) {

        });
    }

});