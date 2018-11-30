using DeploymentTool.Core.Models;
using DeploymentTool.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeploymentTool.API.Controllers
{
    public class FilesystemStateController : ApiController
    {
        // GET: api/FilesystemState
        public FilesystemDifference Get([FromBody]FilesystemStateModel clientFilesystemState)
        {
            ServerProfile profile = SettingsManager.Instance.GetProfile(clientFilesystemState.ProfileID);
            if (profile == null)
            {
                ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest));
                return null;
            }

            var serverState = FilesystemStateModel.GetFullProfileFilesystemState(profile);
            var diff = FilesystemStateModel.GetFilesystemStateDiff(serverState, clientFilesystemState);

            return diff;
        }

        // GET: api/FilesystemState/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/FilesystemState
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/FilesystemState/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FilesystemState/5
        public void Delete(int id)
        {
        }
    }
}
