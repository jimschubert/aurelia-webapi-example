import {CrudApi} from './crud';
export class PostApi extends CrudApi {
	constructor(http){
		super('post', http);
	}
}