using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.CompanyInfo
{
    /// <summary>
    /// 设备固定资产维护
    /// </summary>
    [Table("FixedAssetInfo")]
    public class FixedAsset:FullAuditedEntity<int,SysUser>
    {
        public const int NoMaxLength = 50;
        public const int NameMaxLength = 50;
        public const int ModelMaxLength = 50;
        public const int DescMaxLength = 500;
        /// <summary>
        /// 资产编号
        /// </summary>
        [MaxLength(NoMaxLength)]
        public string No { get; set; }
        /// <summary>
        /// 资产名称
        /// </summary>
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        /// <summary>
        /// 资产类型
        /// </summary>
        [MaxLength(ModelMaxLength)]
        public string Model { get; set; }
        /// <summary>
        /// 资产描述
        /// </summary>
        [MaxLength(DescMaxLength)]
        public string Description { get; set; }
      
        public const int RemarkMaxLength = 500;
        [MaxLength(RemarkMaxLength)]
        public string Remark { get; set; }
        
    }
}