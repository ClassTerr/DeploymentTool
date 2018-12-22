using DeploymentTool.API.Models;
using DeploymentTool.Core.Helpers;
using DeploymentTool.Core.Models;
using DeploymentTool.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace API_TEST_CONSOLE
{
    public class SynchAPI
    {
        public static async Task<List<ClientProfile>> FetchProfiles(string siteUrl, string token)
        {
            if (string.IsNullOrEmpty(siteUrl))
            {
                throw new ArgumentException("URL must be non-empty", nameof(siteUrl));
            }

            string apiUrl = siteUrl + "/Sync/Profiles";

            var profileModels = await RequestHelper.GetAsync<List<ProfileModel>>(apiUrl, token);

            var result = profileModels.Select(x => new ClientProfile()
            {
                ID = x.ID,
                Name = x.Name,
                ExcludedPaths = x.ExcludedPaths,
                APIToken = token,
                URL = siteUrl
            }).ToList();

            return result;
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
