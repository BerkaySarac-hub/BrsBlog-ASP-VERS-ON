using BrsBlogWeb.Data;
using System.Linq;

namespace BrsBlogWeb.Models
{
    public class BlogPost
    {
        
        public int Id { get; set; }
        public string post_img { get; set; }
        public string post_header { get; set; }
        public string post_description { get; set; }
        public string post_keyword { get; set; }
        public string post_url { get; set; }

        
    }
}
