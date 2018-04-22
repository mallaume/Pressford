using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pressford.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<ArticleLike> Likes { get; set; }
        public ICollection<ArticleComment> Comments { get; set; }
    }
}
