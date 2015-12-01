using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AureliaWebApi.Controllers;
using AureliaWebApi.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Microsoft.Extensions.OptionsModel;
using Xunit;

namespace AureliaWebApiTests.Controllers
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class BlogControllerTests
    {   
        /**
        * NOTE:
        * I would consider these tests integration tests, but they're used here as unit tests.
        * There's a fine line between being pedantic and having your codebase accounted for.
        * In the end, there's usually a business decision to get some "wins" whenever possible.
        * This is one of those areas where I'd consider the tradeoff a better alternative (better 
        * than mocking the MVC pipeline).
        **/
        
        #region Get
        
        [Fact]
        public async Task<int> GetShouldReturnResultsWhenDataExists()
        {
            string expected = "Jim's Blog";
            using (var fixture = new BlogContextFixture())
            {
                // Arrange
                fixture.Context.Blogs.Add(new Blog { Name = expected });
                await fixture.Context.SaveChangesAsync();
                
                var controller = new BlogController(fixture.Context) {
                    ActionContext = new ActionContext {
                        HttpContext = new DefaultHttpContext
                        {
                            RequestServices = CreateServices()
                        }
                    }
                };

                // Act
                var result = await controller.Get();
                await result.ExecuteResultAsync(controller.ActionContext);
                var response = result as ObjectResult;
                if(response != null){
                    await response.ExecuteResultAsync(controller.ActionContext);
                }

                // Assert
                Assert.NotNull(response);
                Assert.Equal(StatusCodes.Status200OK, controller.Response.StatusCode);
                Assert.Equal(expected, ((List<Blog>)response.Value).First().Name);
            }
            
            return 0;
        }
        
        [Fact]
        public async Task<int> GetShouldReturn204WhenNoDataExists()
        {
            using (var fixture = new BlogContextFixture())
            {
                // Arrange
                var controller = new BlogController(fixture.Context) {
                    ActionContext = new ActionContext {
                        HttpContext = new DefaultHttpContext
                        {
                            RequestServices = CreateServices()
                        }
                    }
                };

                // Act
                var result = await controller.Get();
                await result.ExecuteResultAsync(controller.ActionContext);
                var response = result as NoContentResult;
                if(response != null){
                    await response.ExecuteResultAsync(controller.ActionContext);
                }

                // Assert
                Assert.IsType<NoContentResult>(result);
                Assert.Equal(StatusCodes.Status204NoContent, controller.Response.StatusCode);
            }
            
            return 0;
        }
        
        #endregion Get
        
        #region Get(int:id)
        
        [Fact]
        public async Task<object> GetByIdShouldReturnNoContentWhenNoBlogs()
        {
            using (var fixture = new BlogContextFixture())
            {
                // Arrange
                var controller = new BlogController(fixture.Context) {
                    ActionContext = new ActionContext {
                        HttpContext = new DefaultHttpContext
                        {
                            RequestServices = CreateServices()
                        }
                    }
                };

                // Act
                var result = await controller.Get(1);
                await result.ExecuteResultAsync(controller.ActionContext);
                var response = result as HttpNotFoundResult;
                if(response != null){
                    await response.ExecuteResultAsync(controller.ActionContext);
                }

                // Assert
                Assert.IsType<HttpNotFoundResult>(result);
                Assert.Equal(StatusCodes.Status404NotFound, controller.Response.StatusCode);
            }
            
            return Task.FromResult<object>(null);
        }
        
        #endregion Get(int:id)
        
        #region Posts(int id)
        
        #endregion Posts(int id)
        
        #region  Put(int id, Blog blog)
        
        #endregion  Put(int id, Blog blog)
        
        #region Delete(int id)
        
        #endregion Delete(int id)
        private IServiceProvider CreateServices()
        {
            var services = new ServiceCollection();
            
            services
                .AddEntityFramework()
                .AddInMemoryDatabase();
            
            services.Add(new ServiceDescriptor(
                typeof(ILoggerFactory),
                NullLoggerFactory.Instance));
                
            services.AddMvc();
                
            // helper taken from 
            // https://github.com/aspnet/Mvc/blob/514054a66db398171d25494d7c0494f134e8f80a/test/Microsoft.AspNet.Mvc.Core.Test/HttpOkObjectResultTest.cs
            // which contains the following setup for ObjectResult testing...
       
            services.Add(new ServiceDescriptor(
                typeof(ILogger<ObjectResult>),
                new Logger<ObjectResult>(NullLoggerFactory.Instance)));

            var optionsAccessor = new OptionsManager<MvcOptions>(Enumerable.Empty<IConfigureOptions<MvcOptions>>());
            optionsAccessor.Value.OutputFormatters.Add(new JsonOutputFormatter());
            services.Add(new ServiceDescriptor(typeof(IOptions<MvcOptions>), optionsAccessor));

            var bindingContext = new ActionBindingContext
            {
                OutputFormatters = optionsAccessor.Value.OutputFormatters,
            };

            var bindingContextAccessor = new ActionBindingContextAccessor
            {
                ActionBindingContext = bindingContext,
            };
            
            services.Add(new ServiceDescriptor(typeof(IActionBindingContextAccessor), bindingContextAccessor));
            
            var executor = new ObjectResultExecutor(optionsAccessor,
                bindingContextAccessor,
                new TestHttpResponseStreamWriterFactory(),
                NullLoggerFactory.Instance);
                
            services.Add(new ServiceDescriptor(typeof(ObjectResultExecutor), executor));
            
            return services.BuildServiceProvider();
        }
    }
}
