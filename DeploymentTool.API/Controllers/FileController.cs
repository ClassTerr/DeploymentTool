using DeploymentTool.API.Helpers;
using DeploymentTool.API.Services;
using DeploymentTool.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DeploymentTool.API.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [TokenAuthorization]
        public ActionResult Test()
        {
            ResponseHelper.Response("1111!!!", 303);
            return Json("asd");
        }

        [HttpPost]
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

        [HttpPost]
        [TokenAuthorization]
        public ActionResult Upload()
        {
            // you can upload file only in case of deploy session opened
            string deploySessionId = HttpContext.Request.Headers["DeploySessionId"];
            string filepath = HttpContext.Request.Headers["Filepath"]; // relative

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

            if (string.IsNullOrWhiteSpace(filepath))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Filepath parameter is empty");
            }

            var profile = ProfileService.GetProfileById(deploySession.ProfileId);

            
            string filename = Path.Combine(profile.RootFolder, filepath);

            using (FileStream output = new FileStream(filename, FileMode.Create))
            {
                Request.InputStream.CopyTo(output);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        // POST: File/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
