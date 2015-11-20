using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AureliaWebApi.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AureliaWebApi.Controllers
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly BlogContext _context;

        public PostController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/post
        [HttpGet]
        public async Task<IEnumerable<Post>> Get()
        {
            return await _context.Posts.ToListAsync();
        }

        // GET api/post/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);

            if (item == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Post post)
        {
            if (post.PostId > 0)
            {
                return HttpBadRequest();
            }

            post.PostId = default(int);
            _context.Posts.Add(post);

            await _context.SaveChangesAsync();

            return new ObjectResult(post);
        }

        // PUT api/post/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Post post)
        {
            throw new NotImplementedException();
        }

        // DELETE api/post/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == id);
            if (post == null) return;
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _context?.Dispose();
        }
    }
}