using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathSoftModelLib
{
    public class UISummeryData<S, F>
    {
        public UISummeryData()
        {
            this.SuccessData = new List<S>();
            this.FailData = new List<F>();
            this.SuccessCount = 0;
            this.FailCount = 0;
        }

        public List<S> SuccessData { get; set; }
        public int SuccessCount;
        public string remark { get; set; }
        public bool suc { get; set; }
        public int status { get; set; }

        public List<F> FailData { get; set; }
        public int FailCount;
    }
}
