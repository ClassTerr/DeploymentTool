using DeploymentTool.API.Models;
using DeploymentTool.Core.Models;
using DeploymentTool.Core.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeploymentTool.API.Controllers
{
    public class DeployController : ApiController
    {
        // GET: api/Deploy
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public DeployInitResult Init(FilesystemStateModel clientFilesystemState)
        {
            Profile profile = SettingsManager.Instance.GetProfile(clientFilesystemState.ProfileID);
            if (profile == null)
            {
                ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest));
                return null;
            }

            var serverState = FilesystemStateModel.GetFullProfileFilesystemState(profile);
            var diff = FilesystemStateModel.GetFilesystemStateDiff(serverState, clientFilesystemState);

            DeployInitResult result = new DeployInitResult()
            {
                FilesystemDifference = diff,
                DeployID = "test"
            };

            return result;
        }

            // GET: api/Deploy/5
            public string Get(int id)
        {
            return "value";
        }

        // POST: api/Deploy
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Deploy/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Deploy/5
        public void Delete(int id)
        {
        }
    }
}
