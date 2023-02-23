using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using MathSoftModelLib;
using DTOModel;
using AutoMapper;
using System.Data.Entity.Infrastructure;

namespace Business
{
    public class BLL_DrugTable : BaseMathRoleAuthorEntities
    {
        #region  局部变量
        private DbSet<DrugTable> contextItem = null;

        private DbSet<DrugIngredientRelationship> subContextItem = null;
        #endregion

        #region  构造函数相关

        public BLL_DrugTable() : base()
        {
            contextItem = yuhongxiadataEntities.DrugTable;
            subContextItem = yuhongxiadataEntities.DrugIngredientRelationship;
        }
        #endregion

        #region  查询信息
        public UIRowsData<UI_DrugTable> Search(int PageIndex, int PageSize, string DrugEnglishName, string DrugCommodityName, string DrugCommonName, string IngredientCNName)
        {
            UIRowsData<UI_DrugTable> retData = null;
            try
            {

                //分页
                string sqland1 = PageIndex > 0 ? string.Format("and t2.number between {0} and {1}", (PageIndex - 1) * PageSize + 1, PageIndex * PageSize) : "";

                //DrugEnglishName
                string sqland2 = string.IsNullOrWhiteSpace(DrugEnglishName) ? "" : string.Format(" and DrugEnglishName like '%{0}%'", DrugEnglishName);

                //DrugCommodityName
                string sqland3 = string.IsNullOrWhiteSpace(DrugCommodityName) ? "" : string.Format(" and DrugCommodityName like '%{0}%'", DrugCommodityName);

                //DrugCommonName
                string sqland4 = string.IsNullOrWhiteSpace(DrugCommonName) ? "" : string.Format(" and DrugCommonName like '%{0}%'", DrugCommonName);

                //[IngredientCNName]
                string sqland5 = string.IsNullOrWhiteSpace(IngredientCNName) ? "" : string.Format(@"and 
   exists (select 1 from [DrugIngredientRelationship]  join [IngredientTable] on IngredientTable.IngredientID=DrugIngredientRelationship.IngredientID
   where t1.DrugId=DrugIngredientRelationship.DrugId and IngredientTable.IngredientCNName like '%{0}%')", IngredientCNName);

                string sql2 = string.Format(@"
select 
 [DrugId]
      ,[DrugEnglishName]
      ,[DrugCommodityName]
      ,[DrugCommonName]
      ,[DrugSpecification]
      ,[DrugMerchant]
      ,[DrugRemark1]
      ,[DrugRemark2]
      ,[InsertTime]
      ,[UpdateTime]
	  ,[number]
	   from (
SELECT  *,
	   ROW_NUMBER() over(order by InsertTime desc ) as number
  FROM  [DrugTable] t1 where  1=1   
   {1}
  ) t2
  where 1=1 {0}", sqland1, string.Join(" ", new string[] { sqland2, sqland3, sqland4, sqland5 }));


         
                List<UI_DrugTable> ret1 = yuhongxiadataEntities
                    .Database.SqlQuery<UI_DrugTable>(sql2, new List<object>().ToArray()).ToList();


                string sqlcounter = string.Format(@"select  count(*) from  [DrugTable] t1 where  1=1    {0}",
                  string.Join(" ", new string[] { sqland2, sqland3, sqland4, sqland5 }));
                List<int> result2 =yuhongxiadataEntities.Database.SqlQuery<int>(sqlcounter, new List<object>().ToArray()).ToList();

                if (ret1.Count > 0)
                {

                    IEnumerable<string> drugids = ret1.Select(item => string.Format("'{0}'", item.DrugId.ToString()));
                    string sqland6 = string.Format(" and DrugId in ({0})", string.Join(" , ", drugids));

                    string sql3 = string.Format(@"SELECT  IngredientTable.IngredientID, IngredientTable.IngredientCNName, IngredientTable.IngredientShortName, 
                   IngredientTable.CPName, IngredientTable.USPName, IngredientTable.EPName, IngredientTable.JPName, 
                   IngredientTable.IngredientAlias, IngredientTable.InsertTime, IngredientTable.LastUpdateTime, 
                   DrugIngredientRelationship.RelationshipId, DrugIngredientRelationship.DrugId
FROM      DrugIngredientRelationship INNER JOIN
                   IngredientTable ON DrugIngredientRelationship.IngredientID = IngredientTable.IngredientID
where 1=1   {0}
", sqland6);


                    List<UI_IngredientTable> ret2 = yuhongxiadataEntities
                       .Database.SqlQuery<UI_IngredientTable>(sql3, new List<object>().ToArray()).ToList();

                    ret1.ForEach(item =>
                    {
                        List<UI_IngredientTable> list = ret2.Where(subitem => subitem.DrugId == item.DrugId).ToList();
                        item.DrugIngredients = list;
                        item.DrugIngredientIds = list.Select(newitem => newitem.IngredientID).ToList();
                    });
                }

             


                retData = new UIRowsData<UI_DrugTable> { remark = string.Empty, rows = ret1, status = 6, total = result2.FirstOrDefault(), suc = true };
            }
            catch (Exception ex)
            {
                retData = new UIRowsData<UI_DrugTable> { remark = ex.Message, rows = new List<UI_DrugTable> { }, status = 7, total = 0, suc = false };
            }
            finally
            {

            }

            return retData;
        }
        #endregion

        #region 信息录入
        /// <summary>
        /// 信息录入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public UIModelData<UI_DrugTable> Add(UI_DrugTable model)
        {
            UIModelData<UI_DrugTable> uIModelData = new UIModelData<UI_DrugTable> { };
            try
            {
                int total = GetTotal(i => i.DrugCommonName == model.DrugCommonName);
                if (total > 0)
                {
                    uIModelData.status = 7;
                    uIModelData.suc = false;
                    uIModelData.remark = string.Format("药品通用名称重复，录入失败");
                }
                else
                {
                    DrugTable insertModal = Mapper.Map<DrugTable>(model);
                    contextItem.Add(insertModal);
                    

                    List<DrugIngredientRelationship> list = new List<DrugIngredientRelationship>();
                    model.UI_DrugIngredientRelationships.ForEach(item =>
                    {
                        DrugIngredientRelationship Relationship = Mapper.Map<DrugIngredientRelationship>(item);
                        list.Add(Relationship);
                    });
                    subContextItem.AddRange(list);

                    yuhongxiadataEntities.SaveChanges();


                    uIModelData.status = 6;
                    uIModelData.suc = true;
                    uIModelData.Data = model;
                }
            }
            catch (Exception ex)
            {
                uIModelData = new UIModelData<UI_DrugTable> { remark = ex.Message, suc = false, status = 7 };
            }

            finally
            {
            }


            return uIModelData;
        }
        #endregion

        #region 获取数量用
        public int GetTotal(Expression<Func<DrugTable, bool>> where = null)
        {
            Expression<Func<DrugTable, bool>> exp1 = where == null ? item => true : where;
            var total = contextItem.Count(exp1);
            return total;
        }
        #endregion

        #region 信息录入
        /// <summary>
        /// 信息录入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public UIModelData<UI_DrugTable> Update(UI_DrugTable model)
        {
            UIModelData<UI_DrugTable> uIModelData = new UIModelData<UI_DrugTable> { };
            try
            {
                int total = GetTotal(i => i.DrugCommonName == model.DrugCommonName && i.DrugId != model.DrugId);
                if (total > 0)
                {
                    uIModelData.status = 7;
                    uIModelData.suc = false;
                    uIModelData.remark = string.Format("药品通用名称重复，录入失败");
                }
                else
                {
                    DrugTable insertModal = Mapper.Map<DrugTable>(model);
                    //更新药品信息
                    DrugTable searchModal = contextItem.FirstOrDefault(i => i.DrugId == model.DrugId);
                    searchModal.DrugCommodityName = model.DrugCommodityName;
                    searchModal.DrugCommonName = model.DrugCommonName;
                    searchModal.DrugEnglishName = model.DrugEnglishName;
                    searchModal.DrugMerchant = model.DrugMerchant;
                    searchModal.DrugRemark1 = model.DrugRemark1;
                    searchModal.DrugRemark2 = model.DrugRemark2;
                    searchModal.DrugSpecification = model.DrugSpecification;
                    searchModal.UpdateTime = model.UpdateTime;
                    yuhongxiadataEntities.SaveChanges();
                    //更新药品信息结束

                    //删除药品和辅料的关联信息
                    
                    IEnumerable<DrugIngredientRelationship> deleteList = subContextItem.Where(i => i.DrugId== model.DrugId);
                    subContextItem.RemoveRange(deleteList);
                    yuhongxiadataEntities.SaveChanges();
                    //删除药品和辅料的关联信息结束


                    //建立关联关系
                    List<DrugIngredientRelationship> list = new List<DrugIngredientRelationship>();
                    model.UI_DrugIngredientRelationships.ForEach(item =>
                    {
                        DrugIngredientRelationship Relationship = Mapper.Map<DrugIngredientRelationship>(item);
                        list.Add(Relationship);
                    });
                    subContextItem.AddRange(list);
                    //建立关联关系
                    yuhongxiadataEntities.SaveChanges();




                    uIModelData.status = 6;
                    uIModelData.suc = true;
                    uIModelData.Data = model;
                }
            }
            catch (Exception ex)
            {
                uIModelData = new UIModelData<UI_DrugTable> { remark = ex.Message, suc = false, status = 7 };
            }

            finally
            {
            }


            return uIModelData;
        }
        #endregion


        #region  删除一条记录
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UIModelData<Guid> Delete(Guid id)
        {

            UIModelData<Guid> modal = null;
            try
            {

                DrugTable model = contextItem.FirstOrDefault(item => item.DrugId == id);
                if (model != null)
                {
                    contextItem.Remove(model);
                    yuhongxiadataEntities.SaveChanges();
                }

                modal = new UIModelData<Guid>() { remark = string.Empty, suc = true, status = 6, Data = id };
            }
            catch (Exception ex)
            {
                modal = new UIModelData<Guid>() { remark = ex.Message, status = 7, suc = false, Data = id };
            }
            finally { }

            return modal;
        }
        #endregion


        #region 批量信息录入
        /// <summary>
        /// 信息录入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public UISummeryData<string, string> BatchAdd(List<UI_DrugTable> models)
        {
            UISummeryData<string, string> uIModelData = new UISummeryData<string, string>() { status = 6, suc = true };
            try
            {
                models.ForEach(model =>
                {
                    int total = GetTotal(i => i.DrugCommonName == model.DrugCommonName);
                    if (total > 0)
                    {
                        uIModelData.FailData.Add(model.DrugCommonName);
                        uIModelData.FailCount++;
                    }
                    else
                    {
                        DrugTable insertModal = Mapper.Map<DrugTable>(model);
                        contextItem.Add(insertModal);
                        yuhongxiadataEntities.SaveChanges();
                        uIModelData.SuccessCount++;
                    }
                });
            }
            catch (Exception ex)
            {
                uIModelData = new UISummeryData<string, string> { remark = ex.Message, suc = false, status = 7 };
            }

            finally
            {
            }


            return uIModelData;
        }
        #endregion
    }
}
