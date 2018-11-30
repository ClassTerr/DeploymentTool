using DeploymentTool.Core.Models;
using DeploymentTool.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeploymentTool.API.Services
{
    public static class TokenService
    {
        public static Token GetToken(string tokenId)
        {
            return SettingsManager.Instance.Tokens.FirstOrDefault(x => x.Id == tokenId);
        }
    }
}