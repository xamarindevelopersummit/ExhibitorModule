using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExhibitorModule.Services.Helpers
{
    public static class HttpExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response)
        {
            if(!response.IsSuccessStatusCode)
                return default(T);

            var content = await response.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(content);
        }

        public static HttpContent ToContent(this object obj)
        {
            try
            {
                var json = JsonConvert.SerializeObject(obj);
                return new StringContent(json, Encoding.UTF8, "application/json");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
