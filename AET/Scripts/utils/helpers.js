var _baseURL = "";

// format dinheiro
Number.prototype.formataDinheiro = function (casasDecimais, emQuantosAgrupar) {

    var foiDec = false;
    var source = replaceAll('.', '', this.toFixed(casasDecimais).toString());
    var retorno = "";
    var index = 1;

    for (var i = source.length; i--; i > -1) {
        retorno += source[i];

        if (foiDec) {
            if (emQuantosAgrupar == index && i != 0) {
                //se proximo for sinal de negativo não coloca ponto
                if (source[i - 1] != "-") retorno += ".";

                index = 0;
            }
        }
        else {
            if (casasDecimais == index) {
                retorno += ",";
                foiDec = true;
                index = 0;
            }
        }

        index++;
    }

    return reverteString(retorno);

    //    var re = '(\\d)(?=(\\d{' + (casasDecimais || 3) + '})+' + (emQuantosAgrupar > 0 ? '\\.' : '$') + ')';
    //    var aux = this.toFixed(Math.max(0, ~ ~emQuantosAgrupar)).replace(new RegExp(re, 'g'), '$1,');
    //    return aux.replace(",", "g").replace(".", ",").replace("g", ".");
};

Number.prototype.parseNumber = function () {
    var valor = parseFloat(this);
    if (isNaN(valor)) valor = 0;
    if (isFinite(valor) == false) valor = 0;
    return valor;
};

String.prototype.parseNumber = function () {
    var valor = parseFloat(this.replace(".", "").replace(",", "."));
    if (isNaN(valor)) valor = 0;
    if (isFinite(valor) == false) valor = 0;
    return valor;
};

// query string manipulator
(function ($) {
    $.QueryString = (function (a) {
        if (a == "") return {};
        var b = {};
        for (var i = 0; i < a.length; ++i) {
            var p = a[i].split('=');
            if (p.length != 2) continue;
            b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
        }
        return b;
    })(window.location.search.substr(1).split('&'))
})(jQuery);

String.prototype.getRotaID = function () {
    var aux = this.split("/");
    return aux[aux.length - 1].split("?")[0].toString();
};

String.prototype.getPath = function () {
    var aux = this.replace("/sistema", "").split("/");
    return aux[1].toString();
};

String.prototype.getQueryString = function () {
    var aux = this.split("?");
    if (aux.length > 1)
        return aux[1].toString();
    else
        return "";
};

String.prototype.limitText = function (maxChar) {
    var retorno = '';
    if (this.length > maxChar) {
        retorno = this.substr(0, maxChar - 3) + "...";
    }
    else
        retorno = this.replace('"', '');

    return retorno;
};

String.prototype.simpleLimitText = function (maxChar) {
    var retorno = '';
    if (this.length > maxChar) {
        retorno = this.substr(0, maxChar);
    }
    else
        retorno = this.replace('"', '');

    return retorno;
};

// Array Remove - By John Resig (MIT Licensed)
Array.prototype.remove = function (from, to) {
    var rest = this.slice((to || from) + 1 || this.length);
    this.length = from < 0 ? this.length + from : from;
    return this.push.apply(this, rest);
};

$.fn.SomenteNumeros =
function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;

            var logica1 = (key == 8 || key == 9 || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105) || (key >= 112 && key <= 123));
            var logica2 = false;

            var except = $(this).attr('numbersexcept');
            if (!stringIsNullOrEmpty(except)) {
                var split = except.split('|');
                for (var i = 0, tamanho = split.length; i < tamanho; i++) {
                    if (split[i].toLowerCase() == String.fromCharCode(e.charCode || e.keyCode || 0).toLowerCase())
                        logica2 = true;
                }
            }

            return logica1 || logica2;
        });
        $(this).change(function (e) {
            if ($(this).val() == '') {
                $(this).val('0');
                $(this).trigger('change');
            }
        });
    });
};

$.fn.Decimal =
function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;

            console.log(key);
            if ($(this).hasClass("negative")) {
                if (key == 189 || key == 109) return true;
            }

            return (
                key == 8 ||
                key == 9 ||
                key == 46 ||
                key == 110 ||
                key == 188 ||
                key == 190 ||
                key == 194 ||
                (key >= 35 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 96 && key <= 105) ||
                (key >= 112 && key <= 123));

        });
        $(this).change(function (e) {
            if ($(this).val() == '') {
                $(this).val('0');
                $(this).trigger('change');
            }
        });
    });
};

function getStringDateFromDate(aux) {
    var day = aux.getDate();
    var month = aux.getMonth();
    var year = aux.getFullYear();

    if (day < 10)
        day = '0' + day;

    if (month < 10)
        month = '0' + month;

    return day + "/" + month + "/" + year;
}

function reverteString(source) {

    var retorno = "";
    for (var i = source.length; i--; i > -1)
        retorno += source[i];

    return retorno;

}

function s4() {
    return Math.floor((1 + Math.random()) * 0x10000)
             .toString(16)
             .substring(1);
};

function guid() {
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
         s4() + '-' + s4() + s4() + s4();
}

function stringIsNullOrEmpty(input) {
    if (input == null || typeof input == "undefined" || input == "")
        return true;
    else
        return false;
}

function escapeRegExp(str) {
    return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
}

function replaceAll(find, replace, str) {
    return str.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}

if (typeof String.prototype.startsWith != 'function') {
    // see below for better implementation!
    String.prototype.startsWith = function (str) {
        return this.indexOf(str) == 0;
    };
}

function baixarArquivo(id) {
    var url = _baseURL + '/ajaxarquivos/getfile?id=' + id;
    downloadFile(guid(), url);
}

// Init 
$(function () {

    $(".disable").prop('disabled', true);

    $(".numbers").SomenteNumeros();

    $(".money").Decimal();

    $(".data").datepicker({
        format: 'dd/mm/yyyy'
    });

    $(".data").mask("99/99/9999");
});