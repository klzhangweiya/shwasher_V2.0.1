using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.States.Dto;
using ShwasherSys.CustomerInfo;
using ShwasherSys.Lambda;
using ShwasherSys.ProductInfo.Dto;
namespace ShwasherSys.ProductInfo
{
    [AbpAuthorize]
    public class ProductsAppService : ShwasherAsyncCrudAppService<Product, ProductDto, string, PagedRequestDto, ProductCreateDto, ProductUpdateDto >, IProductsAppService
    {
        protected IRepository<CustomerDefaultProduct> CustomerDefaultProductRepository;
        public ProductsAppService(IRepository<Product, string> repository, IRepository<CustomerDefaultProduct> customerDefaultProductRepository, IIwbSettingManager settingManager) : base(repository)
        {
            CustomerDefaultProductRepository = customerDefaultProductRepository;
            SettingManager = settingManager;
        }
      

        protected override string GetPermissionName { get; set; }= PermissionNames.PagesProductInfoProducts;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesProductInfoProducts;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesProductInfoProductsCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesProductInfoProductsUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesProductInfoProductsDelete;

        [DisableAuditing]
        public List<SelectListItem> GetProductPropertyList(string pcPropertyName)
        {
            var objList = new List<SelectListItem>();
            if (pcPropertyName.IsNullOrEmpty())
            {
                return objList;
            }
            var entitys = Repository.GetAll().Where(i => i.IsLock == "N");
            var loPropertyInfo = typeof(Product).GetProperty(pcPropertyName,
                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (loPropertyInfo == null)
            {
                throw new ArgumentException($"{pcPropertyName} is not a property of type {typeof(Product)}.");
            }

            IQueryable<string> query = null;
            switch (pcPropertyName)
            {
                case "Material":
                    query = entitys.Select(i => i.Material).Distinct();
                    break;
                case "SurfaceColor":
                    query = entitys.Select(i => i.SurfaceColor).Distinct();
                    break;
                case "Rigidity":
                    query = entitys.Select(i => i.Rigidity).Distinct();
                    break;
            }
            //var r = entitys.Select(i => i.Material).Distinct();
            //var query = entitys.Distinct(new PropertyComparer<Product>(pcPropertyName));//entitys.Distinct(new PropertyComparer<Product>(pcPropertyName));
            if (query != null)
            {
                foreach (var product in query)
                {
                    objList.Add(new SelectListItem()
                    {
                        Text = product,
                        Value = product
                    });
                }
            }
            
            return objList;
        }
     
        [DisableAuditing]
        public  async Task<PagedResultDto<ProductDto>> GetQueryCustomerDefaultProduct(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = query.Where(i => i.IsLock == "N");
            string lcCustomerId = "";
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;
                    if (o.KeyField == "CustomerId"|| o.KeyField == "customerId")
                    {
                        lcCustomerId = keyWords + "";
                        continue;
                    }
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
                var exp = objList.GetExp<Product>();
                if (exp != null)
                {
                    query = query.Where(exp);
                }
                
            }
            List<string> loNotContain = new List<string>();
            if (!lcCustomerId.IsNullOrEmpty())
            {
                var loDefualtProducts = CustomerDefaultProductRepository.GetAll().Where(i => i.CustomerId == lcCustomerId);
                foreach (CustomerDefaultProduct defaultProduct in loDefualtProducts)
                {
                    loNotContain.Add(defaultProduct.ProductNo);
                }
            }

            if (loNotContain.Any())
            {
                query = query.Where(i => !loNotContain.Contains(i.Id));
            }
            

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<ProductDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
            return dtos;
        }

        [AbpAuthorize(PermissionNames.PagesProductInfoProductsExportExcel)]
        public async Task<string> ExportExcel(List<MultiSearchDto> input)
        {
            var query = Repository.GetAll();
            query = query.Where(i => i.IsLock == "N");
            if (input != null && input.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input)
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
                var exp = objList.GetExp<Product>();
                if (exp != null)
                {
                    query = query.Where(exp);
                }
            }
            query = query.OrderByDescending(i => i.Id);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            string downloadUrl = await SettingManager.GetSettingValueAsync("SYSTEMDOWNLOADPATH");
            string lcFilePath = System.Web.HttpRuntime.AppDomainAppPath + "\\" +
                                downloadUrl;
            List<ToExcelObj> columnsList = new List<ToExcelObj>()
            {
                new ToExcelObj(){MapColumn = "Id",ShowColumn = "成品编号"},
                new ToExcelObj(){MapColumn = "ProductName",ShowColumn = "名称"},
                new ToExcelObj(){MapColumn = "SurfaceColor",ShowColumn = "表色"},
                new ToExcelObj(){MapColumn = "Model",ShowColumn = "规格"},
                new ToExcelObj(){MapColumn = "Rigidity",ShowColumn = "硬度"},
                new ToExcelObj(){MapColumn = "Material",ShowColumn = "材质"},
                new ToExcelObj(){MapColumn = "Defprice",ShowColumn = "默认价格"},
            };
            string lcResultFileName = ExcelHelper.ToExcel2003(columnsList, entities, "sheet", lcFilePath);
            return Path.Combine(downloadUrl, lcResultFileName);
         
        }
    }
}
