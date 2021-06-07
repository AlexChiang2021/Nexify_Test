using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using Newtonsoft.Json;
using Nexify_Test.Libary;
using Nexify_Test.Repository;
using Nexify_Test.ViewModels.Platform;

namespace Nexify_Test.Models
{
    /// <summary>
    /// PlatformModel
    /// </summary>
    public class PlatformModel
    {
        private DataRepository repData = new DataRepository();

        #region 查詢
        /// <summary>取得資料</summary>
        /// <param name="strID"></param>
        /// <param name="strName"></param>
        /// <param name="p_intPageIndex"></param>
        /// <param name="p_intPageSize"></param>
        /// <returns></returns>
        public RServiceProvider GetDataList(string strID, string strName, int p_intPageIndex = 0, int p_intPageSize = 0)
        {
            RServiceProvider rsp = new RServiceProvider
            {
                Result = false
            };
            try
            {
                #region 檢查

                #endregion

                #region 資料處理
                List<DataViewModel> vmList = new List<DataViewModel>();
                vmList = repData.GetList(strID, strName, p_intPageIndex, p_intPageSize);
                rsp.ReturnData = vmList;
                rsp.Result = true;
                #endregion
            }
            catch (Exception ex)
            {
                rsp.ReturnMessage = ex.Message;
            }

            return rsp;
        }

        /// <summary>取得資料的數量</summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public RServiceProvider GetDataCount(string strName)
        {
            RServiceProvider rsp = new RServiceProvider
            {
                Result = false
            };
            try
            {
                #region 檢查

                #endregion

                #region 資料處理
                rsp.ReturnData = repData.GetCount(strName);
                rsp.Result = true;
                #endregion

            }
            catch (Exception ex)
            {
                rsp.ReturnMessage = ex.Message;
            }

            return rsp;
        }

        /// <summary>Save Data.</summary>
        /// <param name="p_vmData"></param>
        /// <returns></returns>
        public RServiceProvider SaveData(DataViewModel p_vmData)
        {
            RServiceProvider rsp = new RServiceProvider
            {
                Result = false
            };
            try
            {
                #region 檢查
                if (string.IsNullOrEmpty(p_vmData.Name))
                {
                    throw new Exception("名字 必填!");
                }
                if (p_vmData.Salary < 0 || p_vmData.Salary > 100000)
                {
                    throw new Exception("工資數值範圍 0~100000!");
                }
                if (!string.IsNullOrEmpty(p_vmData.DateOfBirth))
                {
                    DateTime d;
                    if (!DateTime.TryParseExact(p_vmData.DateOfBirth, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                    {
                        throw new Exception("日期格式錯誤!");
                    }
                }
                if (string.IsNullOrEmpty(p_vmData.Id))
                {
                    p_vmData.Id = "0";
                }
                #endregion

                #region 資料處理
                bool isInsert = repData.GetCount(p_vmData.Id) == 0;
                int intResult = 0;
                if (isInsert)
                {
                    intResult = repData.Insert(p_vmData);
                }
                else
                {
                    intResult = repData.Update(p_vmData);
                }

                if (intResult > 0)
                {
                    rsp.ReturnMessage = "儲存成功!";
                    rsp.Result = true;
                }
                else
                {
                    rsp.ReturnMessage = "儲存失敗!";
                }

                #endregion
            }
            catch (Exception ex)
            {
                rsp.ReturnMessage = ex.Message;
            }

            return rsp;
        }

        /// <summary>Delete Data.</summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public RServiceProvider DelData(string strId)
        {
            RServiceProvider rsp = new RServiceProvider
            {
                Result = false
            };
            try
            {
                #region 檢查
                if (string.IsNullOrEmpty(strId))
                {
                    return rsp;
                }
                #endregion

                #region 資料處理
                 int intResult = repData.Delete(strId);

                if (intResult > 0)
                {
                    rsp.ReturnMessage = "刪除成功!";
                    rsp.Result = true;
                }
                else
                {
                    rsp.ReturnMessage = "刪除失敗!";
                }

                #endregion
            }
            catch (Exception ex)
            {
                rsp.ReturnMessage = ex.Message;
            }

            return rsp;
        }

        /// <summary>
        /// 取得x分頁
        /// </summary>
        /// <param name="p_intPageSize">每頁顯示n筆</param>
        /// <param name="p_intDataCount">共m筆資料</param>
        /// <returns></returns>
        public static int GetPageCount(int p_intPageSize, int p_intDataCount)
        {
            int intPageCount = 1;
            intPageCount = p_intDataCount / p_intPageSize;

            if ((p_intDataCount % p_intPageSize) > 0)
                intPageCount++;

            return intPageCount;
        }
        #endregion
    }
}