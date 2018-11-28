using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Settings
{
    [Serializable]
    public class Settings
    {
        public List<ClientProfile> Profiles { get; set; } = new List<ClientProfile>();

        public ClientProfile GetProfile(string profileId)
        {
            return Profiles?.FirstOrDefault(profile => profile.ID == profileId);
        }

        public bool UpdateProfile(ClientProfile profile)
        {
            return UpdateProfile(profile, profile.ID);
        }

        public bool UpdateProfile(ClientProfile profile, string profileID)
        {
            var index = Profiles.FindIndex(x => x.ID == profileID);
            if (index != -1)
            {
                Profiles[index] = profile;
            }

            return index != -1;
        }

        public void RemoveProfile(ClientProfile profile)
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
