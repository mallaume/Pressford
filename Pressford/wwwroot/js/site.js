import { fail } from "assert";

// Write your JavaScript code.
angular.module('pressfordApp', ['ngRoute'])
    .config(['$locationProvider', '$routeProvider',
        function config($locationProvider, $routeProvider)
        {
            $locationProvider.hashPrefix('!');
            $locationProvider.hashPrefix('');

            $routeProvider
                .when('/articles', {
                    templateUrl: '/html/index.html',
                    controller: 'ArticlesController',
                    controllerAs: 'vm'
                })
                .when('/article/:id', {
                    templateUrl: '/Home/Article',
                    controller: 'ArticleController',
                    controllerAs: 'vm'
                })
                .when('/test', {
                    template: '<h2>Hello</h2>'
                })
                .otherwise('/articles');
        }
    ])
    .controller('ArticlesController', function ($http)
    {
        var vm = this;

        vm.articles = [];

        vm.getArticles = getArticles;

        getArticles();

        function getArticles() {
            $http.get('/api/articles').then(function (e) {
                vm.articles = e.data;
            });
        };
    })
    .controller('ArticleController', function ($routeParams, $http)
    {
        var vm = this;

        vm.article = null;

        vm.getArticle = getArticle;
        vm.isAlreadyLiked = isAlreadyLiked;
        vm.like = like;

        getArticle();

        function getArticle() {
            $http.get('/api/articles/' + $routeParams.id).then(function (e) {
                vm.article = e.data;
            });
        };
        function isAlreadyLiked() {
            if (vm.article && vm.article.likes && vm.article.likes.length > 0) {
                for (var i = 0; i < vm.article.likes.length; i++) {
                    if (vm.article.likes[i].liker)
                }
            }
            else {
                return false;
            }
        }
        function like() {
            $http.post('/api/articles/' + $routeParams.id + '/like').then(function (e) {
                getArticle();
            });
        }
    });