using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MathSoftCommonLib;
using MathSoftModelLib;
using Business;
using DataAccess;
using DTOModel;
using System.IO;
using System.Data;

namespace yuhongxiadataServer.Areas.management.Controllers
{
    public class DrugController : Controller
    {
        public ActionResult Add(string DrugCommodityName,
            string DrugCommonName,
            string DrugEnglishName,
            List<Guid> DrugIngredientIds,
           string DrugMerchant,
           string DrugRemark1,
           string DrugRemark2,
           string DrugSpecification
            )
        {
           if(DrugIngredientIds == null)
            {
                DrugIngredientIds = new List<Guid>();
            }
            BLL_DrugTable bll_DrugTable = new BLL_DrugTable();
            Guid DrugId = Guid.NewGuid();
            UI_DrugTable modal = new UI_DrugTable
            {
                DrugCommodityName = DrugCommodityName,
                DrugCommonName = DrugCommonName,
                DrugEnglishName = DrugEnglishName,
                DrugId = DrugId,
                DrugIngredientIds = DrugIngredientIds,
                DrugMerchant = DrugMerchant,
                DrugRemark1 = DrugRemark1,
                DrugRemark2 = DrugRemark2,
                DrugSpecification = DrugSpecification,
                InsertTime = DateTimeHelper.GetTimeStamp(),
                UpdateTime = DateTimeHelper.GetTimeStamp(),
                UI_DrugIngredientRelationships = DrugIngredientIds.Select(item => new UI_DrugIngredientRelationship
                {
                    DrugId = DrugId,
                    IngredientID = item,
                    RelationshipId = Guid.NewGuid(),
                    InsertTime = DateTimeHelper.GetTimeStamp(),
                    LastUpdateTime = DateTimeHelper.GetTimeStamp(),

                }).ToList()
            };
            UIModelData<UI_DrugTable> ret = bll_DrugTable.Add(modal);
            return new JsonResult { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Search(int PageIndex, int PageSize, string DrugEnglishName, string DrugCommodityName, string DrugCommonName, string IngredientCNName)
        {
            UIRowsData<UI_DrugTable> ret = new  BLL_DrugTable().Search(PageIndex, PageSize,DrugEnglishName,DrugCommodityName,DrugCommonName,IngredientCNName);
            return new JsonResult { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Update(string DrugCommodityName,
          string DrugCommonName,
          string DrugEnglishName,
          List<Guid> DrugIngredientIds,
         string DrugMerchant,
         string DrugRemark1,
         string DrugRemark2,
         string DrugSpecification,
         Guid DrugId
          )
        {
            if (DrugIngredientIds == null)
            {
                DrugIngredientIds = new List<Guid>();
            }
            BLL_DrugTable bll_DrugTable = new BLL_DrugTable();
            UI_DrugTable modal = new UI_DrugTable
            {
                DrugCommodityName = DrugCommodityName,
                DrugCommonName = DrugCommonName,
                DrugEnglishName = DrugEnglishName,
                DrugId = DrugId,
                DrugIngredientIds = DrugIngredientIds,
                DrugMerchant = DrugMerchant,
                DrugRemark1 = DrugRemark1,
                DrugRemark2 = DrugRemark2,
                DrugSpecification = DrugSpecification,
                InsertTime = DateTimeHelper.GetTimeStamp(),
                UpdateTime = DateTimeHelper.GetTimeStamp(),
                UI_DrugIngredientRelationships = DrugIngredientIds.Select(item => new UI_DrugIngredientRelationship
                {
                    DrugId = DrugId,
                    IngredientID = item,
                    RelationshipId = Guid.NewGuid(),
                    InsertTime = DateTimeHelper.GetTimeStamp(),
                    LastUpdateTime = DateTimeHelper.GetTimeStamp(),

                }).ToList()
            };
            UIModelData<UI_DrugTable> ret = bll_DrugTable.Update(modal);
            return new JsonResult { Data = ret, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Delete(Guid id)
        {
            UIModelData<Guid> ret = new BLL_DrugTable().Delete(id);
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



                bool containers = DataTableHelper.contain(dt1, new string[] { "英文名称", "商品名", "药品通用名称", "规格", "持证商", "备注1", "备注2" });

                if (containers)
                {
                    List<UI_DrugTable> list = new List<UI_DrugTable>();
                    foreach (DataRow row in dt1.Rows)
                    {
                        if (string.IsNullOrWhiteSpace(row["药品通用名称"].ToString().Trim()))
                        {
                            continue;
                        }

                        UI_DrugTable model = new UI_DrugTable()
                        {

                            DrugId = Guid.NewGuid(),

                            DrugCommodityName = row["商品名"].ToString(),
                            DrugCommonName = row["药品通用名称"].ToString(),
                            DrugEnglishName = row["英文名称"].ToString(),

                            DrugMerchant = row["持证商"].ToString(),
                            DrugRemark1 = row["备注1"].ToString(),
                            DrugRemark2 = row["备注2"].ToString(),
                            DrugSpecification = row["规格"].ToString(),

                            InsertTime = DateTimeHelper.GetTimeStamp(),
                            UpdateTime = DateTimeHelper.GetTimeStamp(),
                        };

                        list.Add(model);
                    }


                    ret = new BLL_DrugTable().BatchAdd(list);
                    return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = ret };
                }
                else
                {


                    return Json(new UIModelData<string> { status = 7, suc = false, remark = "数据处理失败，必须包含 [英文名称],[商品名],[药品通用名称],[规格],[持证商],[备注1],[备注2]" });
                }



            }
            catch (Exception ex)
            {
                return Json(new UIModelData<string> { status = 7, suc = false, remark = ex.Message });
            }
        }
    }
}