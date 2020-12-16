using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.ProductInfo
{
    public class ProductProperty:FullAuditedEntity<int>
    {
        /// <summary>
        /// 属性类别（规格1，材质2，硬度3，表色4）
        /// </summary>
        public string PropertyType { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        public string PropertyNo { get; set; }

        

        public string PropertyValue { get; set; }

        public string DisplayValue { get; set; }

        public string ContentInfo { get; set; }

    }
}
