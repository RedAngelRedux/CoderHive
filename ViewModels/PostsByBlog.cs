using CoderHive.Models;

namespace CoderHive.ViewModels
{
    public class PostsByBlog
    {
        public string? BlogTitle { get; set; }
        public IEnumerable<Post>? Posts { get; set; }

    }
}
