using DeploymentTool.Core.Helpers;
using DeploymentTool.Core.Models;
using DeploymentTool.Settings;
using System;
using System.Threading.Tasks;

namespace API_TEST_CONSOLE
{
    public class DeployAPI
    {
        public static async Task<string> Rollback(string sessionId, string token)
        {
            string apiUrl = Globals.SITE_URL + "/Deploy/Rollback";

            var result = await RequestHelper.PostAsync(apiUrl, token, sessionId);

            var response = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return response.ToObject<string>();
            }
            else
            {
                throw new Exception($"Unable to get filesystem difference. {response} (Code {(int)result.StatusCode}: {result.StatusCode}).");
            }
        }

        public static async Task<string> Deploy(string sessionId, string profileId)
        {
            var profile = SettingsManager.Instance.GetProfile(profileId);
            string apiUrl = profile.URL + "/Deploy/Deploy";

            var result = await RequestHelper.PostAsync(apiUrl, profile.APIToken, sessionId);

            var response = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return response.ToObject<string>();
            }
            else
            {
                throw new Exception($"Unable to process deployment. {response} (Code {(int)result.StatusCode}: {result.StatusCode}).");
            }
        }

        public static async Task<DeployInitResult> Init(string profileId)
        {
            var profile = SettingsManager.Instance.GetProfile(profileId);

            if (profile == null)
            {
                throw new Exception($"Profile with id {profileId} not found!");
            }

            string apiUrl = profile.URL + "/Deploy/Init";

            var filesystemState = FilesystemStateModel.GetProfileFilesystemState(profile);

            var result = await RequestHelper.PostAsync(apiUrl, profile.APIToken, filesystemState.ToJSON());

            var response = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return response.ToObject<DeployInitResult>();
            }
            else
            {
                throw new Exception($"Unable to process deployment process. {response} (Code {(int)result.StatusCode}: {result.StatusCode}).");
            }
        }
    }
}
