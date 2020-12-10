using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using Microsoft.AspNet.Identity;
using ShwasherSys.BasicInfo;
using ShwasherSys.BasicInfo.OutFactory;
using ShwasherSys.BasicInfo.OutFactory.Dto;
using ShwasherSys.BasicInfo.StoreHouseLocations.Dto;
using ShwasherSys.CompanyInfo;
using ShwasherSys.CompanyInfo.FixedAssetInfo.Dto;
using ShwasherSys.CompanyInfo.MoldInfo.Dto;
using ShwasherSys.CustomerInfo;
using ShwasherSys.CustomerInfo.Dto;
using ShwasherSys.Inspection;
using ShwasherSys.Invoice;
using ShwasherSys.Lambda;
using ShwasherSys.NotificationInfo;
using ShwasherSys.NotificationInfo.Dto;
using ShwasherSys.Order;
using ShwasherSys.OrderSendInfo;
using ShwasherSys.ProductInfo;
using ShwasherSys.ProductInfo.Dto;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ProductionOrderInfo.Dto;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.RmStore;
using ShwasherSys.SemiProductStoreInfo;
using TemplateInfo = ShwasherSys.Inspection.TemplateInfo;

namespace ShwasherSys.Common
{
    public interface IQueryAppService : IApplicationService
    {
        /// <summary>
        /// 查询外协厂商信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OutFactoryDto>> GetOutFactory(IwbPagedRequestDto input);
        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CustomerDto>> GetCustomer(IwbPagedRequestDto input);
     

        /// <summary>
        /// 查询半成品实时库存信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ViewCurrentSemiStoreHouse>> GetCurrentSemiStore(IwbPagedRequestDto input);
        /// <summary>
        /// 查询半成品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SemiProductDto>> GetSemiProduct(IwbPagedRequestDto input);

        Task<string> GetSemiProductName(string id);
        Task<SemiProductDto> GetSingleSemiProduct(string input);
        /// <summary>
        /// 查询产品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProductDto>> GetProduct(IwbPagedRequestDto input);
        Task<Product> GetProductById(EntityDto<string> input);
        Task<string> GetProductName(string id);

        /// <summary>
        /// 查询库位信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<StoreHouseLocationDto>> GetStoreHouseLocation(IwbPagedRequestDto input);

        Task<PagedResultDto<ProductionLogDto>> QueryProductionLog(IwbPagedRequestDto input);
        string GetDefualtProductByOrderNo(string orderNo);
        string GetDefualtProductByCustomerId(string customerId);

        Customer GetCustomerInfo(EntityDto<string> input);
        CustomerSend GetCustomerSendInfo(EntityDto<int> input);
        List<CustomerSendDto> GetCustomerSendDtoByCustomerId(CustomerSendDto customerId);

        List<SelectListItem> GetProductPropertyList(string pcPropertyName);
        
        List<CurrentProductStoreHouse> QueryStore(string productNo);
        List<CurrentProductStoreHouse> QueryStoreFilter(string productNo,string customerId);


        BulletinInfo GetBulletinInfo(EntityDto input);
        ShortMessage GetShortMsgDetailInfo(ShortMsgDetailDto input);
        Task<PagedResultDto<Product>> GetQueryCustomerDefaultProduct(IwbPagedRequestDto input);
        List<StoreHouse> QueryStoreHouse(int houseType=0);
        List<SelectListItem> QueryStoreHouseSelect(int houseType=0);
        string QueryStoreHouseSelectStr(int houseType = 0);
        List<ViewSemiEnterStore> QueryEnterStoresByPoNo(string productionOrderNo);

        Task<TemplateInfo> QueryTemplate(string tmpKey, int type);
        Task<List<Currency>> QueryAllCurrency();
        Task<List<CurrencyExchangeRate>> QueryCurrencyRate(string from,string to);

        Task<string> GetExpressNameById(int id);
        PagedResultDto<ViewEmployee> QueryEmployee(IwbPagedRequestDto input);
        Task<PagedResultDto<MoldDto>> QueryMold(IwbPagedRequestDto input);
        Task<PagedResultDto<FixedAssetDto>> QueryFixedAsset(IwbPagedRequestDto input);
        Task<PagedResultDto<ViewOrderSend>> QuerySendGood(IwbPagedRequestDto input);
        Task<PagedResultDto<ViewStickBill>> QueryInvoice(IwbPagedRequestDto input);
        Task<PagedResultDto<RmProduct>>  GetRmProduct(IwbPagedRequestDto input);
        Task<PagedResultDto<ViewCurrentRmStoreHouse>> GetRmCurrentStore(IwbPagedRequestDto input);
    }
    [AbpAuthorize,DisableAuditing]
    public class QueryAppService : ApplicationService, IQueryAppService
    {
        protected ICacheManager CacheManager { get; set; }
        protected new IIwbSettingManager SettingManager { get; set; }


        public QueryAppService(

            IRepository<Customer,string> customerRepository,
                IRepository<ViewCurrentSemiStoreHouse> currentSemiRepository,
            IRepository<SemiProducts, string> semiProductRepository,
            IRepository<Product, string> productRepository,
            IRepository<StoreHouseLocation> shlRepository,
            IIwbSettingManager settingManager,
            ICacheManager cacheManager, IRepository<CustomerDefaultProduct> customerDefaultProductRepository, IRepository<CustomerSend> customerSendRepository, IRepository<OrderHeader, string> orderHeaderRepository, IRepository<CurrentProductStoreHouse> currentProductStoreHouseRepository, IRepository<BulletinInfo> bulletinInfoRepository, IRepository<ShortMessage> shortMessageRepository, IRepository<ShortMsgDetail> shortMsgDetailRepository, IRepository<StoreHouse> storeHouseRepository, IRepository<ViewSemiEnterStore> viewSemiEnterStoreRepository, IRepository<OutFactory, string> outFactoryRepository, IRepository<TemplateInfo> templateInfoRepository, IRepository<Currency, string> currencyRepository, IRepository<CurrencyExchangeRate> currencyExchangeRateRepository, IRepository<ViewEmployee> viewEmployeeRepository, IRepository<RmProduct, string> rmRepository, IRepository<ViewCurrentRmStoreHouse, string> viewCurrentRmStoreHouseRepository, IRepository<ProductionLog> productionLogRepository, IRepository<CustomerDisabledProduct> customerDisabledProductRepository, IRepository<ExpressLogistics> expressRepository, IRepository<Mold> moldRepository, IRepository<FixedAsset> fixedAssetRepository, IRepository<ViewOrderSend> viewOrderSendRepository, IRepository<ViewStickBill, string> viewStickBillRepository)
        {
            CustomerRepository = customerRepository;
            CurrentSemiRepository = currentSemiRepository;
            SemiProductRepository = semiProductRepository;
            ProductRepository = productRepository;
            ShlRepository = shlRepository;
            CacheManager = cacheManager;
            CustomerDefaultProductRepository = customerDefaultProductRepository;
            CustomerSendRepository = customerSendRepository;
            OrderHeaderRepository = orderHeaderRepository;
            CurrentProductStoreHouseRepository = currentProductStoreHouseRepository;
            BulletinInfoRepository = bulletinInfoRepository;
            SettingManager = settingManager;
            ShortMessageRepository = shortMessageRepository;
            ShortMsgDetailRepository = shortMsgDetailRepository;
            StoreHouseRepository = storeHouseRepository;
            ViewSemiEnterStoreRepository = viewSemiEnterStoreRepository;
            OutFactoryRepository = outFactoryRepository;
            TemplateInfoRepository = templateInfoRepository;
            CurrencyRepository = currencyRepository;
            CurrencyExchangeRateRepository = currencyExchangeRateRepository;
            ViewEmployeeRepository = viewEmployeeRepository;
            RmRepository = rmRepository;
            ViewCurrentRmStoreHouseRepository = viewCurrentRmStoreHouseRepository;
            ProductionLogRepository = productionLogRepository;
            CustomerDisabledProductRepository = customerDisabledProductRepository;
            ExpressRepository = expressRepository;
            MoldRepository = moldRepository;
            FixedAssetRepository = fixedAssetRepository;
            ViewOrderSendRepository = viewOrderSendRepository;
            ViewStickBillRepository = viewStickBillRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }
        protected IRepository<FixedAsset> FixedAssetRepository { get; }
        protected IRepository<Mold> MoldRepository { get; }
        protected IRepository<ExpressLogistics> ExpressRepository { get; }
        protected IRepository<ProductionLog> ProductionLogRepository { get; }
        protected IRepository<OutFactory, string> OutFactoryRepository { get; }
        protected IRepository<Customer, string> CustomerRepository { get; }
        protected IRepository<ViewCurrentSemiStoreHouse> CurrentSemiRepository { get; }
        protected IRepository<SemiProducts, string> SemiProductRepository { get; }
        protected IRepository<Product, string> ProductRepository { get; }
        public IRepository<StoreHouseLocation> ShlRepository { get; }
        protected IRepository<CustomerDefaultProduct> CustomerDefaultProductRepository { get; }
        protected IRepository<CustomerSend> CustomerSendRepository { get; }
        protected IRepository<OrderHeader, string> OrderHeaderRepository { get; }
        protected IRepository<CurrentProductStoreHouse> CurrentProductStoreHouseRepository { get; }

        protected IRepository<BulletinInfo> BulletinInfoRepository { get; }
        protected IRepository<ShortMessage> ShortMessageRepository { get; }
        protected IRepository<ShortMsgDetail> ShortMsgDetailRepository { get; }
        protected IRepository<StoreHouse> StoreHouseRepository { get; }
        protected IRepository<ViewSemiEnterStore> ViewSemiEnterStoreRepository { get; }

        protected IRepository<TemplateInfo> TemplateInfoRepository { get; }
        protected IRepository<Currency,string> CurrencyRepository { get; }
        protected IRepository<CurrencyExchangeRate> CurrencyExchangeRateRepository { get; }

        protected IRepository<ViewEmployee> ViewEmployeeRepository { get; }

        protected IRepository<RmProduct,string> RmRepository { get; }
        protected IRepository<ViewCurrentRmStoreHouse, string> ViewCurrentRmStoreHouseRepository { get; }

        protected IRepository<CustomerDisabledProduct> CustomerDisabledProductRepository { get; }
        protected IRepository<ViewOrderSend> ViewOrderSendRepository { get; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        /// <summary>
        /// 查询快递名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetExpressNameById(int id)
        {
            var express = await ExpressRepository.FirstOrDefaultAsync(a => a.Id == id);
            return express?.ExpressName ?? "";
        }


        /// <summary>
        /// 查询外协厂商信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OutFactoryDto>> GetOutFactory(IwbPagedRequestDto input)
        {
            var query = OutFactoryRepository.GetAll().Where(a => a.IsLock == "N");
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
                var exp = objList.GetExp<OutFactory>();
                query = query.Where(exp);
            }
            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(i => i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await query.ToListAsync();

            var dtos = new PagedResultDto<OutFactoryDto>(
                totalCount,
                entities.Select(ObjectMapper.Map<OutFactoryDto>).ToList()
            );
            return dtos;
        }
        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CustomerDto>> GetCustomer(IwbPagedRequestDto input)
        {
            var query = CustomerRepository.GetAll().Where(a => a.IsLock == "N");
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
                var exp = objList.GetExp<Customer>();
                query = query.Where(exp);
            }
            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(i => i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await query.ToListAsync();

            var dtos = new PagedResultDto<CustomerDto>(
                totalCount,
                entities.Select(ObjectMapper.Map<CustomerDto>).ToList()
            );
            return dtos;
        }


        /// <summary>
        /// 查询半成品实时库存信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ViewCurrentSemiStoreHouse>> GetCurrentSemiStore(IwbPagedRequestDto input)
        {
            var query = CurrentSemiRepository.GetAll();
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
                var exp = objList.GetExp<ViewCurrentSemiStoreHouse>();
                query = query.Where(exp);
            }
            query = query.Where(i => (i.ActualQuantity - i.FreezeQuantity) > 0);//过滤掉可用数量小于等于0
            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(i => i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await query.ToListAsync();

            var dtos = new PagedResultDto<ViewCurrentSemiStoreHouse>(totalCount,entities);
            return dtos;
        }


        /// <summary>
        /// 查询半成品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SemiProductDto>> GetSemiProduct(IwbPagedRequestDto input)
        {
            var query = SemiProductRepository.GetAll().Where(a => a.IsLock == "N");
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
                var exp = objList.GetExp<SemiProducts>();
                query = query.Where(exp);
            }
            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(i => i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await query.ToListAsync();

            var dtos = new PagedResultDto<SemiProductDto>(
                totalCount,
                entities.Select(ObjectMapper.Map<SemiProductDto>).ToList()
            );
            return dtos;
        }

        public async Task<SemiProductDto> GetSingleSemiProduct(string input)
        {
            var query = await SemiProductRepository.FirstOrDefaultAsync(i=>i.Id==input);
            return ObjectMapper.Map<SemiProductDto>(query);
        }

        public async Task<string> GetSemiProductName(string id)
        {
            var entity = await CacheManager.GetCache(ShwasherConsts.SemiProductCache)
                .GetAsync(id, () => SemiProductRepository.FirstOrDefaultAsync(i => i.Id == id));
            return entity?.SemiProductName ?? "";

        }

        public async Task<string> GetProductName(string id)
        {
            var entity = await CacheManager.GetCache(ShwasherConsts.FinshedProductCache)
                .GetAsync(id, () => ProductRepository.FirstOrDefaultAsync(i => i.Id == id));
            return entity?.ProductName ?? "";

        }

        /// <summary>
        /// 查询产品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductDto>> GetProduct(IwbPagedRequestDto input)
        {
            var query = ProductRepository.GetAll().Where(a => a.IsLock == "N");
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
                var exp = objList.GetExp<Product>();
                query = query.Where(exp);
            }
            var totalCount = await query.CountAsync();
            
            query = !input.Sorting.IsNullOrWhiteSpace() ? query.OrderBy(input.Sorting) : query.OrderByDescending(i => i.TimeCreated);
           
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await query.ToListAsync();

            var dtos = new PagedResultDto<ProductDto>(
                totalCount,
                entities.Select(ObjectMapper.Map<ProductDto>).ToList()
            );
            return dtos;
        }

        public async Task<Product> GetProductById(EntityDto<string> input)
        {
            return await ProductRepository.FirstOrDefaultAsync(input.Id);
        }
        /// <summary>
        /// 查询库位信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<StoreHouseLocationDto>> GetStoreHouseLocation(IwbPagedRequestDto input)
        {
            var query = ShlRepository.GetAll();
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
                var exp = objList.GetExp<StoreHouseLocation>();
                query = query.Where(exp);
            }
            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(i => i.CreationTime);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await query.ToListAsync();

            var dtos = new PagedResultDto<StoreHouseLocationDto>(
                totalCount,
                entities.Select(ObjectMapper.Map<StoreHouseLocationDto>).ToList()
            );
            return dtos;
        }
        /// <summary>
        /// 根据订单号查询当前客户默认的产品
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public string GetDefualtProductByOrderNo(string orderNo)
        {
            var orderHeader = OrderHeaderRepository.Get(orderNo);
            return GetDefualtProductByCustomerId(orderHeader.CustomerId);
        }
        /// <summary>
        /// 根据客户号查询当前客户默认的产品
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public string GetDefualtProductByCustomerId(string customerId)
        {
            var defualtProducts = CustomerDefaultProductRepository.GetAll().Where(i => i.CustomerId == customerId);
            string lcRetval = "";
            foreach (var item in defualtProducts)
            {
                lcRetval += $"<option value=\"{item.ProductNo}\">{item.ProductNo}</option>";
            }

            return lcRetval;
        }
        public Customer GetCustomerInfo(EntityDto<string> input)
        {
            return CustomerRepository.FirstOrDefault(input.Id);
        }
        public CustomerSend GetCustomerSendInfo(EntityDto<int> input)
        {
            return CustomerSendRepository.FirstOrDefault(input.Id);
        }

        [DisableAuditing]
        public List<SelectListItem> GetProductPropertyList(string pcPropertyName)
        {
            var objList = new List<SelectListItem>();
            if (pcPropertyName.IsNullOrEmpty())
            {
                return objList;
            }
            var entitys = ProductRepository.GetAll().Where(i => i.IsLock == "N");
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



        #region 库存记录查询

        public List<CurrentProductStoreHouse> QueryStore(string productNo)
        {
            if (string.IsNullOrEmpty(productNo))
            {
                CheckErrors(IwbIdentityResult.Failed("请求的产品编码不存在!"));
            }
            var entity = CurrentProductStoreHouseRepository.GetAll().Where(i => i.ProductNo == productNo && (i.Quantity > 0||i.ProductionOrderNo.StartsWith("VS"))).OrderByDescending(i => i.TimeLastMod);
            List<CurrentProductStoreHouse> productionOrder = new List<CurrentProductStoreHouse>();
            List<CurrentProductStoreHouse> hasProductionOrder = new List<CurrentProductStoreHouse>();
            if (entity.Any())
            {
                productionOrder = entity.Where(i => string.IsNullOrEmpty(i.ProductionOrderNo)).ToList();
                hasProductionOrder = entity.Where(i => !string.IsNullOrEmpty(i.ProductionOrderNo)).ToList();
            }

            productionOrder.AddRange(hasProductionOrder);
            return productionOrder;
        }
        public List<CurrentProductStoreHouse> QueryStoreFilter(string productNo,string customerId)
        {
            if (string.IsNullOrEmpty(productNo))
            {
                CheckErrors(IwbIdentityResult.Failed("请求的产品编码不存在!"));
            }
            var entity = CurrentProductStoreHouseRepository.GetAll().Where(i => i.ProductNo == productNo && (i.Quantity > 0 || i.ProductionOrderNo.StartsWith("VS"))&&i.InventoryCheckState!=2);
            var disabledOrderNo = CustomerDisabledProductRepository.GetAll().Where(i => i.CustomerNo == customerId)
                .Select(i => i.ProductOrderNo).Distinct();
            entity = entity.Where(i => !disabledOrderNo.Contains(i.ProductionOrderNo))
                .OrderByDescending(i => i.TimeLastMod);
            List<CurrentProductStoreHouse> productionOrder = new List<CurrentProductStoreHouse>();
            List<CurrentProductStoreHouse> hasProductionOrder = new List<CurrentProductStoreHouse>();
            if (entity.Any())
            {
                productionOrder = entity.Where(i => string.IsNullOrEmpty(i.ProductionOrderNo)&& i.Quantity>0).ToList();
                hasProductionOrder = entity.Where(i => !string.IsNullOrEmpty(i.ProductionOrderNo)&& i.Quantity>0).ToList();
            }

            productionOrder.AddRange(hasProductionOrder);

            return productionOrder;
        }

        #endregion
        public BulletinInfo GetBulletinInfo(EntityDto input)
        {
            var entity = BulletinInfoRepository.FirstOrDefault(i => i.Id == input.Id);
            return entity;
        }
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public ShortMessage GetShortMsgDetailInfo(ShortMsgDetailDto input)
        {
            var entity = ShortMessageRepository.FirstOrDefault(input.MsgID);
            var detailInfo = ShortMsgDetailRepository.FirstOrDefault(input.Id);
            detailInfo.IsRead = "Y";
            ShortMsgDetailRepository.UpdateAsync(detailInfo);
            return entity;
        }

        public List<CustomerSendDto> GetCustomerSendDtoByCustomerId(CustomerSendDto customerId)
        {
            var entities = CustomerSendRepository.GetAll().Where(i => i.CustomerId == customerId.CustomerId && i.IsLock == "N");

            return ObjectMapper.Map<List<CustomerSendDto>>(entities.ToList());
        }

        public async Task<PagedResultDto<Product>> GetQueryCustomerDefaultProduct(IwbPagedRequestDto input)
        {
            var query = ProductRepository.GetAll();
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
                    if (o.KeyField == "CustomerId" || o.KeyField == "customerId")
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


            var totalCount = query.Count();

            query = query.SortBy("Id");
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = query.ToList();

            var dtos = new PagedResultDto<Product>(
                totalCount,
                entities
            );
            return dtos;
        }
        
        public async Task<PagedResultDto<ProductionLogDto>> QueryProductionLog(IwbPagedRequestDto input)
        {
            var query = ProductionLogRepository.GetAllIncluding(a=>a.EmployeeInfo);
            query = ApplyFilter<ProductionLog>(query,input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderBy(a=>a.CreationTime);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ProductionLogDto>(totalCount, entities.Select(a=>new ProductionLogDto
            {
                Id = a.Id,
                ProductionNo = a.ProductionNo,
                ProductOrderNo = a.ProductOrderNo,
                EmployeeId = a.EmployeeId,
                EmployeeNo = a.EmployeeInfo.No,
                EmployeeName = a.EmployeeInfo.Name,
                CarNo = a.CarNo,
                QuantityWeight = a.QuantityWeight,
                QuantityPcs = a.QuantityPcs,
                KgWeight = a.KgWeight,
                CreationTime = a.CreationTime
            }).ToList());
            return dtoList;
        }
        public List<StoreHouse> QueryStoreHouse(int houseType=0)
        {
            /*List<StoreHouse> result =  CacheManager.GetCache(ShwasherConsts.StoreHouseCache).Get("All",
                () => { return StoreHouseRepository.GetAllList(i => i.IsLock == "N"); });*/
            var query = StoreHouseRepository.GetAll().Where(i => i.IsLock == "N" );
            if (houseType > 0)
            {
                query = query.Where(i => i.StoreHouseTypeId == houseType);
            }
            return query.ToList();
        }

        public List<SelectListItem> QueryStoreHouseSelect(int houseType=0)
        {
            var query = StoreHouseRepository.GetAll().Where(i => i.IsLock == "N");
            if (houseType > 0)
            {
                query = query.Where(i => i.StoreHouseTypeId == houseType);
            }
            
            List<SelectListItem> resultList = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = @"请选择仓库...",
                    Value = "",
                    Selected = true
                }
            };
            foreach (var entityHouse in query)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = entityHouse.StoreHouseName,
                    Value = entityHouse.Id.ToString()
                };
                resultList.Add(item);
            }

            return resultList;
        }
        public string QueryStoreHouseSelectStr(int houseType=0)
        {
            var query = StoreHouseRepository.GetAll().Where(i => i.IsLock == "N");
            if (houseType > 0)
            {
                query = query.Where(i => i.StoreHouseTypeId == houseType);
            }

            string str = "";// "<option data-type=\"\" value=\"\">请选择仓库...</option>";
            foreach (var l in query)
            {
                str += $"<option data-type=\"{l.StoreHouseTypeId}\" value=\"{l.Id}\">{l.StoreHouseName}</option>";

            }
            return str;
        }

        public List<ViewSemiEnterStore> QueryEnterStoresByPoNo(string productionOrderNo)
        {
            var entities = ViewSemiEnterStoreRepository.GetAllList(i => i.ProductionOrderNo == productionOrderNo);
            return entities;
        }

        public async Task<TemplateInfo> QueryTemplate(string tmpKey, int type)
        {
            var tmps =await TemplateInfoRepository.FirstOrDefaultAsync(i => i.TempKey == tmpKey && i.Type == type);
            return tmps;
        }

        public async Task<List<Currency>> QueryAllCurrency()
        {
            return await CurrencyRepository.GetAllListAsync();
        }

        public async Task<List<CurrencyExchangeRate>> QueryCurrencyRate(string from="", string to="")
        {
            if (from.IsNullOrEmpty()&&!to.IsNullOrEmpty())
            {
                return await CurrencyExchangeRateRepository.GetAllListAsync(i => i.ToCurrencyId == to);
            }
            else if (!from.IsNullOrEmpty() && to.IsNullOrEmpty())
            {
                return await CurrencyExchangeRateRepository.GetAllListAsync(i => i.FromCurrencyId == from );
            }
            else
            {
                return await CurrencyExchangeRateRepository.GetAllListAsync(i => i.FromCurrencyId == from && i.ToCurrencyId == to);
            }
           
        }

        public PagedResultDto<ViewEmployee> QueryEmployee(IwbPagedRequestDto input)
        {
            var query = ViewEmployeeRepository.GetAll();
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
                var exp = objList.GetExp<ViewEmployee>();
                if (exp != null)
                {
                    query = query.Where(exp);
                }
            }
            var totalCount = query.Count();
            query = query.SortBy("No");
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = query.ToList();
            var dtos = new PagedResultDto<ViewEmployee>(
                totalCount,
                entities
            );
            return dtos;
        }
        /// <summary>
        /// 查询模具信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MoldDto>> QueryMold(IwbPagedRequestDto input)
        {
            var query = MoldRepository.GetAll();
            query = ApplyFilter(query,input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = _ApplySorting(query, input);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<MoldDto>(totalCount, entities.Select(ObjectMapper.Map<MoldDto>).ToList());
            return dtoList;
        }
        /// <summary>
        /// 查询模具信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<FixedAssetDto>> QueryFixedAsset(IwbPagedRequestDto input)
        {
            var query = FixedAssetRepository.GetAll();
            query = ApplyFilter(query,input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = _ApplySorting(query, input);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<FixedAssetDto>(totalCount, entities.Select(ObjectMapper.Map<FixedAssetDto>).ToList());
            return dtoList;
        }
        /// <summary>
        /// 查询发货明细信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ViewOrderSend>> QuerySendGood(IwbPagedRequestDto input)
        {
            var query = ViewOrderSendRepository.GetAll().Where(a=>!string.IsNullOrEmpty(a.OrderSendBillNo));
            query = ApplyFilter(query,input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query =  query.OrderByDescending(a=>a.OrderItemId);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ViewOrderSend>(totalCount, entities);
            return dtoList;
        }

      protected  IRepository<ViewStickBill, string> ViewStickBillRepository { get; }

        /// <summary>
        /// 查询发票信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ViewStickBill>> QueryInvoice(IwbPagedRequestDto input)
        {
            var query = ViewStickBillRepository.GetAll();
            query = ApplyFilter(query,input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query =  query.OrderByDescending(a=>a.TimeCreated);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ViewStickBill>(totalCount, entities);
            return dtoList;
        }
        public async Task<PagedResultDto<RmProduct>> GetRmProduct(IwbPagedRequestDto input)
        {
            var query = RmRepository.GetAll();
            query = ApplyFilter<RmProduct>(query,input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = _ApplySorting(query, input);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<RmProduct>(totalCount, entities);
            return dtoList;
        }

        public async Task<PagedResultDto<ViewCurrentRmStoreHouse>> GetRmCurrentStore(IwbPagedRequestDto input)
        {
            var query = ViewCurrentRmStoreHouseRepository.GetAll();
            query = ApplyFilter<ViewCurrentRmStoreHouse>(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = _ApplySorting(query, input);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ViewCurrentRmStoreHouse>(totalCount, entities);
            return dtoList;
        }
        protected IQueryable<T> ApplyFilter<T>(IQueryable<T> query, IwbPagedRequestDto input)
        {
            var pagedInput = input as IIwbPagedRequest;
            if (pagedInput == null)
            {
                return query;
            }
            if (!string.IsNullOrEmpty(pagedInput.KeyWords))
            {
                object keyWords = pagedInput.KeyWords;
                LambdaObject obj = new LambdaObject()
                {
                    FieldType = (LambdaFieldType)pagedInput.FieldType,
                    FieldName = pagedInput.KeyField,
                    FieldValue = keyWords,
                    ExpType = (LambdaExpType)pagedInput.ExpType
                };
                var exp = obj.GetExp<T>();
                query = exp != null ? query.Where(exp) : query;
            }
            if (pagedInput.SearchList != null && pagedInput.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in pagedInput.SearchList)
                {
                    if (string.IsNullOrEmpty(o.KeyWords))
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
                var exp = objList.GetExp<T>();
                query = exp != null ? query.Where(exp) : query;
            }
            return query;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<T> _ApplyPaging<T>(IQueryable<T> query, IwbPagedRequestDto input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<T> _ApplySorting<T>(IQueryable<T> query, IwbPagedRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(sortInput.Sorting);
                }
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderBy("CreationTime DESC");
            }

            //No sorting
            return query;
        }
    }
}
