import {transient, inject} from 'aurelia-framework';
import {Api} from 'services/api';

@transient()
@inject(Api)
export class PostDetail {
	constructor(api){
		this.api = api;
	}

	activate(params, routeConfig) {
		this.api.posts.one(params.id)
			.then(post => {
			this.post = post;
		});
	}
}