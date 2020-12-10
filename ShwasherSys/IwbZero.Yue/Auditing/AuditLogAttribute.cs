using System;

namespace IwbZero.Auditing
{
    public class AuditLogAttribute : Attribute
    {
        public string Name { get; set; }
        public string MethondNameSuffix { get; set; }

        public AuditLogAttribute(string name, string methondNameSuffix = "")
        {
            Name = name;
            MethondNameSuffix = methondNameSuffix;
        }
    }
}
