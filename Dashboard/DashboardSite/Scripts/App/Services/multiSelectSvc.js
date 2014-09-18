multiSelectModule.factory('multiSelectSvc', ['$http', function ($http) {
    return {
        getRolesByCategory: function(userTypeID){
            return $http.get('/Role/GetRolesByUserType', {params: {userTypeId : userTypeID}})
        },

        getAllLenders: function () {
            return $http.get('/Role/GetLenders');
        },

        getFHAsFromLenders: function () {
            return $http({
                url: '/Role/GetFHAsFromLenders',
                method: "GET",
                headers: { 'Content-Type': 'application/json' }
            });
        },

        saveSelectedRoles: function (userRoles) {
            return $http({
                url: '/Role/SaveUserRoleChanges',
                method: "POST",
                data: JSON.stringify(userRoles),                
                headers: { 'Content-Type': 'application/json' }
            });
        },

        createUserRoles: function (userRoles) {
            return $http({
                url: '/Role/CreateUserRoles',
                method: "POST",
                data: JSON.stringify(userRoles),
                headers: { 'Content-Type': 'application/json' }
            });
        },

        saveSelectedLenders: function (selectedLenders) {
            return $http({
                url: '/Role/SaveSelectedLenders',
                method: "POST",
                data: JSON.stringify(selectedLenders),
                headers: { 'Content-Type': 'application/json' }
            });
        },

        createUserLARLinks: function (selectedFhas) { 
            return $http({
                url: '/Role/CreateUserLARLinks',
                method: "POST",
                data: JSON.stringify(selectedFhas),
                headers: { 'Content-Type': 'application/json' }
            });
        }
    }
}]);