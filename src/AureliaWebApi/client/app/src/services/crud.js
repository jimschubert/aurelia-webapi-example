export class CrudApi {
	constructor(name, http){
		this.name = name;
		this.http = http;
	}
	
	query(){
		return this.http.fetch(`api/${this.name}`)
		.then(response => response.json());
	}
	
	one(id){
		return this.http.fetch(`api/${this.name}/${id}`)
		.then(response => response.json());
	}
	
	create(item){
		// TODO
	}
	
	update(item){
		// TODO
	}
	
	delete(item){
		// TODO
	}
	
	queryNested(id, resource){
		return this.http.fetch(`api/${this.name}/${id}/${resource}`)
		.then(response => response.json());
	}
}