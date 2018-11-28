using System;
using System.Collections.Generic;
using System.Linq;

namespace DeploymentTool.Settings
{
    [Serializable]
    public class Settings
    {
        public List<ServerProfile> Profiles { get; set; } = new List<ServerProfile>();

        public ServerProfile GetProfile(string profileId)
        {
            return Profiles?.FirstOrDefault(profile => profile.ID == profileId);
        }

        public bool UpdateProfile(ServerProfile profile)
        {
            return UpdateProfile(profile, profile.ID);
        }

        public bool UpdateProfile(ServerProfile profile, string profileID)
        {
            var index = Profiles.FindIndex(x => x.ID == profileID);
            if (index != -1)
            {
                Profiles[index] = profile;
            }

            return index != -1;
        }

        public void RemoveProfile(ServerProfile profile)
        {
            RemoveProfile(profile.ID);
        }

        public void RemoveProfile(string Id)
        {
            var index = Profiles.FindIndex(x => x.ID == Id);
            Profiles.RemoveAt(index);
        }
    }
}
