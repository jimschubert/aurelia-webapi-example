using System;
using System.Linq;
using System.Threading.Tasks;
using AureliaWebApi.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AureliaWebApi.Controllers
{
    [Route("api/[controller]")]
    public class BlogController : Controller
    {
        private readonly BlogContext _context;

        public BlogController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/blog
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var blogs = await _context.Blogs.Select(blog => new
            {
                blog.BlogId,
                blog.Name
            }).ToListAsync();

            if (!blogs.Any())
            {
                return HttpNotFound();
            }

            return new ObjectResult(blogs);
        }

        // GET api/blog/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.Blogs.Where(p => p.BlogId == id).Select(blog => new
            {
                blog.BlogId,
                blog.Name
            }).FirstOrDefaultAsync();

            if (item == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(item);
        }

        // GET api/blog/5/posts
        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Posts(int id)
        {
            var item = await _context.Blogs
                .Where(blog => blog.BlogId == id)
                .Include(blog => blog.Posts)
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return HttpNotFound();
            }

            return new ObjectResult(item.Posts);
        }

        // POST api/blog
        [HttpPost]
        public void Post([FromBody] Blog value)
        {
            throw new NotImplementedException();
        }

        // PUT api/blog/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] Blog blog)
        {
            blog.BlogId = id; // no funny business.
            _context.Blogs.Update(blog, GraphBehavior.SingleObject);
            await _context.SaveChangesAsync();
        }

        // DELETE api/blog/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _context?.Dispose();
        }
    }
}