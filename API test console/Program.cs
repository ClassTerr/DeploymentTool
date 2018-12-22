using DeploymentTool.Settings;
using DeploymentTool.Core.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_TEST_CONSOLE
{
    class Program
    {
        public static async Task TestAsync(string siteURL, string token)
        {
            var profiles = await SynchAPI.FetchProfiles(siteURL, token);

            SettingsManager.Instance.Profiles = profiles;

            foreach (var item in profiles)
            {
                item.RootFolder = @"C:\DeploymentTesting\Local"; // for testing
            }

            SettingsManager.SaveConfig();

            var profile = SettingsManager.Instance.Profiles[0];
            var deployInitResult = await DeployAPI.Init(profile.ID);

            Console.WriteLine(deployInitResult.ToJSON());

            var diff = deployInitResult.FilesystemDifference;

            //start files uploading
            foreach (var fileDataModel in diff.CreatedFiles.Union(diff.ModifiedFiles))
            {
                //var filepath = Path.Combine(profile.RootFolder, fileDataModel.Filename); uncomment me
                var filepath = Path.Combine(profile.RootFolder, fileDataModel.Filename);

                var fileResponse = await FileAPI.Upload(profile.URL, profile.APIToken, deployInitResult.DeploySessionID, filepath, fileDataModel.Filename);
                Console.WriteLine(fileResponse);
            }

            //all files uploaded

            //starting deployment
            var deployResult = await DeployAPI.Deploy(deployInitResult.DeploySessionID);
            Console.WriteLine(deployResult);

            //rollback changes
            var rollbackResult = await DeployAPI.Rollback(deployInitResult.DeploySessionID);
            Console.WriteLine(rollbackResult);

        }

        static void Main()
        {
            TestAsync("http://localhost:2222", "f02d8f79-e99b-4e74-867f-5bfd52b93346").GetAwaiter().GetResult();


            //DeploySessionService.Deploy("1");

            /*ProfileBase profile = new ProfileBase()
            {
                ID = "1",
                RootFolder = @"C:\Test1"
            };
            var state = FilesystemStateModel.GetProfileFilesystemState(profile);*/

            //var diff = GetFilesystemDifference(state).GetAwaiter().GetResult();
            //var msg = Upload(@"C:\Users\super\Desktop\$this.Icon.ico").GetAwaiter().GetResult();
            //var msg = Upload(@"C:\Users\super\Desktop\$this.Icon.ico").GetAwaiter().GetResult();
            //RunAsync().GetAwaiter().GetResult();
        }
    }
}