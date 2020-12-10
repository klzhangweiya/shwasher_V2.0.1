using Abp.Application.Services.Dto;

namespace ShwasherSys.CompanyInfo.EmployeeInfo.Dto
{
    public class AccountDto: EntityDto<int>
    {
        public string No  { get; set; }
        public string UserName  { get; set; }

    }
}