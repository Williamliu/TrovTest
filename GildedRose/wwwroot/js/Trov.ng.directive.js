var TROV_NG = angular.module("TrovComponents", []);
TROV_NG.directive("trov.error", function () {
    return {
        restrict: "E",
        replace: true,
        transclude: true,
        scope: {
            store: "="
        },
        template: [
            '<div>',
                '<span ng-if="store.error.code!=0">',
                    '<span class="error">Error Code:</span> <span class="text">{{store.error.code}}</span><br>',
                    '<span class="error">Error Message:</span> <span class="text">{{store.error.message}}</span>',
                '</span>',
            '</div>'
        ].join(''),
        controller: function ($scope) {
        }
    };
});

TROV_NG.directive("trov.products", function () {
    return {
        restrict: "E",
        replace: true,
        transclude: true,
        scope: {
            store: "="
        },
        template: [
            '<div>',
                '<h4>Product List</h4>',
                '<table class="table-striped" width="100%">',
                    '<tr>',
                        '<th>Id</th>',
                        '<th>Name</th>',
                        '<th>Description</th>',
                        '<th width="100">Price</th>',
                        '<th width="60" style="padding-left:12px;">Qty</th>',
                        '<th width="60" style="padding-left:12px;"></th>',
                    '</tr>',
                    '<tr ng-repeat="item in store.items" ng-init="item.quantity=1">',
                        '<td>{{item.id}}</td>',
                        '<td>{{item.name}}</td>',
                        '<td>{{item.description}}</td>',
                        '<td width="100">{{item.price|currency:"$ "}}</td>',
                        '<td width="60" style="padding-left:12px;"><input type="number" ng-model="item.quantity" style="width:60px;text-align:center;" value="1"></td>',
                        '<td width="60" style="padding-left:12px;"><a class="button cart" ng-click="store.cart.AddtoCart(item)" title="Add to shopping cart"></a></td>',
                    '</tr>',
                '</table>',
            '</div>'
        ].join(''),
        controller: function ($scope) {
        }
    };
});

TROV_NG.directive("trov.cart", function () {
    return {
        restrict: "E",
        replace: true,
        transclude: true,
        scope: {
            store: "="
        },
        template: [
            '<div>',
            '<h4>Shopping Cart</h4>',
            '<table class="table-striped" width="100%">',
                '<tr>',
                    '<td width="200">Name</td>',
                    '<td width="100">Price</td>',
                    '<td width="60">Qty</td>',
                    '<td width="120" align="right">Amount</td>',
                '</tr>',
                '<tr ng-repeat="cartItem in store.cart.items">',
                    '<td>{{store.ItemName(cartItem.itemId)}}</td>',
                    '<td>{{store.ItemPrice(cartItem.itemId)|currency:"$ "}}</td>',
                    '<td><input type="number" ng-model="cartItem.quantity" style="width:60px;text-align:center;"></td>',
                    '<td style="padding-left:12px;" align="right">{{ (cartItem.quantity * store.ItemPrice(cartItem.itemId))|currency:"$ "}}</td>',
                '</tr>',
                '<tr ng-if="store.cart.items.length>0">',
                    '<td></td>',
                    '<td>Total</td>',
                    '<td align="center">{{store.QtySum()}}</td>',
                    '<td style="padding-left:12px;" align="right">{{store.AmtSum()|currency:"$ "}}</td>',
                '</tr>',
                '<tr ng-if="store.AmtSum()>0 && store.QtySum()>0 && store.customer.accessToken!=\'\'">',
                    '<td colspan="4" align="center" style="padding-top:12px;"><button ng-click="store.Checkout()">Check Out</button></td>',
                '</tr>',
                '<tr ng-if="store.AmtSum()>0 && store.QtySum()>0 && store.customer.accessToken==\'\'">',
                    '<td colspan="4" align="center" style="padding-top:12px;">Login First</td>',
                '</tr>',
            '</table>',
            '</div>'
        ].join(''),
        controller: function ($scope) {
        }
    };
});


TROV_NG.directive("trov.customer", function () {
    return {
        restrict: "E",
        replace: true,
        transclude: true,
        scope: {
            store: "="
        },
        template: [
            '<div>',
                '<h4>Customer: user1/123 user2/abc</h4>',
                '<div ng-if="store.customer.accessToken!=\'\'">',
                    'Name: <span>{{store.customer.firstName}} {{store.customer.lastName}}</span>', 
                '</div>',
                '<div ng-if="store.customer.accessToken==\'\'">',
                    '<div class="row">',
                        '<div class="col-lg-2 text-right">User</div>',
                        '<div class="col-lg-3"><input id="loginUser" type="textbox" value="user1" /></div>',
                        '<div class="col-lg-2 text-right">Password</div>',
                        '<div class="col-lg-3"><input id="loginPassword" type="password" value="123" /></div>',
                        '<div class="col-lg-2">',
                            '<button ng-click="store.Login()">LOGIN</button>',
                        '</div>',
                    '</div>',
                '</div>',
            '</div>'
        ].join(''),
        controller: function ($scope) {
        }
    };
});

TROV_NG.directive("trov.history", function () {
    return {
        restrict: "E",
        replace: true,
        transclude: true,
        scope: {
            store: "="
        },
        template: [
            '<div>',
            '<h4>Shopping History</h4>',
            '<div ng-repeat="order in store.history|orderBy:\'-id\'">',
                '<div style="margin-top:12px;background-color:#eeeeee;">',
                'Order Id: <b style="color:red;">{{order.id}}</b> - Customer: {{order.customer.firstName}} {{order.customer.firstName}} - Date: {{order.orderDateTime|date:"yyyy-MM-dd hh:mm"}}<br>',
                '</div>',
                '<table class="table" width="100%">',
                '<tr>',
                '<td width="60">ItemId</td>',
                '<td width="200">Name</td>',
                '<td width="100">Price</td>',
                '<td width="60">Qty</td>',
                '<td width="120" align="right">Amount</td>',
                '</tr>',
                '<tr ng-repeat="item in order.items">',
                '<td>{{item.itemId}}</td>',
                '<td>{{item.itemName}}</td>',
                '<td>{{item.price|currency:"$ "}}</td>',
                '<td>{{item.quantity}}</td>',
                '<td style="padding-left:12px;" align="right">{{ (item.quantity * item.price)|currency:"$ "}}</td>',
                '</tr>',
                '</table>',
            '</div>',
            '</div>'
        ].join(''),
        controller: function ($scope) {
        }
    };
});
