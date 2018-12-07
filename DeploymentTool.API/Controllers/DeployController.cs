using DeploymentTool.API.Helpers;
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
            if (SettingsManager.Instance.IsDeployingNow)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                return Content("Another deploy session is uploading now. If you sure that it is an error, please stop deployment process.");
            }

            var clientFilesystemState = await ResponseHelper.GetInputDataAsync<FilesystemStateModel>(HttpContext);


            ServerProfile profile = SettingsManager.Instance.GetProfile(clientFilesystemState.ProfileID);
            if (profile == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Profile not found");
            }

            var serverState = FilesystemStateModel.GetProfileFilesystemState(profile);
            var diff = FilesystemStateModel.GetFilesystemStateDiff(serverState, clientFilesystemState);

            DeploySession session = new DeploySession()
            {
                ProfileName = profile.Name,
                ProfileId = profile.ID,
                FilesystemDifference = diff
            };

            SettingsManager.Instance.DeploySessions.Add(session);

            //TODO

            DeployInitResult result = new DeployInitResult()
            {
                FilesystemDifference = diff,
                DeployID = session.Id
            };

            return Json(result);
        }


        [HttpPost]
        public async Task<ActionResult> DoDeploy()
        {
            if (SettingsManager.Instance.IsDeployingNow)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                return Content("Another deploy session is uploading now. If you sure that it is an error, please stop deployment process.");
            }
            
            var clientFilesystemState = await ResponseHelper.GetInputDataAsync<FilesystemStateModel>(HttpContext);

            ServerProfile profile = SettingsManager.Instance.GetProfile(clientFilesystemState.ProfileID);
            if (profile == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Profile not found");
            }

            var serverState = FilesystemStateModel.GetProfileFilesystemState(profile);
            var diff = FilesystemStateModel.GetFilesystemStateDiff(serverState, clientFilesystemState);

            DeploySession session = new DeploySession()
            {
                ProfileName = profile.Name,
                ProfileId = profile.ID,
                FilesystemDifference = diff
            };

            SettingsManager.Instance.DeploySessions.Add(session);

            //TODO

            DeployInitResult result = new DeployInitResult()
            {
                FilesystemDifference = diff,
                DeployID = session.Id
            };

            return Json(result);
        }
    }
}
