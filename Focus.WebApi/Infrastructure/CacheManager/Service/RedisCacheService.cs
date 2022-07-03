using Infrastructure.CacheManager.IService;
using Infrastructure.Config;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.CacheManager.Service
{
    public class RedisCacheService : ICacheService
    {
        protected StackExchange.Redis.IDatabase _cache;

        private ConnectionMultiplexer _connection;

        private readonly string _instance;


        public RedisCacheService()
        {
            _connection = ConnectionMultiplexer.Connect(AppSetting.GetConfig("RedisConfig:ConnectionString"));
            _cache = _connection.GetDatabase(AppSetting.GetConfigInt32("RedisConfig:DdIndex"));
            _instance = AppSetting.GetConfig("RedisConfig:DefaultKey");
        }

        public string GetKeyForRedis(string key)
        {
            return _instance + key;
        }
        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.KeyExists(key);
        }

        public void ListLeftPush(string key, string val)
        {
            _cache.ListLeftPush(key, val);
        }

        public void ListRightPush(string key, string val)
        {
            _cache.ListRightPush(key, val);
        }


        public T ListDequeue<T>(string key) where T : class
        {
            RedisValue redisValue = _cache.ListRightPop(key);
            if (!redisValue.HasValue)
                return null;
            return JsonConvert.DeserializeObject<T>(redisValue);
        }
        public object ListDequeue(string key)
        {
            RedisValue redisValue = _cache.ListRightPop(key);
            if (!redisValue.HasValue)
                return null;
            return redisValue;
        }

        /// <summary>
        /// 移除list中的数据，keepIndex为保留的位置到最后一个元素如list 元素为1.2.3.....100
        /// 需要移除前3个数，keepindex应该为4
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keepIndex"></param>
        public void ListRemove(string key, int keepIndex)
        {
            _cache.ListTrim(key, keepIndex, -1);
        }

        public bool AddObject(string key, object value, TimeSpan? expiresIn = null, bool isSliding = false)
        {
            return _cache.StringSet(key, JsonConvert.SerializeObject(value), expiresIn);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间,Redis中无效）</param>
        /// <returns></returns>
        public bool Add(string key, string value, TimeSpan? expiresIn = null, bool isSliding = false)
        {
            return _cache.StringSet(key, value, expiresIn);
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.KeyDelete(key);
        }
        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="key">缓存Key集合</param>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            keys.ToList().ForEach(item => Remove(item));
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            var value = _cache.StringGet(key);

            if (!value.HasValue)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T StringGet<T>(string key)
        {
            var value = _cache.StringGet(key);

            if (!value.HasValue)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public string Get(string key)
        {
            return _cache.StringGet(key).ToString();
        }
        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            var dict = new Dictionary<string, object>();
            keys.ToList().ForEach(item => dict.Add(item, Get(item)));
            return dict;
        }

        ///  return JsonConvert.DeserializeObject(value);
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public bool Replace(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exists(key))
                if (!Remove(key))
                    return false;

            return AddObject(key, value);

        }
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exists(key))
                if (!Remove(key))
                    return false;
            if (value.GetType().Name == "String")
            {
                return Add(key, value.ToString(), expiresSliding);
            }
            return AddObject(key, value, expiresSliding);


        }
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exists(key))
                if (!Remove(key)) return false;
            if (value.GetType().Name == "String")
            {
                return Add(key, value.ToString());
            }
            return AddObject(key, value);

        }

        /// <summary>
        /// RedisCache 获取或创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public T Exec<T>(string key, Func<T> action, int expire = -1) where T : class
        {
            //1.0 从缓存中取KEY对应的数据，如果不存在则使用action回调获取，并缓存起来
            if (Exists(key))
            {
                return Get<T>(key);
            }
            else
            {
                T result = action();
                if (expire > 0) AddObject(key, result, TimeSpan.FromMinutes(expire));
                else
                    AddObject(key, result);
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回 -2   表示这个key已过期，已不存在返回 -1, 表示这个key没有设置有效期,返回0以上的值   表示是这个key的剩余有效时间</returns>
        public int GetKeyExpires(string key)
        {
            try
            {
                var redisResult = _cache.ScriptEvaluateAsync(LuaScript.Prepare(
                    " local res = redis.call('ttl', @keypattern) " +
                    " return res "), new { @keypattern = key }).Result;
                if (!redisResult.IsNull)
                {
                    return (int)redisResult;
                }
            }
            catch
            {
            }
            return -2;
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
