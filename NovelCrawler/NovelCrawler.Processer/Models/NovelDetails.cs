using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Processer.Models
{
    public class NovelDetails
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Sort { get; set; }
        public string Author { get; set; }
        public int State { get; set; }
        public string Des { get; set; }

        public string ChapterIndex { get; set; }
    }
}
