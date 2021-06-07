using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Nexify_Test.ViewModels.Platform;


namespace Nexify_Test.Repository
{
    /// <summary>
    /// DataRepository
    /// </summary>
    public class DataRepository
    {
        private Repository rep = new Repository();

        /// <summary>取得清單</summary>
        /// <param name="strID"></param>
        /// <param name="p_intPageIndex"></param>
        /// <param name="p_intPageSize"></param>
        public List<DataViewModel> GetList(string strID, string strName = "", int p_intPageIndex = 0, int p_intPageSize = 0)
        {
            List<DataViewModel> vmResult = null;

            try
            {
                #region 檢查

                #endregion

                DynamicParameters dynParameter = new DynamicParameters();
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("SELECT * FROM [Nexify] {0}", GetWhere(strID, strName, dynParameter));
                if (p_intPageIndex == 0 || p_intPageSize == 0)
                    vmResult = rep.Query<DataViewModel>(sb.ToString(), dynParameter);
                else
                    vmResult = rep.Query<DataViewModel>(sb.ToString(), dynParameter, "ID", true, p_intPageIndex, p_intPageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vmResult;
        }

        /// <summary>取得數量</summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public int GetCount(string strName)
        {
            int intCount = 0;

            try
            {
                #region 檢查


                #endregion

                DynamicParameters dynParameter = new DynamicParameters();
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT COUNT(1) FROM [Nexify] {0}", GetWhere(string.Empty, strName, dynParameter));

                object objResult = rep.ExecuteScalar(sb.ToString(), dynParameter);
                if (objResult != null)
                {
                    intCount = Convert.ToInt32(objResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intCount;
        }

        /// <summary>取得WHERE條件</summary>
        /// <param name="strID"></param>
        /// <param name="p_dynParameter"></param>
        /// <returns></returns>
        private string GetWhere(string strID, string strName, DynamicParameters p_dynParameter)
        {
            StringBuilder sbWhere = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(strID))
                {
                    sbWhere.Append("AND Id = @strID ");
                    p_dynParameter.Add("@strID", strID, DbType.String);
                }
                if (!string.IsNullOrEmpty(strName))
                {
                    sbWhere.Append("AND INSTR(Name, @strName) > 0");
                    p_dynParameter.Add("@strName", strName, DbType.String);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (sbWhere.Length > 0 ? string.Format("WHERE {0}", sbWhere.ToString().TrimStart("AND".ToCharArray())) : string.Empty);
        }

        /// <summary>新增資料</summary>
        /// <param name="p_vmData"></param>
        /// <returns></returns>
        public int Insert(DataViewModel p_vmData)
        {
            int inResult = 0;

            try
            {
                DynamicParameters dynParameter = new DynamicParameters();

                StringBuilder sbSql = new StringBuilder();
                sbSql.Append(@"INSERT INTO [Nexify] ([Name], [DateOfBirth], [Salary], [Address]) 
                                VALUES (@Name, @DateOfBirth, @Salary, @Address)");

                dynParameter.Add("@Name", p_vmData.Name, DbType.String, ParameterDirection.Input, 30);
                dynParameter.Add("@DateOfBirth", !string.IsNullOrEmpty(p_vmData.DateOfBirth) ? p_vmData.DateOfBirth : string.Empty, DbType.String, ParameterDirection.Input);
                dynParameter.Add("@Salary", p_vmData.Salary, DbType.Int32);
                dynParameter.Add("@Address", !string.IsNullOrEmpty(p_vmData.Address) ? p_vmData.Address : string.Empty, DbType.String, ParameterDirection.Input);

                inResult = rep.Insert(sbSql.ToString(), dynParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return inResult;
        }

        /// <summary>更新資料</summary>
        /// <param name="p_vmData">The P_VM Data.</param>
        /// <returns></returns>
        public int Update(DataViewModel p_vmData)
        {
            int inResult = 0;

            try
            {
                #region 檢查

                #endregion

                StringBuilder sbUpdate = new StringBuilder();
                DynamicParameters dynParameter = new DynamicParameters();

                dynParameter.Add("@Id", p_vmData.Id, DbType.String);

                sbUpdate.Append("[Name] = @Name ");
                dynParameter.Add("@Name", p_vmData.Name, DbType.String, ParameterDirection.Input, 30);

                sbUpdate.Append(", [Salary] = @Salary ");
                dynParameter.Add("@Salary", p_vmData.Salary, DbType.Int32);

                if (p_vmData.DateOfBirth != null)
                {
                    sbUpdate.Append(", [DateOfBirth] = @DateOfBirth ");
                    dynParameter.Add("@DateOfBirth", !string.IsNullOrEmpty(p_vmData.DateOfBirth) ? p_vmData.DateOfBirth : string.Empty, DbType.String, ParameterDirection.Input);
                }
                if (p_vmData.Address != null)
                {
                    sbUpdate.Append(", [Address] = @Address ");
                    dynParameter.Add("@Address", !string.IsNullOrEmpty(p_vmData.Address) ? p_vmData.Address : string.Empty, DbType.String, ParameterDirection.Input);
                }

                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat("UPDATE [Nexify] SET {0} WHERE Id = @Id ", sbUpdate.ToString());
                inResult = rep.Update(sbSql.ToString(), dynParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return inResult;
        }

        /// <summary>刪除資料</summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public int Delete(string strId)
        {
            int inResult = 0;

            try
            {
                #region 檢查

                #endregion

                DynamicParameters dynParameter = new DynamicParameters();
                dynParameter.Add("@Id", strId, DbType.String);

                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("DELETE FROM [Nexify] WHERE Id = @Id ");
                inResult = rep.Delete(sbSql.ToString(), dynParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return inResult;
        }
    }

}
