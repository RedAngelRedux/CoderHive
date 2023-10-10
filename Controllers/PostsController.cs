
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoderHive.Data;
using CoderHive.Models;
using CoderHive.Services;
using Microsoft.AspNetCore.Identity;
using SQLitePCL;
using System.Drawing;
using System.Reflection.Metadata;

namespace CoderHive.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISlugService _slugService;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;

        public PostsController(ApplicationDbContext context, ISlugService slugService, IImageService imageService, UserManager<BlogUser> userManager)
        {
            _context = context;
            _slugService = slugService;
            _imageService = imageService;
            _userManager = userManager;
        }


        // GET: Posts/Index/1
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.Posts.Include(p => p.Author).Include(p => p.Blog); 
            var applicationDbContext = 
                _context.Posts.Where(p => p.BlogId == 1)
                .Include(p => p.Author)
                .Include(p => p.Blog)
                .Include(p => p.Tags);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Blog)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
            //return View();
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            //ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,PostTitle,Abstract,Content,Status,Image")] Post post, List<string> tagValues)
        {
            if (ModelState.IsValid)
            {
                // Supply additional necessary information
                post.Created = DateTime.Now;

                var authorId = _userManager.GetUserId(User);
                post.AuthorId = authorId;

                post.ImageData = await _imageService.EncodeImageAsync(post.Image);
                post.ImageType = _imageService.ContentType(post.Image);

                // Create the slug and determine if it is unique
                var slug = _slugService.UrlFriendly(post.PostTitle);
                var validationError = false;
                // Detect Empty Title
                if(string.IsNullOrEmpty(slug))
                {
                    validationError = true;
                    // Add a Model State error, and return the user back to the Create view
                    ModelState.AddModelError("", "The title cannot be empty.");
                }
                // Detect incoming duplicate Slugs
                if (!_slugService.IsUnique(slug)) 
                {
                    validationError = true;
                    // Add a Model State error, and return the user back to the Create view
                    ModelState.AddModelError("", "The title you provided cannot be used as it results in a duplicate URL.");

                }
                if(validationError)
                {
                    // This is not part of the post, since the state of tagValues is handled totally by JavaScript on the client
                    // Therefore, we need to specifically "send it back" to them as they sent it to us
                    ViewData["TagValues"] = string.Join(",", tagValues);

                    return View(post);
                }
                post.Slug = slug;

                _context.Add(post);

                //// Save to the Post table in the DB
                await _context.SaveChangesAsync();

                // Loop over tags
                foreach (var tag in tagValues)
                {
                    _context.Add(new Tag()
                    {
                        PostId = post.Id,
                        AuthorId = authorId,
                        Text = tag
                    });
                }  
                
                // Save to the Tags table in the DB
                await _context.SaveChangesAsync();

                // Forward to Successful Save Page
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Return to Create page if ModelState is not Valid after "reloading" necessary data
                ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Description", post.BlogId);

                return View(post);
            }
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            //var post = await _context.Posts.FindAsync(id);
            var post = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));

            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,PostTitle,Abstract,Content,Status")] Post post,IFormFile image, List<string> tagValues)
        {
            if (id != post.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    //var newPost = await _context.Posts.FindAsync(post.Id);
                    var newPost = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == post.Id);

                    newPost.Updated = DateTime.Now;
                    newPost.PostTitle = post.PostTitle;
                    newPost.Abstract = post.Abstract;
                    newPost.Content = post.Content;
                    newPost.Status = post.Status;

                    if (image is not null)
                    {
                        newPost.ImageData =  await _imageService.EncodeImageAsync(image);
                        newPost.ImageType = _imageService.ContentType(image);
                    }

                    // Remove all Tags previously assiciated with this Post
                    _context.Tags.RemoveRange(newPost.Tags);

                    // Add in the new Tags from the Edit form
                    foreach(var tagText in tagValues)
                    {
                        _context.Add(new Tag()
                        {
                            PostId = post.Id,
                            AuthorId = newPost.AuthorId,
                            Text = tagText
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
            
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Blog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
