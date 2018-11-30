using System;

namespace DeploymentTool.Core.Models
{
    public class Token
    {
        public string Id { get; set; }
        public DateTime ExpirationDate { get; set; }

        public bool IsExpired => ExpirationDate < DateTime.Now;
    }
}