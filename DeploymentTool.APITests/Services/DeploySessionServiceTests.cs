using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeploymentTool.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.API.Services.Tests
{
    [TestClass()]
    public class DeploySessionServiceTests
    {
        [TestMethod()]
        public void DoDeployTest()
        {
            DeploySessionService.Deploy("1");
            Assert.Fail();
        }
    }
}