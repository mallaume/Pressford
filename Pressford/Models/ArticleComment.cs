using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pressford.Models
{
    public class ArticleComment
    {
        public int Id { get; set; }
        public ApplicationUser Commenter { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
