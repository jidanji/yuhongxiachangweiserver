using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOModel
{
    public class UI_IngredientTable
    {
        public Guid IngredientID { get; set; }
        public string IngredientCNName { get; set; }
        public string IngredientShortName { get; set; }
        public string IngredientAlias { get; set; }
        public string CPName { get; set; }
        public string USPName { get; set; }
        public string EPName { get; set; }
        public string JPName { get; set; }

        public long InsertTime { get; set; }
        public long LastUpdateTime { get; set; }

        public Guid? DrugId { get; set; }
    }
}
