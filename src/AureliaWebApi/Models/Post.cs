using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AureliaWebApi.Models
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }


        [JsonIgnore]
        public Blog Blog { get; set; }
    }
}