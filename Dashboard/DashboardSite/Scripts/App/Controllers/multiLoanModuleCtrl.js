/// <reference path="../app.js" />
multiLoanModule.controller('multiLoanModuleCtrl', ['$scope', '$http', 'multiLoanModuleSvc',
    function ($scope, $http, multiLoanModuleSvc) {
        $scope.isSubMode = false;
        $scope.orderBy = { field: 'ProjectName', asc: true };
        load();
        function load() {
            multiLoanModuleSvc.getMultiLoans().success(function (data) {
                $scope.Loans = data;
                angular.forEach($scope.Loans, function (loan) {
                    loan.isSubMode = false;
                    loan.isDetailLoaded = false;
                });
            })
        };

        $scope.toggleSubMode = function (loan) {
            loan.isSubMode = !loan.isSubMode;
            if (!loan.isDetailLoaded) {
                multiLoanModuleSvc.getMultiLoansDetail(loan.PropertyID).success(function (data) {
                    loan.Details = data;
                    loan.isDetailLoaded = true;
                })
            }
        };

        $scope.setOrderBy = function (field) {
            var asc = $scope.orderBy.field === field ? !$scope.orderBy.asc : true;
            $scope.orderBy = { field: field, asc: asc };
        };

    }
]);