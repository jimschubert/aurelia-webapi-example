using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Query.Internal;
using Moq;

namespace AureliaWebApiTests
{
    public static class TestExtensions
    {
		public static Mock<DbSet<T>> CreateMockDbSet<T>(this IQueryable<T> data) where T : class
		{
            var mockSet = new Mock<DbSet<T>>(); 
			
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider); 
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression); 
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType); 
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            
			return mockSet;
		}
    }
    
    // Taken from https://msdn.microsoft.com/en-us/data/dn314429#async, updated for EF7
    // WARNING: not fully tested, included here for future use and tweaking. 
    //          These may not immediately fully work with EF7.
    internal class TestDbAsyncQueryProvider<TEntity> : IAsyncQueryProvider 
    { 
        private readonly IQueryProvider _inner; 
 
        internal TestDbAsyncQueryProvider(IQueryProvider inner) 
        { 
            _inner = inner; 
        } 
 
        public TResult Execute<TResult>(Expression expression) 
        { 
            return _inner.Execute<TResult>(expression); 
        } 
        
        public object Execute(Expression expression) 
        { 
            return _inner.Execute(expression); 
        } 
 
        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression); 
        }

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression); 
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression); 
        }

        IAsyncEnumerable<TResult> IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TResult>(expression); 
        }

        Task<TResult> IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression)); 
        }
    } 
 
    // Taken from https://msdn.microsoft.com/en-us/data/dn314429#async, updated for EF7
    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T> 
    { 
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable) 
            : base(enumerable) 
        { } 
 
        public TestDbAsyncEnumerable(Expression expression) 
            : base(expression) 
        { } 

        IQueryProvider IQueryable.Provider 
        { 
            get { return new TestDbAsyncQueryProvider<T>(this); } 
        }

        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator()); 
        }
    } 
 
    // Taken from https://msdn.microsoft.com/en-us/data/dn314429#async, updated for EF7
    internal class TestDbAsyncEnumerator<T> : IAsyncEnumerator<T>, IEnumerator<T>
    { 
        private readonly IEnumerator<T> _inner; 
 
        public TestDbAsyncEnumerator(IEnumerator<T> inner) 
        { 
            _inner = inner; 
        } 
 
        public void Dispose() 
        { 
            _inner.Dispose(); 
        } 
 
        public Task<bool> MoveNextAsync(CancellationToken cancellationToken) 
        { 
            return Task.FromResult(_inner.MoveNext()); 
        }

        Task<bool> IAsyncEnumerator<T>.MoveNext(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            return _inner.MoveNext();
        }

        public void Reset()
        {
            _inner.Reset();
        }

        public T Current 
        { 
            get { return _inner.Current; } 
        }

        T IAsyncEnumerator<T>.Current
        {
            get
            {
                return Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return _inner.Current;
            }
        }
    } 
}
