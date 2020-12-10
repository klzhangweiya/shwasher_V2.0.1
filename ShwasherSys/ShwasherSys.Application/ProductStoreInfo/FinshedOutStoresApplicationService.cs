using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Timing;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.IdentityFramework;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.Common;
using ShwasherSys.Lambda;
using ShwasherSys.PackageInfo;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ProductInfo;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.SemiProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo.Dto;

namespace ShwasherSys.ProductStoreInfo
{
    [AbpAuthorize]
    public class FinshedOutStoreAppService : ShwasherAsyncCrudAppService<ProductOutStore, ProductOutStoreDto, int, PagedRequestDto, ProductOutStoreCreateDto, ProductOutStoreUpdateDto >,IFinshedOutStoreAppService
    {
        public IRepository<PackageApply> PackageApplyRepository { get; }
        public IRepository<Product ,string> ProductRepository { get; }
        public ICommonAppService CommonAppService { get; }
        public IRepository<BusinessLog> LogRepository { get; }
        public IRepository<ViewProductOutStore> ViewProductOutStoreRepository;
        public IRepository<CurrentProductStoreHouse> CurrentProductStoreHouseRepository;
        public FinshedOutStoreAppService(IRepository<ProductOutStore, int> repository, IRepository<ViewProductOutStore> viewProductOutStoreRepository, IRepository<CurrentProductStoreHouse> currentProductStoreHouseRepository, IRepository<PackageApply> packageApplyRepository,
            IRepository<BusinessLog> logRepository, IRepository<Product, string> productRepository, ICommonAppService commonAppService) : base(repository)
        {
            PackageApplyRepository = packageApplyRepository;
            LogRepository = logRepository;
            ProductRepository = productRepository;
            CommonAppService = commonAppService;
            ViewProductOutStoreRepository = viewProductOutStoreRepository;
            CurrentProductStoreHouseRepository = currentProductStoreHouseRepository;
        }

		protected override string GetPermissionName { get; set; } //= PermissionNames.PagesProductOutStore;
		protected override string GetAllPermissionName { get; set; } //= PermissionNames.PagesProductOutStore;
		protected override string CreatePermissionName { get; set; } //= PermissionNames.PagesProductOutStoreCreate;
		protected override string UpdatePermissionName { get; set; } //= PermissionNames.PagesProductOutStoreUpdate;
		protected override string DeletePermissionName { get; set; } //= PermissionNames.PagesProductOutStoreDelete;

        [AbpAuthorize(new []{ PermissionNames.PagesFinshedStoreInfoFinshedOutStoreApplyMgQuery, PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreMgQuery })]
        public async Task<PagedResultDto<ViewProductOutStore>> GetViewAll(PagedRequestDto input)
        {
            var query = ViewProductOutStoreRepository.GetAll().Where(i => i.IsClose == false);

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
                var exp = objList.GetExp<ViewProductOutStore>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(i => i.ApplyOutDate);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<ViewProductOutStore>(
                totalCount,
                entities
            );
            return dtos;
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedOutStoreApplyMgUpdate), AuditLog("审核通过")]
        public async Task<ProductOutStoreDto> Audit(ProductOutStoreAuditDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == FinshedOutStoreApplyStatusEnum.OutStored.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("已出库不能再操作!"));
            }
            var isCanUpdate =
                CommonAppService.CheckStoreRecordCanUpdate(entity.CurrentProductStoreHouseNo);
            if (!isCanUpdate)
                CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));

            var currentStore = await
                CurrentProductStoreHouseRepository.FirstOrDefaultAsync(i =>
                    i.CurrentProductStoreHouseNo == entity.CurrentProductStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }
            if (currentStore.Quantity < input.ActualQuantity)
            {
                CheckErrors(IwbIdentityResult.Failed("可用库存量不够，请检查出库数量!"));
                return null;
            }

            currentStore.FreezeQuantity -= entity.Quantity;
            currentStore.Quantity -= input.ActualQuantity;
            await CurrentProductStoreHouseRepository.UpdateAsync(currentStore);
            var date = Clock.Now;
            entity.OutStoreDate = date;
            entity.OutStoreUser = AbpSession.UserName;
            entity.ApplyStatus = FinshedOutStoreApplyStatusEnum.OutStored.ToInt();
            entity.ActualQuantity = input.ActualQuantity;
            entity.IsConfirm = false;
            entity.AuditDate = date;
            entity.AuditUser = AbpSession.UserName;
            await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.PStore.WriteLog(LogRepository, "成品出库",
                $"审核通过成品出库申请[{entity.Id}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<ProductOutStoreDto>(entity);
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedOutStoreApplyMgRefuse), AuditLog("拒绝申请")]
        public async Task<ProductOutStoreDto> Refuse(EntityDto<int> input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == FinshedOutStoreApplyStatusEnum.OutStored.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("已出库不能再操作!"));
            }
            var currentStore = await
                CurrentProductStoreHouseRepository.FirstOrDefaultAsync(i => i.CurrentProductStoreHouseNo == entity.CurrentProductStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }
            currentStore.FreezeQuantity -= entity.Quantity;
            await CurrentProductStoreHouseRepository.UpdateAsync(currentStore);
            entity.ApplyStatus = FinshedOutStoreApplyStatusEnum.Refused.ToInt();
            await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.PStore.WriteLog(LogRepository, "成品出库",
                $"拒绝成品出库申请[{entity.Id}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<ProductOutStoreDto>(entity);
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedOutStoreApplyMgRecovery), AuditLog("恢复申请")]
        public async Task<ProductOutStoreDto> Recovery(EntityDto<int> input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus != FinshedOutStoreApplyStatusEnum.Refused.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("该状态下不能恢复!"));
            }
            var currentStore = await
                CurrentProductStoreHouseRepository.FirstOrDefaultAsync(i => i.CurrentProductStoreHouseNo == entity.CurrentProductStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }
            currentStore.FreezeQuantity += entity.Quantity;
            await CurrentProductStoreHouseRepository.UpdateAsync(currentStore);
            entity.ApplyStatus = OutStoreApplyStatusEnum.Applying.ToInt();
            await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.PStore.WriteLog(LogRepository, "成品出库",
                $"恢复成品出库申请[{entity.Id}]",
                entity.ProductionOrderNo);
           
            return ObjectMapper.Map<ProductOutStoreDto>(entity);
        }

        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedOutStoreApplyMgBatchAudit), AuditLog("批量审核")]
        public async Task AuditBatch(ProductOutStoreBatchAuditDto input)
        {
            if (input.Ids.Any())
            {
                var date = Clock.Now;
                foreach (var id in input.Ids)
                {
                    var entity = await Repository.FirstOrDefaultAsync(a => a.Id == id);
                    if (entity.IsClose || entity.ApplyStatus != FinshedOutStoreApplyStatusEnum.Applying.ToInt())
                    {
                        continue;
                    }
                    var currentStore = await
                        CurrentProductStoreHouseRepository.FirstOrDefaultAsync(i =>
                            i.CurrentProductStoreHouseNo == entity.CurrentProductStoreHouseNo);
                    if (currentStore == null)
                    {
                        CheckErrors(IwbIdentityResult.Failed($"[{entity.ProductionOrderNo}]未发现库存!"));
                        return ;
                    }
                    if (currentStore.Quantity < entity.Quantity)
                    {
                        CheckErrors(IwbIdentityResult.Failed($"[{entity.ProductionOrderNo}(库位：{currentStore.StoreLocationNo})]可用库存量不够，请检查出库数量!"));
                        return ;
                    }
                    currentStore.FreezeQuantity -= entity.Quantity;
                    currentStore.Quantity -= entity.Quantity;
                    await CurrentProductStoreHouseRepository.UpdateAsync(currentStore);
                    entity.OutStoreDate = date;
                    entity.OutStoreUser = AbpSession.UserName;
                    entity.ApplyStatus = FinshedOutStoreApplyStatusEnum.OutStored.ToInt();
                    entity.ActualQuantity = entity.Quantity;
                    entity.IsConfirm = false;
                    entity.AuditDate = date;
                    entity.AuditUser = AbpSession.UserName;
                    await Repository.UpdateAsync(entity);
                   
                }
                BusinessLogTypeEnum.PStore.WriteLog(LogRepository, "成品出库",
                    $"批量审核通过成品出库申请[{input.Obj2String()}]");
            }

        }

        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgPackageApply), AuditLog("申请包装")]
        public async Task<string> PackageApply(ProductOutStoreDto input)
        {
            var currentStore =
                CurrentProductStoreHouseRepository.FirstOrDefault(i =>
                    i.CurrentProductStoreHouseNo == input.CurrentProductStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return "";
            }
            var canUseQuantit = currentStore.Quantity - currentStore.FreezeQuantity;
            if (canUseQuantit < input.Quantity)
            {
                CheckErrors(IwbIdentityResult.Failed("可用库存量不够，请检查出库数量!"));
                return "";
            }
            //currentStore.FreezeQuantity += input.Quantity;
            currentStore.Quantity -= input.Quantity;
            await CurrentProductStoreHouseRepository.UpdateAsync(currentStore);
            input.ApplyOutDate = Clock.Now;
            var entity = ObjectMapper.Map<ProductOutStore>(input);
            var date = Clock.Now;
           
            entity.StoreHouseId = currentStore.StoreHouseId;
            entity.ApplyOutStoreSourceType = OutStoreApplyTypeEnum.Package.ToInt();
            //entity.ApplyTypes = OutStoreApplyTypeEnum.Package.ToInt();
            entity.ApplyStatus = OutStoreApplyStatusEnum.OutStored.ToInt();
            entity.IsConfirm = false;
            entity.ActualQuantity = input.Quantity;
            entity.AuditDate = date;
            entity.AuditUser = AbpSession.UserName;
            entity.OutStoreDate = date;
            entity.OutStoreUser = AbpSession.UserName;
            entity.TimeCreated = date;
            entity.CreatorUserId = AbpSession.UserName;
            entity.TimeLastMod = date;
            entity.UserIDLastMod = AbpSession.UserName;
            await Repository.InsertAsync(entity);
            var packageApply = new PackageApply()
            {
                PackageApplyNo = Guid.NewGuid().ToString("N"),
                ProductionOrderNo = entity.ProductionOrderNo,
                ProductNo = entity.ProductNo,
                SourceStore = currentStore.StoreHouseId,
                CurrentSemiStoreHouseNo = entity.CurrentProductStoreHouseNo,
                ApplyQuantity = entity.ActualQuantity,
                ApplyStatus = OutStoreApplyStatusEnum.Applying.ToInt() + "",
                ApplyDate = date,
                TimeCreated = date,
                CreatorUserId = AbpSession.UserName,
                TimeLastMod = date,
                UserIDLastMod = AbpSession.UserName,
            };
            packageApply = await PackageApplyRepository.InsertAsync(packageApply);

            BusinessLogTypeEnum.PStore.WriteLog(LogRepository, "成品出库",
                $"创建出库申请,确认出库包装产品,信息为:[{entity.Obj2String()}]",
                entity.ProductionOrderNo, packageApply.PackageApplyNo, entity.CurrentProductStoreHouseNo);
            return ExportPackageBill(input);
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMgPackageApply), AuditLog("申请改镀")]
        public async Task<ProductOutStore> RePlating(ProductOutStoreDto input)
        {
            var currentStore =
                CurrentProductStoreHouseRepository.FirstOrDefault(i =>
                    i.CurrentProductStoreHouseNo == input.CurrentProductStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }
            var canUseQuantit = currentStore.Quantity - currentStore.FreezeQuantity;
            if (canUseQuantit < input.Quantity)
            {
                CheckErrors(IwbIdentityResult.Failed("可用库存量不够，请检查出库数量!"));
                return null;
            }
            currentStore.FreezeQuantity += input.Quantity;
            //currentStore.Quantity -= input.Quantity;
            await CurrentProductStoreHouseRepository.UpdateAsync(currentStore);
            input.ApplyOutDate = Clock.Now;
            var entity = ObjectMapper.Map<ProductOutStore>(input);
            var date = Clock.Now;

            entity.StoreHouseId = currentStore.StoreHouseId;
            entity.ApplyOutStoreSourceType = OutStoreApplyTypeEnum.RePlating.ToInt();
            //entity.ApplyTypes = OutStoreApplyTypeEnum.Package.ToInt();
            entity.ApplyStatus = OutStoreApplyStatusEnum.Applying.ToInt();
            entity.IsConfirm = false;
            entity.ActualQuantity = input.Quantity;
            entity.AuditDate = date;
            entity.AuditUser = AbpSession.UserName;
            entity.OutStoreDate = date;
            entity.OutStoreUser = AbpSession.UserName;
            entity.TimeCreated = date;
            entity.CreatorUserId = AbpSession.UserName;
            entity.TimeLastMod = date;
            entity.UserIDLastMod = AbpSession.UserName;
            var productOut = await Repository.InsertAsync(entity);

            BusinessLogTypeEnum.PStore.WriteLog(LogRepository, "成品改镀,成品出库",
                $"创建出库申请,确认出库产品,信息为:[{entity.Obj2String()}]",
                entity.ProductionOrderNo,  entity.CurrentProductStoreHouseNo);
            return productOut;
        }
        private string ExportPackageBill(ProductOutStoreDto input)
        {
            var productInfo = ProductRepository.FirstOrDefault(i => i.Id == input.ProductNo);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\PackageBillTemplate\\成品入库单.xls") ;
            var savePath = "Download/Excel/PackageBill";
            var work = ExcelHelper.CreateWorkBook03(path);
            var sheet1 = work.GetSheet("Sheet1");
            var billNo = CommonAppService.GetAppGuid(AppGuidType.PackageEnterBill);
            sheet1.GenerateCell(2, 1).SetValue("单号：" + DateTime.Now.ToString("yy")+(billNo ?? 0));
            sheet1.GenerateCell(9, 1).SetValue("1");
            sheet1.GenerateCell(9, 2).SetValue("");
            sheet1.GenerateCell(9, 3).SetValue(productInfo?.ProductName??"");
            sheet1.GenerateCell(9, 4).SetValue(productInfo?.Model ?? "");
            sheet1.GenerateCell(9, 5).SetValue(productInfo?.Material ?? "");
            sheet1.GenerateCell(9, 6).SetValue(productInfo?.SurfaceColor ?? "");
            sheet1.GenerateCell(9, 7).SetValue(productInfo?.Rigidity?? "改包装通知单");
            sheet1.GenerateCell(9, 8).SetValue("");
            sheet1.GenerateCell(9, 9).SetValue<decimal>(input?.Quantity ?? 0);
            sheet1.GenerateCell(9, 10).SetValue(input?.ProductionOrderNo);
            //sheet1.GenerateCell(5, 10).SetValue(input?.ProductionOrderNo);
            var fileName = $"改包装通知单-{Clock.Now:yyMMddHHmmss}.xls";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }

    }
}
