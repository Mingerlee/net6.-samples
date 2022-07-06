using Infrastructure.Config;
using Infrastructure.Enums;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities
{
    public class MailHelper
    {
        public MailSettings Settings { get; set; }

        public MailHelper()
        {
            Settings = new MailSettings
            {
                StmpHostServer = AppSetting.GetConfig("MailSetting:StmpHostServer"),
                Username= AppSetting.GetConfig("MailSetting:Username"),
                Password= AppSetting.GetConfig("MailSetting:Password"),
                PortNumber = AppSetting.GetConfigInt32("MailSetting:PortNumber"),
                EnableSSL= AppSetting.GetConfigBoolean("MailSetting:EnableSSL"),
                RequireAuth= AppSetting.GetConfigBoolean("MailSetting:RequireAuth"),
                DeliveryOption = (MailDeliveryOptions)AppSetting.GetConfigInt32("MailSetting:DeliveryOption"),
                SpecifiedPickUpDirectoryPath= AppSetting.GetConfig("MailSetting:StmpHostServer")
            };
        }

        public MailHelper(MailSettings s)
        {
            Settings = s;
        }
        private SmtpClient GetSmtpClient()
        {
            SmtpClient smtpClient = new SmtpClient();
            switch (Settings.DeliveryOption)
            {
                case MailDeliveryOptions.UseNetworkSmtp:
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Host = Settings.StmpHostServer;
                    smtpClient.Port = Settings.PortNumber;
                    if (Settings.RequireAuth)
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = Settings.GetCredential;
                    }

                    smtpClient.EnableSsl = Settings.EnableSSL;
                    break;
                case MailDeliveryOptions.UseIISPickUpDirectory:
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                    break;
                case MailDeliveryOptions.SpecifiedPickUpDirectory:
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = Settings.SpecifiedPickUpDirectoryPath;
                    break;
                default:
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Host = Settings.StmpHostServer;
                    smtpClient.Port = Settings.PortNumber;
                    if (Settings.RequireAuth)
                    {
                        smtpClient.Credentials = Settings.GetCredential;
                    }

                    smtpClient.EnableSsl = Settings.EnableSSL;
                    break;
            }

            return smtpClient;
        }
        public static SmtpClient GetSmtpClient(MailSettings Settings)
        {
            SmtpClient smtpClient = new SmtpClient();
            switch (Settings.DeliveryOption)
            {
                case MailDeliveryOptions.UseNetworkSmtp:
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Host = Settings.StmpHostServer;
                    smtpClient.Port = Settings.PortNumber;
                    if (Settings.RequireAuth)
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = Settings.GetCredential;
                    }

                    smtpClient.EnableSsl = Settings.EnableSSL;
                    break;
                case MailDeliveryOptions.UseIISPickUpDirectory:
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                    break;
                case MailDeliveryOptions.SpecifiedPickUpDirectory:
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = Settings.SpecifiedPickUpDirectoryPath;
                    break;
                default:
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Host = Settings.StmpHostServer;
                    smtpClient.Port = Settings.PortNumber;
                    if (Settings.RequireAuth)
                    {
                        smtpClient.Credentials = Settings.GetCredential;
                    }

                    smtpClient.EnableSsl = Settings.EnableSSL;
                    break;
            }

            return smtpClient;
        }
        public static string[] GetAddressArray(string Addresses)
        {
            var AdjustedString = Addresses.Replace(',', ';');
            var addresses = AdjustedString.Split(';');
            return addresses;
        }
        public  ResultMail SendMail(MailMessage Mail)
        {
            if (Settings == null)
            {
                return new ResultMail("Settings not initialised. Please provide mail settings before send.");
            }
            SmtpClient smtpClient = GetSmtpClient();
            try
            {
                smtpClient.Send(Mail);
            }
            catch (Exception ex)
            {
                return new ResultMail(ex.Message);
            }
            smtpClient.Dispose();
            smtpClient = null;
            return new ResultMail();
        }
        public  async Task <ResultMail> SendMailAsync(MailMessage Mail)
        {
            if (Settings == null)
            {
                return new ResultMail("Settings not initialised. Please provide mail settings before send.");
            }
            bool flag = false;
            SmtpClient smtpClient = GetSmtpClient();
            try
            {
               await  smtpClient.SendMailAsync(Mail);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            smtpClient.Dispose();
            smtpClient = null;
            return new ResultMail();
        }
    }
}
