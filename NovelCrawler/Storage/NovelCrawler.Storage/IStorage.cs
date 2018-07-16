using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NovelCrawler.Storage
{
    public interface IStorage
    {

        void Add(string key, string strs);

        void AddImage(string key, byte[] bytes);



        string Get(string key);

        string GetImage(string key);




    }
}
