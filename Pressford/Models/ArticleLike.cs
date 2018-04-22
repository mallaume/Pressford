using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pressford.Models
{
    public class ArticleLike
    {
        public int Id { get; set; }
        public Article Article { get; set; }
        public ApplicationUser Liker { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
