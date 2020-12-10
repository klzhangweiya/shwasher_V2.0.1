using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.CompanyInfo.EmployeeInfo.Dto;

namespace ShwasherSys.CompanyInfo.EmployeeInfo
{
    public interface IEmployeeAppService : IIwbZeroAsyncCrudAppService<EmployeeDto, int, IwbPagedRequestDto, EmployeeCreateDto, EmployeeUpdateDto >
    {
        Task<string> GetAccountUser();
        Task Bind(AccountDto input);
        Task UnBind(EntityDto<int> input);

        #region Get

        Task<string> GetSelectStr2(Expression<Func<Employee, bool>> predicate=null);

        Task<Employee> GetEntity(EntityDto<int> input);
		Task<Employee> GetEntityById(int id);
		Task<Employee> GetEntityByNo(string no);
	
        #endregion

    }
}
