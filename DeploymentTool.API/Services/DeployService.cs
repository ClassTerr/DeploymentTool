using DeploymentTool.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeploymentTool.API.Services
{
    public static class ProfileService
    {
        public static ServerProfile GetDeploySession(string id)
        {
            return SettingsManager.Instance.GetProfile(id);
        }
    }
}