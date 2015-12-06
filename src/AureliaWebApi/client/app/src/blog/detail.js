import {transient, inject} from 'aurelia-framework';
import {Api} from 'services/api';

@transient()
@inject(Api)
export class BlogDetail {
	constructor(api){
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
}