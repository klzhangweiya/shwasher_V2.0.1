using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using IwbZero.AppServiceBase;

namespace ShwasherSys.CompanyInfo.EmployeeInfo.Dto
{
    
    /// <summary>
    /// 人员信息维护
    /// </summary>   
    [AutoMapTo(typeof(Employee))]
    public class EmployeeCreateDto:IwbEntityDto<int>
    {
		
        /// <summary>
        /// 人员编号
        /// </summary>   
        [Required] 
        [StringLength(Employee.NoMaxLength)]
        public string No  { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>   
        [Required] 
        [StringLength(Employee.NameMaxLength)]
        public string Name  { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>   
        [StringLength(Employee.CardIdMaxLength)]
        public string CardId  { get; set; }
        /// <summary>
        /// 性别
        /// </summary>   
        public int Gender  { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>   
        [StringLength(Employee.PhoneMaxLength)]
        public string PhoneNumber  { get; set; }
        /// <summary>
        /// 部门
        /// </summary>   
        [StringLength(20)]
        public string DepartmentNo  { get; set; }
        /// <summary>
        /// 职务
        /// </summary>   
        [StringLength(20)]
        public string DutyNo  { get; set; }
        /// <summary>
        /// 详情
        /// </summary>   
        [StringLength(Employee.DescMaxLength)]
        public string Description  { get; set; }
    }
}
