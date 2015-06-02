var app = angular.module('aet', []);

var _id = window.location.pathname.toString().getRotaID();
var _query = window.location.toString().getQueryString();

var _baseURL = "";