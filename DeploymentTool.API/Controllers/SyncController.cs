using DeploymentTool.API.Helpers;
using DeploymentTool.API.Settings;
using DeploymentTool.Core.Helpers;
using DeploymentTool.Core.Models;
using System.Linq;
using System.Web.Mvc;

namespace DeploymentTool.API.Controllers
{
    public class SyncController : Controller
    {
        [HttpGet]
        [TokenAuthorization]
        public ActionResult Profiles()
        {
            var allProfiles = SettingsManager.Instance.Profiles.Select(x => (ProfileModel)x).ToArray();

            return Content(allProfiles.ToJSON());
        }

        [HttpGet]
        [TokenAuthorization]
        public ActionResult Sessions()
        {
            var allProfiles = SettingsManager.Instance.DeploySessions;

            return Json(allProfiles);
        }
    }
}