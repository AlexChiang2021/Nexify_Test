using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SQLite;
using Nexify_Test.Models;
using Nexify_Test.Libary;
using Nexify_Test.ViewModels.Platform;
using RestSharp.Serialization.Json;

namespace Nexify_Test.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>查詢資料</summary>
        /// <param name="strName"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [AjaxOnly]
        public ActionResult QueryDataList(string strName, int PageIndex, int PageSize)
        {
            PlatformModel moPlatform = new PlatformModel();
            RServiceProvider rsp = moPlatform.GetDataList(string.Empty, strName, PageIndex, PageSize);
            if (rsp.Result)
            {
                return PartialView("~/Views/Home/_DataList.cshtml", rsp.ReturnData as List<DataViewModel>);
            }
            else
            {
                return Content(new JsonSerializer().Serialize(rsp));
            }
        }
        /// <summary>查詢資料數量</summary>
        /// <param name="strName"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [AjaxOnly]
        public ActionResult QueryDataPaging(string strName, int PageSize)
        {
            PlatformModel moPlatform = new PlatformModel();
            RServiceProvider rsp = moPlatform.GetDataCount(strName);

            if (rsp.Result)
            {
                ViewBag.TotalCount = Convert.ToInt32(rsp.ReturnData);
                ViewBag.PageCount = PlatformModel.GetPageCount(PageSize, ViewBag.TotalCount);
                return PartialView("~/Views/Shared/_PagingList.cshtml");
            }
            else
            {
                return Content(new JsonSerializer().Serialize(rsp));
            }
        }

        /// <summary>新增/修改</summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SaveDataList(string Id)
        {
            RServiceProvider rsp = new RServiceProvider();
            DataViewModel vmData = new DataViewModel();
            if (!string.IsNullOrEmpty(Id))
            {
                PlatformModel moPlatform = new PlatformModel();
                rsp = moPlatform.GetDataList(Id, string.Empty);
            }

            if (rsp.Result)
            {
                vmData = (rsp.ReturnData as List<DataViewModel>)[0] as DataViewModel;
                return View(vmData);
            }
            else
                return View(new DataViewModel());
        }

        /// <summary>儲存 新增 / 修改</summary>
        /// <param name="p_vmData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDataList(DataViewModel p_vmData)
        {
            RServiceProvider rsp = new RServiceProvider();
            PlatformModel moPlatform = new PlatformModel();
            rsp = moPlatform.SaveData(p_vmData);
            return Content(new JsonSerializer().Serialize(rsp), "application/json");
        }

        /// <summary>刪除</summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        [AjaxOnly]
        public ActionResult DelData(string strId)
        {
            PlatformModel moPlatform = new PlatformModel();
            RServiceProvider rsp = moPlatform.DelData(strId);
            return Content(new JsonSerializer().Serialize(rsp), "application/json");
        }
    }
}
