using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AureliaWebApi.Models
{
    public class Blog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
    }
}