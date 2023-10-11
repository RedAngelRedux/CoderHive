using CoderHive.Models;
using X.PagedList;

namespace CoderHive.ViewModels
{
    public class PostsByBlog
    {
        public string? BlogTitle { get; set; }
        //public IEnumerable<Post>? Posts { get; set; }
        public int? BlogId { get; set; }
        public IPagedList<Post>? Posts { get; set; }

    }
}
