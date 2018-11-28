﻿using DeploymentTool.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DeploymentTool.Settings
{
    [XmlRoot(ElementName = "Profile")]
    public class ClientProfile : ProfileBase
    {
        public string APICommand { get; set; }
        public string UpadateAPISSH { get; set; }

        public override string ToString()
        {
            return Name ?? "Profile";
        }
    }
}
