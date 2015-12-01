using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AureliaWebApi;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Hosting.Server;
using Microsoft.AspNet.TestHost;
using Xunit;
using System.Linq;
using AureliaWebApi.Models;
using System.Collections.Generic;

namespace AureliaWebApiTests.Functional
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class FunctionalTests_WithTestHost
    {
        /**
         * NOTE: There are lots of examples in the test source for Microsoft.AspNet.TestHost:
         * https://github.com/aspnet/Hosting/blob/dev/test/Microsoft.AspNet.TestHost.Tests/TestServerTests.cs
         */
        
        [Fact]
        public async Task GetBlogById_NotFound()
        {	
            TestServer server = new TestServer(TestServer.CreateBuilder()
                    .UseStartup<Startup>()
                    .UseEnvironment("Development"));
            
            using(server){
                HttpResponseMessage result = await server.CreateClient().GetAsync("/api/blog/9999");
                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
                Assert.Equal(string.Empty, await result.Content.ReadAsStringAsync());
            }
        }
        
        [Fact]
        public async Task GetBlogById_Found()
        {	
            WebHostBuilder definition = TestServer.CreateBuilder()
                    .UseEnvironment("Development")
                    .UseStartup<Startup>();
                
            var server = new TestServer(definition);
            
            definition.UseServer(server);
            var engine = definition.Build();
            
            using(engine.Start()){
                var rand = new System.Random();
                int blogId = rand.Next();
                var context = (BlogContext)engine.ApplicationServices.GetService(typeof(BlogContext));
                var blog = new Blog
                        {
                            BlogId = blogId,
                            Name = "GetBlogById_Found",
                            Posts = new List<Post>(0)
                        };
                context.Blogs.Add(blog);
                await context.SaveChangesAsync();
                    
                HttpResponseMessage result = await server.CreateClient().GetAsync("/api/blog/" + blogId);
                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("{\"BlogId\":"+blogId+",\"Name\":\"GetBlogById_Found\"}", await result.Content.ReadAsStringAsync());
            }
        }
    }
}
