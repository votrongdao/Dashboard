multiLoanModule.factory('multiLoanModuleSvc', ['$http', function ($http) {
    return {
        getMultiLoans: function () {
            return $http.get('/MultiLoan/GetLoans');
        },

        getMultiLoansDetail: function (propertyID) {
            return $http.get('/MultiLoan/GetLoansDetail', { params: { propertyid: propertyID } });
        }
    }
}]);