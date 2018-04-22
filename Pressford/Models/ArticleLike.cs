using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pressford.Models
{
    public class ArticleLike
    {
        public int Id { get; set; }
        [Required]
        public Article Article { get; set; }
        [Required]
        public ApplicationUser Liker { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
