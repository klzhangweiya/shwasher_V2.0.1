using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShwasherSys.Lambda;

namespace ShwasherSys.Common.Dto
{
    public class MultiSearchDtoExt
    {
        public  string KeyField { get; set; }

        public  string KeyWords { get; set; }

        public  int FieldType { get; set; }

        public  int ExpType { get; set; }

        public  int LogicType { get; set; } = 0;
    }
}
