﻿using Spider.util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Models
{
    public class Site
    {
        private Encoding _encoding = Encoding.UTF8;
        private string _encodingName;
        private ConcurrentBag<Request> _startRequests = new ConcurrentBag<Request>();

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 配置下载器下载的内容是Json还是Html, 如果是Auto则会自动检测下载的内容, 建议设为Auto
        /// </summary>
        public ContentType ContentType { get; set; } = ContentType.Auto;

        /// <summary>
        /// 是否去除外链
        /// </summary>
        public bool RemoveOutboundLinks { get; set; }

        /// <summary>
        /// 采集目标的Domain, 如果RemoveOutboundLinks为True, 则Domain不同的链接会被排除, 如果RemoveOutboundLinks为False, 此值没有作用
        /// 需要自行设置
        /// </summary>
        public string[] Domains { get; set; }

        /// <summary>
        /// 设置 User Agent 头
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";

        /// <summary>
        /// 设置 Accept 头
        /// </summary>
        public string Accept { get; set; } = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";

        /// <summary>
        /// 设置是否下载文件、图片
        /// </summary>
        public bool DownloadFiles { get; set; }

        /// <summary>
        /// 去除返回的JSON数据的最外层填补
        /// </summary>
        public string JsonPadding { get; set; }

        /// <summary>
        /// 不再使用
        /// </summary>
        [Obsolete("请在Downloader对象中设置")]
        public int Timeout { get; set; }

        /// <summary>
        /// 设置站点的编码 
        /// 如果没有设值, 下载器会尝试自动识别编码
        /// </summary>
        public string EncodingName
        {
            get { return _encodingName; }
            set
            {
                if (_encodingName != value)
                {
                    _encodingName = value;
                    _encoding = string.IsNullOrEmpty(_encodingName) ? null : Encoding.GetEncoding(_encodingName);
                }
            }
        }

        /// <summary>
        /// 使用何种编码读取下载的内容, 如果没有设置编码, 下载器会尝试自动识别编码。
        /// 通过设置EncodingName才能修改此值
        /// </summary>
        public Encoding Encoding => _encoding;

        /// <summary>
        /// 起始请求
        /// </summary>
        public IReadOnlyCollection<Request> StartRequests => new ReadOnlyEnumerable<Request>(_startRequests);

        /// <summary>
        /// 每处理完一个目标链接后停顿的时间, 单位毫秒 
        /// </summary>
        public int SleepTime { get; set; } = 100;

        /// <summary>
        /// 目标链接的最大重试次数
        /// </summary>
        public int CycleRetryTimes { get; set; } = 5;

        /// <summary>
        /// 目标服务器是否使用了Gzip压缩数据
        /// 默认实现的下载器会自动解压数据, 不需要依赖此值
        /// </summary>
        [Obsolete]
        public bool IsUseGzip { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Site()
        {
#if NETSTANDARD
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        }

        /// <summary>
        /// 添加一个起始链接到当前站点 
        /// </summary>
        /// <param name="url">起始链接</param>
        public void AddStartUrl(string url)
        {
            AddStartUrls(url);
        }

        /// <summary>
        /// 添加多个个起始链接到当前站点 
        /// </summary>
        /// <param name="url">起始链接</param>
        public void AddStartUrls(params string[] urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException($"{nameof(urls)} should not be null.");
            }
            AddStartUrls(urls.AsEnumerable());
        }

        /// <summary>
        /// 添加一个起始链接到当前站点 
        /// </summary>
        /// <param name="url">起始链接</param>
        /// <param name="datas">链接对应的一些额外数据</param>
        public void AddStartUrl(string url, IDictionary<string, dynamic> datas)
        {
            AddStartRequest(new Request(url, datas));
        }

        /// <summary>
        /// 添加多个起始链接到当前站点 
        /// </summary>
        /// <param name="urls">起始链接</param>
        public void AddStartUrls(IEnumerable<string> urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException($"{nameof(urls)} should not be null.");
            }
            foreach (var url in urls)
            {
                AddStartRequest(new Request(url, null));
            }
        }

        /// <summary>
        /// 添加一个请求对象到当前站点
        /// </summary>
        /// <param name="request">请求对象</param>
        public void AddStartRequest(Request request)
        {
            AddStartRequests(request);
        }

        /// <summary>
        /// 添加一个请求对象到当前站点
        /// </summary>
        /// <param name="request">请求对象</param>
        public void AddStartRequests(params Request[] requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException($"{nameof(requests)} should not be null.");
            }
            AddStartRequests(requests.AsEnumerable());
        }

        /// <summary>
        /// 添加请求对象到当前站点
        /// </summary>
        /// <param name="requests">请求对象</param>
        public void AddStartRequests(IEnumerable<Request> requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException($"{nameof(requests)} should not be null.");
            }
            foreach (var request in requests)
            {
                _startRequests.Add(request);
            }
        }

        /// <summary>
        /// 清空所有起始链接
        /// </summary>
        public void ClearStartRequests()
        {
            _startRequests = new ConcurrentBag<Request>();
        }

        /// <summary>
        /// 添加一个全局的HTTP请求头 
        /// </summary>
        public void AddHeader(string key, string value)
        {
            if (Headers.ContainsKey(key))
            {
                Headers[key] = value;
            }
            else
            {
                Headers.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        internal Page AddToCycleRetry(Request request)
        {
            Page page = new Page(request)
            {
                ContentType = ContentType
            };

            request.CycleTriedTimes++;

            if (request.CycleTriedTimes <= CycleRetryTimes)
            {
                request.Priority = 0;
                page.AddTargetRequest(request, false);
                page.Retry = true;
                return page;
            }
            else
            {
                return null;
            }
        }
    }
}
