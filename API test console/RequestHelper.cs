using DeploymentTool.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API_TEST_CONSOLE
{
    public class RequestHelper
    {
        public static Task<HttpResponseMessage> PostAsync(string apiUrl, string token, string content = "")
        {
            return PostAsync(apiUrl, token, new StringContent(content));
        }
        public static async Task<HttpResponseMessage> PostAsync(string apiUrl, string token, HttpContent content)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", token);

                var result = await client.PostAsync(apiUrl, content);
                return result;
            }
        }

        public static async Task<HttpResponseMessage> GetAsync(string apiUrl, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", token);

                var result = await client.GetAsync(apiUrl);
                return result;
            }
        }

        public static async Task<T> GetAsync<T>(string apiUrl, string token)
        {
            var result = await GetAsync(apiUrl, token);
            var obj = await ConvertResponse<T>(result);
            return obj;
        }

        public static async Task<T> PostAsync<T>(string apiUrl, string token, string content = "")
        {
            var result = await PostAsync(apiUrl, token, content);
            var obj = await ConvertResponse<T>(result, "POST");
            return obj;
        }

        public static async Task<T> ConvertResponse<T>(HttpResponseMessage response, string method = "GET")
        {
            method = method.ToUpperInvariant();
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return responseString.ToObject<T>();
            }
            else
            {
                throw new Exception($"Unable to {method} (Code {(int)response.StatusCode}: {response.StatusCode})." +
                    Environment.NewLine + responseString);
            }
        }
    }
}
