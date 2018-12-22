using DeploymentTool.API.Settings;
using DeploymentTool.Core.Helpers;
using DeploymentTool.Core.Models;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DeploymentTool.API.Controllers
{
    public class FilesystemStateController : Controller
    {
        [HttpPost]
        public async Task<string> GetFilesystemDifference()
        {
            string requestData;
            using (var reader = new StreamReader(HttpContext.Request.InputStream))
            {
                requestData = await reader.ReadToEndAsync();
            }

            var clientFilesystemState = requestData.ToObject<FilesystemStateModel>();

            ServerProfile profile = SettingsManager.Instance.GetProfile(clientFilesystemState.ProfileID);
            if (profile == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return "Profile not found";
            }

            var serverState = FilesystemStateModel.GetProfileFilesystemState(profile);
            var diff = FilesystemStateModel.GetFilesystemStateDiff(serverState, clientFilesystemState);
            
            return diff.ToJSON();
        }
    }
}
