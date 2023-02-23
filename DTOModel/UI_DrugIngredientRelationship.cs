using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOModel
{
    public class UI_DrugIngredientRelationship
    {
        public Guid RelationshipId;
        public Guid DrugId;
        public Guid IngredientID;
        public long InsertTime;
        public long LastUpdateTime;
    }
}
