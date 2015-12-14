import {transient, inject} from 'aurelia-framework';
import {Api} from 'services/api';
import $ from 'jquery';

@transient()
@inject(Api)
export class BlogDetail {
    constructor(api) {
        this.api = api;
    }

    activate(params, routeConfig) {
        return Promise.all([
            this.api.blogs.one(params.id)
                .then(blog => {
                    this.blog = blog;
                }),
            this.api.blogs.posts(params.id)
                .then(posts => {
                    this.posts = posts;
                })
        ]);
    }

    attached() {
        $('#newPost').on('hidden.bs.modal', function () {
            $('#newPostTitle').val('');
            $('#newPostContent').val('')
        });
    }

    createPost(evt) {
        if ('save' === evt.target.dataset['type']) {
            let title = $('#newPostTitle').val();
            let content = $('#newPostContent').val();
            this.api.posts.create({
                title: title,
                content: content,
                blogId: this.blog.BlogId
            }).then((post) => {
                this.posts.push(post);
                $('#newPost').modal('hide');
            }, () => {
                alert('Unable to save!');
            })
        }
    }
}