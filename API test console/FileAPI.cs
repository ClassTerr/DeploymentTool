using DeploymentTool.API.Settings;
using DeploymentTool.Core.Helpers;
using DeploymentTool.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace API_TEST_CONSOLE
{
    public class FileAPI
    {
        public static async Task<string> Upload(string siteUrl, string token, string deploySessionId, string filename, string targetFilename)
        {
            return await Upload(siteUrl, token, deploySessionId, new FileToUpload[] { new FileToUpload(filename, targetFilename) });
        }

        public static async Task<string> Upload(string siteUrl, string token, string deploySessionId, IEnumerable<FileToUpload> filesToUpload)
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

            client.DefaultRequestHeaders.Add("Authorization", token);
            client.DefaultRequestHeaders.Add("DeploySessionId", deploySessionId);

            var openedFiles = new List<FileStream>();

            try
            {

                MultipartFormDataContent form = new MultipartFormDataContent();
                foreach (var file in filesToUpload)
                {

                    var stream = File.OpenRead(file.LocalFilename);
                    openedFiles.Add(stream);

                    HttpContent content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = Path.GetFileName(file.TargetRelativeFilename),
                        FileName = file.TargetRelativeFilename
                    };

                    form.Add(content);
                }

                HttpResponseMessage response = await client.PostAsync(apiUrl, form);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            finally
            {
                foreach (var item in openedFiles)
                {
                    item.Dispose();
                }
            }
        }

        public static async Task DownloadFile(string siteUrl, string token, string profileId, string filepath, string targetFilename)
        {
            Stream fileContent = await GetFile(siteUrl, token, profileId, filepath);
            using (Stream file = File.Create(targetFilename))
            {
                await fileContent.CopyToAsync(file);
            }
        }

        public static async Task<Stream> GetFile(string siteUrl, string token, string profileId, string filepath)
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

            request.Headers.Add("Authorization", token);
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

        public static async Task<FilesystemDifference> GetFilesystemDifference(string profileId, string token, FilesystemStateModel clientFilesystemStateModel)
        {
            var profile = SettingsManager.Instance.GetProfile(profileId);
            string apiUrl = profile.URL + "/FilesystemState/GetFilesystemDifference";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);

            var content = new StringContent(clientFilesystemStateModel.ToJSON(), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(apiUrl, content);

            var response = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return response.ToObject<FilesystemDifference>();
            }
            else
            {
                throw new Exception($"Unable to get filesystem difference. {response} (Code {(int)result.StatusCode}: {result.StatusCode}).");
            }
        }

    }
}
