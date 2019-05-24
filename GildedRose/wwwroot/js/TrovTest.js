var TROV = TROV || {};

TROV.Store = function () {
    this.error = new TROV.Error();
    this.customer = new TROV.Customer();
    this.items = [];
    this.history = [];
    this.cart = new TROV.Cart();
    this.scope = null;
}
TROV.Store.prototype = {
    SetScope: function (ngScope) {
        ngScope.Store = this;
        this.scope = ngScope;
    },
    GetItem: function () {
        let _self = this;
        AJAX.Get("/api/Home/GetItem").then(d => {
            _self.items = d;
            _self.ResetError();
            _self.scope.$apply();
        }).catch(e => {
            _self.error = new TROV.Error(e);
            _self.scope.$apply();
        });
    },
    Login: function () {
        let _self = this;
        let data = {
            userName: $("#loginUser").val(),
            password: $("#loginPassword").val()
        };
        AJAX.Post("/api/Home/Login", data).then(d => {
            _self.customer = new TROV.Customer(d);
            localStorage.setItem("jwtToken", d.accessToken);
            _self.ResetError();
            _self.scope.$apply();
        }).catch(e => {
            _self.error = new TROV.Error(e);
            _self.scope.$apply();
        });
    },
    Checkout: function () {
        let _self = this;
        if (this.QtySum() <= 0 || this.AmtSum() <= 0) return;
        if (this.customer.accessToken === "") return;

        let data = {
            accessToken: this.customer.accessToken,
            cart: this.cart
        };
        AJAX.Post("/api/Home/Checkout", data).then(d => {
            _self.history = d;
            _self.cart.items = [];
            _self.ResetError();
            _self.scope.$apply();
        }).catch(e => {
            _self.error = new TROV.Error(e);
            _self.scope.$apply();
        });
    },

    ItemName: function (itemId) {
        let item = $.findInArray(this.items, { id: itemId });
        return item ? item.name : "";
    },
    ItemPrice: function (itemId) {
        let item = $.findInArray(this.items, { id: itemId });
        return item ? item.price : 0;
    },
    QtySum: function () {
        let qtySum = 0;
        for (let i = 0; i < this.cart.items.length; i++) {
            qtySum += this.cart.items[i].quantity;
        }
        return qtySum;
    },
    AmtSum: function () {
        let amtSum = 0;
        for (let i = 0; i < this.cart.items.length; i++) {
            amtSum += this.cart.items[i].quantity * this.ItemPrice(this.cart.items[i].itemId);
        }
        return amtSum;
    },
    ResetError: function () {
        this.error.code = 0;
        this.error.message = "";
    }
}
TROV.Customer = function (customer) {
    if (customer) {
        this.firstName = customer.firstName || "";
        this.lastName = customer.lastName || "";
        this.accessToken = customer.accessToken || "";
    }
    else {
        this.firstName = "";
        this.lastName = "";
        this.accessToken = "";
    }
}

TROV.Cart = function () {
    this.items = [];
}
TROV.Cart.prototype = {
    AddtoCart: function (item) {
        if (item.quantity > 0) {
            let existItem = $.findInArray(this.items, { itemId: item.id });
            if (existItem)
                existItem.quantity += item.quantity;
            else
                this.items.push(new TROV.CartItem(item));
        }
    }
}

TROV.CartItem = function (item) {
    if (item) {
        this.itemId = item.id;
        this.quantity = item.quantity || 0;
    }
    else
    {
        this.itemId = 0;
        this.quantity = 0;
    }
}


TROV.Error = function (error) {
    if (error) {
        this.code = error.code || 0;
        this.message = error.message || "";
    }
    else {
        this.code = 0;
        this.message = "";
    }
};

// AJAX 
var AJAX = {
    Get: function (url) {
        return this.Call(url, "get");
    },
    Post: function (url, data) {
        return this.Call(url, "post", data);
    },
    Call: function (url, method, data) {
        let defer = $.Deferred();
        $.ajax({
            dataType: 'json',
            data: JSON.stringify(data),
            beforeSend: function (xhr) {   //Include the bearer token in header
                xhr.setRequestHeader("Authorization", 'Bearer ' + localStorage.getItem("jwtToken"));
            },
            contentType: "application/json; charset=utf-8",
            error: function (xhr, tStatus, errorTh) {
                let error = new TROV.Error();
                error.code = xhr.status;
                error.message = xhr.responseText;
                defer.reject(error);
            },
            success: function (respData, respStatus) {
                defer.resolve(respData);
            },
            type: method,
            url: url
        });
        return defer.promise();
    }
}

// Functions
$.extend({
    findInArray: function (arr, obj) {
        if ($.isArray(arr)) {
            for (var ridx = 0; ridx < arr.length; ridx++) {
                let find = true;
                var el = arr[ridx];
                if ($.isPlainObject(obj)) {
                    for (var key in obj) {
                        if (el[key] !== obj[key]) {
                            find = false;
                        }
                    }
                }
                else {
                    if (el !== obj) {
                        find = false;
                    }
                }
                if (find) return el;
            }
            return null;
        }
        return null;
    }
});