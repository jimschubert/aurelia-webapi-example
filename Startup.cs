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
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddDirectoryBrowser();

            services.AddEntityFramework()
               .AddSqlite()
               .AddDbContext<BlogContext>(options =>
                   options.UseSqlite(Configuration["Data:DefaultConnection:ConnectionString"]));
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

            using (var context = app.ApplicationServices.GetService<BlogContext>())
            {
                if (!context.Blogs.Any())
                {
                    var blog = new Blog
                    {
                        BlogId = 1,
                        Name = "AureliaWebApi",
                        Posts = new List<Post>(1)
                        {
                            new Post
                            {
                                BlogId = 1,
                                Content = "<div>1,2,3!</div>",
                                PostId = 1,
                                Title = "Testing"
                            }
                        }
                    };
                    context.Blogs.Add(blog);
                    context.SaveChanges();
                }
            }
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

}
