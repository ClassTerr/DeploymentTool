using DeploymentTool.API.Settings;

namespace DeploymentTool.API.Services
{
    public static class ProfileService
    {
        public static ServerProfile GetProfileById(string id)
        {
            return SettingsManager.Instance.GetProfile(id);
        }
    }
}