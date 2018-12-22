using DeploymentTool.API.Settings;
using DeploymentTool.Core.Models;
using System.Linq;

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