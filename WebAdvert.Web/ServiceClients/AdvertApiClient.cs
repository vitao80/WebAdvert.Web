using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AdvertApiClient(IConfiguration configuration, HttpClient httpClient)
        {
            this._configuration = configuration;
            this._httpClient = httpClient;
        }
        public async Task<AdvertResponse> Create(CreateAdvertModel model)
        {
            throw new NotImplementedException();
        }
    }
}
