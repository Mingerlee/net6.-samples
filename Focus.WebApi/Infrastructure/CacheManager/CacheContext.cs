using Infrastructure.AutofacManager;
using Infrastructure.CacheManager.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.CacheManager
{
    public class CacheContext
    {
        static ICacheService _Cache;
        /// <summary>
        /// 获取缓存
        /// </summary>
        public static ICacheService Cache
        {
            get
            {
                if (_Cache != null) return _Cache;
                _Cache = AutofacContainerModule.GetService<ICacheService>();
                return _Cache;
            }
        }
    }
}
