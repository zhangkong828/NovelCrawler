using NovelCrawler.Infrastructure;
using NovelCrawler.Models;
using NovelCrawler.Repository.IRepository;
using NovelCrawler.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Processer
{
    public class ProcessEngine
    {
        private static readonly object _obj = new object();
        private static ProcessEngine _instance;
        private ProcessEngineOptions _options;

        private INovelInfoRepository _novelInfoRepository;

        private ProcessEngine(ProcessEngineOptions options)
        {
            _options = options;

            _novelInfoRepository = new NovelInfoRepository();
        }

        public static ProcessEngine Create(ProcessEngineOptions options = null)
        {
            if (_instance == null)
            {
                lock (_obj)
                {
                    if (_instance == null)
                    {
                        _instance = new ProcessEngine(options ?? new ProcessEngineOptions());
                    }
                }
            }
            return _instance;
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }


        private void Process()
        {
            var rule = XmlHelper.XmlDeserializeFromFile<RuleModel>("testRule.xml", Encoding.UTF8);
            var spider = new Spider(null, rule);
            

        }


    }
}
