
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoderHive.Data;
using CoderHive.Models;
using CoderHive.Services;
using Microsoft.AspNetCore.Identity;

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
            var applicationDbContext = _context.Posts.Where(p => p.BlogId == 1);
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
                if(!_slugService.IsUnique(slug))
                {
                    // Add a Model state error and return the user back to the Create view
                    ModelState.AddModelError("Title", "The title you provided cannot be used as it results in a duplicate slug.");
                    ViewData["TagValues"] = string.Join(",", tagValues);
                    return View(post);
                }
                post.Slug = slug;

                _context.Add(post);
                //// Save to the Post table in the DB
                //await _context.SaveChangesAsync();

                // Loop over tags
                foreach (var tag in tagValues)
                {
                    _context.Add(new Tag()
                    {
                        PostId = post.Id,
                        AuthorId = post.AuthorId,
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

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,Status")] Post post,IFormFile newImage)
        {
            if (id != post.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var newPost = await _context.Posts.FindAsync(post.Id);

                    newPost.Updated = DateTime.Now;
                    newPost.PostTitle = post.PostTitle;
                    newPost.Abstract = post.Abstract;
                    newPost.Content = post.Content;
                    newPost.Status = post.Status;

                    if (newImage is not null)
                    {
                        newPost.ImageData =  await _imageService.EncodeImageAsync(newImage);
                        newPost.ImageType = _imageService.ContentType(newImage);
                    }

                    //_context.Update(post);

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
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Description", post.BlogId);
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
