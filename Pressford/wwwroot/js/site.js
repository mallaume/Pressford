
// Write your JavaScript code.
angular.module('pressfordApp', ['ngRoute', 'nvd3', 'textAngular'])
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
                .when('/new', {
                    templateUrl: '/Home/NewArticle',
                    controller: 'NewArticleController',
                    controllerAs: 'vm'
                })
                .otherwise('/articles');
        }
    ])
    .directive('ngEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEnter, { 'event': event });
                    });

                    event.preventDefault();
                }
            });
        };
    })
    .controller('ArticlesController', function ($http)
    {
        var vm = this;

        vm.articles = [];
        vm.chartData = [
            {
                key: "Likes",
                values: []
            }
        ];
        vm.chartOptions = {
            chart: {
                type: 'discreteBarChart',
                color: ['#f44336', '#9c27b0', '#03a9f4', '#e91e63', '#ffc107'],
                height: 300,
                margin: {
                    top: 20,
                    right: 20,
                    bottom: 60,
                    left: 55
                },
                x: function (d) { return d.label; },
                y: function (d) { return d.value; },
                showValues: true,
                valueFormat: function (d) { return d3.format(',.0f')(d); },
                transitionDuration: 500,
                xAxis: {
                    rotateLabels: -15
                }
            }
        };

        vm.getArticles = getArticles;
        vm.getChartData = getChartData;

        getArticles();
        getChartData();

        function getArticles() {
            $http.get('/api/articles').then(function (e) {
                vm.articles = e.data;
            });
        };
        function getChartData() {
            $http.get('/api/articles/stats').then(function (e) {
                vm.chartData[0].values = e.data;
            });
        }
    })
    .controller('ArticleController', function ($routeParams, $location, $http)
    {
        var vm = this;

        vm.article = null;
        vm.comment = null;
        vm.edit = false;

        vm.getArticle = getArticle;
        vm.saveArticle = saveArticle;
        vm.deleteArticle = deleteArticle;
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
        function deleteArticle() {
            var result = confirm("Want to delete?");

            if (result) {
                $http.delete('/api/articles/' + $routeParams.id).then(function (e) {
                    $location.path('/articles');
                });
            }
        }
        function like() {
            $http.post('/api/articles/' + $routeParams.id + '/like').then(function (e) {
                getArticle();
            }).catch(function (error) {
                if (error.status == 401) {
                    alert('You have reached your quota of articles to like...');
                }
                else if (error.status == 400) {
                    alert('You have already liked this article...');
                }
            });
        }
        function writeComment() {

            if (vm.comment == '')
            {
                return;
            }

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
    })
    .controller('NewArticleController', function ($location, $http) {
        var vm = this;

        vm.title = null;
        vm.body = null;

        vm.saveArticle = saveArticle;

        function saveArticle() {

            var data = {
                title: vm.title,
                body: vm.body
            };

            $http.post('/api/articles/', data).then(function (e) {
                $location.path('/articles');
            });
        }
    });