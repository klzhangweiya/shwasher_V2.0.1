using System;
using System.Collections.Generic;
using System.Linq;
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
using Newtonsoft.Json;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.Common;
using ShwasherSys.Lambda;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo.Dto;
namespace ShwasherSys.SemiProductStoreInfo
{
    [AbpAuthorize,AuditLog("半成品排产出入库维护")]
    public class SemiEnterStoresAppService : ShwasherAsyncCrudAppService<SemiEnterStore, SemiEnterStoreDto, int, PagedRequestDto, SemiEnterStoreCreateDto, SemiEnterStoreUpdateDto >,ISemiEnterStoresAppService
    {
        protected IRepository<BusinessLog> LogRepository;

        protected IRepository<CurrentSemiStoreHouse> CurrentSemiStoreHouseRepository;

        protected IRepository<ViewSemiEnterStore> ViewSemiEnterStoreRepository;
        protected ICommonAppService CommonAppService { get; }
        public SemiEnterStoresAppService(IRepository<BusinessLog> logRepository, IRepository<SemiEnterStore, int> repository, IRepository<CurrentSemiStoreHouse> currentSemiStoreHouseRepository, IRepository<ViewSemiEnterStore> viewSemiEnterStoreRepository, ICommonAppService commonAppService) : base(repository)
        {
            LogRepository = logRepository;
            CurrentSemiStoreHouseRepository = currentSemiStoreHouseRepository;
            ViewSemiEnterStoreRepository = viewSemiEnterStoreRepository;
            CommonAppService = commonAppService;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesSemiProductStoreInfo;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesSemiProductStoreInfo;
		protected override string CreatePermissionName { get; set; } //= PermissionNames.PagesSemiEnterStoreCreate;
		protected override string UpdatePermissionName { get; set; } //= PermissionNames.PagesSemiEnterStoreUpdate;
		protected override string DeletePermissionName { get; set; } //= PermissionNames.PagesSemiEnterStoreDelete;

        [DisableAuditing]
        public async Task<PagedResultDto<ViewSemiEnterStore>> GetViewAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            string s1 = EnterStoreApplyStatusEnum.Applying.ToInt() + "",
                s2 = EnterStoreApplyStatusEnum.Audited.ToInt() + "",
                s3 = EnterStoreApplyStatusEnum.Refused.ToInt() + "",
                s4 = EnterStoreApplyStatusEnum.EnterStored.ToInt() + "";
            var query = ViewSemiEnterStoreRepository.GetAll()
                .Where(a =>
                    (a.ApplyStatus == s1 || a.ApplyStatus == s2 || a.ApplyStatus == s3 || a.ApplyStatus == s4));
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
                var exp = objList.GetExp<ViewSemiEnterStore>();
                
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(i => i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<ViewSemiEnterStore>(
                totalCount,
                entities
            );
            return dtos;
        }

        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiEnterStoreApplyMgUpdate),AuditLog("审核入库申请")]
        public async Task<SemiEnterStoreDto> Audit(SemiEnterStoreAuditDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == EnterStoreApplyStatusEnum.EnterStored.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("已入库不能再操作!"));
            }

            if (entity.IsClose)
            {
                CheckErrors(IwbIdentityResult.Failed("已关闭入库申请,不能再操作!"));
            }
            //包含库位信息
            if (!input.StoreLocationNo.IsNullOrEmpty())
            {
                //检查当前库位是否存在盘点信息
                var isCanChange = CommonAppService.CheckStoreCanUpdateByLocationNo(input.StoreLocationNo, 2);
                if (!isCanChange)
                {
                    CheckErrors(IwbIdentityResult.Failed("该库存库位处于退货或者正在盘点状态,不可进行出入库更新!"));
                }
            }
            var createSourceType = input.CreateSourceType;
            if (createSourceType == 2)
            {
                entity.ApplyStatus = EnterStoreApplyStatusEnum.EnterStored.ToInt().ToString();
                var currentStore = CurrentSemiStoreHouseRepository.GetAll().FirstOrDefault(i => i.ProductionOrderNo == entity.ProductionOrderNo && i.StoreHouseId == input.StoreHouseId&&i.StoreLocationNo==input.StoreLocationNo);
                
                if (currentStore != null)
                {
                   
                    //检查库存记录是否处于盘点中
                    var isCanUpdate =
                        CommonAppService.CheckStoreRecordCanUpdate(currentStore.CurrentSemiStoreHouseNo, 2);
                    if(!isCanUpdate)
                        CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
                    currentStore.ActualQuantity += entity.Quantity;
                    currentStore.TimeLastMod = Clock.Now;
                    currentStore.UserIDLastMod = AbpSession.UserName;
                    currentStore.KgWeight = entity.KgWeight;
                    await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
                }
                else
                {
                    string lcJson = JsonConvert.SerializeObject(entity);
                    currentStore = lcJson.GetModel<CurrentSemiStoreHouse>();
                    currentStore.CurrentSemiStoreHouseNo = Guid.NewGuid().ToString("N");
                    currentStore.TimeCreated = Clock.Now; 
                    currentStore.UserIDLastMod = AbpSession.UserName;
                    currentStore.ActualQuantity = entity.Quantity;
                    currentStore.FreezeQuantity = 0;
                    await CurrentSemiStoreHouseRepository.InsertAsync(currentStore);
                }
            }
            else
            {
                entity.ApplyStatus = EnterStoreApplyStatusEnum.Audited.ToInt().ToString();
            }
           
            entity.ActualQuantity = input.ActualQuantity;
            entity.AuditDate = Clock.Now;
            entity.AuditUser = AbpSession.UserName;
            entity.AuditDate = Clock.Now;
            entity.AuditUser = AbpSession.UserName;
            entity.StoreLocationNo = input.StoreLocationNo;
            entity.StoreHouseId = input.StoreHouseId;
            var loDto = ObjectMapper.Map<SemiEnterStoreUpdateDto>(entity);
            await UpdateEntity(loDto);

            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品入库",
                $"审核通过半成品入库申请[{entity.Id}]，审核数量:[{ entity.ActualQuantity}]",
                entity.ProductionOrderNo);
            return MapToEntityDto(entity);

        }
        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiEnterStoreApplyMgRefuse), AuditLog("拒绝入库申请")]
        public async Task<SemiEnterStoreDto> Refuse(EntityDto<int> input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == EnterStoreApplyStatusEnum.EnterStored.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("已入库不能再操作!"));
            }
            entity.AuditDate = Clock.Now;
            entity.AuditUser = AbpSession.UserName;
            entity.ApplyStatus = EnterStoreApplyStatusEnum.Refused.ToInt().ToString();
            var loDto = ObjectMapper.Map<SemiEnterStoreUpdateDto>(entity);
            await UpdateEntity(loDto);

            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品入库",
                $"拒绝半成品入库申请[{entity.Id}]",
                entity.ProductionOrderNo);
           return MapToEntityDto(entity);
        }

     
    }
}
