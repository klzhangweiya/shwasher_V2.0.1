using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.Timing;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using Newtonsoft.Json;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.Common;
using ShwasherSys.FinshedStoreInfo.Dto;
using ShwasherSys.Lambda;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo.Dto;

namespace ShwasherSys.FinshedStoreInfo
{
    [AbpAuthorize, AuditLog("成品仓库维护")]
    public class FinshedEnterStoreAppService : ShwasherAsyncCrudAppService<FinshedEnterStore, FinshedEnterStoreDto, int, PagedRequestDto, FinshedEnterStoreCreateDto, FinshedEnterStoreUpdateDto>, IFinshedEnterStoreAppService
    {
         
        public FinshedEnterStoreAppService(
            IRepository<BusinessLog> logRepository,
            IIwbSettingManager settingManager,
            ICacheManager cacheManager,
            IRepository<FinshedEnterStore, int> repository, IRepository<ViewProductEnterStore> viewProductEnterStoreRepository, IRepository<CurrentProductStoreHouse> currentProductStoreHouseRepository, ICommonAppService commonAppService, IStatesAppService statesAppService) : base(repository, "")
        {
            SettingManager = settingManager;
            CacheManager = cacheManager;
            StatesAppService = statesAppService;
            LogRepository = logRepository;
            ViewProductEnterStoreRepository = viewProductEnterStoreRepository;
            CurrentProductStoreHouseRepository = currentProductStoreHouseRepository;
            CommonAppService = commonAppService;
        }
        protected IStatesAppService StatesAppService { get; set; }
        protected override bool KeyIsAuto { get; set; } = false;
        public IRepository<BusinessLog> LogRepository { get; }

        public IRepository<ViewProductEnterStore> ViewProductEnterStoreRepository { get; }
        public IRepository<CurrentProductStoreHouse> CurrentProductStoreHouseRepository { get; }

        protected ICommonAppService CommonAppService { get;  }
        #region GetSelect

        [DisableAuditing]
        public async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var slist = new List<SelectListItem> { new SelectListItem { Text = @"请选择...", Value = "", Selected = true } };
            foreach (var l in list)
            {
                //slist.Add(new SelectListItem { Text = l., Value = l. });
            }
            return slist;
        }
        [DisableAuditing]
        public async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string str = "<option value=\"\" selected>请选择...</option>";
            foreach (var l in list)
            {
                //str += $"<option value=\"{l.}\">{l.}</option>"; 
            }
            return str;
        }

        #endregion

        #region CURD

        #region Get

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMgQuery)]
        public Task<FinshedEnterStore> GetEntityById(int id)
        {
            return Repository.GetAsync(id);
        }
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMgQuery)]
        public Task<FinshedEnterStore> GetEntityByNo(string no)
        {
            if (KeyFiledName.IsNullOrEmpty())
            {
                CheckErrors(IwbIdentityResult.Failed("编码/编号字段不明确，请检查后再操作！"));
            }
            LambdaObject obj = new LambdaObject()
            {
                FieldType = LambdaFieldType.S,
                FieldName = KeyFiledName,
                FieldValue = no,
                ExpType = LambdaExpType.Equal
            };
            var exp = obj.GetExp<FinshedEnterStore>();
            return Repository.FirstOrDefaultAsync(exp);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMgQuery)]
        public async Task<FinshedEnterStoreDto> GetDtoById(int id)
        {
            var entity = await GetEntityById(id);
            return MapToEntityDto(entity);
        }
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMgQuery)]
        public async Task<FinshedEnterStoreDto> GetDtoByNo(string no)
        {
            var entity = await GetEntityByNo(no);
            return MapToEntityDto(entity);
        }

        #endregion

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMg)]
        public override async Task<PagedResultDto<FinshedEnterStoreDto>> GetAll(PagedRequestDto input)
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
                var exp = objList.GetExp<FinshedEnterStore>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<FinshedEnterStoreDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
            return dtos;
        }
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMg)]
        public  async Task<PagedResultDto<ViewProductEnterStore>> GetAllView(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = ViewProductEnterStoreRepository.GetAll();
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
                var exp = objList.GetExp<ViewProductEnterStore>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(i => i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<ViewProductEnterStore>(
                totalCount,
                entities
            );
            return dtos;
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMgUpdate), AuditLog("通过申请")]
        public async Task<FinshedEnterStoreDto> Audit(FinshedEnterStoreAuditDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == FinshedEnterStoreApplyStatusEnum.EnterStored.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("已入库不能再操作!"));
            }
            //包含库位信息
            if (!input.StoreLocationNo.IsNullOrEmpty())
            {
                //检查当前库位是否存在盘点信息
                var isCanChange = CommonAppService.CheckStoreCanUpdateByLocationNo(input.StoreLocationNo, 1);
                if (!isCanChange)
                {
                    CheckErrors(IwbIdentityResult.Failed("该库存库位处于退货或者正在盘点状态,不可进行出入库更新!"));
                }
            }
            var createSourceType = input.CreateSourceType??0;
            if (createSourceType == EnterStoreCreateSourceEnum.Balance.ToInt()|| createSourceType == EnterStoreCreateSourceEnum.AntiPlating.ToInt() || createSourceType == EnterStoreCreateSourceEnum.NormalReturnGood.ToInt() || createSourceType == EnterStoreCreateSourceEnum.Downgrade.ToInt())
            {
                entity.ApplyStatus = FinshedEnterStoreApplyStatusEnum.EnterStored.ToInt();
                var date = Clock.Now;
                var currentStore = CurrentProductStoreHouseRepository.GetAll().FirstOrDefault(i => i.ProductionOrderNo == entity.ProductionOrderNo && i.StoreHouseId == entity.StoreHouseId&&i.StoreLocationNo == input.StoreLocationNo && i.ProductNo == entity.ProductNo);
                decimal enterQuantity = input.ActualPackageCount * input.PackageSpecification;
                if (currentStore != null)
                {
                    var isCanUpdate =
                        CommonAppService.CheckStoreRecordCanUpdate(currentStore.CurrentProductStoreHouseNo);
                    if (!isCanUpdate)
                        CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
                    currentStore.Quantity += enterQuantity;
                    currentStore.TimeLastMod = date;
                    currentStore.UserIDLastMod = AbpSession.UserName;
                    currentStore.KgWeight = entity.KgWeight;
                    await CurrentProductStoreHouseRepository.UpdateAsync(currentStore);
                }
                else
                {
                    string lcJson = JsonConvert.SerializeObject(entity);
                    currentStore = lcJson.GetModel<CurrentProductStoreHouse>();
                    currentStore.CurrentProductStoreHouseNo = Guid.NewGuid().ToString("N");
                    currentStore.TimeCreated = date;
                    currentStore.UserIDLastMod = AbpSession.UserName;
                    currentStore.Quantity = enterQuantity;
                    currentStore.FreezeQuantity = 0;
                    currentStore.StoreLocationNo = input.StoreLocationNo;
                    await CurrentProductStoreHouseRepository.InsertAsync(currentStore);
                }
                entity.AuditDate = Clock.Now;
                entity.ActualPackageCount = input.ActualPackageCount;
                entity.PackageSpecification = input.PackageSpecification;
                entity.StoreLocationNo = input.StoreLocationNo;
                entity.Quantity = enterQuantity;
                entity.AuditUser = AbpSession.UserName;
                entity.EnterStoreDate = Clock.Now;
                entity.EnterStoreUser = AbpSession.UserName;
                //string enterCreateSourceName = "";
                string enterCreateSourceName =
                    StatesAppService.GetDisplayValue("FinshedEnterStore", "CreateSourceType", createSourceType + "");
                BusinessLogTypeEnum.PStore.WriteLog(LogRepository, $"{enterCreateSourceName}入库确认",
                    $"{enterCreateSourceName}入库确认通过,数量{entity.ActualPackageCount}",
                    entity.ProductionOrderNo,logExt2: currentStore.CurrentProductStoreHouseNo);
            }
            else
            {
                entity.ApplyStatus = FinshedEnterStoreApplyStatusEnum.Audited.ToInt();
                entity.AuditDate = Clock.Now;
                entity.ActualPackageCount = input.ActualPackageCount;
                entity.StoreLocationNo = input.StoreLocationNo;
                entity.Quantity = input.PackageSpecification * input.ActualPackageCount;
                entity.AuditUser = AbpSession.UserName;
                BusinessLogTypeEnum.PStore.WriteLog(LogRepository, "成品入库申请审核",
                    $"成品入库申请审核通过,审核数量{entity.ActualPackageCount}",
                    entity.ProductionOrderNo);
            }
           
            await Repository.UpdateAsync(entity);
            return MapToEntityDto(entity);

        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMgRefuse), AuditLog("拒绝申请")]
        public async Task<FinshedEnterStoreDto> Refuse(EntityDto<int> input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == FinshedEnterStoreApplyStatusEnum.EnterStored.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("已入库不能再操作!"));
            }
            entity.AuditDate = Clock.Now;
            entity.AuditUser = AbpSession.UserName;
            entity.ApplyStatus = FinshedEnterStoreApplyStatusEnum.Refused.ToInt();
            await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.PStore.WriteLog(LogRepository, "成品入库申请审核",
                $"拒绝成品入库申请",
                entity.ProductionOrderNo);
           
            return MapToEntityDto(entity);
        }

        //public async Task AuditBatch(List<EntityDto> inputs)
        //{
        //    if (inputs.Any())
        //    {
        //    var date = Clock.Now;
        //        foreach (var input in inputs)
        //        {
        //            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
        //            if (entity.IsClose|| entity.ApplyStatus != FinshedEnterStoreApplyStatusEnum.Applying.ToInt())
        //            {
        //                continue;
        //            }
        //            entity.AuditDate = date;
        //            entity.ActualPackageCount = entity.PackageCount;
        //            entity.StoreLocationNo = input.StoreLocationNo;
        //            entity.Quantity = entity.PackageSpecification * entity.ActualPackageCount;
        //            entity.AuditUser = AbpSession.UserName;
        //            entity.ApplyStatus = FinshedEnterStoreApplyStatusEnum.Audited.ToInt();
        //            entity.AuditDate = date;
        //            entity.AuditUser = AbpSession.UserName;
        //            await Repository.UpdateAsync(entity);
        //        }
        //    }
        //}


        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMgQuery)]
        public async Task<PagedResultDto<ViewProductEnterStore>> GetViewAll(PagedRequestDto input)
        {

            var query = ViewProductEnterStoreRepository.GetAll().Where(i=>i.IsClose==false);

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
                var exp = objList.GetExp<ViewProductEnterStore>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(i => i.ApplyEnterDate);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<ViewProductEnterStore>(
                totalCount,
                entities
            );
            return dtos;
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMgUpdate), AuditLog("确认入库")]
        public async Task<FinshedEnterStore> ConfirmEnterStoreQuantity(EntityDto<int> input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == EnterStoreApplyStatusEnum.EnterStored.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("已入库不能再操作!"));
            }

            entity.ApplyStatus = EnterStoreApplyStatusEnum.EnterStored.ToInt();
            var date = Clock.Now;
            entity.EnterStoreDate = date;
            entity.EnterStoreUser = AbpSession.UserName;
            await Repository.UpdateAsync(entity);
            var currentStore = CurrentProductStoreHouseRepository.GetAll().FirstOrDefault(i => i.ProductionOrderNo == entity.ProductionOrderNo && i.StoreLocationNo == entity.StoreLocationNo && i.ProductNo== entity.ProductNo);
            if (currentStore != null)
            {
                currentStore.Quantity += entity.Quantity;
                currentStore.TimeLastMod = date;
                currentStore.UserIDLastMod = AbpSession.UserName;
                currentStore.KgWeight = entity.KgWeight;
                await CurrentProductStoreHouseRepository.UpdateAsync(currentStore);
            }
            else
            {
                string lcJson = JsonConvert.SerializeObject(entity);
                currentStore = lcJson.GetModel<CurrentProductStoreHouse>();
                currentStore.CurrentProductStoreHouseNo = Guid.NewGuid().ToString("N");
                currentStore.TimeCreated = date;
                currentStore.UserIDLastMod = AbpSession.UserName;
                currentStore.Quantity = entity.Quantity;
                currentStore.FreezeQuantity = 0;
                await CurrentProductStoreHouseRepository.InsertAsync(currentStore);
            }
            //取消确认入库自动变更排查单状态
            /*var productionOrder =
                await Repository.FirstOrDefaultAsync(a => a.ProductionOrderNo == entity.ProductionOrderNo);
            if (productionOrder == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现排产单!"));
                return null;
            }
            productionOrder.ProductionOrderStatus = ProductionOrderStatusEnum.EnterStore.ToInt();
            await Repository.UpdateAsync(productionOrder);*/
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "成品入库",
                 $"确认成品入库数量[{entity.Id}],排产单入库信息：[确认数量:{entity.Quantity},库存:{currentStore.Quantity},冻结:{currentStore.FreezeQuantity}]",
                 entity.ProductionOrderNo);
            return entity;
        }

        #endregion


    }
}
