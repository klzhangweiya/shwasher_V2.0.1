using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(ProductProperty))]
    public class ProductPropertyUpdateDto: EntityDto<int>
    {
        
        /// <summary>
        /// 属性类别（规格1，材质2，硬度3，表色4）
        /// </summary>   
		public string PropertyType  { get; set; }
        
        /// <summary>
        /// 属性编码
        /// </summary>   
		public string PropertyNo  { get; set; }
		public string PropertyValue  { get; set; }
		public string DisplayValue  { get; set; }
		public string ContentInfo  { get; set; }
    }
}