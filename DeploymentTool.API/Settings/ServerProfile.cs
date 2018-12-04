﻿using DeploymentTool.Core.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeploymentTool.Settings
{
    [XmlRoot(ElementName = "Profile")]
    public class ServerProfile : ProfileBase
    {
        public string APICommand { get; set; }
        public string UpadateAPISSH { get; set; }
        public List<OperationResult> Backups { get; set; }

        public override string ToString()
        {
            return Name ?? "Profile";
        }
    }
}
