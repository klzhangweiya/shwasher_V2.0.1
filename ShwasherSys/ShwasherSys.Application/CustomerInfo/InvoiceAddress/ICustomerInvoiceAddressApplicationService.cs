using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.CustomerInfo.InvoiceAddress.Dto;

namespace ShwasherSys.CustomerInfo.InvoiceAddress
{
    public interface ICustomerInvoiceAddressAppService : IIwbZeroAsyncCrudAppService<CustomerInvoiceAddressDto, int, IwbPagedRequestDto, CustomerInvoiceAddressCreateDto, CustomerInvoiceAddressUpdateDto >
    {


		#region Get

		Task<CustomerInvoiceAddress> GetEntity(EntityDto<int> input);
		Task<CustomerInvoiceAddress> GetEntityById(int id);
		Task<CustomerInvoiceAddress> GetEntityByNo(string no);
	
        #endregion

    }
}
