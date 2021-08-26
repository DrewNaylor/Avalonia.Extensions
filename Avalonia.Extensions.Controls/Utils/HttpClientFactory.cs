using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Avalonia.Extensions
{
    public class HttpClientFactory
    {
        private static HttpClientFactory instance;
        public static HttpClientFactory Instance
        {
            get
            {
                if (instance == null)
                    instance = new HttpClientFactory();
                return instance;
            }
        }
        private IHttpClientFactory ClientFactory { get; }
        private HttpClientFactory()
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            ClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        }
        public HttpClient CreateClient()
        {
            return ClientFactory.CreateClient();
        }
    }
}