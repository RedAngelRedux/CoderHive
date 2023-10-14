using CoderHive.Models;
using X.PagedList;

namespace CoderHive.ViewModels
{
    /// <summary>
    /// This ViewModel will be used to control the listing of Posts
    /// If BlogTitle & BlogId are null, then SearchTerm should not be
    /// If SearchTerm is null then BlogTitle & BlogId should not bee.
    /// In practice, SearchTerm will used by the SearchAllPostsIndex while
    /// BlogTitle & BlogId will be used by the Post Index action
    /// </summary>
    public class PostsByBlog
    {
        public string? BlogTitle { get; set; } = null;
        //public IEnumerable<Post>? Posts { get; set; }
        public int? BlogId { get; set; } = null;
        public byte[]? BlogImageData { get; set; }
        public string? BlogImageType {  get; set; }
        public string? SearchTerm { get; set; } = null;
        public string? SearchAction { get; set; } = null;
        public IPagedList<Post>? Posts { get; set; }
    }
}
