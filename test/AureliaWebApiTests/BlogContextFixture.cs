using System;
using AureliaWebApi.Models;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;

///<summary>
/// BlogContextFixture creates an in-memory database for integration/functional testing.
///</summary>
public class BlogContextFixture : IDisposable
{
	public BlogContext Context { get; private set; }
	
	public BlogContextFixture()
	{
        // In-memory database must be created in the context of a 
        // separate service provider to avoid persisting data.
        // see https://github.com/aspnet/EntityFramework/pull/3220
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddEntityFramework()
            .AddInMemoryDatabase();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();

		var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();
        optionsBuilder.UseInMemoryDatabase();
		var db = new BlogContext(serviceProvider, optionsBuilder.Options);
		Context = db;
	}

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
				using(Context){}
            }

            // NOTE: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // NOTE: set large fields to null.

            disposedValue = true;
        }
    }

    // NOTE: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~BlogContextFixture() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        // NOTE: uncomment the following line if the finalizer is overridden above.
        // GC.SuppressFinalize(this);
    }
    #endregion

}