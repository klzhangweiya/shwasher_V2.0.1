using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class DecimalPrecisionAttribute : Attribute
    {
        public DecimalPrecisionAttribute(byte precision = 18, byte scale = 3)
        {
            Precision = precision;
            Scale = scale;
        }

        public byte Precision { get; set; }

        public byte Scale { get; set; }
    }
}
