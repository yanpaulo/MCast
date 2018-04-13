using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.Services
{
    public class WebService
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:63071/api/groups/")
        };

        public async Task Register(ClientData data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"register", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UnRegister(ClientData data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"unregister", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
