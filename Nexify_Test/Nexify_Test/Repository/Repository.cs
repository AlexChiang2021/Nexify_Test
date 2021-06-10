using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Data.SQLite;

namespace Nexify_Test.Repository
{
    /// <summary>
    /// 底層
    /// </summary>
    /// <remarks></remarks>
    public class Repository
    {
        /// <summary>取得連線(讀取相對路徑)</summary>
        public static readonly string g_strConn = ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory); 

        #region 查詢 - 回傳單筆
        /// <summary>取得資料單一筆</summary>
        /// <typeparam name="T">預計回傳的型態</typeparam>
        /// <param name="p_strSql">SQL語法(ex: select * from [Data] where...)</param>
        /// <param name="p_objParameter">查詢的參數</param>
        /// <returns></returns>
        /// <exception cref="Exception">請設定排序欄位!</exception>
        public T QuerySingle<T>(string p_strSql, object p_objParameter)
        {
            T varResult;
            try
            {
                using (var conn = new SQLiteConnection(g_strConn))
                {
                    conn.Open();

                    varResult = conn.QuerySingle<T>(p_strSql, p_objParameter);

                    conn.Close();
                }
            }
            catch
            {
                varResult = default(T);
            }

            return varResult;
        }
        #endregion

        #region 查詢 - 回傳多筆
        /// <summary>取得資料</summary>
        /// <typeparam name="T">預計回傳的型態</typeparam>
        /// <param name="p_strSql">SQL語法(ex: select * from [Data] where...)</param>
        /// <param name="p_objParameter">The p_obj parameter.</param>
        /// <returns></returns>
        public List<T> Query<T>(string p_strSql, object p_objParameter)
        {
            return Query<T>(p_strSql, p_objParameter, string.Empty, false, 0, 0);
        }
        /// <summary>取得資料</summary>
        /// <typeparam name="T">預計回傳的型態</typeparam>
        /// <param name="p_strSql">SQL語法(ex: select * from [Data] where...)</param>
        /// <param name="p_objParameter">The p_obj parameter.</param>
        /// <param name="p_strOrderBy">排序語法</param>
        /// <returns></returns>
        public List<T> Query<T>(string p_strSql, object p_objParameter, string p_strOrderBy)
        {
            return Query<T>(p_strSql, p_objParameter, p_strOrderBy, false, 0, 0);
        }
        /// <summary>取得資料</summary>
        /// <typeparam name="T">預計回傳的型態</typeparam>
        /// <param name="p_strSql">SQL語法(ex: select * from [Data] where...)</param>
        /// <param name="p_objParameter"></param>
        /// <param name="p_strOrderBy">排序語法</param>
        /// <param name="p_isPagination">是否要分頁</param>
        /// <param name="p_intPageIndex">顯示第n頁的資料</param>
        /// <param name="p_intPageSize">每一頁m筆資料</param>
        /// <returns></returns>
        /// <exception cref="Exception">請設定排序欄位!</exception>
        public List<T> Query<T>(string p_strSql, object p_objParameter, string p_strOrderBy, bool p_isPagination, int p_intPageIndex, int p_intPageSize)
        {
            List<T> objList = new List<T>();
            try
            {
                using (var conn = new SQLiteConnection(g_strConn))
                {
                    StringBuilder sb = new StringBuilder();
                    conn.Open();

                    #region 分頁語法
                    if (p_isPagination)
                    {
                        //檢查
                        if (string.IsNullOrEmpty(p_strOrderBy))
                        {
                            throw new Exception("請設定排序欄位!");
                        }


                        int intStartIndex = ((p_intPageIndex - 1) * p_intPageSize) + 1;
                        int intEndIndex = (p_intPageIndex * p_intPageSize);
                        sb.Append(" SELECT * FROM ( ")
                             .AppendFormat("  SELECT ROW_NUMBER() OVER (ORDER BY {0}) AS RowNum, * FROM ({1}) temp ", p_strOrderBy, p_strSql)
                             .Append(") AS tbNew")
                             .AppendFormat(" WHERE (RowNum >= {0}) AND (RowNum <= {1})", intStartIndex, intEndIndex);
                        if(!string.IsNullOrEmpty(p_strOrderBy))
                        {
                            sb.AppendFormat(" ORDER BY {0}", p_strOrderBy);
                        }

                        p_strSql = sb.ToString();
                    }
                    #endregion

                    #region 排序語法
                    if (!p_isPagination && !string.IsNullOrEmpty(p_strOrderBy))
                    {
                        p_strSql = string.Format("{0} ORDER BY {1}", p_strSql, p_strOrderBy);
                    }
                    #endregion

                    objList = conn.Query<T>(p_strSql, p_objParameter).AsList();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objList;
        }
        #endregion

        #region 查詢 - 回傳第一列的第一欄資料
        /// <summary>取得第一列的第一欄資料</summary>
        /// <param name="p_strSql">The P_STR SQL.</param>
        /// <returns></returns>
        public object ExecuteScalar(string p_strSql)
        {
            object objResult = null;
            try
            {
                using (var conn = new SQLiteConnection(g_strConn))
                {
                    conn.Open();

                    objResult = conn.ExecuteScalar(p_strSql);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objResult;
        }
        /// <summary>取得第一列的第一欄資料</summary>
        /// <param name="p_strSql">The P_STR SQL.</param>
        /// <param name="p_objParameter">The p_obj parameter.</param>
        /// <returns></returns>
        public object ExecuteScalar(string p_strSql, object p_objParameter)
        {
            object objResult = null;
            try
            {
                using (var conn = new SQLiteConnection(g_strConn))
                {
                    conn.Open();

                    objResult = conn.ExecuteScalar(p_strSql, p_objParameter);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objResult;
        }
        #endregion

        #region 新增
        /// <summary>新增資料</summary>
        /// <param name="p_strSql">SQL語法</param>
        /// <param name="p_objParameter"></param>
        /// <returns></returns>
        public int Insert(string p_strSql, object p_objParameter)
        {
            int intResult = 0;
            try
            {
                using (var conn = new SQLiteConnection(g_strConn))
                {
                    conn.Open();

                    intResult = conn.Execute(p_strSql, p_objParameter);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return intResult;
        }

        /// <summary>新增資料</summary>
        /// <param name="p_strSql">SQL語法</param>
        /// <param name="p_objParameter">The p_obj parameter.</param>
        /// <param name="p_sqlConn">The P_SQL connection.</param>
        /// <param name="p_sqlTran">The P_SQL tran.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">不為新增語法!</exception>
        public int Insert(string p_strSql, object p_objParameter, SQLiteConnection p_sqlConn, SqlTransaction p_sqlTran)
        {
            int intResult = 0;
            try
            {

                if (p_strSql.ToLower().IndexOf("insert into") == -1)
                {
                    throw new Exception("不為新增語法!");
                }

                intResult = p_sqlConn.Execute(p_strSql, p_objParameter, transaction: p_sqlTran);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return intResult;
        }
        #endregion

        #region 修改
        /// <summary>修改資料</summary>
        /// <param name="p_strSql"></param>
        /// <param name="p_objParameter"></param>
        /// <returns></returns>
        /// <exception cref="Exception">請設定排序欄位!</exception>
        public int Update(string p_strSql, object p_objParameter)
        {
            int intResult = 0;
            try
            {
                using (var conn = new SQLiteConnection(g_strConn))
                {
                    conn.Open();

                    intResult = conn.Execute(p_strSql, p_objParameter);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return intResult;
        }
        #endregion

        #region 刪除
        /// <summary>刪除資料</summary>
        /// <param name="p_strSql"></param>
        /// <param name="p_objParameter"></param>
        /// <returns></returns>
        /// <exception cref="Exception">請設定排序欄位!</exception>
        public int Delete(string p_strSql, object p_objParameter)
        {
            int intResult = 0;
            try
            {

                if (p_strSql.ToLower().IndexOf("delete") == -1)
                {
                    throw new Exception("不為刪除語法!");
                }
                using (var conn = new SQLiteConnection(g_strConn))
                {

                    conn.Open();

                    intResult = conn.Execute(p_strSql, p_objParameter);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return intResult;
        }

        /// <summary>刪除資料</summary>
        /// <param name="p_strSql"></param>
        /// <param name="p_objParameter"></param>
        /// <param name="p_sqlConn"></param>
        /// <param name="p_sqlTran"></param>
        /// <returns></returns>
        /// <exception cref="Exception">請設定排序欄位!</exception>
        public int Delete(string p_strSql, object p_objParameter, SQLiteConnection p_sqlConn, SqlTransaction p_sqlTran)
        {
            int intResult = 0;
            try
            {

                if (p_strSql.ToLower().IndexOf("delete") == -1)
                {
                    throw new Exception("不為刪除語法!");
                }

                intResult = p_sqlConn.Execute(p_strSql, p_objParameter, transaction: p_sqlTran);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return intResult;
        }
        #endregion
    }
}
