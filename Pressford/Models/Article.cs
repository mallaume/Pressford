using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pressford.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public ApplicationUser Author { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public ICollection<ArticleLike> Likes { get; set; }
        public ICollection<ArticleComment> Comments { get; set; }
    }
}
