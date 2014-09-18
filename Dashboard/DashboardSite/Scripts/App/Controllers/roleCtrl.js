multiSelectModule.controller('roleCtrl', ['$scope', '$http', 'multiSelectSvc',
    function ($scope, $http, multiSelectSvc) {
        $scope.user = {
            roles: []
        };

        $scope.userTypeChanged = function (e) {
            var userTypeId = $(e).find('option:selected').val();
            var multiSelect = $("#dvMultiSelect");
            if (userTypeId == "1" || userTypeId == "2") {
                $scope.userTypeId = userTypeId;
                multiSelectSvc.getRolesByCategory(userTypeId).success(function (data) {
                    $scope.roles = data;
                    $scope.available = data;
                    // register new user, clear selected roles when user type changes
                    $scope.user.roles = [];
                })
                multiSelect.show();
            }
            else {
                multiSelect.hide();
            }
        };

        $scope.saveSelectedRoles = function () {
            multiSelectSvc.saveSelectedRoles({ RolesToUpdate: $scope.user.roles, RoleSourceType: $scope.userTypeId })
                .success(function (data, status, headers, config) {
                    if(data.Success)
                        $(location).attr('href', '/Account/Register');
                }).error(function (data, status, headers, config) {
                    alert(data.Message);
                });
        };

        $scope.createUserRoles = function () {
            multiSelectSvc.createUserRoles({ RolesToUpdate: $scope.user.roles, RoleSourceType: $scope.userTypeId })
                .success(function (data, status, headers, config) {
                    if (data.Success) {
                        data.IsLAR ? $(location).attr('href', '/Account/RegisterLenderAcctRep1') :
                            data.IsServicer ? $(location).attr('href', '/Account/RegisterServicer') :
                                $(location).attr('href', '/Account/Register');
                    }
                }).error(function (data, status, headers, config) {
                    alert(data.Message);
                });
        };
    }
]);


multiSelectModule.controller('assignLendersCtrl', ['$scope', '$http', 'multiSelectSvc',
    function ($scope, $http, multiSelectSvc) {
        $scope.selectedLenders = {
            lenders: []
        };

        load();
        function load() {
            multiSelectSvc.getAllLenders()
                .success(function (data, status, headers, config) {
                    $scope.Lenders = data;
                }).error(function (data, status, headers, config) {
                    alert(data.Message);
                });
        };
        
        $scope.createLenderServicerLinks = function () { 
            multiSelectSvc.saveSelectedLenders($scope.selectedLenders.lenders)
                .success(function (data, status, headers, config) {
                    if (data.Success) {
                        //data.IsServicer ? $(location).attr('href', '/Account/RegisterServicer') : $(location).attr('href', '/Account/Register');
                        $(location).attr('href', '/Account/Register');
                    }
                }).error(function (data, status, headers, config) {
                    alert(data.Message);
                });
        };

        $scope.saveSelectedLendersForLAR = function () {
            multiSelectSvc.saveSelectedLenders($scope.selectedLenders.lenders)
                .success(function (data, status, headers, config) {
                    if (data.Success) {
                        $(location).attr('href', '/Account/RegisterLenderAcctRep2');
                    }
                }).error(function (data, status, headers, config) {
                    alert(data.Message);
                });
        };
    }
]);

multiSelectModule.controller('assignFHAsCtrl', ['$scope', '$http', 'multiSelectSvc',
    function ($scope, $http, multiSelectSvc) {
        $scope.selectedFHAs = {
            fhas: []
        };

        load();
        function load() {
            multiSelectSvc.getFHAsFromLenders()
                .success(function (data, status, headers, config) {
                    $scope.FHAs = data.Fhas;
                }).error(function (data, status, headers, config) {
                    alert(data.Message);
                });
        };

        $scope.createUserLARLinks = function () {
            multiSelectSvc.createUserLARLinks($scope.selectedFHAs.fhas)
                .success(function (data, status, headers, config) {
                    if (data.Success) {
                        $(location).attr('href', '/Account/Register');
                    }
                }).error(function (data, status, headers, config) {
                    alert(data.Message);
                });
        };
    }
]);