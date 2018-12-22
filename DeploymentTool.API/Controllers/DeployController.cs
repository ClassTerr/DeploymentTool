using DeploymentTool.API.Helpers;
using DeploymentTool.API.Models;
using DeploymentTool.API.Services;
using DeploymentTool.API.Settings;
using DeploymentTool.Core.Models;
using System;
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

            if (clientFilesystemState == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Can't parse filesystem state");
            }


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
            SettingsManager.SaveConfig();

            DeployInitResult result = new DeployInitResult()
            {
                FilesystemDifference = diff,
                DeploySessionID = session.Id,
                Expires = session.Expires
            };

            return Json(result);
        }


        [HttpPost]
        public async Task<ActionResult> Deploy()
        {
            if (SettingsManager.Instance.IsDeployingNow)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                return Content("Another deploy session is uploading now. If you sure that it is an error, please stop deployment process.");
            }
            
            var sessionId = await ResponseHelper.GetInputDataAsync<string>(HttpContext);
            var session = DeploySessionService.GetDeploySession(sessionId);
            
            if (session.IsClosed)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                return Content("Session is closed");
            }

            try
            {
                DeploySessionService.Deploy(sessionId);
                return Json("Deployed!");
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Rollback()
        {
            if (SettingsManager.Instance.IsDeployingNow)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                return Content("Another deploy session is uploading now. If you sure that it is an error, please stop deployment process.");
            }
            
            var sessionId = await ResponseHelper.GetInputDataAsync<string>(HttpContext);
            var session = DeploySessionService.GetDeploySession(sessionId);

            try
            {
                BackupService.Rollback(session);
                return Json($"Session { sessionId } reverted!");
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(ex.Message);
            }
        }
    }
}
