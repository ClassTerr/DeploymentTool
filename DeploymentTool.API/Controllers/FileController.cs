using DeploymentTool.API.Helpers;
using DeploymentTool.API.Services;
using DeploymentTool.API.Settings;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DeploymentTool.API.Controllers
{
    public class FileController : Controller
    {
        /// <summary>
        /// Downloads file from live site
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [TokenAuthorization]
        public ActionResult Download()
        {
            string profileId = HttpContext.Request.Headers["ProfileId"];
            string filepath = HttpContext.Request.Headers["Filepath"]; // relative

            var profile = ProfileService.GetProfileById(profileId);

            if (profile == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Profile not found");
            }

            if (filepath == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Target folder parameter is empty");
            }

            string absolutePath = Path.Combine(profile.RootFolder, filepath);
            string fileName = Path.GetFileName(absolutePath);

            if (!System.IO.File.Exists(absolutePath))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("File not found");
            }

            FileStream fs = new FileStream(absolutePath, FileMode.Open);
            return File(fs, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        /// <summary>
        /// Uploads file to deploy session preparation folder
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [TokenAuthorization]
        public ActionResult Upload()
        {
            // you can upload file only in case of deploy session opened
            string deploySessionId = HttpContext.Request.Headers["DeploySessionId"];

            var deploySession = DeploySessionService.GetDeploySession(deploySessionId);

            if (deploySession == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Content("Deploy session not found");
            }

            if (deploySession.IsExpired)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Content("Deploy session expired!");
            }

            var profile = ProfileService.GetProfileById(deploySession.ProfileId);

            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];

                string filePath = Path.Combine( SettingsManager.Instance.DeploySessionFolder,
                                                deploySession.GetDirectoryName(),
                                                file.FileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                file.SaveAs(filePath);
            }

            return Content("Saved " + Request.Files.Count + " file(s)");
        }
    }
}
