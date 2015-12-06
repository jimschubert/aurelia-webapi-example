import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import 'fetch';
import {BlogApi} from './blog';
import {PostApi} from './post';

@inject(HttpClient)
export class Api {
	constructor(http){
		http.configure(config => {
			config.useStandardConfiguration();
		});

		this.http = http;
		
		this.blogs = new BlogApi(http);
		this.posts = new PostApi(http);
	}
}
