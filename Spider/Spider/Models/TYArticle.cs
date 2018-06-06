using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Models
{
    public class TYArticle : Article
    {
        public List<TYAricleBody> TYAriclelist { get; set; }
    }

    public class TYAricleBody: ArticleBody
    {
        public string author { get; set; }
        public string applyid { get; set; }
    }
}
