using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ShwasherSys.CompanyInfo.EmployeeInfo.Dto
{
    
    /// <summary>
    /// 人员信息维护
    /// </summary>   
    [AutoMapTo(typeof(Employee)),AutoMapFrom(typeof(Employee))]
    public class EmployeeDto: EntityDto<int>
    {
        /// <summary>
        /// 人员编号
        /// </summary>   
        public string No  { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>   
        public string Name  { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>   
        public string CardId  { get; set; }
        /// <summary>
        /// 性别
        /// </summary>   
        public int Gender  { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>   
        public string PhoneNumber  { get; set; }
        /// <summary>
        /// 部门
        /// </summary>   
        public string DepartmentNo  { get; set; }
        /// <summary>
        /// 职务
        /// </summary>   
        public string DutyNo  { get; set; }
        /// <summary>
        /// 详情
        /// </summary>   
        public string Description  { get; set; }
        /// <summary>
        /// 登陆账号
        /// </summary>   
		public string UserName  { get; set; }
    }
}