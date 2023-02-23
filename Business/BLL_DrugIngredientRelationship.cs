using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using DataAccess;

namespace Business
{
    public class BLL_DrugIngredientRelationship : BaseMathRoleAuthorEntities
    {
        private DbSet<IngredientsInWhichDrugs_View> contextItem = null;
        public BLL_DrugIngredientRelationship() : base()
        {


            contextItem = yuhongxiadataEntities.IngredientsInWhichDrugs_View;
        }
        public List<IngredientsInWhichDrugs_View> Search(int PageIndex, int PageSize, out int total)
        {

            total = contextItem.Count();
            IQueryable<IngredientsInWhichDrugs_View> lazyList = contextItem.OrderByDescending(item => item.IngredientShortName);
            return PageIndex >= 0 ? lazyList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList() : lazyList.ToList();
        }
    }
}
