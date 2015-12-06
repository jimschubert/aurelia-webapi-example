using System.Collections.Generic;
using System.Linq;
using AureliaWebApi.Models;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.StaticFiles;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;

namespace AureliaWebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            
            Configuration["Data:DefaultConnection:ConnectionString"] = $"Data Source={ appEnv.ApplicationBasePath }/blog.db";
            
            Environment = env;
        }

        public IConfigurationRoot Configuration { get; set; }
        
        private IHostingEnvironment Environment{ get; set; } 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // We want to run an in-memory database in Development, real database in Production and other environments..
            if(Environment.IsDevelopment())
            {
                // See here for more: http://docs.asp.net/en/latest/fundamentals/environments.html
                services.AddMvc();
                services.AddEntityFramework()
                    .AddInMemoryDatabase();
                    
                var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();
                optionsBuilder.UseInMemoryDatabase();
                var db = new BlogContext(services.BuildServiceProvider(), optionsBuilder.Options);
                
                services.Add(new ServiceDescriptor(typeof(BlogContext), db));
                
            } else {
                // Add framework services.
                services.AddMvc();
                services.AddDirectoryBrowser();
    
                services.AddEntityFramework()
                .AddSqlite()
                .AddDbContext<BlogContext>(options =>
                    options.UseSqlite(Configuration["Data:DefaultConnection:ConnectionString"]));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();
            
            var staticFileServer = new FileServerOptions
            {
                RequestPath = new PathString(string.Empty),
                EnableDirectoryBrowsing = true,
                StaticFileOptions =
                {
                    ServeUnknownFileTypes = true,
                    DefaultContentType = "text/html"
                },
                EnableDefaultFiles = true
            };
            app.UseFileServer(staticFileServer);

            app.UseMvc();
            
            var context = app.ApplicationServices.GetService<BlogContext>();
            // context.Database.Migrate isn't directly supported by the 
            // in memory database configured for the development environment.
            if(!env.IsDevelopment()) context.Database.Migrate();
            
            if (!context.Blogs.Any())
            {
                var blog = new Blog
                {
                    BlogId = 1,
                    Name = "AureliaWebApi",
                    Posts = new List<Post>{
                        new Post
                        {
                            BlogId = 1,
                            Content = "<div>Testing… 1,2,3!</div>",
                            Title = "First Post"
                        },
                        new Post
                        {
                            BlogId = 1,
                            Content = "<div>小洞不补，大洞吃苦</div>",
                            Title = "Unicode test"
                        },
                        new Post
                        {
                            BlogId = 1,
                            Content = @"
                            #### daringfireball.net
                            I get 10 times more traffic from [Google] [1] than from
                            [Yahoo] [2] or [MSN] [3].
                            
                            [1]: http://google.com/        ""Google""
                            [2]: http://search.yahoo.com/  ""Yahoo Search""
                            [3]: http://search.msn.com/    ""MSN Search""
                            ",
                            Title = "Does markdown work?"
                        },
                        new Post
                        {
                            BlogId = 1,
                            Content = "<img src=\"http://placehold.it/350x150\"/>",
                            Title = "Images Test"
                        }
                    }
                };
                context.Blogs.Add(blog);
                
                context.SaveChanges(true);
            }
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

}
