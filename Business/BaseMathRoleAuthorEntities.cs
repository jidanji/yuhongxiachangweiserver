using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BaseMathRoleAuthorEntities
    {
        private yuhongxiadataEntities _yuhongxiadataEntities = null;
        public yuhongxiadataEntities yuhongxiadataEntities { get => _yuhongxiadataEntities; set => _yuhongxiadataEntities = value; }
        public BaseMathRoleAuthorEntities()
        {
            _yuhongxiadataEntities = new yuhongxiadataEntities();
        }
    }
}
