using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeploymentTool.API.Controllers
{
    public class SendFileController : ApiController
    {
        // GET: api/SendFile
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/SendFile/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SendFile
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/SendFile/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SendFile/5
        public void Delete(int id)
        {
        }
    }
}
