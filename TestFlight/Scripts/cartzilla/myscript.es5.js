//РАБОТА С КУКИ

"use strict";

function setCookie(name, value) {
    var options = arguments.length <= 2 || arguments[2] === undefined ? {} : arguments[2];

    options = {
        path: '/'
    };

    if (options.expires instanceof Date) {
        options.expires = options.expires.toUTCString();
    }

    var updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);

    for (var optionKey in options) {
        updatedCookie += "; " + optionKey;
        var optionValue = options[optionKey];
        if (optionValue !== true) {
            updatedCookie += "=" + optionValue;
        }
    }
    document.cookie = updatedCookie;
}

function getCookie(name) {
    var matches = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
    return matches ? decodeURIComponent(matches[1]) : null;
}

function deleteCookie(name) {
    setCookie(name, "", { 'max-age': -1 });
}

//Редактировать избранное

function editWishList(productId) {
    if (getCookie('wishlist') === null) {
        setCookie('wishlist', String(productId), { 'max-age': 3600 });
        return false;
    } else {
        var wishArray = getCookie('wishlist').split(',');
        var newWishlist = '';
        var ifInclude = false;
        for (var i = 0; i < wishArray.length; i++) {
            if (productId === wishArray[i]) {
                wishArray.splice(i, 1);
                ifInclude = true;
                break;
            }
        };
        if (!ifInclude) wishArray.push(productId);

        var _iteratorNormalCompletion = true;
        var _didIteratorError = false;
        var _iteratorError = undefined;

        try {
            for (var _iterator = wishArray[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
                var wishItem = _step.value;
                newWishlist = newWishlist + String(wishItem) + ',';
            }
        } catch (err) {
            _didIteratorError = true;
            _iteratorError = err;
        } finally {
            try {
                if (!_iteratorNormalCompletion && _iterator["return"]) {
                    _iterator["return"]();
                }
            } finally {
                if (_didIteratorError) {
                    throw _iteratorError;
                }
            }
        }

        setCookie('wishlist', newWishlist.slice(0, newWishlist.length - 1), { 'max-age': 3600 });
        return ifInclude;
    }
}

//РАБОТА С ЗАКАЗОМ

// Активация кнопки

function submitButton() {
    var hour = new Date().getHours();
    var minute = new Date().getMinutes();
    var timeSpan = hour * 60 + minute;

    if (timeSpan < 420 || timeSpan > 1365) document.getElementById('js-submit').setAttribute('disabled', true);else document.getElementById('js-submit').removeAttribute('disabled');
}

//Обновление итога

function updateOrder(max_Quant) {
    var initAmount = document.getElementById('js-amount').getAttribute('value');
    var total = 0;
    var amount = 0;
    for (var i = 0; i < initAmount; i++) {
        var h_Input = document.getElementById('js-price_' + i);
        if (h_Input != null) {
            amount = amount + 1;
            total = total + parseFloat(h_Input.getAttribute('value').replace(',', '.')) * parseInt(document.getElementById('Basket_ContentQuantity_' + i).getAttribute('value'));
            if (parseInt(document.getElementById('Basket_ContentQuantity_' + i).getAttribute('value')) > max_Quant) {
                document.getElementById('Basket_ContentQuantity_' + i).setAttribute('value', max_Quant);
            }
        }
    };

    $("span#quantity").text(String(amount));
    $("span#total").text(String(total.toFixed(2)));

    var totalSmall = '00';
    if (String(total).split('.').length > 1) totalSmall = String(total).split('.')[1].substr(0, 1);

    $("span#orderTotalSmall").html('₽ ' + String(total).split('.')[0] + '.<small>' + totalSmall + '</small>');
    $("h3#orderTotalBig").html('₽ ' + String(total).split('.')[0] + '.<small>' + totalSmall + '</small>');
}

//Валидация заказа

function orderValidation(min_order, del_time) {

    var isValid = true;
    var phonePattern = /^\d[\d\(\)\ -]{4,14}\d$/;
    var digitPattern = /\-?\d/;
    var valid = {
        name: $('#Customer_Name'),
        surName: $('#Customer_SurName'),
        phone: $('#Customer_Phone'),
        street: $('#Customer_Street'),
        flat: $('#Customer_HomeNr'),
        asap: $('input[name="DeliveryASAP"]:checked'),
        delTime: $('#time-input'),
        summ: document.getElementById('orderTotalSmall'),
        isAuthent: $('#js-authent')
    };

    if (valid.name.val() === "") {
        valid.name.attr('style', 'border:double; border-color:red');
        setTimeout(function () {
            return valid.name.removeAttr('style');
        }, 1000);
        valid.name.attr('placeholder', 'Введите Ваше имя пожалуйста');
        isValid = false;
    } else {
        valid.name.removeAttr('style');
        valid.name.removeAttr('placeholder');
    }

    if (valid.surName.val() === "") {
        valid.surName.attr('style', 'border:double; border-color:red');
        setTimeout(function () {
            return valid.surName.removeAttr('style');
        }, 1000);
        valid.surName.attr('placeholder', 'Введите Вашу фамилию пожалуйста');
        isValid = false;
    } else {
        valid.surName.removeAttr('style');
        valid.surName.removeAttr('placeholder');
    }

    if (!phonePattern.test(valid.phone.val())) {
        valid.phone.attr('style', 'border:double; border-color:red');
        setTimeout(function () {
            return valid.phone.removeAttr('style');
        }, 1000);
        valid.phone.attr('placeholder', 'Укажите контактный номер телефона, пожалуйста');
        isValid = false;
    } else {
        valid.phone.removeAttr('style');
        valid.phone.removeAttr('placeholder');
    }

    if (valid.street.val() === "") {
        valid.street.attr('style', 'border:double; border-color:red');
        setTimeout(function () {
            return valid.street.removeAttr('style');
        }, 1000);
        valid.street.attr('placeholder', 'Укажите улицу, пожалуйста');
        isValid = false;
    } else {
        valid.street.removeAttr('style');
        valid.street.removeAttr('placeholder');
    }

    if (!digitPattern.test(valid.flat.val())) {
        valid.flat.attr('style', 'border:double; border-color:red');
        setTimeout(function () {
            return valid.flat.removeAttr('style');
        }, 1000);
        valid.flat.attr('placeholder', 'Укажите номер дома, пожалуйста');
        isValid = false;
    } else {
        valid.flat.removeAttr('style');
        valid.flat.removeAttr('placeholder');
    }

    if (valid.asap.val() === 'false') {
        var hour = Number(valid.delTime.val().split(':')[0]);
        var minute = Number(valid.delTime.val().split(':')[1]);
        if (hour * 60 + minute < del_time[0] * 60 + del_time[1] || hour * 60 + minute > del_time[2] * 60 + del_time[3]) $('.val-delTime').toast('show');
        isValid = false;
    }

    if (Number(valid.summ.textContent.substr(2, valid.summ.textContent.length)) < min_order) {
        $('.val-summ').toast('show');
        isValid = false;
    }

    if (valid.isAuthent.val() !== '1') {
        $('.val-auth').toast('show');
        isValid = false;
    }

    return isValid;
}

//РАБОТА С КОРЗИНОЙ

//Создание новой корзины

function getNewBasket() {
    var BasketId = 0;
    $.ajax({
        url: "/api/basket/",
        method: "POST",
        async: false,
        dataType: "text",
        success: function success(data) {
            BasketId = data;
        }
    });
    return BasketId;
}

//Положить в корзину

function putIntoBasket(basketId, productId) {
    $.ajax({
        url: "/api/basket/PutIntoBasket?BasketId=" + basketId + "&ProductId=" + productId,
        async: false,
        method: "POST",
        dataType: "text",
        success: function success(data) {}
    });

    $("#basket_content").dropdown('dispose');
    showBasket(basketId);
    updateHeader(basketId);
}

//Удалить из корзины

function deleteFromBasket(basketId, productId) {
    $.ajax({
        url: "/api/basket/Delete?BasketId=" + basketId + "&ProductId=" + productId,
        async: false,
        method: "DELETE",
        dataType: "text",
        success: function success(data) {}
    });

    $("#basket_content").dropdown('dispose');
    showBasket(basketId);
    updateHeader(basketId);
}

//Редактировать количество

function editBasket(basketId, productId, typeId) {
    var type = false;
    if (typeId == 'plus') type = 1;else if (typeId == 'minus') type = 0;
    $.ajax({
        url: "/api/basket/EditBasket?BasketId=" + basketId + "&ProductId=" + productId + "&Type=" + type,
        async: false,
        method: "PUT",
        dataType: "text",
        success: function success(data) {}
    });

    $("#basket_content").dropdown('dispose');
    showBasket(basketId);
    updateHeader(basketId);
}

//Обновить заголовок

function updateHeader(basketId) {
    var i = 0;
    var total = 0;
    var totalMain = '';
    var totalSmall = '';

    $.get({
        url: "/api/basket/" + basketId,
        method: "GET",
        dataType: "text",
        success: function success(data) {
            var basketList = [];
            var productString = JSON.parse(data);

            $.each(productString, function (key, product) {
                var obj_basket = {};
                for (var prop in product) obj_basket[prop] = product[prop];
                basketList.push(obj_basket);
            });

            var _iteratorNormalCompletion2 = true;
            var _didIteratorError2 = false;
            var _iteratorError2 = undefined;

            try {
                for (var _iterator2 = basketList[Symbol.iterator](), _step2; !(_iteratorNormalCompletion2 = (_step2 = _iterator2.next()).done); _iteratorNormalCompletion2 = true) {
                    var obj = _step2.value;

                    i++;
                    total = total + obj["Price"] * obj["Quant"];
                }
            } catch (err) {
                _didIteratorError2 = true;
                _iteratorError2 = err;
            } finally {
                try {
                    if (!_iteratorNormalCompletion2 && _iterator2["return"]) {
                        _iterator2["return"]();
                    }
                } finally {
                    if (_didIteratorError2) {
                        throw _iteratorError2;
                    }
                }
            }

            totalMain = String(total).split(".")[0];
            if (String(total).split(".").length > 1) totalSmall = String(total).split(".")[1].substr(0, 1);else totalSmall = "00";

            $("span#quantity").text(String(i));
            $("span#total").text(String(totalMain + "." + totalSmall));
        }
    });
}

//Контейнер основной части корзины

function basketConteiner(deleteFlag, removeButtonId, imageLink, imageSource, nameLink, name, priceMain, priceSmall, quantity, editPointer) {
    this.itemClass = '<div class="widget-cart-item py-2 border-bottom" id="' + deleteFlag + '">';
    this.removeButton = '<button class="close text-danger" id="' + removeButtonId + '" type="button" aria-label="Remove"><span aria-hidden="true">&times;</span></button>';
    this.itemContentClass = '<div class="media align-items-center">';
    this.imageClass = '<a class="d-block mr-2" href="/Home/Product/' + imageLink + '"><img width="64" src="/Images/Products/' + imageSource + '" alt="Product" /></a>';
    this.bodyClass = '<div class="media-body">';
    this.nameClass = '<h6 class="widget-product-title"><a href="/Home/Product/' + nameLink + '">' + name + '</a></h6>';
    this.priceClass = '<div class="widget-product-meta"><span class="text-accent mr-2">' + priceMain + '.<small>' + priceSmall + '</small></span>';
    this.quantClass = '<span class="text-muted">x ' + quantity + '</span></div></div>';
    this.editButtonClass = '<div class="btn-group btn-group-sm mr-2 mb-2" role="group" aria-label="Second group">';
    this.plusButton = '<button type="button" id="' + editPointer + '" class="btn js-minus btn-outline-secondary">-</button>';
    this.minusButton = '<button type="button" id="' + editPointer + '" class="btn js-plus btn-outline-secondary">+</button></div>';
    this.end = '</div></div >';
}

//Контейнер итоговой части корзины

function totalConteiner(totalMain, totalSmall, checkOutLink) {
    this.wrapClass = '<div class="d-flex flex-wrap justify-content-between align-items-center pt-3">';
    this.totalClass = '<div class="font-size-sm mr-2 py-2"><span class="text-muted">Итог: ₽</span>';
    this.total = '<span class="text-accent font-size-base ml-1">' + totalMain + '.<small>' + totalSmall + '</small></span></div>';
    this.chekoutLink = '<a class="btn btn-primary btn-sm" href="/Customer/OrderList/' + checkOutLink + '">';
    this.chekout = '<i class="czi-card mr-2 font-size-base align-middle"></i>Купить<i class="czi-arrow-right ml-1 mr-n1"></i></a></div>';
}

//Формирование содержимого корзины

function basketContent(basketList, basketId) {

    var summPart = '';
    var summFull = '<div class="widget widget-cart px-3 pt-2 pb-3"><div style="height: 15rem;" data-simplebar data-simplebar-auto-hide="false">';
    var basketArr = [];
    var priceMain = '';
    var priceSmall = '';
    var i = 0;
    var total = 0;
    var totalMain = '';
    var totalSmall = '';

    var _iteratorNormalCompletion3 = true;
    var _didIteratorError3 = false;
    var _iteratorError3 = undefined;

    try {
        for (var _iterator3 = basketList[Symbol.iterator](), _step3; !(_iteratorNormalCompletion3 = (_step3 = _iterator3.next()).done); _iteratorNormalCompletion3 = true) {
            var obj = _step3.value;

            priceMain = String(obj["Price"]).split(".")[0];
            if (String(obj["Price"]).split(".").length > 1) priceSmall = String(obj["Price"]).split(".")[1];else priceSmall = "00";
            total = total + obj["Price"] * obj["Quant"];

            basketArr[i] = new basketConteiner(obj['ProductId'], obj['ProductId'], obj['ProductId'], obj['ProductPhoto'], obj['ProductId'], obj['ProductName'], priceMain, priceSmall, obj['Quant'], obj['ProductId']);
            for (var prop in basketArr[i]) {
                summPart = summPart + basketArr[i][prop];
            }summFull = summFull + summPart;

            i++;
            summPart = '';
            priceMain = '';
            priceSmall = '';
        }
    } catch (err) {
        _didIteratorError3 = true;
        _iteratorError3 = err;
    } finally {
        try {
            if (!_iteratorNormalCompletion3 && _iterator3["return"]) {
                _iterator3["return"]();
            }
        } finally {
            if (_didIteratorError3) {
                throw _iteratorError3;
            }
        }
    }

    summFull = summFull + '</div>';

    totalMain = String(total).split(".")[0];
    if (String(total).split(".").length > 1) totalSmall = String(total).split(".")[1].substr(0, 1);else totalSmall = "00";
    var totalCont = new totalConteiner(totalMain, totalSmall, basketId);
    for (var tprop in totalCont) {
        summFull = summFull + totalCont[tprop];
    }document.getElementById('basket_content').innerHTML = summFull + '</div>';
}

//Показ карзины

function showBasket(basketId) {
    if (basketId === 0 || basketId === null) {
        document.getElementById('basket_content').innerHTML = '<div class="widget widget-cart" style="text-align:center; color:#7d879c;">Ваша корзина пуста</div>';
    } else if (basketId !== 0) {
        $.get({
            url: "/api/basket/" + basketId,
            method: "GET",
            dataType: "text",
            success: function success(data) {
                var basketList = [];
                var productString = JSON.parse(data);
                $.each(productString, function (key, product) {
                    var obj_basket = {};
                    for (var prop in product) obj_basket[prop] = product[prop];
                    basketList.push(obj_basket);
                });

                if (basketList.length == 0) {
                    document.getElementById('basket_content').innerHTML = '<div class="widget widget-cart" style="text-align:center; color:#7d879c;">Ваша корзина пуста</div>';
                } else basketContent(basketList, basketId);
            }
        });
    };
}

// РАБОТА С КАТАЛОГОМ

// Контейнер продукта

function productConteiner(tagType, tag, wishId, imageLink, imageSource, typeLink, type, nameLink, name, priceMain, priceSmall, oldPrice, oldPriceSm, buyButtonId) {
    this.product = '<div class="card product-card card-static pb-3">';
    this.tag = '<span class="badge badge-' + tagType + ' badge-shadow">' + tag + '</span>';
    this.wishButton = '<button class="btn-wishlist btn-sm" id="' + wishId + '" type="button" data-toggle="tooltip" data-placement="left" title="В избранное"><i class="czi-heart"></i></button>';
    this.imageLink = '<a class="card-img-top d-block overflow-hidden" href="/Home/Product/' + imageLink + '">';
    this.image = '<img src="/Images/Products/' + imageSource + '" alt="Product"></a>';
    this.type = '<div class="card-body py-2"><a class="product-meta d-block font-size-xs pb-1" href="' + typeLink + '">' + type + '</a>';
    this.name = '<h3 class="product-title font-size-sm"><a href="/Home/Product/' + nameLink + '">' + name + '</a></h3>';
    this.price = '<div class="product-price"><span class="text-accent">₽' + priceMain + '.' + '<small>' + priceSmall + '</small></span>';
    this.oldPrice = '<del class="font-size-sm text-muted">' + oldPrice + '<small>' + oldPriceSm + '</small></del></div></div>';
    this.buyButton = '<div class="product-floating-btn"><button class="btn js-put btn-primary btn-shadow btn-sm" Id="' + buyButtonId + '" type="button">+<i class="czi-cart font-size-base ml-1"></i></button>';
    this.endProduct = '</div></div>';
}

// Формирование содержимого по запросу

function catalogOutput(productList) {

    if (productList != null) {
        var summPart = "";
        var summFull = "";
        var tagType = "";
        var tag = "";
        var photo = "";
        var mainPrice = "";
        var mainSmall = "";
        var oldPrice = "";
        var oldPriceSm = "";
        var pCont = [];
        var i = 0;
        var currentDate = new Date();

        var _iteratorNormalCompletion4 = true;
        var _didIteratorError4 = false;
        var _iteratorError4 = undefined;

        try {
            for (var _iterator4 = productList[Symbol.iterator](), _step4; !(_iteratorNormalCompletion4 = (_step4 = _iterator4.next()).done); _iteratorNormalCompletion4 = true) {
                var obj = _step4.value;

                var CrDate = new Date(String(obj["CrDate"]).slice(0, 10));
                mainPrice = String(obj["Price"]).split(".")[0];
                if (String(obj["Price"]).split(".").length > 1) mainSmall = String(obj["Price"]).split(".")[1];else mainSmall = "00";
                if (obj["Photo"] !== "") photo = obj["Photo"].split("#")[0];else photo = "";

                if (obj["DiscountProd"] == true) {
                    tagType = "danger";
                    tag = "Sale";
                    if (obj["OldPrice"] != null) {
                        oldPrice = " " + String(obj["OldPrice"]).split(".")[0] + ".";
                        if (String(obj["OldPrice"]).split(".").length > 1) oldPriceSm = String(obj["OldPrice"]).split(".")[1];else oldPriceSm = "00";
                    }
                } else if (obj["Popularity"] > 6) {
                    tagType = "warning";
                    tag = "Popular";
                    oldPrice = "";
                    oldPriceSm = "";
                } else if (CrDate.getFullYear() * 365 + CrDate.getMonth() * 30 + CrDate.getDate() - (currentDate.getFullYear() * 365 + currentDate.getMonth() * 30 + currentDate.getDate()) < 50) {
                    tagType = "info";
                    tag = "New";
                    oldPrice = "";
                    oldPriceSm = "";
                }
                pCont[i] = new productConteiner(tagType, tag, obj["Id"], obj["Id"], photo, "", obj["ProdTypeName"], obj["Id"], obj["Name"], mainPrice, mainSmall, oldPrice, oldPriceSm, obj["Id"]);
                for (var prop in pCont[i]) {
                    summPart = summPart + pCont[i][prop];
                }summFull = summFull + '<div class="col-xl-3 col-lg-6 col-md-4 col-sm-6 px-2 mb-3">' + summPart + '</div>';

                i++;
                summPart = "";
                tagType = "";
                tag = "";
                photo = "";
                mainPrice = "";
                mainSmall = "";
                oldPrice = "";
                oldPriceSm = "";
            }
        } catch (err) {
            _didIteratorError4 = true;
            _iteratorError4 = err;
        } finally {
            try {
                if (!_iteratorNormalCompletion4 && _iterator4["return"]) {
                    _iterator4["return"]();
                }
            } finally {
                if (_didIteratorError4) {
                    throw _iteratorError4;
                }
            }
        }

        ;
        document.getElementById('main_conteiner').innerHTML = summFull;
        $('[data-toggle="tooltip"]').tooltip('enable');
    };
};

// Поиск по типу продуктов

function getFromType(ProdType, SubType, ThSubType, filterType) {

    var typeFilter = [ProdType, SubType, ThSubType];
    var productList = [];

    $.get({
        url: "/api/product",
        method: "GET",
        dataType: "text",
        success: function success(data) {

            var productString = JSON.parse(data);
            $.each(productString, function (key, product) {

                var obj_type = {};

                for (var prop in product) {
                    obj_type[prop] = product[prop];
                };

                if (obj_type['Avail'] === true) {

                    if (typeFilter[1] === "all" && typeFilter[2] === "all") {
                        if (obj_type["ProdTypeId"] == typeFilter[0]) productList.push(obj_type);
                    } else if (typeFilter[2] === "all") {
                        if (obj_type["ProdTypeId"] == typeFilter[0] && obj_type["SubTypeId"] == typeFilter[1]) productList.push(obj_type);
                    } else {
                        if (obj_type["ProdTypeId"] == typeFilter[0] && obj_type["SubTypeId"] == typeFilter[1] && obj_type["ThSubTypeId"] == typeFilter[2]) productList.push(obj_type);
                    };
                };
            });

            if (productList.length === 0) document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но по Вашему запросу ничего не найдено...</h4>';else {
                switch (filterType) {
                    case 'pop':
                        productList.sort(function (a, b) {
                            return b.Popularity - a.Popularity;
                        });break;
                    case 'cheap':
                        productList.sort(function (a, b) {
                            return a.Price - b.Price;
                        });break;
                    case 'exp':
                        productList.sort(function (a, b) {
                            return b.Price - a.Price;
                        });break;
                }
                catalogOutput(productList);
            };
        }
    });
};

// Поиск по текстовому фильтру

function getFromText(inputText, prodType, filterType) {

    var nameFilter = {
        name: inputText,
        type: prodType
    };
    var productList = [];

    $.get({
        url: "/api/product",
        method: "GET",
        dataType: "text",
        success: function success(data) {

            var productString = JSON.parse(data);
            $.each(productString, function (key, product) {

                var obj_name = {};

                for (var prop in product) {
                    obj_name[prop] = product[prop];
                };

                if (obj_name['Avail'] === true) {
                    if (String(obj_name["Finder"]).includes(nameFilter.name) && nameFilter.type === "all") productList.push(obj_name);else if (String(obj_name["Finder"]).includes(nameFilter.name) && nameFilter.type == obj_name["ProdTypeId"]) productList.push(obj_name);
                };
            });

            if (productList.length === 0) document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но по Вашему запросу ничего не найдено...</h4>';else {
                switch (filterType) {
                    case 'pop':
                        productList.sort(function (a, b) {
                            return b.Popularity - a.Popularity;
                        });break;
                    case 'cheap':
                        productList.sort(function (a, b) {
                            return a.Price - b.Price;
                        });break;
                    case 'exp':
                        productList.sort(function (a, b) {
                            return b.Price - a.Price;
                        });break;
                }
                catalogOutput(productList);
            };
        }
    });
};

//Избранное

function getWishList(filterType) {

    if (getCookie('wishlist') === null) document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но Ваш список избранного пуст...</h4>';else {
        (function () {
            var wishArray = getCookie('wishlist').split(',');
            var productList = [];
            $.get({
                url: "/api/product",
                method: "GET",
                dataType: "text",
                success: function success(data) {

                    var productString = JSON.parse(data);
                    $.each(productString, function (key, product) {

                        var obj = {};
                        for (var prop in product) {
                            obj[prop] = product[prop];
                        };
                        if (obj['Avail'] === true) {
                            var _iteratorNormalCompletion5 = true;
                            var _didIteratorError5 = false;
                            var _iteratorError5 = undefined;

                            try {
                                for (var _iterator5 = wishArray[Symbol.iterator](), _step5; !(_iteratorNormalCompletion5 = (_step5 = _iterator5.next()).done); _iteratorNormalCompletion5 = true) {
                                    var productId = _step5.value;
                                    if (obj['Id'] == productId) productList.push(obj);
                                }
                            } catch (err) {
                                _didIteratorError5 = true;
                                _iteratorError5 = err;
                            } finally {
                                try {
                                    if (!_iteratorNormalCompletion5 && _iterator5["return"]) {
                                        _iterator5["return"]();
                                    }
                                } finally {
                                    if (_didIteratorError5) {
                                        throw _iteratorError5;
                                    }
                                }
                            }
                        }
                    });

                    if (productList.length === 0) document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но Ваш список избранного пуст...</h4>';else {
                        switch (filterType) {
                            case 'pop':
                                productList.sort(function (a, b) {
                                    return b.Popularity - a.Popularity;
                                });break;
                            case 'cheap':
                                productList.sort(function (a, b) {
                                    return a.Price - b.Price;
                                });break;
                            case 'exp':
                                productList.sort(function (a, b) {
                                    return b.Price - a.Price;
                                });break;
                        }
                        catalogOutput(productList);
                    };
                }
            });
        })();
    }
}

//Популярные продукты

function getBestsellers(filterType) {

    var productList = [];
    $.get({
        url: "/api/product",
        method: "GET",
        dataType: "text",
        success: function success(data) {

            var productString = JSON.parse(data);
            $.each(productString, function (key, product) {

                var obj = {};
                for (var prop in product) {
                    obj[prop] = product[prop];
                };
                if (obj['Avail'] === true && obj['Popularity'] > 6) productList.push(obj);
            });

            if (productList.length === 0) document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но по Вашему запросу ничего не найдено...</h4>';else {
                switch (filterType) {
                    case 'pop':
                        productList.sort(function (a, b) {
                            return b.Popularity - a.Popularity;
                        });break;
                    case 'cheap':
                        productList.sort(function (a, b) {
                            return a.Price - b.Price;
                        });break;
                    case 'exp':
                        productList.sort(function (a, b) {
                            return b.Price - a.Price;
                        });break;
                }
                catalogOutput(productList);
            };
        }
    });
}

//Продукты со скидкой

function getDiscounts(filterType) {

    var productList = [];
    $.get({
        url: "/api/product",
        method: "GET",
        dataType: "text",
        success: function success(data) {

            var productString = JSON.parse(data);
            $.each(productString, function (key, product) {

                var obj = {};
                for (var prop in product) {
                    obj[prop] = product[prop];
                };
                if (obj['Avail'] === true && obj['DiscountProd'] === true) productList.push(obj);
            });

            if (productList.length === 0) document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но по Вашему запросу ничего не найдено...</h4>';else {
                switch (filterType) {
                    case 'pop':
                        productList.sort(function (a, b) {
                            return b.Popularity - a.Popularity;
                        });break;
                    case 'cheap':
                        productList.sort(function (a, b) {
                            return a.Price - b.Price;
                        });break;
                    case 'exp':
                        productList.sort(function (a, b) {
                            return b.Price - a.Price;
                        });break;
                }
                catalogOutput(productList);
            };
        }
    });
}

