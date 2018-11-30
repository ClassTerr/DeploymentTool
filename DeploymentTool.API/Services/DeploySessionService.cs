using DeploymentTool.Core.Models;
using DeploymentTool.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeploymentTool.API.Services
{
    public static class DeploySessionService
    {
        public static DeploySession GetDeploySession(string id)
        {
            return SettingsManager.Instance.DeploySessions.FirstOrDefault(x => x.Id == id);
        }
    }
}