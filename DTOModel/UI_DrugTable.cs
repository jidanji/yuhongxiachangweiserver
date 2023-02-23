using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOModel
{
    public class UI_DrugTable
    {
        public Guid DrugId { get; set; }
        public string DrugEnglishName { get; set; }
        public string DrugCommodityName { get; set; }
        public string DrugCommonName { get; set; }
        public string DrugSpecification { get; set; }
        public string DrugMerchant { get; set; }
        public string DrugRemark1 { get; set; }
        public string DrugRemark2 { get; set; }
        public long InsertTime { get; set; }
        public long UpdateTime { get; set; }
        public List<Guid> DrugIngredientIds = new List<Guid>();
        public List<UI_IngredientTable> DrugIngredients = new List<UI_IngredientTable>();
        public List<UI_DrugIngredientRelationship> UI_DrugIngredientRelationships = new List<UI_DrugIngredientRelationship>();
    }
}
