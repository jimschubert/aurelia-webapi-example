import {CrudApi} from './crud';
export class BlogApi extends CrudApi {
	constructor(http){
		super('blog', http);
	}
	
	posts(blogId) {
		return super.queryNested(blogId, 'posts');
	}
}
