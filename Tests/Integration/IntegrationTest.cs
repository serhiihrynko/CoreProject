using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Tests.Integration
{
    public class IntegrationTest
    {
        protected readonly HttpClient _client;

        public IntegrationTest()
        {
            WebApplicationFactory<API.Startup> appFactory = new WebApplicationFactory<API.Startup>();

            _client = appFactory.CreateClient();
        }
    }
}
