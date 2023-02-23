using System.Collections.Generic;

namespace MathSoftModelLib
{
    public class UIRowsData<T>
    {
        public int total { get; set; }
        public List<T> rows { get; set; }
        public string remark { get; set; }
        public bool suc { get; set; }
        public int status { get; set; }

       
    }
}
