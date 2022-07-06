using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config
{
    public class MailSettings
    {
        public string StmpHostServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int PortNumber { get; set; }
        public bool EnableSSL { get; set; }
        public bool RequireAuth { get; set; }
        public MailDeliveryOptions DeliveryOption { get; set; }
        public string SpecifiedPickUpDirectoryPath { get; set; }
        public NetworkCredential GetCredential => new NetworkCredential(Username, Password);
    }
}
