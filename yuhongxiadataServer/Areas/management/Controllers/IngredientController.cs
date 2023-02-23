using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DTOModel;
using Business;
using MathSoftCommonLib;
using MathSoftModelLib;
using System.IO;
using System.Data;

namespace yuhongxiadataServer.Areas.management.Controllers
{
    #region 用于辅料的增删查改
    /// <summary>
    /// 用于辅料的增删查改
    /// </summary>
    public class IngredientController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add(string IngredientAlias, string CPName, string USPName, string EPName, string JPName, string IngredientCNName, string IngredientShortName)
        {

            UI_IngredientTable data = new UI_IngredientTable
            {
                IngredientAlias = IngredientAlias,
                CPName = CPName,
                USPName = USPName,
                EPName = EPName,
                JPName = JPName,
                IngredientCNName = IngredientCNName,
                IngredientID = Guid.NewGuid(),
                IngredientShortName = IngredientShortName,
                InsertTime = DateTimeHelper.GetTimeStamp(),
                LastUpdateTime = DateTimeHelper.GetTimeStamp(),
            };
            UIModelData<UI_IngredientTable> ret = new BLL_IngredientTable().Add(data);
            return new JsonResult { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Search(string IngredientCNName,
          string IngredientShortName, int PageIndex, int PageSize)
        {
            UIRowsData<UI_IngredientTable> ret = new BLL_IngredientTable().Search(IngredientCNName, IngredientShortName, PageIndex, PageSize);
            return new JsonResult { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Update(Guid IngredientID, string IngredientAlias, string CPName, string USPName, string EPName, string JPName, string IngredientCNName, string IngredientShortName)
        {
            UI_IngredientTable data = new UI_IngredientTable()
            {
                IngredientAlias = IngredientAlias,
                CPName = CPName,
                USPName = USPName,
                EPName = EPName,
                JPName = JPName,
                IngredientCNName = IngredientCNName,
                IngredientID = IngredientID,
                IngredientShortName = IngredientShortName,
                LastUpdateTime = DateTimeHelper.GetTimeStamp(),
            };
            UIModelData<UI_IngredientTable> ret = new BLL_IngredientTable().Update(data);
            return new JsonResult { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Delete(Guid id)
        {
            UIModelData<Guid> ret = new BLL_IngredientTable().Delete(id);
            return new JsonResult { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult UpLoadExcel()
        {
            UISummeryData<string, string> ret = null;
            try
            {

                var file = Request.Files[0]; //获取选中文件  
                var filecombin = file.FileName.Split('.');
                if (file == null || String.IsNullOrEmpty(file.FileName) || file.ContentLength == 0 || filecombin.Length < 2)
                {
                    return Json(new
                    {
                        fileid = 0,
                        src = "",
                        name = "",
                        msg = "上传出错 请检查文件名 或 文件内容"
                    });
                }
                //定义本地路径位置
                string local = "Upload\\";
                string filePathName = string.Empty;
                string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, local);

                var tmpName = Server.MapPath("~/Upload/");
                //var tmp = file.FileName;
                var tmp = "Ingredient" + "." + file.FileName.Split(new char[] { '.' })[1];

                var tmpIndex = 0;
                //判断是否存在相同文件名的文件 相同累加1继续判断
                while (System.IO.File.Exists(tmpName + tmp))
                {
                    tmp = filecombin[0] + "_" + ++tmpIndex + "." + filecombin[1];
                }

                //不带路径的最终文件名
                filePathName = tmp;

                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);
                string localURL = Path.Combine(local, filePathName);
                string p1 = Path.Combine(localPath, filePathName);
                file.SaveAs(p1);   //保存图片（文件夹）
                DataTable dt1 = ExcelHelper.TransExcelToTable(p1, "Sheet1");
                bool containers = DataTableHelper.contain(dt1, new string[] { "中文名", "简称", "别名", "CP名称", "USP-NF名称", "EP名称", "JP/JPE名称" });

                if (containers)
                {
                    List<UI_IngredientTable> list = new List<UI_IngredientTable>();
                    foreach (DataRow row in dt1.Rows)
                    {
                        if (string.IsNullOrWhiteSpace(row["中文名"].ToString().Trim()))
                        {
                            continue;
                        }
                        UI_IngredientTable model = new UI_IngredientTable()
                        {

                            IngredientAlias = row["别名"].ToString(),
                            CPName = row["CP名称"].ToString(),
                            USPName = row["USP-NF名称"].ToString(),
                            EPName = row["EP名称"].ToString(),
                            JPName = row["JP/JPE名称"].ToString(),
                            IngredientCNName = row["中文名"].ToString(),
                            IngredientID = Guid.NewGuid(),
                            IngredientShortName = row["简称"].ToString(),
                            InsertTime = DateTimeHelper.GetTimeStamp(),
                            LastUpdateTime = DateTimeHelper.GetTimeStamp(),
                        };

                        list.Add(model);
                    }
                    ret = new BLL_IngredientTable().BatchAdd(list);
                    return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = ret };
                }
                else
                {
                    return Json(new UIModelData<string> { status = 7, suc = false, remark = "数据处理失败，必须包含 [中文名], [简称], [别名], [CP名称], [USP-NF名称] [EP名称][JP/JPE名称]" });
                }



            }
            catch (Exception ex)
            {
                return Json(new UIModelData<string> { status = 7, suc = false, remark = ex.Message });
            }
        }
    }
    #endregion
}