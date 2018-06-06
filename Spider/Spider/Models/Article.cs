using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Models
{
    public class Article
    {
        public string Title { get; set; }
        public int PageCount { get; set; }
    }

    public class ArticleBody
    {
        public string article { get; set; }
        public int pageindex { get; set; }
    }
}
