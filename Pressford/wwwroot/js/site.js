
// Write your JavaScript code.
angular.module('pressfordApp', ['ngRoute'])
    .config(['$locationProvider', '$routeProvider',
        function config($locationProvider, $routeProvider)
        {
            $locationProvider.hashPrefix('!');
            $locationProvider.hashPrefix('');

            $routeProvider
                .when('/articles', {
                    templateUrl: '/Home/Articles',
                    controller: 'ArticlesController',
                    controllerAs: 'vm'
                })
                .when('/article/:id', {
                    templateUrl: '/Home/Article',
                    controller: 'ArticleController',
                    controllerAs: 'vm'
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
        vm.comment = null;
        vm.edit = false;

        vm.getArticle = getArticle;
        vm.saveArticle = saveArticle;
        vm.like = like;
        vm.writeComment = writeComment;
        vm.toggleEdit = toggleEdit;

        getArticle();

        function getArticle() {
            $http.get('/api/articles/' + $routeParams.id).then(function (e) {
                vm.article = e.data;
            });
        }
        function saveArticle() {
            var data = {
                body: vm.article.body
            };

            $http.put('/api/articles/' + $routeParams.id, data).then(function (e) {
                vm.edit = false;
                getArticle();
            });
        }
        function like() {
            $http.post('/api/articles/' + $routeParams.id + '/like').then(function (e) {
                getArticle();
            });
        }
        function writeComment() {
            var data = {
                text: vm.comment
            };

            $http.post('/api/articles/' + $routeParams.id + '/comment/', data).then(function (e) {
                vm.comment = null;
                getArticle();
            });
        }
        function toggleEdit() {
            vm.edit = !vm.edit;
        }
    });