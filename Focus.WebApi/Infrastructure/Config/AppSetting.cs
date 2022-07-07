using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrastructure.Config
{
    public class AppSetting
    {
        private static object objLock = new object();
        private static AppSetting instance = null;

        /// <summary>
        /// 
        /// </summary>
        private IConfigurationRoot Config { get; }

        /// <summary>
        /// 
        /// </summary>
        private AppSetting()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Config = builder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AppSetting GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new AppSetting();
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfig(string name)
        {
            return GetInstance().Config.GetSection(name).Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetConfigInt32(string name)
        {
            return Convert.ToInt32(GetInstance().Config.GetSection(name).Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool GetConfigBoolean(string name)
        {
            return Convert.ToBoolean(GetInstance().Config.GetSection(name).Value);
        }
        public static bool ConfigExist(string name)
        {
            try
            {
                var result = GetInstance().Config.GetSection(name).Value;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class Connection
    {
        public string DBType { get; set; }
        public string DbConnectionString { get; set; }
        public string RedisConnectionString { get; set; }
        public bool UseRedis { get; set; }
    }

    public class CreateMember : TableDefaultColumns
    {
    }
    public class ModifyMember : TableDefaultColumns
    {
    }

    public abstract class TableDefaultColumns
    {
        public string UserIdField { get; set; }
        public string UserNameField { get; set; }
        public string DateField { get; set; }
    }
}
