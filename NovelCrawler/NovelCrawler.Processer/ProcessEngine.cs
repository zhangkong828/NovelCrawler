using NovelCrawler.Infrastructure;
using NovelCrawler.Infrastructure.Router;
using NovelCrawler.Models;
using NovelCrawler.Processer.Models;
using NovelCrawler.Repository.IRepository;
using NovelCrawler.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace NovelCrawler.Processer
{
    public class ProcessEngine
    {
        private static readonly object _obj = new object();
        private static readonly string _ruleDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "Rules";
        private static readonly CancellationTokenSource _cancellation = new CancellationTokenSource();
        private static bool _isWorking = false;

        private static ProcessEngine _instance;
        private ProcessEngineOptions _options;
        private CancellationToken _cancellationToken;

        private INovelInfoRepository _novelInfoRepository;
        private INovelIndexRepository _novelIndexRepository;
        private INovelChapterRepository _novelChapterRepository;

        private ProcessEngine(ProcessEngineOptions options)
        {
            _options = options;
            _cancellationToken = _cancellation.Token;
            _cancellationToken.Register(() => { Logger.Info("正在取消任务..."); });

            _novelInfoRepository = new NovelInfoRepository();
            _novelIndexRepository = new NovelIndexRepository();
            _novelChapterRepository = new NovelChapterRepository();
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
            var rules = LoadRules();
            if (rules == null || rules.Count == 0)
                return;

            foreach (var item in rules)
            {
                var rule = item.Value;
                var task = Task.Run(() =>
                  {
                      while (true)
                      {
                          _isWorking = true;
                          try
                          {
                              _cancellationToken.ThrowIfCancellationRequested();
                              Process(rule);
                          }
                          catch (Exception ex)
                          {
                              if (ex is OperationCanceledException)
                              {
                                  Logger.Info("{0} 已取消", rule.SiteUrl);
                                  break;
                              }
                          }
                          finally
                          {
                              var milliSeconds = (int)_options.SpiderIntervalTime.TotalMilliseconds;
                              milliSeconds = milliSeconds <= 0 ? 60000 : milliSeconds;
                              Logger.ColorConsole(string.Format("抓取结束，休眠{0}s", milliSeconds));
                              Thread.Sleep(milliSeconds);
                          }

                      }
                  }, _cancellationToken);
            }
        }

        /// <summary>
        /// 等待抓取结束，停止
        /// </summary>
        public void Stop()
        {
            _isWorking = false;
            _cancellation.Cancel();
        }

        private Dictionary<string, RuleModel> LoadRules()
        {
            var _rules = new Dictionary<string, RuleModel>();
            if (!Directory.Exists(_ruleDirectoryPath))
            {
                throw new Exception("Rules目录不存在");
            }

            var files = Directory.GetFiles(_ruleDirectoryPath, "*.xml");
            if (files.Length == 0)
            {
                Logger.Info("Rules目录下未找到相关xml规则文件");
                return null;
            }

            foreach (var file in files)
            {
                try
                {
                    var filename = Path.GetFileName(file);
                    if (_rules.ContainsKey(filename))
                    {
                        Logger.Info("出现同名xml规则，忽略");
                        continue;
                    }
                    var rule = XmlHelper.XmlDeserializeFromFile<RuleModel>(file, Encoding.UTF8);
                    _rules.Add(filename, rule);
                }
                catch { }
            }

            return _rules;
        }

        private void Process(RuleModel rule)
        {
            var spider = new Spider(null, rule);

            try
            {
                //获取更新列表
                var novelKeys = spider.GetUpdateList().Result;

                //并行抓取
                Parallel.ForEach(novelKeys, (novelKey, loopState) =>
                {
                    try
                    {
                        if (!_isWorking)
                            return;

                        var model = new NovelInfo();
                        //获取小说详情
                        var info = spider.GetNovelInfo(novelKey).Result;
                        //判断是否已入库
                        if (_novelInfoRepository.Exists(x => x.Name == info.Name && x.Author == info.Author, out model))
                        {
                            if (model.State == 1 && _options.SpiderOptions.不处理已完成小说)
                                return;

                            if (_options.SpiderOptions.强制清空重采)
                            {
                                //todo
                            }

                            ProcessUpdate(spider, novelKey, info, model).Wait();//更新
                        }
                        else
                        {
                            if (_options.SpiderOptions.添加新书)
                                ProcessAdd(spider, novelKey, info).Wait();//新增
                        }

                    }
                    catch (SpiderException ex)
                    {
                        Logger.Error("{0}，{1} 抓取失败：{2}", rule.SiteUrl, novelKey, ex.Message);
                    }
                });
            }
            catch (SpiderException ex)
            {
                Logger.Error("{0} 抓取失败：{1}", rule.SiteUrl, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "ProcessEngine.Process");
            }
        }


        private async Task ProcessAdd(Spider spider, string novelKey, NovelDetails info)
        {
            var chapterIndex = info.ChapterIndex;
            //小说id
            var novelId = ObjectId.NextId();
            //目录索引id
            var novelIndexId = ObjectId.NextId();
            //小说封面
            var novelCover = spider.DownLoadImageToBase64(info.ImageUrl);

            /*
             * 1 >>> 获取章节列表
             */
            var chapterList = await spider.GetNovelChapterList(novelKey, chapterIndex);
            var indexes = new List<Index>();
            //抓取章节  单个抓取 需要延迟 不然容易被封
            for (int i = 0; i < chapterList.Count; i++)
            {
                if (!_isWorking)
                    break;

                var chapter = chapterList[i];
                try
                {
                    var content = await spider.GetContent(novelKey, chapterIndex, chapter.Value);
                    var chapterId = ObjectId.NextId();
                    var chapterEntity = new NovelChapter()
                    {
                        Id = chapterId,
                        NovelId = novelId,
                        ChapterName = chapter.Key,
                        UpdateTime = DateTime.Now,
                        WordCount = Utils.GetWordCount(content),
                        Content = content
                    };
                    _novelChapterRepository.Insert(novelId, chapterEntity);
                    indexes.Add(new Index() { ChapterId = chapterId, ChapterName = chapter.Key });//索引目录
                    Thread.Sleep(500);
                }
                catch (SpiderException ex)
                {
                    Logger.Error("{0}-{1} 小说章节抓取失败：{2}", chapter.Key, chapter.Value, ex.Message);

                    if (_options.SpiderOptions.错误章节处理 == 错误章节处理.停止本书_继续采集下一本)
                    {
                        Logger.ColorConsole2(string.Format("{0}-{1} 错误章节处理.停止本书_继续采集下一本", chapter.Key, chapter.Value, ex.Message));
                        break;
                    }
                    else if (_options.SpiderOptions.错误章节处理 == 错误章节处理.入库章节名_继续采集下一章)
                    {
                        Logger.ColorConsole2(string.Format("{0}-{1} 错误章节处理.入库章节名_继续采集下一章", chapter.Key, chapter.Value, ex.Message));
                        var chapterId = ObjectId.NextId();
                        var chapterEntity = new NovelChapter()
                        {
                            Id = chapterId,
                            NovelId = novelId,
                            ChapterName = chapter.Key,
                            UpdateTime = DateTime.Now,
                            WordCount = 0,
                            Content = ""
                        };
                        _novelChapterRepository.Insert(novelId, chapterEntity);
                        indexes.Add(new Index() { ChapterId = chapterId, ChapterName = chapter.Key });
                    }
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex, "ProcessEngine.ProcessAdd");
                    break;
                }
            }

            /*
             * 2 >>> 写入索引目录
             */
            var novelIndex = new NovelIndex()
            {
                Id = novelIndexId,
                NovelId = novelId,
                UpdateTime = DateTime.Now,
                Indexex = indexes
            };
            _novelIndexRepository.Insert(novelIndex);

            /*
             * 3 >>> 写入小说详情
             */
            var novelInfo = new NovelInfo()
            {
                Id = novelId,
                Name = info.Name,
                Author = info.Author,
                Sort = spider.MatchSort(info.Sort),
                State = info.State,
                Des = info.Des,
                Cover = novelCover,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                LatestChapter = indexes.LastOrDefault()?.ChapterName,
                LatestChapterId = indexes.LastOrDefault()?.ChapterId,
                IndexId = novelIndexId
            };
            _novelInfoRepository.Insert(novelInfo);
        }


        private async Task ProcessUpdate(Spider spider, string novelKey, NovelDetails info, NovelInfo model)
        {
            var chapterIndex = info.ChapterIndex;
            var novelInfo = _novelInfoRepository.FindOrDefault(x => x.Name == info.Name && x.Author == info.Author);
            if (novelInfo == null)
                return;
            //对比章节，判断是否需要新增
            var oldIndexes = _novelIndexRepository.FindOrDefault(x => x.Id == novelInfo.IndexId);//老索引
            var oldChapterList = oldIndexes?.Indexex.Select(x => x.ChapterName).ToList();//老的章节列表
            var chapterList = await spider.GetNovelChapterList(novelKey, chapterIndex);//抓取最新章节
            var newChapterList = chapterList.Select(x => x.Key).ToList();//新的章节列表
            int updateIndex = 0;

            if (ChapterListNeedUpdate(oldChapterList, newChapterList, out updateIndex))
            {
                var indexes = new List<Index>();//更新的列表
                //更新章节
                for (int i = updateIndex; i < chapterList.Count; i++)
                {
                    if (!_isWorking)
                        break;

                    var chapter = chapterList[i];
                    try
                    {
                        var content = await spider.GetContent(novelKey, chapterIndex, chapter.Value);
                        var chapterId = ObjectId.NextId();
                        var chapterEntity = new NovelChapter()
                        {
                            Id = chapterId,
                            NovelId = novelInfo.Id,
                            ChapterName = chapter.Key,
                            UpdateTime = DateTime.Now,
                            WordCount = Utils.GetWordCount(content),
                            Content = content
                        };
                        _novelChapterRepository.Insert(novelInfo.Id, chapterEntity);
                        indexes.Add(new Index() { ChapterId = chapterId, ChapterName = chapter.Key });//索引目录
                        Thread.Sleep(500);
                    }
                    catch (SpiderException ex)
                    {
                        Logger.Error("{0}-{1} 小说章节抓取失败：{2}", chapter.Key, chapter.Value, ex.Message);

                        if (_options.SpiderOptions.错误章节处理 == 错误章节处理.停止本书_继续采集下一本)
                        {
                            Logger.ColorConsole2(string.Format("{0}-{1} 错误章节处理.停止本书_继续采集下一本", chapter.Key, chapter.Value, ex.Message));
                            break;
                        }
                        else if (_options.SpiderOptions.错误章节处理 == 错误章节处理.入库章节名_继续采集下一章)
                        {
                            Logger.ColorConsole2(string.Format("{0}-{1} 错误章节处理.入库章节名_继续采集下一章", chapter.Key, chapter.Value, ex.Message));
                            var chapterId = ObjectId.NextId();
                            var chapterEntity = new NovelChapter()
                            {
                                Id = chapterId,
                                NovelId = novelInfo.Id,
                                ChapterName = chapter.Key,
                                UpdateTime = DateTime.Now,
                                WordCount = 0,
                                Content = ""
                            };
                            _novelChapterRepository.Insert(novelInfo.Id, chapterEntity);
                            indexes.Add(new Index() { ChapterId = chapterId, ChapterName = chapter.Key });
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Fatal(ex, "ProcessEngine.ProcessUpdate");
                        break;
                    }
                }
                //更新索引目录
                oldIndexes.Indexex.AddRange(indexes);
                _novelIndexRepository.Update(x => x.Id == oldIndexes.Id, oldIndexes);
                //更新小说详情
                novelInfo.State = info.State;
                novelInfo.UpdateTime = DateTime.Now;
                novelInfo.LatestChapter = oldIndexes.Indexex.LastOrDefault()?.ChapterName;
                novelInfo.LatestChapterId = oldIndexes.Indexex.LastOrDefault()?.ChapterId;

                if (_options.SpiderOptions.自动更新分类)
                    novelInfo.Sort = spider.MatchSort(info.Sort);
                if (_options.SpiderOptions.自动更新封面)
                    novelInfo.Cover = spider.DownLoadImageToBase64(info.ImageUrl);
                if (_options.SpiderOptions.自动更新简介)
                    novelInfo.Des = info.Des;

                _novelInfoRepository.Update(x => x.Id == novelInfo.Id, novelInfo);
            }

        }

        /// <summary>
        /// 章节列表是否需要更新
        /// </summary>
        private bool ChapterListNeedUpdate(List<string> oldIndexes, List<string> newIndexes, out int num)
        {
            double similarity;
            num = 0;
            if (oldIndexes == null || oldIndexes.Count == 0)
                return true;
            //对比 最后一章
            for (int i = newIndexes.Count - 1; i >= 0; i--)
            {
                var oldChapter = oldIndexes.LastOrDefault();
                var newChapter = newIndexes[i];
                if (oldChapter == newChapter || Utils.CompareChapter(oldChapter, newChapter, out similarity))
                {
                    num = i + 1;
                    return !(i == newIndexes.Count - 1);//如果是最后一章 不更新
                }
            }

            //对比 倒数第二章
            if (oldIndexes.Count >= 2)
            {
                for (int i = newIndexes.Count - 1; i >= 0; i--)
                {
                    var oldChapter = oldIndexes[oldIndexes.Count - 2];
                    var newChapter = newIndexes[i];
                    if (oldChapter == newChapter || Utils.CompareChapter(oldChapter, newChapter, out similarity))
                    {
                        num = i + 1;
                        return !(i == newIndexes.Count - 1);//如果是最后一章 不更新
                    }
                }
            }

            //如果库里有数据，是一定不会到这里！除非一章都没对上
            Logger.Error("章节列表对比失败！");
            return false;
        }


        private string ProcessContent(string content)
        {
            if (!_options.SpiderOptions.入库章节时是否添加文字广告)
                return content;

            if (_options.SpiderOptions.文字广告集合.Count == 0)
                return content;


            //todo

            return content;
        }

    }
}
