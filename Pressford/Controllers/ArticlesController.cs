using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pressford.Data;
using Pressford.Models;

namespace Pressford.Controllers
{
    [Produces("application/json")]
    [Route("api/articles")]
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticlesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticles()
        {
            var articles = await _context.Articles
                .Include(e => e.Author)
                .Include(e => e.Likes)
                .Include(e => e.Comments)
                .OrderByDescending(e => e.CreationDate).ToListAsync();

            return Ok(articles);
        }
        [HttpGet("stats")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticlesStats()
        {
            var articles = await _context.Articles
                .Include(e => e.Likes)
                .ToListAsync();

            var sorted = from e in articles
                         orderby e.Likes.Count descending
                         select new
                         {
                             label = e.Title,
                             value = e.Likes.Count
                         };

            return Ok(sorted.Take(10));
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            var article = await _context.Articles
                .Include(e => e.Author)
                .Include(e => e.Likes)
                .Include(e => e.Comments)
                .ThenInclude(c => c.Commenter)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Publisher")]
        public async Task<IActionResult> UpdateArticle([FromRoute] int id, [FromBody] Article article)
        {
            var item = await _context.Articles.SingleOrDefaultAsync(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            item.Body = article.Body;

            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "Publisher")]
        public async Task<IActionResult> CreateArticle([FromBody] Article article)
        {
            var user = await _userManager.GetUserAsync(User);

            var item = new Article()
            {
                Author = user,
                Title = article.Title,
                Body = article.Body,
                CreationDate = DateTime.UtcNow
            };

            _context.Articles.Add(item);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Publisher")]
        public async Task<IActionResult> DeleteArticle([FromRoute] int id)
        {
            var article = await _context.Articles
                .Include(e => e.Comments)
                .Include(e => e.Likes)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            _context.ArticleLikes.RemoveRange(article.Likes);
            _context.ArticleComments.RemoveRange(article.Comments);
            _context.Articles.Remove(article);

            await _context.SaveChangesAsync();

            return Ok(article);
        }

        [HttpPost("{id}/like")]
        [Authorize]
        public async Task<IActionResult> LikeArticle([FromRoute] int id)
        {
            if (!ArticleExists(id))
            {
                return BadRequest();
            }

            var article = await _context.Articles
                .Include(e => e.Likes)
                .ThenInclude(l => l.Liker)
                .SingleOrDefaultAsync(e => e.Id == id);

            if (!article.Likes.Where(e => e.Liker.UserName == User.Identity.Name).Any())
            {
                var user = await _userManager.GetUserAsync(User);

                article.Likes.Add(new ArticleLike()
                {
                    Liker = user,
                    TimeStamp = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        [HttpPost("{id}/comment")]
        [Authorize]
        public async Task<IActionResult> PostComment([FromRoute] int id, [FromBody] ArticleComment comment)
        {
            if (!ArticleExists(id))
            {
                return BadRequest();
            }

            var article = await _context.Articles
                .Include(e => e.Comments)
                .SingleOrDefaultAsync(e => e.Id == id);

            var user = await _userManager.GetUserAsync(User);

            article.Comments.Add(new ArticleComment()
            {
                Commenter = user,
                Text = comment.Text,
                TimeStamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}