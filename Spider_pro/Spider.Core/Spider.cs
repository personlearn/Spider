//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Spider.Core.Downloader;
//using Spider.Core.Pipeline;
//using Spider.Core.Processor;
//using Spider.Core.Infrastructure;
//using System.Threading;
//using System.Diagnostics;
//using Spider.Core.Scheduler;

//namespace Spider.Core
//{
//    public class Spider : AppBase, ISpider
//    {
//        private IDownloader _downloader;
//        private Site _site = new Site();
//        protected readonly List<IPipeline> _pipelines = new List<IPipeline>();
//        protected readonly List<IPageProcessor> _pageProcessors = new List<IPageProcessor>();
//        private int _threadNum = 1;
//        private IScheduler _scheduler;
//        private int _waitCountLimit = 1500;
//        public event Action<Request> OnRequestSucceeded;
//        public event Action<Spider> OnCompleted;
//        public event Action<Spider> OnClosed;


//        protected DateTime StartTime { get; private set; }

//        public Status Status { get; private set; } = Status.Init;

//        protected int WaitInterval { get; } = 10;

//        public IDownloader Downloader
//        {
//            get
//            {
//                return _downloader;
//            }

//            set
//            {
//                _downloader = value;
//                if (value == null)
//                {
//                    throw new ArgumentNullException($"{nameof(Downloader)} should not be null.");
//                }
//            }
//        }

//        public IReadOnlyCollection<IPageProcessor> PageProcessors => new ReadOnlyEnumerable<IPageProcessor>(_pageProcessors);

//        public IReadOnlyCollection<IPipeline> Pipelines => new ReadOnlyEnumerable<IPipeline>(_pipelines);

//        public int ThreadNum
//        {
//            get { return _threadNum; }
//            set
//            {
//                if (value <= 0)
//                {
//                    throw new ArgumentException($"{nameof(ThreadNum)} should be more than one!");
//                }
//                _threadNum = value;
//            }
//        }
//        public Site Site
//        {
//            get
//            {
//                return _site;
//            }
//            set
//            {
//                _site = value;
//                if (value == null)
//                {
//                    throw new ArgumentException($"{nameof(Site)} should not be null.");
//                }
//            }
//        }

//        public IScheduler Scheduler
//        {
//            get { return _scheduler; }
//            set
//            {
//                _scheduler = value;
//                if (value == null)
//                {
//                    throw new ArgumentNullException($"{nameof(Scheduler)} should not be null.");
//                }
//            }
//        }

//        public void Contiune()
//        {
//            if (Status == Status.Paused)
//            {
//                Status = Status.Running;
//                Logger.Warning("Continue...");
//            }
//            else
//            {
//                Logger.Information("Crawler was not paused, can not continue...");
//            }
//        }

//        public void Dispose()
//        {
//            try
//            {
//                _pipelines.GetEnumerator().Dispose();
//                _pageProcessors.GetEnumerator().Dispose();
//                _downloader.Dispose();
//            }
//            catch (Exception e)
//            {
//                Logger.Error(e.ToString());
//            }
//        }

//        public void Exit(Action action = null)
//        {
//            if (Status == Status.Running || Status == Status.Paused)
//            {
//                Status = Status.Exited;
//                Logger.Information("Exit...");
//                return;
//            }
//            Logger.Warning(Identity, "Crawler is not running.");
//            if (action != null)
//            {
//                Task.Factory.StartNew(() =>
//                {
//                    action();
//                });
//            }
//        }

//        public void Pause(Action action = null)
//        {
//            bool isRunning = Status == Status.Running;
//            if (!isRunning)
//            {
//                Logger.Warning(Identity, "Crawler is not running.");
//            }
//            else
//            {
//                Status = Status.Paused;
//                Logger.Information(Identity, "Stop running...");
//            }
//            action?.Invoke();
//        }

//        protected override void Execute(params string[] arguments)
//        {
//            if (Status == Status.Running)
//            {
//                Logger.Warning("Crawler is running...");
//                return;
//            }

//            StartTime = DateTime.Now;
//            Status = Status.Running;

//            while (Status == Status.Running || Status == Status.Paused)
//            {
//                // 暂停则一直停在此处
//                if (Status == Status.Paused)
//                {
//                    Thread.Sleep(50);
//                    continue;
//                }

//                Parallel.For(0, ThreadNum, new ParallelOptions
//                {
//                    MaxDegreeOfParallelism = ThreadNum
//                }, i =>
//                {
//                    int waitCount = 1;
//                    // 每个线程使用一个下载器实例, 在使用如WebDriverDownloader时不需要管理WebDriver实例了
//                    var downloader = Downloader.Clone();
//                    while (Status == Status.Running)
//                    {
//                        // 从队列中取出一个请求
//                        Request request = Scheduler.Poll();

//                        // 如果队列中没有需要处理的请求, 则开始等待, 一直到设定的 EmptySleepTime 结束, 则认为爬虫应该结束了
//                        if (request == null)
//                        {
//                            if (waitCount > _waitCountLimit)
//                            {
//                                Status = Status.Finished;
//                                OnCompleted?.Invoke(this);
//                                break;
//                            }

//                            // wait until new url added
//                            WaitNewUrl(ref waitCount);
//                        }
//                        else
//                        {
//                            waitCount = 1;

//                            try
//                            {
//                                Stopwatch sw = new Stopwatch();
//                                HandleRequest(sw, request, downloader);
//                                Thread.Sleep(Site.SleepTime);
//                            }
//                            catch (Exception e)
//                            {
//                                //OnError(request);
//                                Logger.Error($"Crawler {request.Url} failed: {e}.");
//                            }
//                            finally
//                            {
//                                //if (request.Proxy != null)
//                                //{
//                                //    var statusCode = request.StatusCode;
//                                //    Site.HttpProxyPool.ReturnProxy(request.Proxy, statusCode ?? HttpStatusCode.Found);
//                                //}
//                            }
//                        }
//                    }
//                });
//            }
//        }

//        private void WaitNewUrl(ref int waitCount)
//        {
//            Thread.Sleep(WaitInterval);
//            ++waitCount;
//        }

//        protected void HandleRequest(Stopwatch stopwatch, Request request, IDownloader downloader)
//        {
//            Page page = null;

//            try
//            {
//                stopwatch.Reset();
//                stopwatch.Start();

//                page = downloader.Download(request, this).Result;

//                stopwatch.Stop();
//                //CalculateDownloadSpeed(stopwatch.ElapsedMilliseconds);

//                if (page == null || page.Skip)
//                {
//                    return;
//                }

//                if (page.Exception == null)
//                {
//                    stopwatch.Reset();
//                    stopwatch.Start();

//                    foreach (var processor in _pageProcessors)
//                    {
//                        processor.Process(page, this);
//                    }

//                    stopwatch.Stop();
//                    //CalculateProcessorSpeed(stopwatch.ElapsedMilliseconds);
//                }
//                else
//                {
//                    //OnError(page.Request);
//                }
//            }
//            catch (DownloadException de)
//            {
//                if (page != null)
//                {
//                    //OnError(page.Request);
//                }
//                Logger.Error($"Should not catch download exception: {request.Url}.");
//            }
//            catch (Exception e)
//            {
//                //if (Site.CycleRetryTimes > 0)
//                //{
//                //    page = Site.AddToCycleRetry(request);
//                //}
//                //if (page != null)
//                //{
//                //    OnError(page.Request);
//                //}
//                Logger.Error($"Extract {request.Url} failed, please check your pipeline: {e}.");
//            }

//            if (page == null)
//            {
//                return;
//            }
//            // 此处是用于需要循环本身的场景, 不能使用本身Request的原因是Request的尝试次数计算问题
//            //if (page.Retry)
//            //{
//            //    RetriedTimes.Inc();
//            //    ExtractAndAddRequests(page);
//            //    return;
//            //}

//            bool excutePipeline = false;
//            if (!page.SkipTargetUrls)
//            {
//                if (page.ResultItems.IsEmpty)
//                {
//                    if (SkipTargetUrlsWhenResultIsEmpty)
//                    {
//                        Logger.Warning($"Skip {request.Url} because extract 0 result.");
//                        _OnSuccess(request);
//                    }
//                    // 场景: 此链接就是用来生产新链接的, 因此不会有内容产出
//                    else if (page.TargetRequests != null && page.TargetRequests.Count > 0)
//                    {
//                        ExtractAndAddRequests(page);
//                    }
//                    else
//                    {
//                        if (Site.CycleRetryTimes > 0)
//                        {
//                            page = Site.AddToCycleRetry(request);
//                            if (page != null && page.Retry)
//                            {
//                                RetriedTimes.Inc();
//                                ExtractAndAddRequests(page);
//                            }
//                            Logger.Warning($"Download {request.Url} success, retry becuase extract 0 result.");
//                        }
//                        else
//                        {
//                            Logger.Warning($"Download {request.Url} success, will not retry because Site.CycleRetryTimes is 0.");
//                            _OnSuccess(request);
//                        }
//                    }
//                }
//                else
//                {
//                    excutePipeline = true;
//                    ExtractAndAddRequests(page);
//                }
//            }
//            else
//            {
//                excutePipeline = !page.ResultItems.IsEmpty;
//            }

//            if (!excutePipeline)
//            {
//                return;
//            }

//            if (page.Exception == null)
//            {
//                stopwatch.Reset();
//                stopwatch.Start();

//                int countOfResults = 0, effectedRows = 0;

//                ResultItems[] resultItems = null;
//                if (PipelineCachedSize == 1)
//                {
//                    resultItems = new[] { page.ResultItems };
//                }
//                else
//                {
//                    lock (this)
//                    {
//                        _cached.Add(page.ResultItems);
//                        if (_cached.Count >= PipelineCachedSize)
//                        {
//                            resultItems = _cached.ToArray();
//                            _cached.Clear();
//                        }
//                    }
//                }

//                foreach (IPipeline pipeline in Pipelines)
//                {
//                    try
//                    {
//                        _pipelineRetryPolicy.Execute(() =>
//                        {
//                            pipeline.Process(new[] { page.ResultItems }, this);
//                        });
//                    }
//                    catch (Exception e)
//                    {
//                        Logger.Error($"Execute pipeline failed: {e}");
//                    }
//                }

//                foreach (var item in resultItems)
//                {
//                    countOfResults += item.Request.CountOfResults.HasValue ? item.Request.CountOfResults.Value : 0;
//                    effectedRows += item.Request.EffectedRows.HasValue ? item.Request.EffectedRows.Value : 0;
//                }

//                Logger.Information($"Crawl: {request.Url} success, results: { request.CountOfResults}, effectedRow: {request.EffectedRows}.");

//                //_OnSuccess(request);

//                stopwatch.Stop();
//                //CalculatePipelineSpeed(stopwatch.ElapsedMilliseconds);
//            }
//        }


//        public virtual Spider AddPageProcessor(IPageProcessor processor)
//        {
//            return AddPageProcessors(processor);
//        }

//        public virtual Spider AddPageProcessors(params IPageProcessor[] processors)
//        {
//            if (processors == null)
//            {
//                throw new ArgumentNullException($"{nameof(processors)} should not be null.");
//            }
//            return AddPageProcessors(processors.AsEnumerable());
//        }

//        public virtual Spider AddPageProcessors(IEnumerable<IPageProcessor> processors)
//        {
//            if (processors == null)
//            {
//                throw new ArgumentNullException($"{nameof(processors)} should not be null.");
//            }
//            if (processors.Count() > 0)
//            {
//                foreach (var processor in processors)
//                {
//                    if (processor != null)
//                    {
//                        _pageProcessors.Add(processor);
//                    }
//                }
//            }
//            return this;
//        }

//        public virtual Spider AddPipeline(IPipeline pipeline)
//        {
//            return AddPipelines(pipeline);
//        }

//        public virtual Spider AddPipelines(params IPipeline[] pipelines)
//        {
//            if (pipelines == null)
//            {
//                throw new ArgumentNullException($"{nameof(pipelines)} should not be null.");
//            }
//            return AddPipelines(pipelines.AsEnumerable());
//        }


//        public virtual Spider AddPipelines(IEnumerable<IPipeline> pipelines)
//        {
//            if (pipelines == null)
//            {
//                throw new ArgumentNullException($"{nameof(pipelines)} should not be null.");
//            }
//            if (pipelines.Count() > 0)
//            {
//                foreach (var pipeline in pipelines)
//                {
//                    if (pipeline != null)
//                    {
//                        _pipelines.Add(pipeline);
//                    }
//                }
//            }
//            return this;
//        }
//    }
//}
