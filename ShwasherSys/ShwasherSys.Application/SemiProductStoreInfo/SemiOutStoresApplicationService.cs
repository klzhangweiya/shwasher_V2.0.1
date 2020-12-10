using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.IdentityFramework;
using NPOI.HPSF;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.Common;
using ShwasherSys.Lambda;
using ShwasherSys.PackageInfo;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.SemiProductStoreInfo.Dto;
namespace ShwasherSys.SemiProductStoreInfo
{
    [AbpAuthorize, AuditLog("半成品排产出入库维护")]
    public class SemiOutStoreAppService : ShwasherAsyncCrudAppService<SemiOutStore, SemiOutStoreDto, int, PagedRequestDto, SemiOutStoreCreateDto, SemiOutStoreUpdateDto >, ISemiOutStoreAppService
    {
        public IRepository<PackageApply> PackageApplyRepository { get; }
        protected IRepository<BusinessLog> LogRepository { get; set; }
        protected IRepository<CurrentSemiStoreHouse> CurrentSemiStoreHouseRepository;
        protected IRepository<ViewSemiOutStore> ViewSemiOutStoreRepository { get; }
        protected ICommonAppService CommonAppService { get; }
        public SemiOutStoreAppService(
            IRepository<PackageApply> packageApplyRepository,
            IRepository<BusinessLog> logRepository,
            IRepository<SemiOutStore, int> repository, 
            IRepository<CurrentSemiStoreHouse> currentSemiStoreHouseRepository, 
            IRepository<ViewSemiOutStore> viewSemiOutStoreRepository, ICommonAppService commonAppService) : base(repository)
        {
            PackageApplyRepository = packageApplyRepository;
            LogRepository = logRepository;
            CurrentSemiStoreHouseRepository = currentSemiStoreHouseRepository;
            ViewSemiOutStoreRepository = viewSemiOutStoreRepository;
            CommonAppService = commonAppService;
        }
        
        protected override string GetPermissionName { get; set; } = PermissionNames.PagesProductionInfoProductionOutStoreApplyMg;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesProductionInfoProductionOutStoreApplyMg;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesProductionInfoProductionOutStoreApplyMgCreate;
		protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesProductionInfoProductionOutStoreApplyMgUpdate;
		//protected override string DeletePermissionName { get; set; } = PermissionNames.PagesProductionInfoProductionOutStoreApplyMgDelete;

        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiOutStoreApplyMgUpdate),AuditLog("审核出库申请")]
        public async Task<SemiOutStoreDto> Audit(SemiEnterStoreAuditDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == OutStoreApplyStatusEnum.OutStored.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("已出库不能再操作!"));
            }
            var currentStore = await 
                CurrentSemiStoreHouseRepository.FirstOrDefaultAsync(i =>
                    i.CurrentSemiStoreHouseNo == entity.CurrentSemiStoreHouseNo);
            if (currentStore==null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }
            if (currentStore.ActualQuantity  < input.ActualQuantity)
            {
                CheckErrors(IwbIdentityResult.Failed("可用库存量不够，请检查出库数量!"));
                return null;
            }
            currentStore.FreezeQuantity -=
                entity.ActualQuantity == (decimal)0.00 ? entity.Quantity : entity.ActualQuantity;
            var createType = input.CreateSourceType??0;
            if (createType == 2)
            {
                var isCanUpdate =
                    CommonAppService.CheckStoreRecordCanUpdate(currentStore.CurrentSemiStoreHouseNo, 2);
                if (!isCanUpdate)
                    CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
                entity.ApplyStatus = OutStoreApplyStatusEnum.OutStored.ToInt() + "";
                currentStore.ActualQuantity -= input.ActualQuantity;
                entity.OutStoreDate = Clock.Now;
                entity.OutStoreUser = AbpSession.UserName;
            }
            else
            {
                entity.ApplyStatus = OutStoreApplyStatusEnum.Audited.ToInt() + "";
                currentStore.FreezeQuantity += input.ActualQuantity;
            }
            currentStore.TimeLastMod = Clock.Now;
            currentStore.UserIDLastMod = AbpSession.UserName;
            await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
           
            entity.ActualQuantity = input.ActualQuantity;
            entity.IsConfirm = false;
            entity.AuditDate = Clock.Now;
            entity.AuditUser = AbpSession.UserName;
            entity.TimeLastMod = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserName;
            await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品出库",
                $"审核通过半成品出库申请[{entity.Id}]，[审核数量:{ entity.ActualQuantity},库存数量:{currentStore.ActualQuantity},冻结数量:{currentStore.FreezeQuantity}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiOutStoreDto>(entity);
        }

        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiOutStoreApplyMgRefuse), AuditLog("拒绝出库申请")]
        public async Task<SemiOutStoreDto> Refuse(EntityDto<int> input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == OutStoreApplyStatusEnum.OutStored.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("已出库不能再操作!"));
            }
            var currentStore = await 
                CurrentSemiStoreHouseRepository.FirstOrDefaultAsync(i => i.CurrentSemiStoreHouseNo == entity.CurrentSemiStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }
            currentStore.FreezeQuantity -= entity.Quantity;
            await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
            entity.ApplyStatus = OutStoreApplyStatusEnum.Refused.ToInt() + "";
            await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品出库",
                $"拒绝半成品出库申请[{entity.Id}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiOutStoreDto>(entity);
        }

        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoCurrentSemiStoreHouseMgPackageApply), AuditLog("申请包装")]
        public async Task PackageApply(SemiOutStoreCreateDto input)
        {
            var currentStore =
                CurrentSemiStoreHouseRepository.FirstOrDefault(i =>
                    i.CurrentSemiStoreHouseNo == input.CurrentSemiStoreHouseNo);

           
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return ;
            }
            var isCanUpdate =
                CommonAppService.CheckStoreRecordCanUpdate(currentStore.CurrentSemiStoreHouseNo, 2);
            if (!isCanUpdate)
                CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));

            var canUseQuantit = currentStore.ActualQuantity - currentStore.FreezeQuantity;
            if (canUseQuantit < input.Quantity)
            {
                CheckErrors(IwbIdentityResult.Failed("可用库存量不够，请检查出库数量!"));
                return ;
            }

            //currentStore.FreezeQuantity += input.Quantity;
            currentStore.ActualQuantity -= input.Quantity;
            await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
            input.ApplyOutDate = Clock.Now;
            var entity = ObjectMapper.Map<SemiOutStore>(input);
            var date = Clock.Now;
            entity.OutStoreDate = date;
            entity.OutStoreUser = AbpSession.UserName;
            entity.StoreHouseId = currentStore.StoreHouseId;
            entity.ApplyOutStoreSource = OutStoreApplyTypeEnum.Package.ToInt() + "";
            entity.ApplyTypes = OutStoreApplyTypeEnum.Package.ToInt();
            entity.ApplyStatus = OutStoreApplyStatusEnum.OutStored.ToInt() + "";
            entity.IsConfirm = false;
            entity.ActualQuantity = input.Quantity;
            entity.AuditDate = date;
            entity.AuditUser = AbpSession.UserName;
            entity.TimeCreated =date;
            entity.CreatorUserId = AbpSession.UserName;
            entity.TimeLastMod = date;
            entity.UserIDLastMod = AbpSession.UserName;
            await Repository.InsertAsync(entity);
            var packageApply= new PackageApply()
            {
                PackageApplyNo = Guid.NewGuid().ToString("N"),
                ProductionOrderNo = entity.ProductionOrderNo,
                SemiProductNo = entity.SemiProductNo,
                SourceStore = currentStore.StoreHouseId,
                CurrentSemiStoreHouseNo = entity.CurrentSemiStoreHouseNo,
                ApplyQuantity = entity.ActualQuantity,
                KgWeight = entity.KgWeight,
                ApplyStatus= OutStoreApplyStatusEnum.Applying.ToInt()+"",
                ApplyDate = date,
                TimeCreated = date,
                CreatorUserId = AbpSession.UserName,
                TimeLastMod = date,
                UserIDLastMod = AbpSession.UserName,
            };
            packageApply= await PackageApplyRepository.InsertAsync(packageApply);

            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品出库",
                $"创建出库申请,确认出库包装产品,信息为:[{entity.Obj2String()}]",
                entity.ProductionOrderNo, packageApply.PackageApplyNo, entity.CurrentSemiStoreHouseNo);
        }

      
        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiOutStoreApplyMgQuery), DisableAuditing]
        public PagedResultDto<ViewSemiOutStore> GetViewAll(PagedRequestDto input)
        {
            string s1 = OutStoreApplyStatusEnum.Applying.ToInt() + "",
                s2 =  OutStoreApplyStatusEnum.Audited.ToInt() + "",
                s3 = OutStoreApplyStatusEnum.Refused.ToInt() + "",
                s4 = OutStoreApplyStatusEnum.Canceled.ToInt() + "",
                s5 = OutStoreApplyStatusEnum.OutStored.ToInt() + "";
            var query = ViewSemiOutStoreRepository.GetAll()
                .Where(a => (a.ApplyStatus == s1 || a.ApplyStatus == s2 || a.ApplyStatus == s3|| a.ApplyStatus == s4|| a.ApplyStatus == s5));
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
                var exp = objList.GetExp<ViewSemiOutStore>();
                query = query.Where(exp);
            }
            var totalCount = query.Count();

            query = query.OrderByDescending(i => i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = query.ToList();

            var dtos = new PagedResultDto<ViewSemiOutStore>(
                totalCount, entities
            );
            return dtos;
        }

    }
}
