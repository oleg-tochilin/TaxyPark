(function () 
{
    'use strict';

    var app = angular.module('app',
        [
            'ui.grid',                 
            'ui.grid.pagination',      
            'ui.grid.resizeColumns',   
            'ui.grid.moveColumns',     
            'ui.grid.pinning',         
            'ui.grid.selection',       
            'ui.grid.autoResize',      
            'ui.grid.exporter',
            'ngMaterial'
       
        ]
    );

  
    var FindEntity = function FindEntity(data,e)
    {
      for (var i=0; i < data.length; i++) 
      {
        if (data[i].Id === e.Id) 
          return data[i];
      }
    };


    app.controller("LoginController", function ($scope, $http)
    {
      $http.get("/Account/Check")
        .then(function (response)
      {   
        $scope.login = response.data;
      });

      $scope.login = {};

      $scope.Login=function(l)
      {
        $http.post('/Account/EnterUser', l)
        .then(function(r)
          {
            $scope.login.Message = r.data.Message;
            if(r.data.IsSuccess)
              window.location="/";     
          });
      };


      $scope.Register=function(l)
      {
        $http.post('/Account/CreateUser', l)
        .then(function(r)
          {
            $scope.login.Message = r.data.Message;
            if(r.data.IsSuccess)
              window.location = r.data.RedirectUrl;
          });
      };
    });



    app.controller("ClientOrderController", function ($scope, $http)
    {
      $scope.newOrder = {};
      
      $scope.CreateOrder=function(order)
      {
        $http.post('/Home/CreateOrder', order)
        .then(function(r)
          {
            window.location="/?id="+r.data.ExternalId; 
            window.reload(true);
          });
      };

      $scope.CancelOrder=function(id)
      {
        $http.post('/Home/CancelOrder', {ExternalId: id})
        .then(function(r)
          {
            window.location.reload(true); 
          });
      };

    });
    
    app.controller("DispatcherConroller", function ($scope, $http, $mdDialog)
    {
      $http.get("/Dispatcher/GetOrders")
        .then(function (response)
      {   
        $scope.gridOptions.data = response.data;
      });
      
      function DialogController($scope, $mdDialog,order,drivers,statuses)
      {
        $scope.order = order;
        $scope.drivers = drivers;
        $scope.statuses = statuses;

        $scope.hide = function () 
        {
          $mdDialog.hide();
        };

        $scope.cancel = function () 
        {
          $mdDialog.cancel();
        };

        $scope.answer = function (answer)
        {
          $mdDialog.hide(answer);
        };
      }

      
      $scope.ShowEdit = function (order)
      {
           
        $http.get("/Dispatcher/GetDrivers")
            .then(function (dr)
        {

          $http.get("/Dispatcher/GetStatuses")
            .then(function (sr) 
            {

              $mdDialog.show(
              {
                controller: DialogController,
                templateUrl: '/Dispatcher/EditOrder',

                parent: angular.element(document.body),
                targetEvent: order,
                clickOutsideToClose: true,
                locals: 
                {
                  order: order,
                  drivers: dr.data,
                  statuses: sr.data
                },
                fullscreen: false,
                preserveScope: false
              })
              .then(function (order) 
              {
                if (order !== undefined) 
                {
                    
                  $http.post('/Dispatcher/SetOrder', order).then(function (r) 
                  {
                    var e = FindEntity($scope.gridOptions.data,r.data);
                    if(e!==undefined)
                    {
                      e.DriverName = r.data.DriverName;
                      e.StatusName = r.data.StatusName;
                    }
                  });

                }
              });
            });       
        });
      };

      $scope.gridOptions = 
      {
        //useExternalPagination: true,
        //useExternalSorting: true,
        enableFiltering: true,
        enableSorting: true,
        enableRowSelection: false,
        enableSelectAll: false,
        enableGridMenu: true,

        onRegisterApi: function (gridApi) { $scope.gridApi = gridApi; },

        columnDefs: 
        [
          {
              name: ' ',
              enableFiltering: false,
              enableSorting: false,
              width: '32',
              enableColumnResizing: false,
              cellTemplate: '<md-button class="md-primary" ng-click="grid.appScope.ShowEdit(row.entity)"><img src="/Content/edit.png"></md-button>'
          },

          { name: 'Id', displayName: '#', width: '8%' },
          { name: 'ClientName', displayName: 'Клиент', width: '14%' },
          { name: 'ClientPhoneNumber', displayName: 'Телефон', width: '14%' },
          { name: 'FromPoint', displayName: 'Откуда', width: '14%'},
          { name: 'ToPoint', displayName: 'Куда', width: '14%' },
          { name: 'DriverName', displayName: 'Водитель', width: '14%' },
          { name: 'StatusName', displayName: 'Статус', width: '14%' }
        ]
      };
    });

    
    //driver
    app.controller("DriverConroller", function ($scope, $http, $mdDialog)
    {
        $http.get("/Driver/GetOrders")
          .then(function (r)
        {   
          $scope.gridOptions.data = r.data;
        });

        function DialogController($scope, $mdDialog,order,statuses)
        {
          $scope.order = order;
          $scope.statuses = statuses;

          $scope.hide = function () 
          {
            $mdDialog.hide();
          };

          $scope.cancel = function () 
          {
            $mdDialog.cancel();
          };

          $scope.answer = function (answer)
          {
            $mdDialog.hide(answer);
          };
        }

        $scope.ChangeOrderStatus = function (order)
        {
          $http.post("/Driver/GetStatuses",order)
            .then(function(sr)
            {
              $mdDialog.show(
              {
                controller: DialogController,
                templateUrl: '/Driver/EditOrder',

                parent: angular.element(document.body),
                targetEvent: order,
                clickOutsideToClose: true,
                locals: 
                {
                  order: order,
                  statuses: sr.data
                },
                fullscreen: false,
                preserveScope: false
              })
              .then(function (order) 
              {
                if (order !== undefined) 
                {
                    
                  $http.post('/Driver/SetOrder', order).then(function (r) 
                  {
                    var e = FindEntity($scope.gridOptions.data,r.data);
                    if(e!==undefined)
                    {
                      e.StatusId = r.data.StatusId;
                      e.StatusName = r.data.StatusName;
                    }
                  });

                }
              });
            });
        };

        $scope.gridOptions = 
        {
          enableFiltering: true,
          enableSorting: true,
          enableRowSelection: false,
          enableSelectAll: false,
          enableGridMenu: true,

          onRegisterApi: function (gridApi) { $scope.gridApi = gridApi; },

          columnDefs: 
          [
            {
                name: ' ',
                enableFiltering: false,
                enableSorting: false,
                width: '32',
                enableColumnResizing: false,
                cellTemplate: '<md-button class="md-primary" ng-click="grid.appScope.ChangeOrderStatus(row.entity)"><img src="/Content/edit.png"></md-button>'
            },

            { name: 'Id', displayName: '#', width: '5%' },
            { name: 'ClientName', displayName: 'Клиент', width: '16%' },
            { name: 'ClientPhoneNumber', displayName: 'Телефон', width: '16%' },
            { name: 'FromPoint', displayName: 'Откуда', width: '16%'},
            { name: 'ToPoint', displayName: 'Куда', width: '16%' },
            { name: 'StatusName', displayName: 'Статус', width: '16%' }     
          ]
      };
    });


  //admin
    app.controller("AdminConroller", function ($scope, $http, $mdDialog)
    {
        $http.get("/Admin/GetUsers")
          .then(function (r)
        {   
          $scope.gridOptions.data = r.data;
        });

        function DialogController($scope, $mdDialog,user,roles)
        {
          $scope.user = user;
          $scope.roles = roles;

          $scope.hide = function () 
          {
            $mdDialog.hide();
          };

          $scope.cancel = function () 
          {
            $mdDialog.cancel();
          };

          $scope.answer = function (answer)
          {
            $mdDialog.hide(answer);
          };
        }

        $scope.ChangeUserRole = function (user)
        {
          $http.post("/Admin/GetRoles",user)
            .then(function(sr)
            {
              $mdDialog.show(
              {
                controller: DialogController,
                templateUrl: '/Admin/EditUser',

                parent: angular.element(document.body),
                targetEvent: user,
                clickOutsideToClose: true,
                locals: 
                {
                  user: user,
                  roles: sr.data
                },
                fullscreen: false,
                preserveScope: false
              })
              .then(function (user) 
              {
                if (user !== undefined) 
                {
                    
                  $http.post('/Admin/SetUser', user).then(function (r) 
                  {
                    var e = FindEntity($scope.gridOptions.data,r.data);
                    if(e!==undefined)
                    {
                      e.Role = r.data.Role;
                      e.RoleName = r.data.RoleName;
                    }
                  });

                }
              });
            });
        };

        $scope.gridOptions = 
        {
          enableFiltering: true,
          enableSorting: true,
          enableRowSelection: false,
          enableSelectAll: false,
          enableGridMenu: true,

          
          columnDefs: 
          [
            {
                name: ' ',
                enableFiltering: false,
                enableSorting: false,
                width: '32',
                enableColumnResizing: false,
                cellTemplate: '<md-button class="md-primary" ng-click="grid.appScope.ChangeUserRole(row.entity)"><img src="/Content/edit.png"></md-button>'
            },

            { name: 'Id', displayName: '#', width: '5%' },
            { name: 'Name', displayName: 'Имя', width: '25%' },
            { name: 'Login', displayName: 'Логин', width: '25%' },
            { name: 'Role', displayName: 'Роль', width: '25%'}
          ]
      };
    });
    
})();