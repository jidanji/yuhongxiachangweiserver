using DataAccess;
using MathSoftModelLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTOModel;
using System.Linq.Expressions;
using AutoMapper;

using MathSoftCommonLib;


namespace Business
{
    public class BLL_IngredientTable : BaseMathRoleAuthorEntities
    {
        #region  构造函数相关
        private DbSet<IngredientTable> contextItem = null;
        public BLL_IngredientTable() : base()
        {
            contextItem = yuhongxiadataEntities.IngredientTable;
        }
        #endregion

        #region 信息更改

        /// <summary>
        /// 信息更改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public UIModelData<UI_IngredientTable> Update(UI_IngredientTable model)
        {
            UIModelData<UI_IngredientTable> uIModelData = null;
            try
            {
                uIModelData = new UIModelData<UI_IngredientTable> { };
                int total = GetTotal(i => i.IngredientCNName == model.IngredientCNName && i.IngredientID != model.IngredientID);
                if (total > 0)
                {
                    uIModelData.status = 7;
                    uIModelData.suc = false;
                    uIModelData.remark = string.Format("辅料中文名称重复，录入失败");
                }
                else
                {
                    IngredientTable updateModal = Mapper.Map<IngredientTable>(model);
                    IngredientTable searchModal = contextItem.FirstOrDefault(i => i.IngredientID == updateModal.IngredientID);
                    searchModal.IngredientAlias = updateModal.IngredientAlias;

                    searchModal.CPName = updateModal.CPName;
                    searchModal.EPName = updateModal.EPName;
                    searchModal.JPName = updateModal.JPName;
                    searchModal.USPName = updateModal.USPName;


                    searchModal.IngredientCNName = updateModal.IngredientCNName;
                    searchModal.IngredientShortName = updateModal.IngredientShortName;
                    searchModal.LastUpdateTime = updateModal.LastUpdateTime;
                    yuhongxiadataEntities.SaveChanges();
                    uIModelData.status = 6;
                    uIModelData.suc = true;
                    uIModelData.Data = model;
                }
            }
            catch (Exception ex)
            {
                uIModelData = new UIModelData<UI_IngredientTable> { status = 7, suc = false, remark = ex.Message };


            }

            return uIModelData;
        }
        #endregion

        #region 信息录入
        /// <summary>
        /// 信息录入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public UIModelData<UI_IngredientTable> Add(UI_IngredientTable model)
        {
            UIModelData<UI_IngredientTable> uIModelData = new UIModelData<UI_IngredientTable> { };
            try
            {
                int total = GetTotal(i => i.IngredientCNName == model.IngredientCNName);
                if (total > 0)
                {
                    uIModelData.status = 7;
                    uIModelData.suc = false;
                    uIModelData.remark = string.Format("辅料中文名称重复，录入失败");
                }
                else
                {
                    IngredientTable insertModal = Mapper.Map<IngredientTable>(model);
                    contextItem.Add(insertModal);
                    yuhongxiadataEntities.SaveChanges();
                    uIModelData.status = 6;
                    uIModelData.suc = true;
                    uIModelData.Data = model;
                }
            }
            catch (Exception ex)
            {
                uIModelData = new UIModelData<UI_IngredientTable> { remark = ex.Message, suc = false, status = 7 };
            }

            finally
            {
            }


            return uIModelData;
        }
        #endregion

        #region 批量信息录入
        /// <summary>
        /// 信息录入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public UISummeryData<string, string> BatchAdd(List<UI_IngredientTable> models)
        {
            UISummeryData<string, string> uIModelData = new UISummeryData<string, string>() { status = 6, suc = true };
            try
            {
                models.ForEach(model =>
                {
                    int total = GetTotal(i => i.IngredientCNName == model.IngredientCNName);
                    if (total > 0)
                    {
                        uIModelData.FailData.Add(model.IngredientCNName);
                        uIModelData.FailCount++;
                    }
                    else
                    {
                        IngredientTable insertModal = Mapper.Map<IngredientTable>(model);
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

        #region 获取数量用
        public int GetTotal(Expression<Func<IngredientTable, bool>> where = null)
        {
            Expression<Func<IngredientTable, bool>> exp1 = where == null ? item => true : where;
            var total = contextItem.Count(exp1);
            return total;
        }
        #endregion

        #region  查询方法

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="IngredientCNName"></param>
        /// <param name="IngredientShortName"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public UIRowsData<UI_IngredientTable> Search(string IngredientCNName,
          string IngredientShortName, int PageIndex, int PageSize)
        {
            UIRowsData<UI_IngredientTable> retData = null;
            int total = 0;
            try
            {
                Expression<Func<IngredientTable, bool>> where1 = string.IsNullOrWhiteSpace(IngredientCNName) ? i => true : (Expression<Func<IngredientTable, bool>>)(i => i.IngredientCNName.Contains(IngredientCNName));
                Expression<Func<IngredientTable, bool>> where2 = string.IsNullOrWhiteSpace(IngredientShortName) ? i => true : (Expression<Func<IngredientTable, bool>>)(i => i.IngredientShortName.Contains(IngredientShortName));
                total = GetTotal(where1.And(where1));
                IQueryable<IngredientTable> lazyList = contextItem.Where(where1.And(where2)).OrderByDescending(item => item.InsertTime);
                List<IngredientTable> ret = PageIndex >= 0 ? lazyList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList() : lazyList.ToList();
                List<UI_IngredientTable> listUI = new List<UI_IngredientTable>();
                foreach (IngredientTable item in ret)
                {
                    UI_IngredientTable uimodal = Mapper.Map<UI_IngredientTable>(item);
                    listUI.Add(uimodal);
                }
                retData = new UIRowsData<UI_IngredientTable> { remark = string.Empty, rows = listUI, status = 6, suc = true, total = total };
            }
            catch
            {

                retData = new UIRowsData<UI_IngredientTable> { remark = string.Empty, rows = new List<UI_IngredientTable>() { }, status = 6, suc = false, total = 0 };
            }

            finally
            {

            }
            return retData;

        }

        #endregion

        #region  获取单条数据
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UIModelData<UI_IngredientTable> GetSingle(Guid id)
        {
            UIModelData<UI_IngredientTable> ret = null;
            try
            {

                ret = new UIModelData<UI_IngredientTable>
                {
                    Data = Mapper.Map<UI_IngredientTable>(contextItem.FirstOrDefault(item => item.IngredientID == id)),
                    remark = string.Empty,
                    status = 6,
                    suc = true
                };
            }
            catch (Exception ex)
            {
                ret = new UIModelData<UI_IngredientTable>
                {

                    remark = ex.Message,
                    status = 7,
                    suc = false
                };
            }

            finally { }

            return ret;
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
                IngredientTable model = contextItem.FirstOrDefault(item => item.IngredientID == id);
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

        #region  批量删除条记录
        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UIModelData<List<Guid>> BatchDelete(List<Guid> ids)
        {

            UIModelData<List<Guid>> modal = null;
            try
            {
                ids.ForEach(id =>
                {
                    IQueryable<IngredientTable> models = contextItem.Where(item => ids.Contains(item.IngredientID));
                    contextItem.RemoveRange(models);
                    yuhongxiadataEntities.SaveChanges();

                });
                modal = new UIModelData<List<Guid>>() { Data = ids, status = 6, suc = true };
            }
            catch (Exception ex)
            {
                modal = new UIModelData<List<Guid>>() { status = 7, suc = false, remark = ex.Message };
            }
            return modal;
        }
        #endregion
    }
}
