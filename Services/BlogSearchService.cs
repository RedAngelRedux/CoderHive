using CoderHive.Data;
using CoderHive.Enums;
using CoderHive.Models;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;

namespace CoderHive.Services
{
    public class BlogSearchService
    {
        private readonly ApplicationDbContext _context;

        public BlogSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Search(string searchTerm)
        {
            // This gets a list of all Production Ready Posts from the Database
            var posts = _context.Posts.Where(p => p.Status == PostStatus.ProductionReady).AsQueryable();

            // This narrows it down to the Search Term, if any
            if (searchTerm is not null)
            {
                searchTerm = searchTerm.ToLower();

                posts = posts.Where
                (
                    p => p.PostTitle.ToLower().Contains(searchTerm) ||
                    p.Abstract.ToLower().Contains(searchTerm) ||
                    p.Content.ToLower().Contains(searchTerm) ||
                    p.Comments.Any(c => c.Body.ToLower().Contains(searchTerm) ||
                    c.ModeratedBody.ToLower().Contains(searchTerm) ||
                    c.Author.FirstName.ToLower().Contains(searchTerm) ||
                                        c.Author.LastName.ToLower().Contains(searchTerm) ||
                                        c.Author.Email.ToLower().Contains(searchTerm)));
            }

            // This orders the remaining records (either all or searchTerm filtered)
            return posts.OrderByDescending(p => p.Created); 
        }
    }
}
