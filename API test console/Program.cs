using DeploymentTool.Core.Helpers;
using DeploymentTool.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API_TEST_CONSOLE
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

    class Program
    {
        private const string API_KEY = "f02d8f79-e99b-4e74-867f-5bfd52b93346";
        private const string SITE_URL = "http://localhost:2222";
        static async Task<string> Upload(string siteUrl, string deploySessionId, string filename, string targetFilename)
        {
            return await Upload(siteUrl, deploySessionId, new FileToUpload[] { new FileToUpload(filename, targetFilename) });
        }

        static async Task<string> Upload(string siteUrl, string deploySessionId, IEnumerable<FileToUpload> filesToUpload)
        {
            if (string.IsNullOrWhiteSpace(siteUrl))
            {
                throw new ArgumentException("siteUrl must be non-empty", nameof(siteUrl));
            }

            if (string.IsNullOrWhiteSpace(deploySessionId))
            {
                throw new ArgumentException("deploySessionId must be non-empty", nameof(deploySessionId));
            }

            if (filesToUpload == null)
            {
                throw new ArgumentNullException(nameof(filesToUpload));
            }

            string apiUrl = siteUrl + "/File/Upload";

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", API_KEY);
            client.DefaultRequestHeaders.Add("DeploySessionId", deploySessionId);

            MultipartFormDataContent form = new MultipartFormDataContent();
            foreach (var file in filesToUpload)
            {

                using (var stream = File.OpenRead(file.LocalFilename))
                {
                    HttpContent content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = Path.GetFileName(file.TargetRelativeFilename),
                        FileName = file.TargetRelativeFilename
                    };

                    form.Add(content);
                }
            }

            HttpResponseMessage response = await client.PostAsync(apiUrl, form);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        static async Task DownloadFile(string siteUrl, string profileId, string filepath, string targetFilename)
        {
            Stream fileContent = await GetFile(siteUrl, profileId, filepath);
            using (Stream file = File.Create(targetFilename))
            {
                await fileContent.CopyToAsync(file);
            }
        }

        static async Task<Stream> GetFile(string siteUrl, string profileId, string filepath)
        {
            if (string.IsNullOrWhiteSpace(siteUrl))
            {
                throw new ArgumentException("siteUrl must be non-empty", nameof(siteUrl));
            }

            if (string.IsNullOrEmpty(profileId))
            {
                throw new ArgumentException("profileId must be non-empty", nameof(profileId));
            }

            if (string.IsNullOrEmpty(filepath))
            {
                throw new ArgumentException("filename must be non-empty", nameof(filepath));
            }

            string apiUrl = siteUrl + "/File/Download";

            var request = WebRequest.Create(apiUrl);

            request.Headers.Add("Authorization", API_KEY);
            request.Headers.Add("ProfileId", profileId);
            request.Headers.Add("Filepath", filepath);

            request.Method = WebRequestMethods.Http.Get;

            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                return response.GetResponseStream();
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                var responseStream = ex.Response.GetResponseStream();
                var reader = new StreamReader(responseStream, Encoding.UTF8);
                var exceptionMessage = await reader.ReadToEndAsync();
                throw new Exception($"Unable to get file. {exceptionMessage} (Code {(int)response.StatusCode}: {response.StatusDescription}).");
            }
        }

        static async Task<FilesystemDifference> GetFilesystemDifference(FilesystemStateModel clientFilesystemStateModel)
        {
            string apiUrl = SITE_URL + "/FilesystemState/GetFilesystemDifference";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", API_KEY);

            var content = new StringContent(clientFilesystemStateModel.ToJSON(), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(apiUrl, content);

            var response = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return response.ToObject<FilesystemDifference>();
            }
            else
            {
                throw new Exception($"Unable to init deploy. {response} (Code {(int)result.StatusCode}: {result.StatusCode}).");
            }
        }

        static void Main()
        {
            ProfileBase profile = new ProfileBase()
            {
                ID = "1",
                RootFolder = @"C:\Test1"
            };
            var state = FilesystemStateModel.GetFullProfileFilesystemState(profile);

            var diff = GetFilesystemDifference(state).GetAwaiter().GetResult();
            //var msg = Upload(@"C:\Users\super\Desktop\$this.Icon.ico").GetAwaiter().GetResult();
            //var msg = Upload(@"C:\Users\super\Desktop\$this.Icon.ico").GetAwaiter().GetResult();
            //RunAsync().GetAwaiter().GetResult();
        }
    }
}