using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Timing;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using NPOI.SS.Formula.Functions;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Common;
using ShwasherSys.Common.Dto;
using ShwasherSys.CustomerInfo;
using ShwasherSys.EntityFramework;
using ShwasherSys.Inspection;
using ShwasherSys.Lambda;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.SemiProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo.Dto;

namespace ShwasherSys.ProductStoreInfo
{
    [AbpAuthorize]
    public class CurrentFinshedStoreHouseAppService : ShwasherAsyncCrudAppService<CurrentProductStoreHouse, CurrentProductStoreHouseDto, int, PagedRequestDto, CurrentProductStoreHouseCreateDto, CurrentProductStoreHouseUpdateDto >, ICurrentFinshedStoreHouseAppService
    {
        public IRepository<ViewCurrentProductStoreHouse> ViewCurrentProductStoreHouseRepository;
        protected IRepository<ProductionOrder> ProductionOrderRepository;
        protected IRepository<FinshedEnterStore> FinshedEnterStoreRepository;
        protected IRepository<ProductOutStore> ProductOutStoreRepository;
        protected IRepository<CustomerDisabledProduct> CustomerDisabledProductRepository;
        protected IRepository<Customer,string> CustomerRepository;
        protected ISqlExecuter SqlExecuter;
        protected ICommonAppService CommonAppService { get; }
        public CurrentFinshedStoreHouseAppService(IRepository<CurrentProductStoreHouse, int> repository, IRepository<ViewCurrentProductStoreHouse> viewCurrentProductStoreHouseRepository, IIwbSettingManager settingManager, IRepository<ProductionOrder> productionOrderRepository, IRepository<FinshedEnterStore> finshedEnterStoreRepository, IRepository<ProductOutStore> productOutStoreRepository, ISqlExecuter sqlExecuter, ICommonAppService commonAppService, IRepository<CustomerDisabledProduct> customerDisabledProductRepository, IRepository<Customer, string> customerRepository) : base(repository, "CurrentProductStoreHouseNo")
        {
            ViewCurrentProductStoreHouseRepository = viewCurrentProductStoreHouseRepository;
            ProductionOrderRepository = productionOrderRepository;
            FinshedEnterStoreRepository = finshedEnterStoreRepository;
            ProductOutStoreRepository = productOutStoreRepository;
            SqlExecuter = sqlExecuter;
            CommonAppService = commonAppService;
            CustomerDisabledProductRepository = customerDisabledProductRepository;
            SettingManager = settingManager;
            CustomerRepository = customerRepository;
        }

		protected override string GetPermissionName { get; set; } //= PermissionNames.PagesCurrentProductStoreHouse;
		protected override string GetAllPermissionName { get; set; } //= PermissionNames.PagesCurrentProductStoreHouse;
		protected override string CreatePermissionName { get; set; } //= PermissionNames.PagesCurrentProductStoreHouseCreate;
		protected override string UpdatePermissionName { get; set; } //= PermissionNames.PagesCurrentProductStoreHouseUpdate;
		protected override string DeletePermissionName { get; set; } //= PermissionNames.PagesCurrentProductStoreHouseDelete;

        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgQuery)]
        public async Task<PagedResultDto<ViewCurrentProductStoreHouse>> GetViewAll(PagedRequestDto input)
        {
            var query = ViewCurrentProductStoreHouseRepository.GetAll();
            
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    
                    object keyWords = o.KeyWords;
                    if (o.KeyField == "isUseDowngrade")
                    {
                        var queryDisProductionOrderNo =
                            CustomerDisabledProductRepository.GetAll().Select(i => i.ProductOrderNo).Distinct().ToList();
                        query = Convert.ToInt16(keyWords) == 1
                            ? query.Where(i => queryDisProductionOrderNo.Contains(i.ProductionOrderNo))
                            : query.Where(i => !queryDisProductionOrderNo.Contains(i.ProductionOrderNo));
                        continue;
                    }
                    if (o.KeyField == "showZoreCheckBox")
                    {
                        query = keyWords.ToString() == "0" ? query.Where(i => i.ActualQuantity > 0) : query;
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
                var exp = objList.GetExp<ViewCurrentProductStoreHouse>();
                if (exp != null)
                {
                    query = query.Where(exp);
                }
               
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderByDescending(i => i.SurfaceColor).ThenByDescending(i=>i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = new PagedResultDto<ViewCurrentProductStoreHouse>(
                totalCount,
                entities
            );
            return dtos;
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgExportExcel)]
        public async Task<string> ExportExcel(List<MultiSearchDtoExt> input)
        {
            var query = ViewCurrentProductStoreHouseRepository.GetAll();

            if (input != null && input.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;
                    //var logicType = LogicType.And;
                    //if (o.LogicType == 1)
                    //{
                    //    logicType = LogicType.Or;
                    //}
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType,
                        //LogicType = logicType
                    });
                }
                var exp = objList.GetExp<ViewCurrentProductStoreHouse>();
                query = query.Where(exp);
            }
            query = query.OrderByDescending(i => i.SurfaceColor).ThenByDescending(i => i.TimeCreated);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
         
            string downloadUrl = await SettingManager.GetSettingValueAsync("SYSTEMDOWNLOADPATH");
            string lcFilePath = System.Web.HttpRuntime.AppDomainAppPath + "\\" +
                                downloadUrl;
            List<ToExcelObj> columnsList = new List<ToExcelObj>()
            {
                new ToExcelObj(){MapColumn = "ProductionOrderNo",ShowColumn = "批次号"},
                new ToExcelObj(){MapColumn = "ProductNo",ShowColumn = "成品编号"},
                new ToExcelObj(){MapColumn = "Quantity",ShowColumn = "当前库存数量(千件)"},
                new ToExcelObj(){MapColumn = "FreezeQuantity",ShowColumn = "被冻结数量"},
                new ToExcelObj(){MapColumn = "ProductName",ShowColumn = "名称"},
                new ToExcelObj(){MapColumn = "SurfaceColor",ShowColumn = "表色"},
                new ToExcelObj(){MapColumn = "Model",ShowColumn = "规格"},
                new ToExcelObj(){MapColumn = "Rigidity",ShowColumn = "硬度"},
                new ToExcelObj(){MapColumn = "Material",ShowColumn = "材质"},
                new ToExcelObj(){MapColumn = "TimeCreated",ShowColumn = "创建时间"}
            };
            string lcResultFileName = ExcelHelper.ToExcel2003(columnsList, entities, "sheet", lcFilePath);
            return Path.Combine(downloadUrl, lcResultFileName);
        }
        //[AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgAddVirtualStore)]
        //public async Task<CurrentProductStoreHouseDto> AddVirtualStore(CurrentProductStoreHouseCreateDto input)
        //{
        //    input.Quantity = 0;
        //    input.FreezeQuantity = 0;
        //    input.CurrentProductStoreHouseNo = Guid.NewGuid().ToString("N");
        //    return await CreateEntity(input);
        //}
        //[AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgAddVirtualStore)]
        //public async Task<string> GetVirtualProOrderNo()
        //{
        //    string lcRetVal;
        //    DateTime loTiem = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
        //    var orders = (await Repository.GetAllListAsync(i => i.TimeCreated >= loTiem && i.ProductionOrderNo.StartsWith("VS"))).OrderByDescending(i => i.ProductionOrderNo).ToList();
        //    var orderNo = orders.FirstOrDefault()?.ProductionOrderNo;
        //    if (!string.IsNullOrEmpty(orderNo))
        //    {
        //        var liTempNo = Convert.ToInt32(orderNo.Substring(3, 4));
        //        liTempNo++;
        //        lcRetVal = liTempNo.ToString();
        //        while (lcRetVal.Length < 4)
        //        {
        //            lcRetVal = "0" + lcRetVal;
        //        }
        //    }
        //    else
        //    {
        //        lcRetVal = "0001";
        //    }
        //    int liMonth = DateTime.Today.Month;
        //    lcRetVal = "VS" + (liMonth < 10 ? liMonth + "" : Convert.ToString(liMonth, 16).ToUpper()) + lcRetVal;
        //    return lcRetVal;
        //}
       // [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgAddVirtualStore)]
        public async Task<string> GetVirtualBlanceProOrderNo()
        {
            string lcRetVal;
            DateTime loTiem = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
            var orders = (await FinshedEnterStoreRepository.GetAllListAsync(i => i.TimeCreated >= loTiem && i.ProductionOrderNo.StartsWith("C"))).OrderByDescending(i => i.ProductionOrderNo).ToList();
            var orderNo = orders.FirstOrDefault()?.ProductionOrderNo;
            if (!string.IsNullOrEmpty(orderNo))
            {
                var liTempNo = Convert.ToInt32(orderNo.Substring(4,3));
                liTempNo++;
                if (liTempNo >= 999)
                {
                    CheckErrors(IwbIdentityResult.Failed("当月新增虚拟批次号超出上限999！"));
                }
                lcRetVal = liTempNo.ToString();
                while (lcRetVal.Length < 3)
                {
                    lcRetVal = "0" + lcRetVal;
                }
            }
            else
            {
                lcRetVal = "001";
            }

            var year = DateTime.Now.Year.ToString().Substring(2, 2);
            int liMonth = DateTime.Today.Month;
            lcRetVal = "C" + year + (liMonth < 10 ? liMonth + "" : Convert.ToString(liMonth, 16).ToUpper()) + lcRetVal;
            return lcRetVal;
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgAddEnter)]
        public async Task<FinshedEnterStore> AddEnter(AddEnterStoreDto input)
        {
            FinshedEnterStore enter = ObjectMapper.Map<FinshedEnterStore>(input);
            enter.ActualPackageCount = input.PackageCount??0;
            enter.PackageSpecification = input.PackageSpecification??0;
            enter.ApplyEnterDate = Clock.Now;
            enter.ApplyStatus = FinshedEnterStoreApplyStatusEnum.Applying.ToInt();
            enter.ApplySourceType = EnterStoreApplySourceEnum.Balance.ToInt();
            enter.ApplyQuantity = enter.ActualPackageCount* enter.PackageSpecification;
            enter.Quantity = enter.ApplyQuantity;
            enter.CreateSourceType = CreateSourceType.Manual.ToInt();
            enter.CreatorUserId = AbpSession.UserName;
            enter.TimeCreated = Clock.Now;
            enter.UserIDLastMod = AbpSession.UserName;
            enter.TimeLastMod = Clock.Now;
            enter.PackageApplyNo = Guid.NewGuid().ToString("N");
            enter.PackageProductNo = "";
            enter.IsClose = false;
           
            return await FinshedEnterStoreRepository.InsertAsync(enter);
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgAddOut)]
        public async Task<ProductOutStore> AddOut(AddOutStoreDto input)
        {
            var entity =
                await Repository.FirstOrDefaultAsync(i =>
                    i.CurrentProductStoreHouseNo == input.CurrentProductStoreHouseNo);
            entity.FreezeQuantity += input.Quantity??0;
            ProductOutStore outStore = new ProductOutStore()
            {
                ProductionOrderNo = entity.ProductionOrderNo,
                CurrentProductStoreHouseNo = entity.CurrentProductStoreHouseNo,
                ProductNo = entity.ProductNo,
                StoreHouseId = entity.StoreHouseId,
                ApplyStatus = FinshedOutStoreApplyStatusEnum.Applying.ToInt(),
                IsClose =false,
                IsConfirm = false,
                Quantity = input.Quantity??0,
                ActualQuantity = input.Quantity ?? 0,
                ApplyOutDate = Clock.Now,
                TimeCreated = Clock.Now,
                CreatorUserId = AbpSession.UserName,
                UserIDLastMod= AbpSession.UserName,
                ApplyOutStoreSourceType = OutStoreApplyTypeEnum.Balance.ToInt(),
                CreateSourceType = CreateSourceType.Manual.ToInt(),
                KgWeight=0
            };
            await Repository.UpdateAsync(entity);
            return await ProductOutStoreRepository.InsertAsync(outStore);
        }

        //public  void PreMonthExcute()
        //{
        //    DateTime dt = DateTime.Now;
        //    DateTime montLastDay = dt.AddDays(1 - dt.Day).AddMonths(1).AddDays(-1);
        //    if (dt.Day == montLastDay.Day)
        //    {
        //        int result = SqlExecuter.Execute("update CurrentProductStoreHouse set PreMonthQuantity = Quantity");
        //    }
           
        //}
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgUpdateStoreLocation),AuditLog("更新库位信息")]
        public async Task UpdateStoreLocation(UpdateLocationDto input)
        {
            var entity =
                await Repository.GetAsync(input.Id);
            var isCanUpdate =
                CommonAppService.CheckStoreRecordCanUpdate(entity.CurrentProductStoreHouseNo);
            if (!isCanUpdate)
                CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
            var target = await Repository.FirstOrDefaultAsync(i =>
                i.ProductionOrderNo == entity.ProductionOrderNo && i.StoreLocationNo == input.StoreLocationNo);
            if (target != null)
            {
                isCanUpdate =
                    CommonAppService.CheckStoreRecordCanUpdate(target.CurrentProductStoreHouseNo);
                if (!isCanUpdate)
                    CheckErrors(IwbIdentityResult.Failed("目标库存处于退货或者正在盘点状态,不可进行出入库更新!"));
                target.FreezeQuantity += entity.FreezeQuantity;
                target.Quantity += entity.Quantity;
                target.PreMonthQuantity += entity.Quantity;
                await SqlExecuter.ExecuteAsync(" Update ProductOutStore set CurrentProductStoreHouseNo = '" +
                                         target.CurrentProductStoreHouseNo + "' where CurrentProductStoreHouseNo = '" +
                                         entity.CurrentProductStoreHouseNo +
                                         "'; update OrderSend set CurrentProductStoreHouseNo ='" +
                                         target.CurrentProductStoreHouseNo + "' where CurrentProductStoreHouseNo = '" +
                                         entity.CurrentProductStoreHouseNo + "'; delete from CurrentProductStoreHouse where Id="+input.Id+";");
                target.TimeLastMod = Clock.Now;
                target.UserIDLastMod = AbpSession.UserName;
                await Repository.UpdateAsync(target);
            }
            else
            {
                entity.StoreLocationNo = input.StoreLocationNo;
                entity.TimeLastMod = Clock.Now;
                entity.UserIDLastMod = AbpSession.UserName;
                await Repository.UpdateAsync(entity);
            }
           
           
        }
         [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgUpdateStoreLocation),AuditLog("成品移库")]
        public async Task UpdateStoreLocation(ChangeStoreHouseDto input)
        {
            var entity = Repository.Get(input.Id);
            if (entity.StoreHouseId == input.StoreHouseId&&entity.StoreLocationNo==input.StoreLocationNo)
            {
                CheckErrors(IwbIdentityResult.Failed("变更前后仓库库位一致,请选中其它仓库库位！"));
            }
            var isCanUpdate =
                CommonAppService.CheckStoreRecordCanUpdate(entity.CurrentProductStoreHouseNo, 1);
            if (!isCanUpdate)
                CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));

            var canUserQuantity = entity.Quantity - entity.FreezeQuantity;
            if (canUserQuantity < input.ChangeQuantity)
            {
                CheckErrors(IwbIdentityResult.Failed("变更数量不能大于实际可用库存数量！"));
            }
            //查看半成品库存中是否存在变更之后库存和批次相同的库存记录，如果有在此记录上增加数量，没有增加新记录
            var targetEntity = Repository.FirstOrDefault(i =>
                i.ProductionOrderNo == entity.ProductionOrderNo && i.StoreHouseId == input.StoreHouseId && i.StoreLocationNo == input.StoreLocationNo);
            if (targetEntity != null)
            {
                isCanUpdate =
                    CommonAppService.CheckStoreRecordCanUpdate(targetEntity.CurrentProductStoreHouseNo, 1);
                if (!isCanUpdate)
                    CheckErrors(IwbIdentityResult.Failed("待移动的目标库存处于退货或者正在盘点状态,不可进行出入库更新!"));
                //preOrderEntity.FreezeQuantity += entity.FreezeQuantity;
                targetEntity.Quantity += input.ChangeQuantity;
                await Repository.UpdateAsync(targetEntity);
            }
            else
            {
                var nEntity = new CurrentProductStoreHouse()
                {
                    CurrentProductStoreHouseNo = Guid.NewGuid().ToString("N"),
                    Quantity = input.ChangeQuantity,
                    KgWeight = entity.KgWeight,
                    ProductionOrderNo = entity.ProductionOrderNo,
                    ProductNo = entity.ProductNo,
                    FreezeQuantity = 0,
                    Remark = entity.Remark,
                    StoreHouseId = input.StoreHouseId,
                    StoreLocationNo = input.StoreLocationNo,
                    TimeCreated = Clock.Now,
                    TimeLastMod = Clock.Now,
                    CreatorUserId = AbpSession.UserName,
                    UserIDLastMod = AbpSession.UserName
                };
                await Repository.InsertAsync(nEntity);
            }
            entity.Quantity = entity.Quantity - input.ChangeQuantity;
            await Repository.UpdateAsync(entity);
        }


        public List<ProductionOrderDisCustomerDto> GetDisCustomerInfo(EntityDto<string> input)
        {
           return  CustomerDisabledProductRepository.GetAll().Where(i=>i.ProductOrderNo==input.Id).Join(CustomerRepository.GetAll(), p => p.CustomerNo, c => c.Id,
                (p, c) =>
                    new ProductionOrderDisCustomerDto()
                    {
                        ProductionOrderNo = p.ProductOrderNo,
                        CustomerName = c.CustomerName,
                        CustomerId = p.CustomerNo
                    }).ToList();
        }

    }
}
