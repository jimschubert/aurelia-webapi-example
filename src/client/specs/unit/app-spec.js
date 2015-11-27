import {App} from 'app';
describe('App', () => {

	var test;

	beforeEach(() => {
		test = new App();
	});

	it('has the expected message', () => {
		expect(test.message).toEqual('Welcome to Aurelia!');
	});
});