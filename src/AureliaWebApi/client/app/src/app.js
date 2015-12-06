export class App {
    header = 'Really Simple Blog';
    subheader = 'ASP.NET 5 Web API, Aurelia, Gulp';
    description = `
        This is a simple Aurelia application powered by an
        ASP.NET 5 Web API backend.

        The intention of this application is to demonstrate a
        common project setup with a gulp workflow for both
        client and server.
    `;

    configureRouter(config, router) {
        config.title = 'Aurelia';
        config.map([
            { route: ['','home'], name: 'home', moduleId: './blog/list', nav: true, title: 'Blog' },
            { route: 'blog/:id', name: 'blog.detail',  moduleId: './blog/detail', title: 'Blog' },
            { route: 'blog', name: 'blog', moduleId: 'blog', nav: true, title: 'Blog' }
        ]);

        this.router = router;
    }
}