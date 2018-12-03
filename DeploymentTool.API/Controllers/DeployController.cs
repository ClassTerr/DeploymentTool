using DeploymentTool.API.Models;
using DeploymentTool.Core.Helpers;
using DeploymentTool.Core.Models;
using DeploymentTool.Settings;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DeploymentTool.API.Controllers
{
    public class DeployController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Init()
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
                return Content("Profile not found");
            }

            var serverState = FilesystemStateModel.GetFullProfileFilesystemState(profile);
            var diff = FilesystemStateModel.GetFilesystemStateDiff(serverState, clientFilesystemState);

            DeployInitResult result = new DeployInitResult()
            {
                FilesystemDifference = diff,
                DeployID = "test"
            };

            return Json(result);
        }
    }
}
