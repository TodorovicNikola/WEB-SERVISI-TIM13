(function (angular) {
    angular.module('app.Comment.resource', ['ngResource'])
	.factory('Comment', function ($resource) {
	    var comment = $resource('../../api/comments/:commentId',
			{
			    commentId: '@commentId',
			},
			{ update: { method: 'PUT' } });
	    return comment;
	})
}(angular));