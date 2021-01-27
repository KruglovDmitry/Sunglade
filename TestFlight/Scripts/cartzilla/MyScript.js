//РАБОТА С КУКИ

function setCookie(name, value, options = {}) {
    options = {
        path: '/',
    };

    if (options.expires instanceof Date) {
        options.expires = options.expires.toUTCString();
    }

    let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);

    for (let optionKey in options) {
        updatedCookie += "; " + optionKey;
        let optionValue = options[optionKey];
        if (optionValue !== true) {
            updatedCookie += "=" + optionValue;
        }
    }
    document.cookie = updatedCookie;
}

function getCookie(name)
{
    let matches = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
    return matches ? decodeURIComponent(matches[1]) : 0;
}

function deleteCookie(name)
{
    setCookie(name, "", { 'max-age': 0 })
}

//Редактировать избранное

function colorWishlist()
{
    let isInList = false;
    let wishArray = getCookie('wishlist').toString().split(',');
    for (var button of document.getElementsByClassName('btn-wishlist')) {
        for (var itemId of wishArray)
            if (button.getAttribute('id') == itemId) {
                button.firstChild.setAttribute('style', 'color:red');
                isInList = true;
            }
        if (!isInList) button.firstChild.removeAttribute('style');
        isInList = false;
    }
}

function editWishList(productId) 
{
    if (getCookie('wishlist') == 0) {
        setCookie('wishlist', String(productId), { 'max-age': 518400 });
        return false;
    }
    else 
    {
        let wishArray = getCookie('wishlist').split(',');
        let newWishlist = '';
        let ifInclude = false;
        for (let i = 0; i < wishArray.length; i++)
            if (productId === wishArray[i]) {
                wishArray.splice(i, 1);
                ifInclude = true;
                break
            };
            if (!ifInclude) wishArray.push(productId);

        for (let wishItem of wishArray) newWishlist = newWishlist + String(wishItem) + ',';
        setCookie('wishlist', newWishlist.slice(0, newWishlist.length - 1), { 'max-age': 518400 });
        return ifInclude;
    }
}

//РАБОТА С ЗАКАЗОМ

// Активация кнопки

function submitButton()
{
    let hour = new Date().getHours();
    let minute = new Date().getMinutes();
    let timeSpan = hour * 60 + minute;

    if ((timeSpan < 420) || (timeSpan > 1365)) 
        document.getElementById('js-submit').setAttribute('disabled', true);
    else
        document.getElementById('js-submit').removeAttribute('disabled');
}

//Обновление итога

function updateOrder(max_Quant)
{
    let initAmount = document.getElementById('js-amount').getAttribute('value');
    let total = 0;
    let amount = 0;
    for (let i = 0; i < initAmount; i++) {
        let h_Input = document.getElementById('js-price_' + i);
        if (h_Input != null) {
            amount = amount + 1;
            total = total + parseFloat(h_Input.getAttribute('value').replace(',', '.')) * parseInt(document.getElementById('Order_ContentQuantity_' + i).getAttribute('value'));
            if (parseInt(document.getElementById('Order_ContentQuantity_' + i).getAttribute('value')) > max_Quant) {
                document.getElementById('Order_ContentQuantity_' + i).setAttribute('value', max_Quant);
            }
        }
    };

    $("span#quantity").text(String(amount));
    $("span#total").text(String(total.toFixed(2)));

    let totalSmall = '00';
    if (String(total).split('.').length > 1) totalSmall = String(total).split('.')[1].substr(0, 1);

    $("span#orderTotalSmall").html('₽ ' + String(total).split('.')[0] + '.<small>' + totalSmall + '</small>');
    $("h3#orderTotalBig").html('₽ ' + String(total).split('.')[0] + '.<small>' + totalSmall + '</small>');
}

//Валидация заказа

function orderValidation(min_order, del_time) {

    let isValid = true;
    let phonePattern = /^\d[\d\(\)\ -]{4,14}\d$/;
    let digitPattern = /\-?\d/;
    let valid = {
        name: $('#Actual_Name'),
        surName: $('#Actual_SurName'),
        phone: $('#Actual_Phone'),
        street: $('#Actual_Street'),
        flat: $('#Actual_HomeNr'),
        asap: $('input[name="DeliveryASAP"]:checked'),
        delTime: $('#time-input'),
        summ: document.getElementById('orderTotalSmall'),
        isAuthent: $('#js-authent')
    };

    if (valid.name.val() === "") {
        valid.name.attr('style', 'border:double; border-color:red');
        setTimeout(() => valid.name.removeAttr('style'), 1000);
        valid.name.attr('placeholder', 'Введите Ваше имя пожалуйста');
        isValid = false;
    }
    else
    {
        valid.name.removeAttr('style');
        valid.name.removeAttr('placeholder');
    }

    if (valid.surName.val() === "") {
        valid.surName.attr('style', 'border:double; border-color:red');
        setTimeout(() => valid.surName.removeAttr('style'), 1000);
        valid.surName.attr('placeholder', 'Введите Вашу фамилию пожалуйста');
        isValid = false;
    }
    else
    {
        valid.surName.removeAttr('style');
        valid.surName.removeAttr('placeholder');
    }

    if (!phonePattern.test(valid.phone.val())) {
        valid.phone.attr('style', 'border:double; border-color:red');
        setTimeout(() => valid.phone.removeAttr('style'), 1000);
        valid.phone.attr('placeholder', 'Укажите контактный номер телефона, пожалуйста');
        isValid = false;
    }
    else
    {
        valid.phone.removeAttr('style');
        valid.phone.removeAttr('placeholder');
    }

    if (valid.street.val() === "") {
        valid.street.attr('style', 'border:double; border-color:red');
        setTimeout(() => valid.street.removeAttr('style'), 1000);
        valid.street.attr('placeholder', 'Укажите улицу, пожалуйста');
        isValid = false;
    }
    else
    {
        valid.street.removeAttr('style');
        valid.street.removeAttr('placeholder');
    }

    if (!digitPattern.test(valid.flat.val())) {
        valid.flat.attr('style', 'border:double; border-color:red');
        setTimeout(() => valid.flat.removeAttr('style'), 1000);
        valid.flat.attr('placeholder', 'Укажите номер дома, пожалуйста');
        isValid = false;
    }
    else
    {
        valid.flat.removeAttr('style');
        valid.flat.removeAttr('placeholder');
    }

    if (valid.asap.val() === 'false') {
        let hour = Number(valid.delTime.val().split(':')[0]);
        let minute = Number(valid.delTime.val().split(':')[1]);
        if (((hour * 60 + minute) < (del_time[0] * 60 + del_time[1])) || ((hour * 60 + minute) > (del_time[2] * 60 + del_time[3]))) {
            $('.val-delTime').toast('show');
            isValid = false;
        }
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

function getNewBasket()
{
    sessionStorage.setItem('Basket', '');
    $.get({
        url: "/api/product",
        method: "GET",
        async: false,
        dataType: "text",
        success: function (data) {

            let productString = JSON.parse(data);
            let productArr = '';
            $.each(productString, function (key, product) {

                let obj = {};
                let newElement = '';

                for (var prop in product) {
                    obj[prop] = product[prop];
                };

                if (obj['Avail'] === true) {
                    newElement = obj['Id'] + '#' + obj['Photo'].split('#')[0] + '#' + obj['Name'] + '#' + obj['Price'];
                    productArr = productArr + newElement + '&';
                };
            });
            sessionStorage.setItem('Products', productArr.substr(0, productArr.length - 1));
        }
    });
}

//Отправка корзины на сервер

function getNewBasketOnServer() {

    let BasketArr = [];
    for (var element of sessionStorage.getItem('Basket').toString().split('&')) 
        if (element != '') {
            var obj = {
                'ProductId' : element.split('#')[0],
                'Quantity' : element.split('#')[1]
            }
            BasketArr.push(obj);
        }
    var JSONdata = JSON.stringify(BasketArr);

    $.ajax({
        url: "/api/basket",
        method: "POST",
        contentType: 'application/json',
        data: JSONdata,
        success: function success(response) {
            if (response != 0) document.location.replace('/Customer/OrderList/' + response);
        }
    });
}

//Положить в корзину

function putIntoBasket(productId, quantity)
{
    let oldBasketArr = sessionStorage.getItem('Basket').toString().split('&');
    let newBasketArr = [];
    var newBasketString = '';
    let i = 0;
    let isInbasket = false;

    if (oldBasketArr.length == 1) newBasketString = productId + '#' + quantity + '&';
    else {
        for (var basketEl of oldBasketArr) {
            if (basketEl.split('#')[0] == productId) {
                isInbasket = true;
                newBasketArr[i] = productId + '#' + (parseInt(basketEl.split('#')[1]) + parseInt(quantity)).toString();
            }
            else newBasketArr[i] = basketEl;
            i++;
        }
        for (var string of newBasketArr) if (string != '') newBasketString = newBasketString + string + '&';
        if (!isInbasket) newBasketString = newBasketString + productId + '#' + quantity + '&';
    }
    sessionStorage.setItem('Basket', newBasketString);
}

//Удалить из корзины

function deleteFromBasket(productId)
{
    document.getElementById('item_delete_' + productId).remove();
    let oldBasketArr = sessionStorage.getItem('Basket').toString().split('&');
    let newBasketArr = [];
    var newBasketString = '';
    let i = 0;
    for (var basketEl of oldBasketArr) {
        if (basketEl.split('#')[0] != productId) {
            newBasketArr[i] = basketEl;
            i++;
        }
    }
    for (var string of newBasketArr) if (string != '') newBasketString = newBasketString + string + '&';
    sessionStorage.setItem('Basket', newBasketString);
}

//Редактировать количество

function editBasket(productId, typeId)
{
    let itemQuant = document.getElementById('item_quant_' + productId);
    let quant = parseInt(itemQuant.textContent.substr(1, itemQuant.textContent.length));
    switch (typeId) {
        case 'minus':
            if (quant > 1) quant--;
            else document.getElementById('item_delete_' + productId).remove();
            break;
        case 'plus': quant++;
            break;
    }
    itemQuant.textContent = 'x ' + quant;

    let oldBasketArr = sessionStorage.getItem('Basket').toString().split('&');
    let newBasketArr = [];
    var newBasketString = '';
    let i = 0;

    for (var basketEl of oldBasketArr) {
        if (basketEl.split('#')[0] == productId) {
            switch (typeId) {
                case 'minus':
                    if (parseInt(basketEl.split('#')[1]) > 1) {
                        newBasketArr[i] = productId + '#' + (parseInt(basketEl.split('#')[1]) - 1).toString();
                        i++;
                    }
                    break;
                case 'plus':
                    newBasketArr[i] = productId + '#' + (parseInt(basketEl.split('#')[1]) + 1).toString();
                    i++;
                    break;
            }
        }
        else {
            newBasketArr[i] = basketEl;
            i++;
        }

    }
    for (var string of newBasketArr) if (string != '') newBasketString = newBasketString + string + '&';
    sessionStorage.setItem('Basket', newBasketString);
}

//Обновить заголовок

function updateHeader()
{
    let i = 0;
    let j = 0;
    let total = 0;
    let prices = [];
    let quantities = [];
    for (let price of document.getElementsByClassName('js-item-price')) { prices[i] = parseInt(price.textContent); i++ };
    for (let quant of document.getElementsByClassName('js-item-quant')) { quantities[j] = parseInt(quant.textContent.substr(1, quant.textContent.length)); j++ };
    i = 0;
    while (i < j) {
        total = total + prices[i] * quantities[i];
        i++;
    }

    $("span#quantity").text(String(i));
    $("span#total").text(String(total) + '.00');
    if (total == 0) document.getElementById('basket_content').innerHTML = '<div class="widget widget-cart" style="text-align:center; color:#7d879c;">Ваша корзина пуста</div>';
    else $("span#js-basket-total").html(String(total) + '.<small>00</small>');

}

//Контейнер основной части корзины

function emptyBasket()
{
    return '<div class="widget widget-cart" style="text-align:center; color:#7d879c;">Ваша корзина пуста</div>';
}

function basketConteiner(productId, imageSource, name, price, quantity)
{
    this.itemClass = '<div class="widget-cart-item py-2 border-bottom" id="item_delete_' + productId + '">';
    this.removeButton = '<button class="close text-danger" id="' + productId + '" type="button" aria-label="Remove"><span aria-hidden="true">&times;</span></button>';
    this.itemContentClass = '<div class="media align-items-center">';
    this.imageClass = '<a class="d-block mr-2" href="/Home/Product/'+ productId + '"><img width="64" src="/Images/Products/' + imageSource + '" alt="Product" /></a>';
    this.bodyClass = '<div class="media-body">';
    this.nameClass = '<h6 class="widget-product-title"><a href="/Home/Product/' + productId + '">' + name + '</a></h6>';
    this.priceClass = '<div class="widget-product-meta"><span class="text-accent js-item-price mr-2">' + price + '.<small>00</small></span>';
    this.quantClass = '<span class="text-muted js-item-quant" id="item_quant_' + productId + '">x ' + quantity + '</span></div></div>';
    this.editButtonClass = '<div class="btn-group btn-group-sm mr-2 mb-2" role="group" aria-label="Second group">';
    this.plusButton = '<button type="button" id="' + productId + '" class="btn js-minus btn-outline-secondary">-</button>';
    this.minusButton = '<button type="button" id="' + productId + '" class="btn js-plus btn-outline-secondary">+</button></div>';
    this.end = '</div></div >';
    
}

//Контейнер итоговой части корзины

function totalConteiner(totalMain)
{
    this.wrapClass = '<div class="d-flex flex-wrap justify-content-between align-items-center pt-3">';
    this.totalClass = '<div class="font-size-sm mr-2 py-2"><span class="text-muted">Итог: ₽</span>';
    this.total = '<span class="text-accent font-size-base ml-1" id="js-basket-total">' + totalMain + '.<small>00</small></span></div>';
    this.chekoutLink = '<button class="btn btn-primary js-order btn-sm">';
    this.chekout = '<i class="czi-card mr-2 font-size-base align-middle"></i>Купить<i class="czi-arrow-right ml-1 mr-n1"></i></a></div>';
}

//Показ корзины

function updateBasket() {

    let summPart = '';
    let summFull = '<div class="widget widget-cart px-3 pt-2 pb-3"><div style="height: 15rem;" data-simplebar data-simplebar-auto-hide="false">';
    let basketArr = [];
    let i = 0;
    let total = 0;
    let nullBasket = true;

    if (sessionStorage.getItem('Basket') != null) {
        for (var obj of sessionStorage.getItem('Basket').toString().split('&')) {
            for (var product of sessionStorage.getItem('Products').toString().split('&')) {
                if (obj.split('#')[0] == product.split('#')[0]) {
                    nullBasket = false;
                    total = total + parseInt(product.split('#')[3]) * parseInt(obj.split('#')[1]);
                    basketArr[i] = new basketConteiner(product.split('#')[0], product.split('#')[1], product.split('#')[2], product.split('#')[3], obj.split('#')[1]);
                    for (let prop in basketArr[i]) summPart = summPart + basketArr[i][prop];
                    summFull = summFull + summPart;
                    i++;
                }
            }
            summPart = '';
        }
        summFull = summFull + '</div>';

        let totalCont = new totalConteiner(total)
        for (let tprop in totalCont) summFull = summFull + totalCont[tprop];
        if (nullBasket) document.getElementById('basket_content').innerHTML = emptyBasket();
        else document.getElementById('basket_content').innerHTML = summFull + '</div>';
    }
    else document.getElementById('basket_content').innerHTML = emptyBasket();
}

// РАБОТА С КАТАЛОГОМ

// Контейнер продукта

function productConteiner(tagType, tag, wishId, imageLink, imageSource, typeLink, type, nameLink, name, priceMain, priceFor, buyButtonId)
{
    this.product = '<div class="card product-card card-static pb-3">';
    this.tag = '<span class="badge badge-' + tagType + ' badge-shadow">' + tag + '</span>';
    this.wishButton = '<button class="btn-wishlist btn-sm" id="' + wishId + '" type="button" data-toggle="tooltip" data-placement="left" title="В избранное"><i class="czi-heart"></i></button>';
    this.imageLink = '<a class="card-img-top d-block overflow-hidden" href="/Home/Product/' + imageLink + '">';
    this.image = '<img src="/Images/Products/' + imageSource + '" alt="Product"></a>';
    this.type = '<div class="card-body py-2"><a class="product-meta d-block font-size-xs" href="/Home/Catalog?Filter=Type&id=' + typeLink + '&sid=all&tid=all">' + type + '</a>';
    this.name = '<h3 class="product-title font-size-base" style="margin-bottom:0.3rem"><a href="/Home/Product/' + nameLink + '">' + name + '</a></h3>';
    this.price = '<div class="product-price"><span class="text-accent">₽ ' + priceMain + '<small>.00</small></span>';
    this.oldPrice = '<span class="text-body"> / ' + priceFor + '</span></div></div>';
    this.buyButton = '<div class="product-floating-btn"><button class="btn js-put btn-primary btn-shadow btn-sm" Id="' + buyButtonId + '" type="button">+<i class="czi-cart font-size-base ml-1"></i></button>';
    this.endProduct = '</div></div>';
}

// Формирование содержимого по запросу

function catalogOutput(productList) {

    if (productList != null) {
        let summPart = "";
        let summFull = "";
        let tagType = "";
        let tag = "";
        let pCont = [];
        let i = 0;
        let currentDate = new Date();

        for (var obj of productList) {

            let CrDate = new Date(String(obj["CrDate"]).slice(0, 10));
            if (obj["DiscountProd"] == true) {
                tagType = "danger";
                tag = "Sale";
            } else if (obj["Popularity"] > 6) {
                tagType = "warning";
                tag = "Popular";
            } else if (((CrDate.getFullYear() * 365 + CrDate.getMonth() * 30 + CrDate.getDate()) - (currentDate.getFullYear() * 365 + currentDate.getMonth() * 30 + currentDate.getDate())) < 50) {
                tagType = "info";
                tag = "New"
            }
            pCont[i] = new productConteiner(tagType, tag, obj["Id"], obj["Id"], obj["Photo"].split("#")[0], obj["ProdTypeId"], obj["ProdTypeName"], obj["Id"], obj["Name"], obj["Price"], obj["PriceFor"], obj["Id"]);
            for (let prop in pCont[i]) summPart = summPart + pCont[i][prop];
            summFull = summFull + '<div class="col-xl-3 col-lg-6 col-md-4 col-sm-6 px-2 mb-3">' + summPart + '</div>';

            i++;
            summPart = "";
            tagType = "";
            tag = "";
        };
        document.getElementById('main_conteiner').innerHTML = summFull;
        $('[data-toggle="tooltip"]').tooltip('enable');
    };
};

// Поиск по типу продуктов

function getFromType(ProdType, SubType, ThSubType, filterType) {

    let typeFilter = [ProdType, SubType, ThSubType];
    let productList = [];
    
    $.get({
        url: "/api/product",
        method: "GET",
        dataType: "text",
        success: function (data) {
            
            let productString = JSON.parse(data);
            $.each(productString, function (key, product) {

                let obj_type = {};

                for (var prop in product) {
                    obj_type[prop] = product[prop];
                };

                if (obj_type['Avail'] === true) {

                    if ((typeFilter[1] === "all") && (typeFilter[2] === "all")) {
                        if (obj_type["ProdTypeId"] == typeFilter[0]) productList.push(obj_type);
                    }
                    else if (typeFilter[2] === "all") {
                        if ((obj_type["ProdTypeId"] == typeFilter[0]) && (obj_type["SubTypeId"] == typeFilter[1])) productList.push(obj_type);
                    }
                    else {
                        if ((obj_type["ProdTypeId"] == typeFilter[0]) && (obj_type["SubTypeId"] == typeFilter[1]) && (obj_type["ThSubTypeId"] == typeFilter[2])) productList.push(obj_type);
                    };
                };
            });

            if (productList.length === 0)
                document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но по Вашему запросу ничего не найдено...</h4>';
            else {
                switch (filterType) {
                    case 'pop': productList.sort((a, b) => b.Popularity - a.Popularity); break;
                    case 'cheap': productList.sort((a, b) => a.Price - b.Price); break;
                    case 'exp': productList.sort((a, b) => b.Price - a.Price); break;
                }
                catalogOutput(productList);
                colorWishlist();
            };
        }
    });
};

// Поиск по текстовому фильтру

function getFromText(inputText, prodType, filterType) {

    let nameFilter = {
        name: inputText,
        type: prodType
    };
    let productList = [];

    $.get({
        url: "/api/product",
        method: "GET",
        dataType: "text",
        success: function (data) {

            let productString = JSON.parse(data);
            $.each(productString, function (key, product) {

                let obj_name = {};

                for (var prop in product) {
                    obj_name[prop] = product[prop];
                };

                if (obj_name['Avail'] === true) {
                    if ((String(obj_name["Finder"]).includes(nameFilter.name)) && (nameFilter.type === "all")) productList.push(obj_name);
                    else if ((String(obj_name["Finder"]).includes(nameFilter.name)) && (nameFilter.type == obj_name["ProdTypeId"])) productList.push(obj_name);
                };
            });

            if (productList.length === 0)
                document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но по Вашему запросу ничего не найдено...</h4>';
            else {
                switch (filterType) {
                    case 'pop': productList.sort((a, b) => b.Popularity - a.Popularity); break;
                    case 'cheap': productList.sort((a, b) => a.Price - b.Price); break;
                    case 'exp': productList.sort((a, b) => b.Price - a.Price); break;
                }
                catalogOutput(productList);
                colorWishlist();
            };
        },
    });
};

//Избранное

function getWishList(filterType) {

    if (getCookie('wishlist') === null)
        document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но Ваш список избранного пуст...</h4>';
    else
    {
        let wishArray = getCookie('wishlist').split(',');
        let productList = [];
            $.get({
                url: "/api/product",
                method: "GET",
                dataType: "text",
                success: function (data) {

                    let productString = JSON.parse(data);
                    $.each(productString, function (key, product) {

                        let obj = {};
                        for (var prop in product) {
                            obj[prop] = product[prop];
                        };
                        if (obj['Avail'] === true) {
                            for (let productId of wishArray) if (obj['Id'] == productId) productList.push(obj);
                        }
                    });
 
                    if (productList.length === 0)
                        document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но Ваш список избранного пуст...</h4>';
                    else {
                        switch (filterType) {
                            case 'pop': productList.sort((a, b) => b.Popularity - a.Popularity); break;
                            case 'cheap': productList.sort((a, b) => a.Price - b.Price); break;
                            case 'exp': productList.sort((a, b) => b.Price - a.Price); break;
                        }
                        catalogOutput(productList);
                        colorWishlist();
                    };
                }
            });
    }
}

//Популярные продукты

function getBestsellers(filterType) {

    let productList = [];
    $.get({
        url: "/api/product",
        method: "GET",
        dataType: "text",
        success: function (data) {

            let productString = JSON.parse(data);
            $.each(productString, function (key, product) {

                let obj = {};
                for (var prop in product) {
                    obj[prop] = product[prop];
                };
                if ((obj['Avail'] === true) && (obj['Popularity'] > 6)) productList.push(obj);
            });

            if (productList.length === 0)
                document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но по Вашему запросу ничего не найдено...</h4>';
            else {
                switch (filterType) {
                    case 'pop': productList.sort((a, b) => b.Popularity - a.Popularity); break;
                    case 'cheap': productList.sort((a, b) => a.Price - b.Price); break;
                    case 'exp': productList.sort((a, b) => b.Price - a.Price); break;
                }
                catalogOutput(productList);
                colorWishlist();
            };
        }
    });
}

//Продукты со скидкой

function getDiscounts(filterType) {

    let productList = [];
    $.get({
        url: "/api/product",
        method: "GET",
        dataType: "text",
        success: function (data) {

            let productString = JSON.parse(data);
            $.each(productString, function (key, product) {

                let obj = {};
                for (var prop in product) {
                    obj[prop] = product[prop];
                };
                if ((obj['Avail'] === true) && (obj['DiscountProd'] === true)) productList.push(obj);
            });

            if (productList.length === 0)
                document.getElementById('main_conteiner').innerHTML = '<h4>Извините, но по Вашему запросу ничего не найдено...</h4>';
            else {
                switch (filterType) {
                    case 'pop': productList.sort((a, b) => b.Popularity - a.Popularity); break;
                    case 'cheap': productList.sort((a, b) => a.Price - b.Price); break;
                    case 'exp': productList.sort((a, b) => b.Price - a.Price); break;
                }
                catalogOutput(productList);
                colorWishlist();
            };
        }
    });
}


