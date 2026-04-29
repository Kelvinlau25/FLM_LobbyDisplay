(function() {
  angular.module('myApp', ['darthwade.loading'])
      .controller('myCtrl',['$scope',function($scope){
        $scope.skm_LockScreen = function(){
            $loading.start('users');
        };
      }]);
})();