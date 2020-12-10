using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Timing;
using Abp.UI;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.CustomerInfo.Dto;
using ShwasherSys.Lambda;
using ShwasherSys.Order;
using ShwasherSys.ProductInfo;
using ShwasherSys.ProductInfo.Dto;

namespace ShwasherSys.CustomerInfo
{
    [AbpAuthorize]
    public class CustomerDefaultProductAppService :ShwasherAsyncCrudAppService<CustomerDefaultProduct, CustomerDefaultProductDto, int, PagedRequestDto, CustomerDefaultProductCreateDto, CustomerDefaultProductUpdateDto >, ICustomerDefaultProductAppService
    {
        protected IRepository<Product,string> ProductRepository;
        protected IRepository<OrderItem> OrderItemRepository;
        protected IRepository<OrderHeader,string> OrderHeaderRepository;
        public CustomerDefaultProductAppService(IRepository<CustomerDefaultProduct, int> repository, IRepository<Product, string> productRepository, IRepository<OrderItem> orderItemRepository, IRepository<OrderHeader, string> orderHeaderRepository) : base(repository)
        {
            ProductRepository = productRepository;
            OrderItemRepository = orderItemRepository;
            OrderHeaderRepository = orderHeaderRepository;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomers;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomers;
		protected override string CreatePermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomersCreateDefaultProduct;
		protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomersUpdateDefaultProduct;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomersDeleteDefaultProduct;

        public override async Task<PagedResultDto<CustomerDefaultProductDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;
                   
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
                var exp = objList.GetExp<CustomerDefaultProduct>();
                query = query.Where(exp);
            }
          

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var loProducts = ProductRepository.GetAll().Where(i=>i.IsLock=="N");
            var loCustomerDefaultProductDto = from d in query join p in loProducts on d.ProductNo  equals p.Id
                orderby d.TimeLastMod descending
                select new CustomerDefaultProductDto()
                {
                    CustomerId = d.CustomerId,
                    CustomerProductName = d.CustomerProductName,
                    Id = d.Id,
                    ProductNo = d.ProductNo,
                    ProductName = p.ProductName,
                    Sequence = d.Sequence,
                    TimeLastMod = d.TimeLastMod
                };
            loCustomerDefaultProductDto = loCustomerDefaultProductDto.OrderBy(i => i.Sequence);
            //query = ApplySorting(query, input);
            loCustomerDefaultProductDto = loCustomerDefaultProductDto.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = await AsyncQueryableExecuter.ToListAsync(loCustomerDefaultProductDto);

            var dtos = new PagedResultDto<CustomerDefaultProductDto>(
                totalCount,
                entities
            );
            return dtos;
        }
        public override async Task<CustomerDefaultProductDto> Create(CustomerDefaultProductCreateDto input)
        {
            CheckCreatePermission();
            string lcProductNos = input?.ProductNo;
            string lcCustomerId = input?.CustomerId;
            if (!lcProductNos.IsNullOrEmpty() && lcProductNos.EndsWith(","))
            {
                lcProductNos = lcProductNos.Substring(0, lcProductNos.Length - 1);
            }
            if (lcProductNos.IsNullOrEmpty()|| lcCustomerId.IsNullOrEmpty())
            {
                throw new UserFriendlyException("传入参数有误！");
            }

            var loExistProducts = Repository.GetAll().Where(i => i.CustomerId == lcCustomerId).OrderByDescending(i => i.Sequence);
            var obj = loExistProducts.FirstOrDefault();
            string[] pNos = lcProductNos?.Split(',');
            int index = obj == null ? 1 : obj.Sequence;
            if (pNos != null)
                foreach (var s in pNos)
                {
                  
                    CustomerDefaultProduct loCustomerDefaultProduct = new CustomerDefaultProduct()
                    {
                        CustomerId = lcCustomerId,
                        ProductNo = s,
                        Sequence = index,
                        TimeLastMod = Clock.Now
                    };
                    await Repository.InsertAsync(loCustomerDefaultProduct);
                    index++;
                }

            return new CustomerDefaultProductDto();
        }

        /*public string GetDefualtProductByOrderItemNo(int orderItemNo)
        {
            var orderItem = OrderItemRepository.Get(orderItemNo);
            return GetDefualtProductByOrderNo(orderItem.OrderNo);
        }

        public string GetDefualtProductByOrderNo(string orderNo)
        {
            var orderHeader = OrderHeaderRepository.Get(orderNo);
            return GetDefualtProductByCustomerId(orderHeader.CustomerId);
        }

        public string GetDefualtProductByCustomerId(string customerId)
        {
            var defualtProducts = Repository.GetAll().Where(i => i.CustomerId == customerId);
            string lcRetval = "";
            foreach (var item in defualtProducts)
            {
                lcRetval += $"<option value=\"{item.ProductNo}\">{item.ProductNo}</option>";
            }

            return lcRetval;
        }*/



    }
}
