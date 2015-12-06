import {transient, inject} from 'aurelia-framework';
import {Api} from 'services/api';

@transient()
@inject(Api)
export class BlogList {
	constructor(api){
		this.api = api;
	}
	
	attached() {
		this.api.blogs.query()
		.then(blogs => {
			this.blogs = blogs;
		});
	}
}