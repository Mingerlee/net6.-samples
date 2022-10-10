using Dapper.Contrib.Extensions;
using Focus.Repository.DBContext;
using Focus.Repository.Models;
using Infrastructure.Config;

namespace Focus.WebApi.Extensions
{
    public static class DBEntityVerification
    {
        public static bool IsValid<T>(this T TEntity) where T : BaseEntity
        {
            var objType = typeof(T);
            var customAttributesData = objType.GetCustomAttributesData().Where(attr => attr.AttributeType == typeof(TableAttribute));
            if (!customAttributesData.Any()) { return true; }

            var tableName = customAttributesData.FirstOrDefault().ConstructorArguments[0].Value.ToString();

            var properties = objType.GetProperties().Where(p =>p.GetCustomAttributes(typeof(DBEntityVerificationAttribute), false).Any());

            string whereStr = "WHERE 1=1 ";
            foreach (var property in properties)
            {
                whereStr += $" AND {properties.First().Name}='{properties.First().GetValue(TEntity)}'  ";
            }
           
            string sqlStr = $"SELECT 1 FROM {tableName} {whereStr}";
            //var s = Convert.ToInt32(DbHelperFactory.Create().GetScalar(sqlStr));

            return Convert.ToInt32(DbHelperFactory.Create().GetScalar(sqlStr)) <= 0;
        }
    }
}
