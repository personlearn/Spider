using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core.Downloader
{
    public class HttpClientDownloader : IDownloader
    {
        private readonly string _downloadFolder;
        private readonly bool _decodeHtml;
        private readonly double _timeout = 8000;

        public async Task<Page> Download(Request request, ISpider spider)
        {
            HttpResponseMessage response = null;
            try
            {
                var httpMessage = GenerateHttpRequestMessage(request, spider.Site);
                response = await new HttpClient().SendAsync(httpMessage);
                request.StatusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();

                byte[] contentBytes =response.Content.ReadAsByteArrayAsync().Result;
                Page page = new Page(request) { Content = spider.Site.Encoding.GetString(contentBytes, 0, contentBytes.Length) };
                page.TargetUrl = response.RequestMessage.RequestUri.AbsoluteUri;
                return  page;
            }
            catch (Exception ex)
            {
                return new Page(request) { Exception = ex };
            }
            finally
            {
                response?.Dispose();
            }
        }

        private HttpRequestMessage GenerateHttpRequestMessage(Request request, Site site)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(request.Method ?? HttpMethod.Get, request.Url);
            var userAgentHeader = "User-Agent";
            httpRequestMessage.Headers.TryAddWithoutValidation(userAgentHeader, site.Headers.ContainsKey(userAgentHeader) ? site.Headers[userAgentHeader] : site.UserAgent);

            if (!string.IsNullOrWhiteSpace(request.Referer))
            {
                httpRequestMessage.Headers.TryAddWithoutValidation("Referer", request.Referer);
            }

            if (!string.IsNullOrWhiteSpace(request.Origin))
            {
                httpRequestMessage.Headers.TryAddWithoutValidation("Origin", request.Origin);
            }

            if (!string.IsNullOrWhiteSpace(site.Accept))
            {
                httpRequestMessage.Headers.TryAddWithoutValidation("Accept", site.Accept);
            }

            var contentTypeHeader = "Content-Type";

            foreach (var header in site.Headers)
            {
                if (header.Key.ToLower() == "cookie")
                {
                    continue;
                }
                if (!string.IsNullOrWhiteSpace(header.Key) && !string.IsNullOrWhiteSpace(header.Value) && header.Key != contentTypeHeader && header.Key != userAgentHeader)
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
            if (httpRequestMessage.Method == HttpMethod.Post)
            {
                var data = string.IsNullOrWhiteSpace(site.EncodingName) ? Encoding.UTF8.GetBytes(request.PostBody) : site.Encoding.GetBytes(request.PostBody);
                httpRequestMessage.Content = new StreamContent(new MemoryStream(data));


                if (site.Headers.ContainsKey(contentTypeHeader))
                {
                    httpRequestMessage.Content.Headers.TryAddWithoutValidation(contentTypeHeader, site.Headers[contentTypeHeader]);
                }

                var xRequestedWithHeader = "X-Requested-With";
                if (site.Headers.ContainsKey(xRequestedWithHeader) && site.Headers[xRequestedWithHeader] == "NULL")
                {
                    httpRequestMessage.Content.Headers.Remove(xRequestedWithHeader);
                }
                else
                {
                    if (!httpRequestMessage.Content.Headers.Contains(xRequestedWithHeader) && !httpRequestMessage.Headers.Contains(xRequestedWithHeader))
                    {
                        httpRequestMessage.Content.Headers.TryAddWithoutValidation(xRequestedWithHeader, "XMLHttpRequest");
                    }
                }
            }
            return httpRequestMessage;
        }

        public IDownloader Clone()
        {
            return MemberwiseClone() as IDownloader;
        }

        public void Dispose()
        {

        }
    }
}
