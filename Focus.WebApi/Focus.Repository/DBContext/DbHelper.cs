using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Focus.Repository.Models;
using System.Text.RegularExpressions;
using System.Reflection;
using Infrastructure.Enums;
using Infrastructure.Config;
using Dapper.Contrib.Extensions;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System.Data.SqlClient;
using Infrastructur.AutofacManager;

namespace Focus.Repository.DBContext
{
    public class DbHelper : IDisposable
    {
        private static IDbConnection _sharedConn;
        private DBType _dbType = DBType.SqlServer;
        private DbProviderFactory _factory;
        private string _sqlPage =string.Empty;
        private string _sqlCount = string.Empty;
        private string _paramPrefix = "@";

        private static Regex rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex rxGroupBy = new Regex(@"\bGROUP\s+BY\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        public DbHelper()
        {
            DbHelperFactory.ConnectionString = AppSetting.GetConfig("ConnectionStrings:FocusOADev");

            if (string.IsNullOrEmpty(DbHelperFactory.ConnectionString))
            {
                throw new InvalidOperationException("未能找到指定名称的连接字符串！");
            }

            _sharedConn = DbHelperFactory.DbConnection;

            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            string providerName = "System.Data.SqlClient";

            _factory = DbProviderFactories.GetFactory(providerName);
            //_sharedConn = _factory.CreateConnection();

            if (_sharedConn.State == ConnectionState.Closed)
            {
                _sharedConn.Open();
            }
        }

        /// <summary>
        /// 添加Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public long Insert<T>(T model) where T : class
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return SqlMapperExtensions.Insert(_sharedConn, model, _tran);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        public async Task<long> InsertAsync<T>(T model) where T : BaseEntity
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return await SqlMapperExtensions.InsertAsync(_sharedConn, model, _tran);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }

        /// <summary>
        /// 删除Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete<T>(T model) where T : class
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return SqlMapperExtensions.Delete(_sharedConn, model, _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        public async Task<bool> DeleteAsync<T>(T model) where T : BaseEntity
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return await SqlMapperExtensions.DeleteAsync(_sharedConn, model, _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }

        /// <summary>
        /// 修改Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update<T>(T model) where T : class
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return SqlMapperExtensions.Update(_sharedConn, model, _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        public async Task<bool> UpdateAsync<T>(T model) where T : BaseEntity
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return await SqlMapperExtensions.UpdateAsync(_sharedConn, model, _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public int Execute(string sql, params object[] args)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return Dapper.SqlMapper.Execute(_sharedConn, sql, ConvertParam(args), _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        public async Task<int> ExecuteAsync(string sql, params object[] args)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return await Dapper.SqlMapper.ExecuteAsync(_sharedConn, sql, ConvertParam(args), _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        /// <summary>
        /// 执行批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public int ExecuteInsertList<T>(string sql, List<T> models)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return Dapper.SqlMapper.Execute(_sharedConn, sql, models, _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        public async Task<int> ExecuteInsertListAsync<T>(string sql, List<T> models)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return await Dapper.SqlMapper.ExecuteAsync(_sharedConn, sql, models, _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int Execute(string sql, Dapper.DynamicParameters parameters)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return Dapper.SqlMapper.Execute(_sharedConn, sql, parameters, _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        public async Task<int> ExecuteAsync(string sql, Dapper.DynamicParameters parameters)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return await Dapper.SqlMapper.ExecuteAsync(_sharedConn, sql, parameters, _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        /// <summary>
        /// 查询Scalar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return Dapper.SqlMapper.ExecuteScalar<T>(_sharedConn, sql, ConvertParam(args), _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        public async Task<T> ExecuteScalarAsync<T>(string sql, params object[] args)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return await Dapper.SqlMapper.ExecuteScalarAsync<T>(_sharedConn, sql, ConvertParam(args), _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        /// <summary>
        /// 查询Scalar
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object GetScalar(string sql, params object[] args)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return Dapper.SqlMapper.ExecuteScalar(_sharedConn, sql, ConvertParam(args), _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        /// <summary>
        /// 查询Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T QueryModel<T>(string sql, params object[] args) where T : class
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return Dapper.SqlMapper.QuerySingleOrDefault<T>(_sharedConn, sql, ConvertParam(args), _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        public async Task<T> QueryModelAsync<T>(string sql, params object[] args) where T : BaseEntity
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }

                return await Dapper.SqlMapper.QueryFirstOrDefaultAsync<T>(_sharedConn, sql, ConvertParam(args), _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        /// <summary>
        /// 根据id查询Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="idValue">id列为主键</param>
        /// <returns></returns>
        public T QueryById<T>(string idValue) where T : class
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return SqlMapperExtensions.Get<T>(_sharedConn, idValue, _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }

        /// <summary>
        /// 根据id查询Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="idValue">id列为主键</param>
        /// <returns></returns>
        public T QueryById<T>(long idValue) where T : class
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return SqlMapperExtensions.Get<T>(_sharedConn, idValue, _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }

        /// <summary>
        /// 查询返回表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public DataTable QueryTable(string sql, params object[] args)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }

                DataTable dt = new DataTable();
                DbDataAdapter adapter = _factory.CreateDataAdapter();
                IDbCommand cmd = CreateCommand(CommandType.Text, sql, args);
                adapter.SelectCommand = (DbCommand)cmd;
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }

        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryList<T>(string sql, params object[] args)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return Dapper.SqlMapper.Query<T>(_sharedConn, sql, ConvertParam(args), _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }
        public async Task<IEnumerable<T>> QueryListAsync<T>(string sql, params object[] args)
        {
            try
            {
                if (_sharedConn.State == ConnectionState.Closed)
                {
                    _sharedConn.Open();
                }
                return await Dapper.SqlMapper.QueryAsync<T>(_sharedConn, sql, ConvertParam(args), _tran);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_tran == null)
                {
                    _sharedConn.Close();
                }
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="currentPage"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public ListPage<T> QueryList<T>(string sql, int itemsPerPage = 20, int currentPage = 1, params object[] args) where T : class
        {
            //当前sql查询结果条数
            ListPage<T> page = new ListPage<T>() { CurrentPage = currentPage, PerPageItems = itemsPerPage };
            Type t = typeof(T);
            string orderCol = "Id";
            foreach (PropertyInfo p in t.GetProperties())
            {
                object[] attributeKey = p.GetCustomAttributes(typeof(KeyAttribute), false);
                object[] attributesExplicitKey = p.GetCustomAttributes(typeof(ExplicitKeyAttribute), false);
                if (attributeKey.Length > 0)
                {
                    orderCol = p.Name;
                    break;
                }
                if (attributesExplicitKey.Length > 0)
                {
                    orderCol = p.Name;
                    break;
                }
            }
            BuilderPageSql(sql, itemsPerPage, currentPage, orderCol);
            if (itemsPerPage == 0)
            {
                page.data = QueryList<T>(_sqlPage, args).ToList<T>();
                page.TotalItems = page.data.Count;
            }
            else
            {
                int itemcount = ExecuteScalar<int>(_sqlCount, args);
                page.TotalItems = itemcount;
                page.TotalPages = ((page.TotalItems - 1) / itemsPerPage) + 1;
                if (itemcount == 0)
                    page.data = new List<T>();
                else
                    page.data = QueryList<T>(_sqlPage, args).ToList<T>();
            }

            return page;
        }
        public async Task<ListPage<T>> QueryListAsync<T>(string sql, int itemsPerPage = 20, int currentPage = 1, params object[] args) where T : class
        {
            //当前sql查询结果条数
            ListPage<T> page = new ListPage<T>() { CurrentPage = currentPage, PerPageItems = itemsPerPage };
            Type t = typeof(T);
            string orderCol = "Id";
            foreach (PropertyInfo p in t.GetProperties())
            {
                object[] attributeKey = p.GetCustomAttributes(typeof(KeyAttribute), false);
                object[] attributesExplicitKey = p.GetCustomAttributes(typeof(ExplicitKeyAttribute), false);
                if (attributeKey.Length > 0)
                {
                    orderCol = p.Name;
                    break;
                }
                if (attributesExplicitKey.Length > 0)
                {
                    orderCol = p.Name;
                    break;
                }
            }
            BuilderPageSql(sql, itemsPerPage, currentPage, orderCol);
            if (itemsPerPage == 0)
            {
                page.data = (await QueryListAsync<T>(_sqlPage, args)).ToList<T>();
                page.TotalItems = page.data.Count;
            }
            else
            {
                int itemcount = await ExecuteScalarAsync<int>(_sqlCount, args);
                page.TotalItems = itemcount;
                page.TotalPages = ((page.TotalItems - 1) / itemsPerPage) + 1;
                if (itemcount == 0)
                    page.data = new List<T>();
                else
                    page.data = (await QueryListAsync<T>(_sqlPage, args)).ToList<T>();
            }

            return page;
        }
        public async Task<ListPage<T>> QueryListAsync<T>(string sql, QueryModel sc, params object[] args) where T : class
        {
            int currentPage = sc.CurrentPageIndex>0? sc.CurrentPageIndex:1;
            int itemsPerPage = sc.PageRows > 0 ? sc.PageRows:20;
            QueryCondition qc = QueryParam.GetQueryCondition(sc);
            sql += qc.WhereString;
            //当前sql查询结果条数
            ListPage<T> page = new ListPage<T>() { CurrentPage = currentPage, PerPageItems = itemsPerPage };
            Type t = typeof(T);
            string orderCol = "Id";
            foreach (PropertyInfo p in t.GetProperties())
            {
                object[] attributeKey = p.GetCustomAttributes(typeof(KeyAttribute), false);
                object[] attributesExplicitKey = p.GetCustomAttributes(typeof(ExplicitKeyAttribute), false);
                if (attributeKey.Length > 0)
                {
                    orderCol = p.Name;
                    break;
                }
                if (attributesExplicitKey.Length > 0)
                {
                    orderCol = p.Name;
                    break;
                }
            }
            BuilderPageSql(sql, itemsPerPage, currentPage, orderCol);
            if (itemsPerPage == 0)
            {
                page.data = (await QueryListAsync<T>(_sqlPage, args)).ToList<T>();
                page.TotalItems = page.data.Count;
            }
            else
            {
                int itemcount = await ExecuteScalarAsync<int>(_sqlCount, args);
                page.TotalItems = itemcount;
                page.TotalPages = ((page.TotalItems - 1) / itemsPerPage) + 1;
                if (itemcount == 0)
                    page.data = new List<T>();
                else
                    page.data = (await QueryListAsync<T>(_sqlPage, args)).ToList<T>();
            }

            return page;
        }
        /// <summary>
        /// 分页查询(汇总部分列)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="currentPage"></param>
        /// <param name="sumCols"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public ListPage<T> QueryList<T>(string sql, int itemsPerPage = 20, int currentPage = 1, List<string> sumCols = null, params object[] args) where T : class
        {
            ListPage<T> listPage = QueryList<T>(sql, itemsPerPage, currentPage, args);
            if (sumCols != null && sumCols.Count > 0)
            {
                string strCols = string.Empty;
                foreach (string col in sumCols)
                {
                    strCols += $"ISNULL(SUM({col}),0) {col},";
                }
                if (sql.ToUpper().Contains("ORDER BY"))
                {
                    sql = sql.Substring(0, sql.ToUpper().IndexOf("ORDER BY"));
                }
                string strNewSql = $"SELECT {strCols.Trim(',')} FROM ({sql}) T";
                DataTable dt = QueryTable(strNewSql);
                if (dt.Rows.Count > 0)
                {
                    List<ColumnSum> columnSums = new List<ColumnSum>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        columnSums.Add(new ColumnSum()
                        {
                            ColName = column.ColumnName,
                            SumValue = Convert.ToDecimal(dt.Rows[0][column.ColumnName])
                        });
                    }
                    listPage.Sums = columnSums;
                }
            }
            return listPage;
        }

        public async Task<ListPage<T>> QueryListAsync<T>(string sql, int itemsPerPage = 20, int currentPage = 1, List<string> sumCols = null, params object[] args) where T : class
        {
            ListPage<T> listPage = await QueryListAsync<T>(sql, itemsPerPage, currentPage, args);
            if (sumCols != null && sumCols.Count > 0)
            {
                string strCols = string.Empty;
                foreach (string col in sumCols)
                {
                    strCols += $"ISNULL(SUM({col}),0) {col},";
                }
                if (sql.ToUpper().Contains("ORDER BY"))
                {
                    sql = sql.Substring(0, sql.ToUpper().IndexOf("ORDER BY"));
                }
                string strNewSql = $"SELECT {strCols.Trim(',')} FROM ({sql}) T";
                DataTable dt = QueryTable(strNewSql);
                if (dt.Rows.Count > 0)
                {
                    List<ColumnSum> columnSums = new List<ColumnSum>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        columnSums.Add(new ColumnSum()
                        {
                            ColName = column.ColumnName,
                            SumValue = Convert.ToDecimal(dt.Rows[0][column.ColumnName])
                        });
                    }
                    listPage.Sums = columnSums;
                }
            }
            return listPage;
        }

        #region 事务处理
        IDbTransaction _tran = null;
        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            _tran = _sharedConn.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            if (_tran != null)
            {
                _tran.Commit();
                this.DisposeTransaction();
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            if (_tran != null)
            {
                _tran.Rollback();
                this.DisposeTransaction();
            }
        }
        /// <summary>
        /// 释放事务资源
        /// </summary>
        private void DisposeTransaction()
        {
            if (_tran != null)
            {
                _tran.Dispose();
                _tran = null;
                _sharedConn.Close();
            }
        }
        #endregion

        /// <summary>
        /// 创建用于分页查询的sql语句
        /// </summary>
        private void BuilderPageSql(string _sql, int _pageSize, int _currentPage, string orderCol, bool noCount = false)
        {
            string sqlSelectRemoved, sqlOrderBy;
            if (!SplitSqlForPaging(_sql, out sqlSelectRemoved, out sqlOrderBy))
                throw new Exception("分页查询时不能解析用于查询的SQL语句");
            //sqlSelectRemoved = rxOrderBy.Replace(sqlSelectRemoved, "");
            if (_pageSize != 0)
            {
                if (rxDistinct.IsMatch(sqlSelectRemoved) && _dbType != DBType.MySql)
                {
                    sqlSelectRemoved = "jlg_inner.* FROM (SELECT " + sqlSelectRemoved + ") jlg_inner";
                }
                if (_dbType == DBType.SqlServer)
                {
                    //_sqlPage = string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) rn, {1}) jlg_paged WHERE rn> {2} AND rn<= {3}", sqlOrderBy == null ? "ORDER BY (SELECT NULL)" : sqlOrderBy, sqlSelectRemoved, (_currentPage - 1) * _pageSize, noCount ? (_currentPage * _pageSize) + 1 : _currentPage * _pageSize);
                    _sqlPage = string.Format("SELECT {0} {1} OFFSET {2} ROW FETCH NEXT {3} ROW ONLY", sqlSelectRemoved, sqlOrderBy == null ? " ORDER BY (SELECT " + orderCol + ") DESC " : sqlOrderBy, (_currentPage - 1) * _pageSize, _pageSize);
                }
                else if (_dbType == DBType.Oracle)
                {
                    //cyding
                    //_sqlPage = string.Format("select * from (select rownum rn,pages.* from (select {1} {0})pages where rownum<={3})hz_paged where rn > {2}", sqlOrderBy == null ? "ORDER BY  NULL" : sqlOrderBy, sqlSelectRemoved, (_currentPage - 1) * _pageSize, noCount ? (_currentPage * _pageSize) + 1 : _currentPage * _pageSize);
                    _sqlPage = string.Format("select * from (select rownum rn,pages.* from (select {1} {0})pages )jlg_paged where rn > {2} and rn <={3}", "", sqlSelectRemoved, (_currentPage - 1) * _pageSize, noCount ? (_currentPage * _pageSize) + 1 : _currentPage * _pageSize);
                }    //  sqlOrderBy == null ? "ORDER BY  NULL" : sqlOrderBy, —>{0} 位置参数
                else if (_dbType == DBType.MySql)
                {
                    _sqlPage = string.Format("SELECT {0} {1} LIMIT {2},{3}", sqlSelectRemoved, "", (_currentPage - 1) * _pageSize, _pageSize + 1);
                } //sqlOrderBy  —>{1} 位置参数
            }
            else
            {
                _sqlPage = string.Format("SELECT {0} {1}", sqlSelectRemoved, ""); // sqlOrderBy —>{1} 位置参数
            }

        }

        /// <summary>
        /// 为分页查询对sql语句进行解析分割
        /// </summary>
        /// <param name="_sql"></param>
        /// <param name="sqlSelectRemoved"></param>
        /// <param name="sqlOrderBy"></param>
        /// <returns></returns>
        private bool SplitSqlForPaging(string _sql, out string sqlSelectRemoved, out string sqlOrderBy)
        {
            sqlSelectRemoved = null;
            sqlOrderBy = null;

            var m = rxOrderBy.Match(_sql);
            Group g;
            if (!m.Success)
            {
                sqlOrderBy = null;
            }
            else
            {
                g = m.Groups[0];
                sqlOrderBy = g.ToString();
                _sql = _sql.Substring(0, g.Index) + _sql.Substring(g.Index + g.Length);
            }

            m = rxColumns.Match(_sql);
            if (!m.Success)
                return false;

            g = m.Groups[1];
            sqlSelectRemoved = _sql.Substring(g.Index);

            if (rxDistinct.IsMatch(sqlSelectRemoved))
            {
                _sqlCount = string.Format("select count(*) from ({0} {1} {2})A", _sql.Substring(0, g.Index), m.Groups[1].ToString().Trim(), _sql.Substring(g.Index + g.Length));
            }
            else if (rxGroupBy.IsMatch(sqlSelectRemoved))
            {
                _sqlCount = string.Format("select count(*) from ({0} {1} {2})A", _sql.Substring(0, g.Index), m.Groups[1].ToString().Trim(), _sql.Substring(g.Index + g.Length));

            }
            else
            {
                _sqlCount = _sql.Substring(0, g.Index) + "COUNT(*) " + _sql.Substring(g.Index + g.Length);
            }
            return true;
        }

        /// <summary>
        /// 将object参数转化为DynamicParameters
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private Dapper.DynamicParameters ConvertParam(params object[] args)
        {
            Dapper.DynamicParameters parameters = null;
            if (args != null && args.Length > 0)
            {
                parameters = new Dapper.DynamicParameters();
                foreach (object obj in args)
                {
                    parameters.AddDynamicParams(obj);
                }
            }
            return parameters;
        }

        public void Dispose()
        {
            if (_tran != null)
            {
                _tran.Dispose();
                _tran = null;
            }

            if (_sharedConn != null)
            {
                _sharedConn.Close();
                _sharedConn.Dispose();
                _sharedConn = null;
            }
        }

        /// <summary>
        /// 构建IDbCommand
        /// </summary>
        /// <param name="commType"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private IDbCommand CreateCommand(CommandType commType, string sql, params object[] args)
        {
            if (_sharedConn.State != ConnectionState.Open)
            {
                _sharedConn.Open();
            }
            // 创建command并且加载参数
            IDbCommand cmd = _sharedConn.CreateCommand();
            cmd.Connection = _sharedConn;
            cmd.CommandText = sql;
            if (_tran != null)
            {
                cmd.Transaction = _tran;
            }
            cmd.CommandType = commType;

            //正则，\w匹配字母或数字或下划线或汉字
            Regex rxParams = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);
            foreach (object param in args)
            {
                if (param is IDbDataParameter)
                {
                    cmd.Parameters.Add(param as IDbDataParameter);
                }
                else
                {
                    var properties = param.GetType().GetProperties();
                    foreach (var prop in properties)
                    {
                        IDbDataParameter p = cmd.CreateParameter();
                        p.ParameterName = _paramPrefix + prop.Name;
                        p.Value = prop.GetValue(param);
                        cmd.Parameters.Add(p);
                    }
                }
            }
            return cmd;
        }
    }
}
