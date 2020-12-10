using System;
using System.Collections.Generic;
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
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Common;
using ShwasherSys.EntityFramework;
using ShwasherSys.Inspection;
using ShwasherSys.Lambda;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.SemiProductStoreInfo.Dto;
namespace ShwasherSys.SemiProductStoreInfo
{
    [AbpAuthorize]
    public class CurrentSemiStoreHousesAppService : ShwasherAsyncCrudAppService<CurrentSemiStoreHouse, CurrentSemiStoreHouseDto, int, PagedRequestDto, CurrentSemiStoreHouseCreateDto, CurrentSemiStoreHouseUpdateDto >, ICurrentSemiStoreHousesAppService
    {
        protected IRepository<ViewCurrentSemiStoreHouse> ViewCurrentSemiStoreHouseRepository;
        protected IRepository<ProductionOrder> ProductionOrderRepository;
        protected IRepository<SemiEnterStore> SemiEnterStoreRepository { get; }
        protected IRepository<SemiOutStore> SemiOutStoreRepository { get; }
        protected ISqlExecuter SqlExecuter;
        protected IRepository<CustomerDisabledProduct> CustomerDisabledProductRepository;
        protected ICommonAppService CommonAppService { get; }
        public CurrentSemiStoreHousesAppService(IRepository<CurrentSemiStoreHouse, int> repository, IRepository<ViewCurrentSemiStoreHouse> viewCurrentSemiStoreHouseRepository, IRepository<ProductionOrder> productionOrderRepository, IRepository<SemiEnterStore> semiEnterStoreRepository, IRepository<SemiOutStore> semiOutStoreRepository, ISqlExecuter sqlExecuter, ICommonAppService commonAppService, IRepository<CustomerDisabledProduct> customerDisabledProductRepository) : base(repository, "CurrentSemiStoreHouseNo")
        {
            ViewCurrentSemiStoreHouseRepository = viewCurrentSemiStoreHouseRepository;
            ProductionOrderRepository = productionOrderRepository;
            SemiEnterStoreRepository = semiEnterStoreRepository;
            SemiOutStoreRepository = semiOutStoreRepository;
            SqlExecuter = sqlExecuter;
            CommonAppService = commonAppService;
            CustomerDisabledProductRepository = customerDisabledProductRepository;
        }

		protected override string GetPermissionName { get; set; } //= PermissionNames.PagesCurrentSemiStoreHouse;
		protected override string GetAllPermissionName { get; set; } //= PermissionNames.PagesCurrentSemiStoreHouse;
		protected override string CreatePermissionName { get; set; } //= PermissionNames.PagesCurrentSemiStoreHouseCreate;
		protected override string UpdatePermissionName { get; set; } //= PermissionNames.PagesCurrentSemiStoreHouseUpdate;
		protected override string DeletePermissionName { get; set; } //= PermissionNames.PagesCurrentSemiStoreHouseDelete;

        [DisableAuditing]
        public  async Task<PagedResultDto<ViewCurrentSemiStoreHouse>> GetViewAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = ViewCurrentSemiStoreHouseRepository.GetAll();
          
        
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
                var exp = objList.GetExp<ViewCurrentSemiStoreHouse>();
                if (exp != null)
                {
                    query = query.Where(exp);
                }
                
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(i=>i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<ViewCurrentSemiStoreHouse>(
                totalCount,
                entities
            );
            return dtos;
        }
        /// <summary>
        /// 更新实时库存千斤重
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoCurrentSemiStoreHouseMgUpdateKgWeight), AuditLog("修改千斤重")]
        public async Task UpdateKgWeight(UpdateKgWeightDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity==null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现实时库存记录！"));
                return ;
            }
            entity.KgWeight = input.KgWeight;
            await Repository.UpdateAsync(entity);
        }

      
        public async Task<string> GetVirtualProOrderNo()
        {
            string lcRetVal;
            DateTime loTiem = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
            var orders = (await SemiEnterStoreRepository.GetAllListAsync(i => i.TimeCreated >= loTiem && i.ProductionOrderNo.StartsWith("B"))).OrderByDescending(i => i.ProductionOrderNo).ToList();
            var orderNo = orders.FirstOrDefault()?.ProductionOrderNo;
            if (!string.IsNullOrEmpty(orderNo))
            {
                var liTempNo = Convert.ToInt32(orderNo.Substring(4,3));
                if (liTempNo >= 999)
                {
                    CheckErrors(IwbIdentityResult.Failed("当月新增虚拟批次号超出上限999！"));
                }
                liTempNo++;
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
            lcRetVal = "B" + year + (liMonth < 10 ? liMonth + "" : Convert.ToString(liMonth, 16).ToUpper()) + lcRetVal;
            return lcRetVal;
        }
        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoCurrentSemiStoreHouseMgAddEnter)]
        public async Task<SemiEnterStore> AddEnter(AddSemiEnterStoreDto input)
        {
            SemiEnterStore enter = ObjectMapper.Map<SemiEnterStore>(input);
            enter.ApplyEnterDate = Clock.Now;
            enter.ProductionOrderNo = input.ProductionOrderNo;
            enter.SemiProductNo = input.SemiProductNo;
            enter.ApplyStatus = EnterStoreApplyStatusEnum.Applying.ToInt().ToString();
            enter.ApplySource = EnterStoreApplySourceEnum.Balance.ToInt().ToString();
            enter.CreateSourceType = CreateSourceType.Manual.ToInt();
            enter.ActualQuantity = 0;
            enter.StoreHouseId = input.StoreHouseId;
            enter.StoreLocationNo = input.StoreLocationNo;
            enter.AuditDate = Clock.Now;
            enter.AuditUser = AbpSession.UserName;
            enter.CreatorUserId = AbpSession.UserName;
            enter.TimeCreated = Clock.Now;
            enter.UserIDLastMod = AbpSession.UserName;
            enter.TimeLastMod = Clock.Now;
            enter.IsClose = false;
           return await SemiEnterStoreRepository.InsertAsync(enter);
        }
        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoCurrentSemiStoreHouseMgAddOut)]
        public async Task<SemiOutStore> AddOut(AddSemiOutStoreDto input)
        {
            var entity =
                await Repository.FirstOrDefaultAsync(i =>
                    i.CurrentSemiStoreHouseNo == input.CurrentSemiStoreHouseNo);
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到库存！"));
                return null;
            }
            entity.FreezeQuantity += input.Quantity??0;
            await Repository.UpdateAsync(entity);
            SemiOutStore outStore = new SemiOutStore()
            {
                ProductionOrderNo = entity.ProductionOrderNo,
                CurrentSemiStoreHouseNo = entity.CurrentSemiStoreHouseNo,
                SemiProductNo = entity.SemiProductNo,
                StoreHouseId = entity.StoreHouseId,
                ApplyStatus = FinshedOutStoreApplyStatusEnum.Applying.ToInt().ToString(),
                ApplyOutStoreSource = OutStoreApplySourceTypeEnum.SemiProduct.ToInt()+"",
                IsClose = false,
                IsConfirm = false,
                Quantity = input.Quantity ?? 0,
                ActualQuantity = input.Quantity ?? 0,
                ApplyOutDate = Clock.Now,
                TimeCreated = Clock.Now,
                CreatorUserId = AbpSession.UserName,
                UserIDLastMod = AbpSession.UserName,
                CreateSourceType = CreateSourceType.Manual.ToInt(),
                KgWeight = 0
            };

            return await SemiOutStoreRepository.InsertAsync(outStore);
        }
        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoCurrentSemiStoreHouseMgChangeStoreHouse),AuditLog("半成品移库")]
        public async Task ChangeStoreHouse(ChangeStoreHouseDto input)
        {
            var entity = Repository.Get(input.Id);
            if (entity.StoreHouseId == input.StoreHouseId&&entity.StoreLocationNo==input.StoreLocationNo)
            {
                CheckErrors(IwbIdentityResult.Failed("变更前后仓库库位一致,请选中其它仓库库位！"));
            }
            var isCanUpdate =
                CommonAppService.CheckStoreRecordCanUpdate(entity.CurrentSemiStoreHouseNo, 2);
            if (!isCanUpdate)
                CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));

            var canUserQuantity = entity.ActualQuantity - entity.FreezeQuantity;
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
                    CommonAppService.CheckStoreRecordCanUpdate(targetEntity.CurrentSemiStoreHouseNo, 2);
                if (!isCanUpdate)
                    CheckErrors(IwbIdentityResult.Failed("待移动的目标库存处于退货或者正在盘点状态,不可进行出入库更新!"));
                //preOrderEntity.FreezeQuantity += entity.FreezeQuantity;
                targetEntity.ActualQuantity += input.ChangeQuantity;
                await Repository.UpdateAsync(targetEntity);
            }
            else
            {
                CurrentSemiStoreHouse nEntity = new CurrentSemiStoreHouse()
                {
                    CurrentSemiStoreHouseNo = Guid.NewGuid().ToString("N"),
                    ActualQuantity = input.ChangeQuantity,
                    ApplyEnterDate = DateTime.Now,
                    KgWeight = entity.KgWeight,
                    ProductionOrderNo = entity.ProductionOrderNo,
                    SemiProductNo = entity.SemiProductNo,
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
            entity.ActualQuantity = entity.ActualQuantity - input.ChangeQuantity;
            await Repository.UpdateAsync(entity);
        }
        /// <summary>
        /// 半成品移动库位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoCurrentSemiStoreHouseMgChangeStoreHouse), AuditLog("更新库位信息")]
        public async Task UpdateStoreLocation(UpdateLocationDto input)
        {
            var entity = await Repository.GetAsync(input.Id);
            var isCanUpdate = CommonAppService.CheckStoreRecordCanUpdate(entity.CurrentSemiStoreHouseNo);
            if (!isCanUpdate)
                CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
            var target = await Repository.FirstOrDefaultAsync(i =>
                i.ProductionOrderNo == entity.ProductionOrderNo && i.StoreLocationNo == input.StoreLocationNo);
            if (target != null)
            {
                target.FreezeQuantity += entity.FreezeQuantity;
                target.ActualQuantity+= entity.ActualQuantity;
                target.PreMonthQuantity += entity.ActualQuantity;
                await SqlExecuter.ExecuteAsync(" Update SemiOutStores set CurrentSemiStoreHouseNo = '" +
                                               target.CurrentSemiStoreHouseNo + "' where CurrentSemiStoreHouseNo = '" +
                                               entity.CurrentSemiStoreHouseNo +
                                               "'; update PackageApply set CurrentSemiStoreHouseNo ='" +
                                               target.CurrentSemiStoreHouseNo + "' where CurrentSemiStoreHouseNo = '" +
                                               entity.CurrentSemiStoreHouseNo + "'; delete from CurrentSemiStoreHouse where Id=" + input.Id + ";");
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
        /*
public async Task<CurrentSemiStoreHouseDto> AddVirtualStore(CurrentSemiStoreHouseCreateDto input)
{
   input.ActualQuantity = 0;
   input.FreezeQuantity = 0;
   input.CurrentSemiStoreHouseNo = Guid.NewGuid().ToString("N");

   return await CreateEntity(input);
}

//[AbpAuthorize(PermissionNames.)]
public async Task<string> GetVirtualProOrderNo()
{
   string lcRetVal;
   DateTime loTiem = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
   var orders = (await ProductionOrderRepository.GetAllListAsync(i => i.TimeCreated >= loTiem && i.ProcessingLevel == "1")).OrderByDescending(i => i.Id).ToList();
   var orderNo = orders.FirstOrDefault()?.ProductionOrderNo;
   if (!string.IsNullOrEmpty(orderNo))
   {
       var liTempNo = Convert.ToInt32(orderNo.Substring(3, 4));
       liTempNo++;
       lcRetVal = liTempNo.ToString();
       while (lcRetVal.Length < 4)
       {
           lcRetVal = "0" + lcRetVal;
       }
   }
   else
   {
       lcRetVal = "0001";
   }
   int liMonth = DateTime.Today.Month;
   lcRetVal = "VS" + (liMonth < 10 ? liMonth + "" : Convert.ToString(liMonth, 16).ToUpper()) + lcRetVal;
   return lcRetVal;
}
*/




    }
}
