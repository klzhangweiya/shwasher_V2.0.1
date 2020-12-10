using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using IwbZero.Helper;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.Common;
using ShwasherSys.CompanyInfo;
using ShwasherSys.FinshedStoreInfo.Dto;
using ShwasherSys.Lambda;
using ShwasherSys.PackageInfo.Dto;
using ShwasherSys.ProductInfo;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo;
using LambdaExpType = ShwasherSys.Lambda.LambdaExpType;
using LambdaFieldType = ShwasherSys.Lambda.LambdaFieldType;
using LambdaObject = ShwasherSys.Lambda.LambdaObject;

namespace ShwasherSys.PackageInfo
{
    [AbpAuthorize, AuditLog("产品包装信息")]
    public class PackInfoApplyAppService : ShwasherAsyncCrudAppService<PackageApply, PackageApplyDto, int, PagedRequestDto, PackageApplyCreateDto, PackageApplyUpdateDto >
        , IPackInfoApplyAppService
    {
        public PackInfoApplyAppService(
            IRepository<CurrentProductStoreHouse> finshedCurrentStoreRepository,
            IRepository<FinshedEnterStore> finshedEnterStoreRepository,
            IRepository<SemiEnterStore> semiEnterStoreRepository,
            IRepository<SemiOutStore> semiOutStoreRepository,
            IRepository<CurrentSemiStoreHouse> currentSemiStoreHouseRepository,
            IRepository<BusinessLog> logRepository,
            IRepository<Product, string> productRepository,
            IIwbSettingManager settingManager,
            ICacheManager cacheManager,
			IRepository<PackageApply, int> repository, IRepository<SemiProducts, string> semiProductsRepository, IRepository<ViewPackageApply> viewPackageApplyRepository, IRepository<EmployeeWorkPerformance> performanceRepository, ICommonAppService commonAppService, IRepository<ViewProductEnterStore> viewProductEnterStoreRepository) : base(repository, "PackageApplyNo")
        {
			SettingManager = settingManager;
            CacheManager = cacheManager;
            FinshedCurrentStoreRepository = finshedCurrentStoreRepository;
            FinshedEnterStoreRepository = finshedEnterStoreRepository;
            SemiEnterStoreRepository = semiEnterStoreRepository;
            SemiOutStoreRepository = semiOutStoreRepository;
            CurrentSemiStoreHouseRepository = currentSemiStoreHouseRepository;
            LogRepository = logRepository;
            ProductRepository = productRepository;
            SemiProductsRepository = semiProductsRepository;
            ViewPackageApplyRepository = viewPackageApplyRepository;
            PerformanceRepository = performanceRepository;
            CommonAppService = commonAppService;
            ViewProductEnterStoreRepository = viewProductEnterStoreRepository;
        }

        protected override bool KeyIsAuto { get; set; } = true;
        public IRepository<EmployeeWorkPerformance> PerformanceRepository { get; }
        public IRepository<CurrentProductStoreHouse> FinshedCurrentStoreRepository { get; }
        public IRepository<FinshedEnterStore> FinshedEnterStoreRepository { get; }
        public IRepository<ViewProductEnterStore> ViewProductEnterStoreRepository { get; }

        public IRepository<SemiEnterStore> SemiEnterStoreRepository { get; }
        public IRepository<SemiOutStore> SemiOutStoreRepository { get; }
        public IRepository<CurrentSemiStoreHouse> CurrentSemiStoreHouseRepository { get; }
        public IRepository<BusinessLog> LogRepository { get; }
        public IRepository<Product, string> ProductRepository { get; }
        public IRepository<SemiProducts, string> SemiProductsRepository { get; }
        public IRepository<ViewPackageApply> ViewPackageApplyRepository { get; }

        public ICommonAppService CommonAppService { get; }

        #region GetSelect

        [DisableAuditing]
        public async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var slist = new List<SelectListItem> {new SelectListItem {Text = @"请选择...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.ProductionOrderNo, Value = l.ProductionOrderNo });
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
                str += $"<option value=\"{l.ProductionOrderNo}\">{l.ProductionOrderNo}</option>";
            }
            return str;
        }
     
        #endregion

        #region CURD
        
		#region Get

		[DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgQuery)]
        public  Task<PackageApply> GetEntityById(int id)
        {
            return Repository.GetAsync(id);
        }
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgQuery)]
        public  Task<PackageApply> GetEntityByNo(string no)
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
            var exp = obj.GetExp<PackageApply>();
            return Repository.FirstOrDefaultAsync(exp);
        }

		[DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgQuery)]
        public  async Task<PackageApplyDto> GetDtoById(int id)
        {
            var entity = await GetEntityById(id);
            return MapToEntityDto(entity);
        }
		[DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgQuery)]
        public  async Task<PackageApplyDto> GetDtoByNo(string no)
        {
            var entity = await GetEntityByNo(no);
            return MapToEntityDto(entity);
        }

        #endregion

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgQuery)]
        public override async Task<PagedResultDto<PackageApplyDto>> GetAll(PagedRequestDto input)
        {
            var query = ViewPackageApplyRepository.GetAll();
            var packType = "";
            var querySemiProduct = SemiProductsRepository.GetAll();
            var queryFinshedProduct = ProductRepository.GetAll();
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    if (o.KeyField != "PackType") continue;
                    packType = o.KeyWords;
                    continue;
                }
            }
            IQueryable<PackageApplyDto> queryEntity=null;
            if (packType == "1")
            {
                queryEntity = from ispec in query
                    join sp in querySemiProduct on ispec.SemiProductNo equals sp.Id into isp
                    from unall in isp.DefaultIfEmpty()
                    select new PackageApplyDto
                    {
                        ActualQuantity = ispec.ActualQuantity,
                        ApplyStatus = ispec.ApplyStatus,
                        ApplyQuantity = ispec.ApplyQuantity,
                        CreatorUserId = ispec.CreatorUserId,
                        ApplyDate = ispec.ApplyDate,
                        TimeCreated = ispec.TimeCreated,
                        PackType=1,
                        CurrentSemiStoreHouseNo = ispec.CurrentSemiStoreHouseNo,
                        Id = ispec.Id,
                        IsClose = ispec.IsClose,
                        ProductName = unall.SemiProductName??"",
                        Material = unall.Material ?? "",
                        Model = unall.Model ?? "",
                        Rigidity = unall.Rigidity ?? "",
                        SurfaceColor = unall.SurfaceColor ?? "",
                        PartNo = unall.PartNo ?? "",
                        TimeLastMod = ispec.TimeLastMod,
                        UserIDLastMod = ispec.UserIDLastMod,
                        PackageApplyNo = ispec.PackageApplyNo,
                        ProductionOrderNo = ispec.ProductionOrderNo,
                        Remark = ispec.Remark,
                        SemiProductNo = ispec.SemiProductNo,
                        ProcessingNum = ispec.ProcessingNum,
                        RemainApplyQuantity = (ispec.ApplyQuantity - (ispec.IsApplyEnterQuantity ?? 0)),
                        IsApplyEnterQuantity = ispec.IsApplyEnterQuantity,
                        KgWeight = ispec.KgWeight??0
                    };
            }
            else 
            {
                queryEntity = from ispec in query
                    join sp in queryFinshedProduct on ispec.ProductNo equals sp.Id into isp
                    from unall in isp.DefaultIfEmpty()
                    select new PackageApplyDto
                    {
                        ActualQuantity = ispec.ActualQuantity,
                        ApplyStatus = ispec.ApplyStatus,
                        ApplyQuantity = ispec.ApplyQuantity,
                        CreatorUserId = ispec.CreatorUserId,
                        ApplyDate = ispec.ApplyDate,
                        TimeCreated = ispec.TimeCreated,
                        PackType=2,
                        CurrentSemiStoreHouseNo = ispec.CurrentSemiStoreHouseNo,
                        Id = ispec.Id,
                        IsClose = ispec.IsClose,
                        ProductName = unall.ProductName ?? "",
                        Material = unall.Material ?? "",
                        Model = unall.Model ?? "",
                        Rigidity = unall.Rigidity ?? "",
                        SurfaceColor = unall.SurfaceColor ?? "",
                        PartNo = unall.PartNo ?? "",
                        TimeLastMod = ispec.TimeLastMod,
                        UserIDLastMod = ispec.UserIDLastMod,
                        PackageApplyNo = ispec.PackageApplyNo,
                        ProductionOrderNo = ispec.ProductionOrderNo,
                        Remark = ispec.Remark,
                        ProcessingNum = ispec.ProcessingNum,
                        ProductNo = ispec.ProductNo,
                        RemainApplyQuantity = (ispec.ApplyQuantity- (ispec.IsApplyEnterQuantity2??0)),
                        IsApplyEnterQuantity = ispec.IsApplyEnterQuantity2,
                        KgWeight = ispec.KgWeight??0

                    };
                
            }
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {

                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    if (o.KeyField == "PackType")
                    {
                        packType = o.KeyWords;
                        queryEntity = packType == "1"
                            ? queryEntity.Where(a => !string.IsNullOrEmpty(a.SemiProductNo))
                            : queryEntity.Where(a => !string.IsNullOrEmpty(a.ProductNo));
                        continue;
                    }
                    object keyWords = o.KeyWords;
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }

                if (objList.Any())
                {
                    var exp = objList.GetExp<PackageApplyDto>();
                    queryEntity = queryEntity.Where(exp);
                }

            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(queryEntity);
            queryEntity = queryEntity.OrderByDescending(i => i.TimeCreated);
            queryEntity = queryEntity.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = await AsyncQueryableExecuter.ToListAsync(queryEntity);
            var dtos = new PagedResultDto<PackageApplyDto>(
                totalCount,
                entities
            );
            return dtos;
        }


        /// <summary>
        /// 确认包装申请，创建包装明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgCreate), AuditLog("确认包装申请")]
        public async Task CreatePackInfo(CreatePackInfosDto input)
        {
            if (input.PackageInfos!=null && input.PackageInfos.Any())
            {
               
                //var entity = await Repository.FirstOrDefaultAsync(a => a.PackageApplyNo == input.PackageApplyNo);
                var entity = Repository.FirstOrDefault(i => i.PackageApplyNo == input.PackageApplyNo);
                if (entity==null)
                {
                    CheckErrors(IwbIdentityResult.Failed("未发现包装申请!"));
                    return;
                }
                ////半成品检验下是否存在同批次的其它产品入库，成品改包装暂时取消校验
                //if (input.PackType == 1)
                //{
                //    var preExistEnter = await GetProductionOrderHasCreate(input.ProductionOrderNo);
                //    if (preExistEnter != null && preExistEnter.ProductNo != input.ProductNo)
                //    {
                //        CheckErrors(IwbIdentityResult.Failed("此流转单号已有其它产品入库!"));
                //        return;
                //    }
                //}
               
                entity.ApplyStatus = PackageApplyStatusEnum.Audited.ToInt() + "";
                await Repository.UpdateAsync(entity);
                var date = Clock.Now;
                foreach (var info in input.PackageInfos)
                {
                    var f = new FinshedEnterStore()
                    {
                        PackageApplyNo = input.PackageApplyNo,
                        ProductionOrderNo = input.ProductionOrderNo,
                        PackageProductNo = input.PackageProductNo,
                        ProductNo= input.ProductNo,
                        SourceStoreHouseId = entity.SourceStore,
                        StoreHouseId = 1,
                        ApplyQuantity = info.ActualQuantity,
                        ApplyQuantity2 = info.ActualQuantity2,
                        PackageCount = info.PackageCount,
                        PackageSpecification = info.PackageSpecification,
                        Quantity = info.PackageCount * info.PackageSpecification,
                        PackageEnterNum = info.PackageEnterNum,
                        ActualPackageCount = 0,
                        ApplyStatus = FinshedEnterStoreApplyStatusEnum.New.ToInt(),
                        ApplySourceType= input.PackType,
                        IsClose = false,
                        ApplyEnterDate = date,
                        KgWeight = info.KgWeight,
                        VerifyUser = info.VerifyUser,
                        PackageUser = info.PackageUser,
                        TimeLastMod = date,
                        TimeCreated = date,
                        CreatorUserId = AbpSession.UserName,
                        UserIDLastMod = AbpSession.UserName,
                    };
                    var id=  await FinshedEnterStoreRepository.InsertAndGetIdAsync(f);
                    await CreatePerformance(info, entity.ProductionOrderNo,id);
                    await CreatePerformance(info, entity.ProductionOrderNo,id,false);
                }

                BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                    $"确认包装,分配包装规格信息为:[{input.Obj2String()}]",
                    entity.ProductionOrderNo,entity.PackageApplyNo);
            }
            else
            {
                CheckErrors(IwbIdentityResult.Failed("未发现包装信息!"));
            }
           
        }

        private async Task CreatePerformance(PackInfoDto info,string productOrderNo,int id,bool isVerify=true)
        {
            var p = new EmployeeWorkPerformance()
            {
                PerformanceNo =await WorkTypeDefinition.GetPerformanceNo(PerformanceRepository,isVerify?WorkTypeDefinition.VerifyPackage : WorkTypeDefinition.Package),
                ProductOrderNo = productOrderNo,
                EmployeeId =isVerify? info.VerifyUserId:info.PackageUserId,
                WorkType = isVerify ? WorkTypeDefinition.VerifyPackage : WorkTypeDefinition.Package,
                RelatedNo = id+"",
                Performance = info.ActualQuantity2,
                PerformanceUnit = "千件",
                PerformanceDesc =
                    $"{(isVerify ? $"{info.VerifyUser}核件" : $"{info.PackageUser}负责包装")}{info.ActualQuantity2}千件,总重:{info.ActualQuantity}kg,千件重：{info.KgWeight}",
            };
            await PerformanceRepository.InsertAsync(p);
            await CurrentUnitOfWork.SaveChangesAsync();

        }
       

        /// <summary>
        /// 拒绝包装申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgRefuse), AuditLog("拒绝包装申请")]
        public async Task RefusePackInfoApply(RefusePackInfoDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus != PackageApplyStatusEnum.Applying.ToInt()+"")
            {
                CheckErrors(IwbIdentityResult.Failed("申请已被审核(或拒绝),不能再操作!"));
            }
            
            var date = Clock.Now;
            if (input.PackType==1)
            {
                SemiEnterStore semiEnterStore = new SemiEnterStore
                {
                    ApplyEnterDate = Clock.Now,
                    ApplySource = EnterStoreApplySourceEnum.RefusePackage.ToInt().ToString(),
                    ApplyStatus = EnterStoreApplyStatusEnum.EnterStored.ToInt().ToString(),
                    ProductionOrderNo = entity.ProductionOrderNo,
                    SemiProductNo = entity.SemiProductNo,
                    Quantity = entity.ApplyQuantity,
                    ActualQuantity = entity.ApplyQuantity,
                    EnterStoreDate = date,
                    AuditDate = date,
                    EnterStoreUser = AbpSession.UserName,
                    AuditUser = AbpSession.UserName,
                    TimeCreated = date,
                    CreatorUserId = AbpSession.UserName,
                    TimeLastMod = date,
                    UserIDLastMod = AbpSession.UserName,
                    IsClose = true,
                    Remark = "申请包装被拒绝",
                    StoreHouseId = entity.SourceStore,
                };
                var currentStore = await
                    CurrentSemiStoreHouseRepository.FirstOrDefaultAsync(i =>
                        i.CurrentSemiStoreHouseNo == entity.CurrentSemiStoreHouseNo);

                if (currentStore == null)
                {
                    CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                    return;
                }
                currentStore.ActualQuantity += entity.ApplyQuantity;
                await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
                semiEnterStore.KgWeight = currentStore.KgWeight;
                await SemiEnterStoreRepository.InsertAsync(semiEnterStore);
            }
            else if(input.PackType == 2)
            {
                FinshedEnterStore finshedEnterStore = new FinshedEnterStore
                {
                    PackageApplyNo = entity.PackageApplyNo,
                    ProductionOrderNo = entity.ProductionOrderNo,
                    PackageProductNo = entity.ProductNo,
                    ProductNo = entity.ProductNo,
                    StoreHouseId = entity.SourceStore,
                    Quantity = entity.ApplyQuantity,
                    PackageCount = 1,
                    PackageSpecification = entity.ApplyQuantity,
                    ActualPackageCount = 1,
                    ApplyStatus = FinshedEnterStoreApplyStatusEnum.EnterStored.ToInt(),
                    ApplySourceType = entity.SourceStore,
                    IsClose = true,
                    ApplyEnterDate = date,
                    EnterStoreDate = date,
                    
                    AuditDate = date,
                    EnterStoreUser = AbpSession.UserName,
                    AuditUser = AbpSession.UserName,
                    TimeLastMod = date,
                    TimeCreated = date,
                    Remark = "申请包装被拒绝",
                    CreatorUserId = AbpSession.UserName,
                    UserIDLastMod = AbpSession.UserName,
                };
                var currentStore = await
                    FinshedCurrentStoreRepository.FirstOrDefaultAsync(i =>
                        i.CurrentProductStoreHouseNo == entity.CurrentSemiStoreHouseNo);
                if (currentStore == null)
                {
                    CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                    return;
                }
                currentStore.Quantity += entity.ApplyQuantity;
                await FinshedCurrentStoreRepository.UpdateAsync(currentStore);
                finshedEnterStore.StoreLocationNo = currentStore.StoreLocationNo;
                await FinshedEnterStoreRepository.InsertAsync(finshedEnterStore);
            }


            entity.ApplyStatus = PackageApplyStatusEnum.Refused.ToInt() + "";
            await Repository.UpdateAsync(entity);

            BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                "拒绝半成品包装申请",
                entity.ProductionOrderNo, entity.PackageApplyNo);
        }

        /// <summary>
        /// 关闭包装申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgClose), AuditLog("关闭包装申请")]
        public async Task ClosePackInfoApply(EntityDto<int> input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == PackageApplyStatusEnum.Applying.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("申请还未被审核(或拒绝),不能操作!"));
            }

            int newStatusEnum = FinshedEnterStoreApplyStatusEnum.New.ToInt();
            int enteredStatusEnum = FinshedEnterStoreApplyStatusEnum.EnterStored.ToInt();
            var item = await FinshedEnterStoreRepository.FirstOrDefaultAsync(i =>
                i.PackageApplyNo == entity.PackageApplyNo &&
                (i.ApplyStatus != newStatusEnum && i.ApplyStatus != enteredStatusEnum)&&!i.IsClose);
            if (item != null)
            {
                CheckErrors(IwbIdentityResult.Failed("有未完成的入库申请,不能操作!"));
                return;
            }
            entity.IsClose = true;
            await Repository.UpdateAsync(entity);


            BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                "关闭半成品包装申请",
                entity.ProductionOrderNo, entity.PackageApplyNo);
        }

        //根据批次号获取非取消和拒绝状态的入库记录
        public async Task<FinshedEnterStore> GetHasExistProductionOrderNo(string pcProductionOrderNo)
        {
            int refusedStatus = FinshedEnterStoreApplyStatusEnum.Refused.ToInt();
            int canceledStatus = FinshedEnterStoreApplyStatusEnum.Canceled.ToInt();
            var entity = await FinshedEnterStoreRepository.GetAll().Where(a => a.ProductionOrderNo == pcProductionOrderNo && (a.ApplyStatus != refusedStatus && a.ApplyStatus != canceledStatus)).OrderByDescending(i => i.ApplyEnterDate).FirstOrDefaultAsync();
            return entity;
        }
        //根据批次号获取非取消和拒绝状态的入库记录
        public async Task<ViewProductEnterStore> GetHasExistProductionOrderNoView(string pcProductionOrderNo)
        {
            int refusedStatus = FinshedEnterStoreApplyStatusEnum.Refused.ToInt();
            int canceledStatus = FinshedEnterStoreApplyStatusEnum.Canceled.ToInt();
            var entity = await ViewProductEnterStoreRepository.GetAll().Where(a => a.ProductionOrderNo == pcProductionOrderNo && (a.ApplyStatus != refusedStatus && a.ApplyStatus != canceledStatus)).OrderByDescending(i=>i.ApplyEnterDate).FirstOrDefaultAsync();
            return entity;
        }
        /// <summary>
        /// 检测申请入库批次号中的产品是否与之前申请的一致
        /// </summary>
        /// <param name="pcProductionOrderNo"></param>
        /// <param name="pcProductNo"></param>
        /// <returns>true:是与之前申请的一致 false:不一致或者没有入库记录</returns>
        private async Task<bool> CheckProductionOrderHasCreate(string pcProductionOrderNo,string pcProductNo)
        {
            var entity =await GetProductionOrderHasCreate(pcProductionOrderNo);
            //var b = entity?.ProductNo == pcProductNo;
            if (entity != null)
            {
                return entity.ProductNo == pcProductNo;
            }

            return false;
        }
        /// <summary>
        /// 检测申请入库批次号中的产品是否与之前申请的一致
        /// </summary>
        /// <param name="pcProductionOrderNo"></param>
        /// <returns>true:是与之前申请的一致 false:不一致或者没有入库记录</returns>
        private async Task<FinshedEnterStore> GetProductionOrderHasCreate(string pcProductionOrderNo)
        {
            var entity = await GetHasExistProductionOrderNo(pcProductionOrderNo);
            return entity;
        }
        //protected override IQueryable<PackageApply> ApplySorting(IQueryable<PackageApply> query, PagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<PackageApply> ApplyPaging(IQueryable<PackageApply> query, PagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #region 包装后成品入库

        /// <summary>
        /// 查询包装明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing,AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgQuery)]
        public PagedResultDto<FinshedEnterStore> GetFinishedEnterStoreApply(PagedRequestDto input)
        {
            var query = FinshedEnterStoreRepository.GetAll();
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
            var totalCount = query.Count();
            query = query.OrderByDescending(i => i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = query.ToList();
            var dtos = new PagedResultDto<FinshedEnterStore>(
                totalCount, entities
            );
            return dtos;
        }
      
        /// <summary>
        /// 修改包装明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgUpdate), AuditLog("修改包装明细")]
        public async Task UpdatePackInfo(UpdatePackInfoDto input)
        {
            var entity = await FinshedEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity==null)
            {
                CheckErrors(IwbIdentityResult.Failed("未找到包装明细！"));
                return;
            }

            entity.TimeLastMod = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserName;
            entity.ApplyQuantity = input.Quantity;
            entity.PackageSpecification = input.PackageSpecification;
            entity.PackageCount = input.PackageCount;
            entity.Quantity = input.PackageSpecification* input.PackageCount;
            entity.PackageEnterNum = input.PackageEnterNum;
            await FinshedEnterStoreRepository.UpdateAsync(entity);

            BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                $"修改包装明细[{entity.Id}],信息为:[{entity.Obj2String()}]",
                entity.ProductionOrderNo, entity.PackageApplyNo);
        }

        /// <summary>
        /// 添加包装明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgUpdate), AuditLog("添加包装明细")]
        public async Task AddPackInfo(CreatePackInfoDto input)
        {
            var entity2 = await Repository.FirstOrDefaultAsync(a => a.PackageApplyNo == input.PackageApplyNo);
            if (entity2.IsClose)
            {
                CheckErrors(IwbIdentityResult.Failed("申请已关闭,不能操作!"));
            }
            var date = Clock.Now;
            var f = new FinshedEnterStore()
            {
                PackageApplyNo = input.PackageApplyNo,
                ProductionOrderNo = input.ProductionOrderNo,
                PackageProductNo = input.PackageProductNo,
                ProductNo= input.ProductNo,
                SourceStoreHouseId = entity2.SourceStore,
                StoreHouseId = 1,
                ApplyQuantity = input.Quantity,
                ApplyQuantity2 = input.Quantity2,
                KgWeight = input.KgWeight,
                Quantity = input.PackageSpecification * input.PackageCount,
                PackageCount = input.PackageCount,
                PackageSpecification = input.PackageSpecification,
                ActualPackageCount = 0,
                ApplyStatus = FinshedEnterStoreApplyStatusEnum.New.ToInt(),
                ApplySourceType = input.PackType,
                IsClose = false,
                VerifyUser = input.VerifyUser,
                PackageUser = input.PackageUser,
                ApplyEnterDate = date,
                TimeLastMod = date,
                TimeCreated = date,
                CreatorUserId = AbpSession.UserName,
                UserIDLastMod = AbpSession.UserName,
                PackageEnterNum = input.PackageEnterNum
            };
             var entity= await FinshedEnterStoreRepository.InsertAsync(f);

            BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                $"添加包装明细[{entity.Id}],信息为:[{input.Obj2String()}]",
                input.ProductionOrderNo, input.PackageApplyNo);
        }
        /// <summary>
        /// 删除包装明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgDelete), AuditLog("删除包装明细")]
        public async Task DeletePackInfo(EntityDto input)
        {
            var entity = await FinshedEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未找到包装明细！"));
                return;
            }

            await FinshedEnterStoreRepository.DeleteAsync(entity);

            BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                $"删除包装明细[{entity.Id}],信息为:[{entity.Obj2String()}]",
                entity.ProductionOrderNo, entity.PackageApplyNo);
        }

        /// <summary>
        /// 包装完成入库申请批量
        /// </summary>
        /// <param name="applyNo"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgEnter), AuditLog("包装完成发起成品入库申请（批量）")]
        public async Task CreateProductApplyBatch(string applyNo)
        {
            var entities = await FinshedEnterStoreRepository.GetAllListAsync(a => a.PackageApplyNo == applyNo);
            if (entities!=null && entities.Any())
            {
                foreach (var entity in entities)
                {
                    if (entity.ApplyStatus == FinshedEnterStoreApplyStatusEnum.New.ToInt())
                    {
                        entity.ApplyStatus = FinshedEnterStoreApplyStatusEnum.Applying.ToInt();
                        await FinshedEnterStoreRepository.UpdateAsync(entity);
                        BusinessLogTypeEnum.Package.WriteLog(LogRepository, "产品包装",
                            $"包装完成发起成品入库申请（批量)[{entity.Id}]",
                            entity.ProductionOrderNo, entity.PackageApplyNo,entity.ProductNo);
                       
                    }
                }
            }
            
        }

        /// <summary>
        /// 包装完成入库申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgEnter), AuditLog("包装完成发起成品入库申请")]
        public async Task CreateProductApply(EntityDto input)
        {
            var entity = await FinshedEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未找到包装明细！"));
                return;
            }
            entity.ApplyStatus = FinshedEnterStoreApplyStatusEnum.Applying.ToInt();
            await FinshedEnterStoreRepository.UpdateAsync(entity);
            BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                $"包装完成发起成品入库申请[{entity.Id}]",
                entity.ProductionOrderNo, entity.PackageApplyNo, entity.ProductNo);
           
        }
        
        /// <summary>
        /// 确认入库数量（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgConfirm), AuditLog("确认包装成品入库数量")]
        public async Task ConfirmProductApply(EntityDto<int> input)
        {
            var entity = await FinshedEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未找到包装明细！"));
                return;
            }

            if (entity.ApplyStatus == FinshedEnterStoreApplyStatusEnum.EnterStored.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("该记录已确认入库！"));
                return;
            }
            var date = Clock.Now;
            entity.EnterStoreDate = date;
            entity.EnterStoreUser = AbpSession.UserName;
            entity.ApplyStatus = FinshedEnterStoreApplyStatusEnum.EnterStored.ToInt();
            var currentStore =await FinshedCurrentStoreRepository.FirstOrDefaultAsync(a => a.ProductionOrderNo == entity.ProductionOrderNo&& a.StoreLocationNo==entity.StoreLocationNo && a.ProductNo == entity.ProductNo);
            if (currentStore != null)
            {
                var isCanUpdate =
                    CommonAppService.CheckStoreRecordCanUpdate(currentStore.CurrentProductStoreHouseNo);
                if (!isCanUpdate)
                    CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
                currentStore.Quantity += entity.ActualPackageCount * entity.PackageSpecification;
                currentStore.TimeLastMod = date;
                currentStore.UserIDLastMod = AbpSession.UserName;
                await FinshedCurrentStoreRepository.UpdateAsync(currentStore);
            }
            else
            {
                string lcJson = entity.Obj2String();
                currentStore = lcJson.GetModel<CurrentProductStoreHouse>();
                currentStore.CurrentProductStoreHouseNo = Guid.NewGuid().ToString("N");
                currentStore.TimeCreated = date;
                currentStore.UserIDLastMod = AbpSession.UserName;
                currentStore.Quantity = entity.ActualPackageCount * entity.PackageSpecification;
                currentStore.FreezeQuantity = 0;
                await FinshedCurrentStoreRepository.InsertAsync(currentStore);
            }

            await FinshedEnterStoreRepository.UpdateAsync(entity);
            BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                $"确认成品入库数量[{entity.Id}]",
                entity.ProductionOrderNo, currentStore.CurrentProductStoreHouseNo, entity.ProductNo,entity.StoreLocationNo);
           
            
        }
        
        /// <summary>
        /// 取消入库申请（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgCancel), AuditLog("取消包装成品入库申请")]
        public async Task CancelProductApply(EntityDto<int> input)
        {
            var entity = await FinshedEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未找到包装明细！"));
                return;
            }
            entity.ApplyStatus = FinshedEnterStoreApplyStatusEnum.Canceled.ToInt();
            await FinshedEnterStoreRepository.UpdateAsync(entity);
            BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                $"取消包装成品入库申请[{entity.Id}]",
                entity.ProductionOrderNo, entity.PackageApplyNo, entity.ProductNo);
        }

        /// <summary>
        /// 关闭入库申请 （by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgClose), AuditLog("关闭包装成品入库申请")]
        public async Task CloseProductApply(EntityDto<int> input)
        {
            var entity = await FinshedEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未找到包装明细！"));
                return;
            }
            entity.IsClose = true;
            await FinshedEnterStoreRepository.UpdateAsync(entity);

            BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                $"关闭包装成品入库申请[{entity.Id}]",
                entity.ProductionOrderNo, entity.PackageApplyNo, entity.ProductNo);
            
        }

        /// <summary>
        /// 恢复入库申请（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgRecovery), AuditLog("恢复包装成品入库申请")]
        public async Task RecoveryProductApply(EntityDto<int> input)
        {
            var entity = await FinshedEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未找到包装明细！"));
                return;
            }

            //var preEntity = await GetHasExistProductionOrderNo(entity.ProductionOrderNo);
            ////如果要恢复入库申请之前已经有这个批次其它产品绑定入库
            //if (preEntity.ProductNo!= entity.ProductNo)
            //{
            //    CheckErrors(IwbIdentityResult.Failed("此流转单号已有其它产品入库!"));
            //    return;
            //}
            entity.ApplyStatus = FinshedEnterStoreApplyStatusEnum.Applying.ToInt();
            await FinshedEnterStoreRepository.UpdateAsync(entity);

            BusinessLogTypeEnum.Package.WriteLog(LogRepository, "半成品包装",
                $"恢复包装成品入库申请[{entity.Id}]",
                entity.ProductionOrderNo, entity.PackageApplyNo, entity.ProductNo);
        }
       
        #endregion

        #region 包装日报统计

        public async Task<PackageDayDateDto> QueryPackageDaily(DateTime date)
        {
            var sd= new DateTime(date.Year,date.Month,date.Day);
            var ed = sd.AddDays(1);
            var status = new int[2]
            {
                FinshedEnterStoreApplyStatusEnum.New.ToInt(),
                FinshedEnterStoreApplyStatusEnum.Canceled.ToInt()
            };
            var query = FinshedEnterStoreRepository.GetAll().Where(a =>
                a.ApplyEnterDate >= sd && a.ApplyEnterDate < ed && !status.Contains(a.ApplyStatus)&&
                (a.ApplySourceType == 1 || a.ApplySourceType == 2)).OrderBy(a=>a.ApplyEnterDate).Join(
                ProductRepository.GetAll(), a => a.ProductNo, s => s.Id, (a, s) => new PackageDayDateItem()
                {
                    PackageApplyNo = a.PackageApplyNo,
                    ProductionOrderNo = a.ProductionOrderNo,
                    ProductNo = a.ProductNo,
                    ProductName = s.ProductName,
                    PartNo = s.PartNo,
                    Model = s.Model,
                    Material = s.Material,
                    SurfaceColor = s.SurfaceColor,
                    Rigidity = s.Rigidity,
                    KgQuantity = a.ApplyQuantity,
                    PcsQuantity = a.ApplyQuantity2,
                    KgWeight = a.KgWeight,
                    PackageCount = a.PackageCount,
                    PackageSpecification = a.PackageSpecification,
                    PackageEnterNum = a.PackageEnterNum,
                    PackageUser = a.PackageUser,
                    VerifyUser = a.VerifyUser,
                    ApplySourceType = a.ApplySourceType
                });
            var packageItems = await query.ToListAsync();
            var day = 26;
            var prevDate = date.Day < 26
                ? date.Month == 1
                    ? new DateTime(date.Year - 1, 12, day)
                    : new DateTime(date.Year, date.Month - 1, day)
                : new DateTime(date.Year, date.Month, day);
            var totalQuery = FinshedEnterStoreRepository.GetAll().Where(a =>
                a.ApplyEnterDate >= prevDate && a.ApplyEnterDate < ed && !status.Contains(a.ApplyStatus) &&
                (a.ApplySourceType == 1 || a.ApplySourceType == 2));
            int index =await totalQuery.CountAsync();
            decimal kgCount = 0, pcsCount = 0;
            if (index>0)
            {
                kgCount = await totalQuery.SumAsync(a => a.ApplyQuantity);
                pcsCount = await totalQuery.SumAsync(a => a.ApplyQuantity2);
            }
            PackageDayDateDto dto = new PackageDayDateDto(date,packageItems,index,kgCount,pcsCount);
            return dto;
        }


        

        #endregion

    }
}
