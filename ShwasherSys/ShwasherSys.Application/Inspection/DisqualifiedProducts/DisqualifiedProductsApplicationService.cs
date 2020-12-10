using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Timing;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.Helper;
using IwbZero.IdentityFramework;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.Common;
using ShwasherSys.Inspection.DisqualifiedProducts.Dto;
using ShwasherSys.ProductInfo;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.ReturnGoods;
using ShwasherSys.ScrapStore;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.Inspection.DisqualifiedProducts
{
    [AbpAuthorize, AuditLog("不合格产品")]
    public class DisqualifiedProductAppService : IwbZeroAsyncCrudAppService<DisqualifiedProduct, DisqualifiedProductDto, int, IwbPagedRequestDto, DisqualifiedProductCreateDto, DisqualifiedProductUpdateDto >, IDisqualifiedProductAppService
    {
        public DisqualifiedProductAppService(
			ICacheManager cacheManager,
			IRepository<DisqualifiedProduct, int> repository, IRepository<CustomerDisabledProduct> cdpRepository, IRepository<BusinessLog> logRepository, ICommonAppService commonAppService, IRepository<SemiEnterStore> semiEnterStoreRepository, IRepository<CurrentSemiStoreHouse> currentSemiStoreHouseRepository, IRepository<ScrapEnterStore,string> scrapEnterStoreRepository, IRepository<CurrentProductStoreHouse> currentProductStoreHouseRepository, IRepository<FinshedEnterStore> finshedEnterStoreRepository, IRepository<Product, string> productRepository, IRepository<ViewCurrentProductStoreHouse> viewCurrentProductStoreHouseRepository, IRepository<ReturnGoodOrder> rgRepository) : base(repository, "DisqualifiedNo")
        {
            CdpRepository = cdpRepository;
            LogRepository = logRepository;
            CommonAppService = commonAppService;
            SemiEnterStoreRepository = semiEnterStoreRepository;
            CurrentSemiStoreHouseRepository = currentSemiStoreHouseRepository;
            ScrapEnterStoreRepository = scrapEnterStoreRepository;
            CurrentProductStoreHouseRepository = currentProductStoreHouseRepository;
            FinshedEnterStoreRepository = finshedEnterStoreRepository;
            ProductRepository = productRepository;
            ViewCurrentProductStoreHouseRepository = viewCurrentProductStoreHouseRepository;
            RgRepository = rgRepository;
            CacheManager = cacheManager;
        }

        public IRepository<BusinessLog> LogRepository { get; }
        protected ICommonAppService CommonAppService { get; }
        protected IRepository<CurrentSemiStoreHouse> CurrentSemiStoreHouseRepository;
        protected IRepository<CurrentProductStoreHouse> CurrentProductStoreHouseRepository;
        protected IRepository<ViewCurrentProductStoreHouse> ViewCurrentProductStoreHouseRepository;
        protected IRepository<SemiEnterStore> SemiEnterStoreRepository { get; }
        protected IRepository<FinshedEnterStore> FinshedEnterStoreRepository { get; }
        protected IRepository<ScrapEnterStore,string> ScrapEnterStoreRepository { get; }
        protected override bool KeyIsAuto { get; set; } = false;
        protected IRepository<CustomerDisabledProduct> CdpRepository { get; } 
        protected IRepository<ReturnGoodOrder> RgRepository { get; } 
        protected IRepository<Product,string> ProductRepository { get; } 

        #region GetSelect

        [DisableAuditing]
        public override async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var sList = new List<SelectListItem> {new SelectListItem {Text = @"请选择...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                //sList.Add(new SelectListItem { Value = l.Id, Text = l. });
            }
            return sList;
        }
        [DisableAuditing]
        public override async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string str = "<option value=\"\" selected>请选择...</option>";
            foreach (var l in list)
            {
                //str += $"<option value=\"{l.Id}\">{l.}</option>";
            }
            return str;
        }

        #endregion

        #region CURD

        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMg)]
        public override async Task Create(DisqualifiedProductCreateDto input)
        {
            await CreateEntity(input);
        }

        //[AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgUpdate)]
        public override async Task Update(DisqualifiedProductUpdateDto input)
        {
            await UpdateEntity(input);
        }

        //[AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgDelete)]
        public override Task Delete(EntityDto<int> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgQuery)]
        public override async Task<PagedResultDto<DisqualifiedProductDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<DisqualifiedProductDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgQuery)]
        public override async Task<DisqualifiedProductDto> GetDto(EntityDto<int> input)
        {
            var entity = await GetEntity(input);
            return MapToEntityDto(entity);
        }

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgQuery)]
        public override async Task<DisqualifiedProductDto> GetDtoById(int id)
        {
            var entity = await GetEntityById(id);
            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 查询实体Dto（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgQuery)]
        public override async Task<DisqualifiedProductDto> GetDtoByNo(string no)
        {
            var entity = await GetEntityByNo(no);
            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgQuery)]
        public override async Task<DisqualifiedProduct> GetEntity(EntityDto<int> input)
        {
            var entity = await GetEntityById(input.Id);
            if (entity == null)
            {
                CheckErrors("未查询到记录");
            }
            return entity;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgQuery)]
        public override async Task<DisqualifiedProduct> GetEntityById(int id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgQuery)]
        public override async Task<DisqualifiedProduct> GetEntityByNo(string no)
        {
            //CheckGetPermission();
            if (string.IsNullOrEmpty(KeyFiledName))
            {
                ThrowError("NoKeyFieldName");
            }
            return await base.GetEntityByNo(no);
        }

        #endregion

		#region Hide
       
		///// <summary>
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{DisqualifiedProduct}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<DisqualifiedProduct> CreateFilteredQuery(IwbPagedRequestDto input)
        //{
        //    var query = Repository.GetAll();
        //    var pagedInput = input as IIwbPagedRequest;
        //    if (pagedInput == null)
        //    {
        //        return query;
        //    }
        //    if (!string.IsNullOrEmpty(pagedInput.KeyWords))
        //    {
        //        object keyWords = pagedInput.KeyWords;
        //        LambdaObject obj = new LambdaObject()
        //        {
        //            FieldType = (LambdaFieldType)pagedInput.FieldType,
        //            FieldName = pagedInput.KeyField,
        //            FieldValue = keyWords,
        //            ExpType = (LambdaExpType)pagedInput.ExpType
        //        };
        //        var exp = obj.GetExp<DisqualifiedProduct>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    if (pagedInput.SearchList != null && pagedInput.SearchList.Count > 0)
        //    {
        //        List<LambdaObject> objList = new List<LambdaObject>();
        //       foreach (var o in pagedInput.SearchList)
        //        {
        //            if (string.IsNullOrEmpty(o.KeyWords))
        //                continue;
        //           object keyWords = o.KeyWords;
        //            objList.Add(new LambdaObject
        //            {
        //                FieldType = (LambdaFieldType)o.FieldType,
        //                FieldName = o.KeyField,
        //                FieldValue = keyWords,
        //                ExpType = (LambdaExpType)o.ExpType
        //            });
        //        }
        //        var exp = objList.GetExp<DisqualifiedProduct>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<DisqualifiedProduct> ApplySorting(IQueryable<DisqualifiedProduct> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<DisqualifiedProduct> ApplyPaging(IQueryable<DisqualifiedProduct> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion

        /// <summary>
        /// 检验降级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgCheckDowngrade),AuditLog("检验降级")]
        public async Task CheckDowngrade(ReturnOrderDto input)
        {
            var entity = await GetEntity(input);
            if (entity.HandleType != DisProductStateDefinition.NoHandle)
            {
                CheckErrors("已被处理，不能再操作。");
            }

            var date = DateTime.Now;
            entity.HandleType = input.InspectType;
            entity.HandleDate = date;
          
            if (input.InspectType==DisProductStateDefinition.NormalReturn)
            {
                //正常退货直接入库
                await ProductEnterStore(entity, input.StoreHouseId??0, input.StoreLocationNo, date, productOrderNo:entity.ProductOrderNo,createSourceType:EnterStoreCreateSourceEnum.NormalReturnGood.ToInt(),applySourceType:EnterStoreApplySourceEnum.NormalReturnGood.ToInt());

            }else if (input.InspectType==DisProductStateDefinition.Downgrade||input.InspectType==DisProductStateDefinition.Scrapped)
            {
                //填写退货单
                await ReturnGoodOrder(input,entity.ReturnOrderNo);
            }else if (input.InspectType == DisProductStateDefinition.SpecialPurchase)
            {
                await SpecialPurchase(input);
            }else if (input.InspectType == DisProductStateDefinition.AntiPlating && entity.ProductType==1)//成品返镀退货
            {
                //正常退货直接入库
                await ProductEnterStore(entity, input.StoreHouseId ?? 0, input.StoreLocationNo, date, productOrderNo: entity.ProductOrderNo, createSourceType: EnterStoreCreateSourceEnum.AntiPlating.ToInt(), applySourceType: EnterStoreApplySourceEnum.AntiPlating.ToInt());
            }
            await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "不合格产品处理",$"检验后决定产品降级[{entity.Id}]",
                entity.ProductOrderNo, entity.ProductNo);

        }
       
        /// <summary>
        /// 降级使用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgUseDowngrade),AuditLog("降级使用")]
        public async Task UseDowngrade(DowngradeDto input)
        {
            var entity = await GetEntity(input);
            if (entity.HandleType != DisProductStateDefinition.Downgrade && entity.HandleType != DisProductStateDefinition.ScrappedDowngrade)
            {
                CheckErrors("该记录未被降级，不能操作。");
            }
            if (input.CustomerNos == null || !input.CustomerNos.Any())
            {
                CheckErrors("未发现客户编码");
                return;
            }
            var date = Clock.Now;
            string productNo = string.IsNullOrEmpty(input.ProductNo) ?  entity.ProductNo:input.ProductNo ;
            if (entity.ProductType == ProductTypeDefinition.Semi ||
                (input.StoreHouseType == 2 && entity.ProductType == ProductTypeDefinition.Finish))  //半成品降级（或成品降级为半成品），创建入库申请，变更实时库存。
            {
                #region 半成品降级（或成品降级为半成品）

                if (entity.ProductType == ProductTypeDefinition.Semi)
                {
                    productNo = entity.ProductNo;//半成品降级不改变产品编号
                }
                var currentStore = CurrentSemiStoreHouseRepository.GetAll().FirstOrDefault(i => i.ProductionOrderNo == entity.ProductOrderNo && i.StoreHouseId == input.StoreHouseId && i.StoreLocationNo == input.StoreLocationNo && i.SemiProductNo==productNo);
                var productOrderNo =entity.ProductType == ProductTypeDefinition.Finish ? await CommonAppService.GetProductionOrderNo("T", entity.ProductOrderNo) :entity.ProductOrderNo;
                //var productOrderNo = entity.ProductOrderNo;
                if (currentStore != null)
                {
                    var isCanUpdate =
                        CommonAppService.CheckStoreRecordCanUpdate(currentStore.CurrentSemiStoreHouseNo, 2);
                    if (!isCanUpdate)
                        CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
                    currentStore.ActualQuantity += input.Quantity??0;
                    currentStore.TimeLastMod = date;
                    currentStore.UserIDLastMod = AbpSession.UserName;
                    currentStore.KgWeight = entity.KgWeight;
                    await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
                }
                else
                {
                    var isCanUpdate =
                        CommonAppService.CheckStoreCanUpdateByLocationNo(input.StoreLocationNo, 2);
                    if (!isCanUpdate)
                        CheckErrors(IwbIdentityResult.Failed("该库位正在盘点状态,不可进行出入库更新!"));
                    currentStore = new CurrentSemiStoreHouse
                    {
                        CurrentSemiStoreHouseNo = Guid.NewGuid().ToString("N"),
                        ProductionOrderNo = productOrderNo,
                        SemiProductNo = productNo,
                        StoreHouseId = input.StoreHouseId,
                        StoreLocationNo = input.StoreLocationNo,
                        KgWeight = entity.KgWeight,
                        ApplyEnterDate = date,
                        TimeCreated = date,
                        CreatorUserId = AbpSession.UserName,
                        UserIDLastMod = AbpSession.UserName,
                        ActualQuantity =input.Quantity??0,
                        FreezeQuantity = 0,
                        PreMonthQuantity = 0,
                        InventoryCheckState = 1,
                        ReturnState = 1,
                    };
                    await CurrentSemiStoreHouseRepository.InsertAsync(currentStore);
                }
                SemiEnterStore semiEnter = new SemiEnterStore
                {
                    ApplyEnterDate = date,
                    ProductionOrderNo = productOrderNo,
                    SemiProductNo =productNo,
                    ApplyStatus = EnterStoreApplyStatusEnum.EnterStored.ToInt().ToString(),
                    ApplySource = EnterStoreApplySourceEnum.Downgrade.ToInt().ToString(),
                    CreateSourceType = CreateSourceType.Normal.ToInt(),
                    ActualQuantity = entity.QuantityWeight,
                    Quantity = input.Quantity??0,
                    KgWeight = entity.KgWeight,
                    StoreHouseId = input.StoreHouseId,
                    StoreLocationNo = input.StoreLocationNo,
                    AuditDate = date,
                    AuditUser = AbpSession.UserName,
                    CreatorUserId = AbpSession.UserName,
                    TimeCreated = date,
                    UserIDLastMod = AbpSession.UserName,
                    TimeLastMod = date,
                    IsClose = false
                };
                await SemiEnterStoreRepository.InsertAsync(semiEnter);

                #endregion
                if (entity.ProductType == ProductTypeDefinition.Finish)
                {
                    await ReturnGoodCheck(entity.ReturnOrderNo);
                }
            }
            else  if (entity.ProductType == ProductTypeDefinition.Finish) // 成品降级
            {
                //var productOrderNo = await GetProductionOrderNo();
                //await ProductEnterStore(entity, input.StoreHouseId, input.StoreLocationNo, date, productOrderNo,
                //    productNo);
                //成品降级产品不变，批次号不变
                var productOrderNo = entity.ProductOrderNo;
                await ProductEnterStore(entity, input.StoreHouseId, input.StoreLocationNo, date, productOrderNo,
                    entity.ProductNo,createSourceType: EnterStoreCreateSourceEnum.Downgrade.ToInt(), applySourceType: EnterStoreApplySourceEnum.Downgrade.ToInt());
                await DisabledCustomer(productOrderNo,input.CustomerNos);
                if (input.ProductOrderNos!=null)
                {
                    foreach (var no in input.ProductOrderNos)
                    {
                        await DisabledCustomer(no,input.CustomerNos);
                    }
                }
            }
            entity.HandleType = DisProductStateDefinition.DowngradeHandled;
            entity.HandleDate = DateTime.Now;
            await Repository.UpdateAsync(entity);
            await DisabledCustomer(entity.ProductOrderNo,input.CustomerNos);
           
           
            BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "不合格产品处理",$"降级处理[{entity.Id}]，不能使用的客户[{string.Join(",",input.CustomerNos)}]",entity.ProductOrderNo, entity.ProductNo,string.Join(",",input.ProductOrderNos??new List<string>()));
        }

        private async Task SemiProductEnterStore(string productOrderNo, string semiProductNo, int storeHouseId,
            string storeLocationNo, DateTime date, EnterStoreApplySourceEnum enterStoreApplySourceEnum= EnterStoreApplySourceEnum.SpecialPurchase, decimal quantity =0,decimal kgWeight=0,string remark="")
        {
            var currentStore = CurrentSemiStoreHouseRepository.GetAll().FirstOrDefault(i => i.ProductionOrderNo == productOrderNo && i.StoreHouseId == storeHouseId && i.StoreLocationNo == storeLocationNo && i.SemiProductNo == semiProductNo);
            //var productOrderNo =entity.ProductType == ProductTypeDefinition.Finish ? await GetProductionOrderNo():entity.ProductOrderNo;
            
            if (currentStore != null)
            {
                var isCanUpdate =
                    CommonAppService.CheckStoreRecordCanUpdate(currentStore.CurrentSemiStoreHouseNo, 2);
                if (!isCanUpdate)
                    CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
                currentStore.ActualQuantity += quantity;
                currentStore.TimeLastMod = date;
                currentStore.UserIDLastMod = AbpSession.UserName;
                currentStore.KgWeight = kgWeight;
                await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
            }
            else
            {
                var isCanUpdate =
                    CommonAppService.CheckStoreCanUpdateByLocationNo(storeLocationNo, 2);
                if (!isCanUpdate)
                    CheckErrors(IwbIdentityResult.Failed("该库位正在盘点状态,不可进行出入库更新!"));
                currentStore = new CurrentSemiStoreHouse
                {
                    CurrentSemiStoreHouseNo = Guid.NewGuid().ToString("N"),
                    ProductionOrderNo = productOrderNo,
                    SemiProductNo = semiProductNo,
                    StoreHouseId = storeHouseId,
                    StoreLocationNo = storeLocationNo,
                    KgWeight = kgWeight,
                    ApplyEnterDate = date,
                    TimeCreated = date,
                    CreatorUserId = AbpSession.UserName,
                    UserIDLastMod = AbpSession.UserName,
                    ActualQuantity = quantity ,
                    FreezeQuantity = 0,
                    PreMonthQuantity = 0,
                    InventoryCheckState = 1,
                    ReturnState = 1,
                };
                await CurrentSemiStoreHouseRepository.InsertAsync(currentStore);
            }
            SemiEnterStore semiEnter = new SemiEnterStore
            {
                ApplyEnterDate = date,
                ProductionOrderNo = productOrderNo,
                SemiProductNo = semiProductNo,
                ApplyStatus = EnterStoreApplyStatusEnum.EnterStored.ToInt().ToString(),
                ApplySource = enterStoreApplySourceEnum.ToInt().ToString(),
                CreateSourceType = CreateSourceType.Normal.ToInt(),
                ActualQuantity = quantity,
                Quantity = quantity,
                KgWeight = kgWeight,
                StoreHouseId = storeHouseId,
                StoreLocationNo = storeLocationNo,
                AuditDate = date,
                AuditUser = AbpSession.UserName,
                CreatorUserId = AbpSession.UserName,
                TimeCreated = date,
                UserIDLastMod = AbpSession.UserName,
                TimeLastMod = date,
                IsClose = false,
                Remark = remark
            };
            await SemiEnterStoreRepository.InsertAsync(semiEnter);
        }

        /// <summary>
        /// 成品入库
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="storeId"></param>
        /// <param name="locationNo"></param>
        /// <param name="date"></param>
        /// <param name="productOrderNo"></param>
        /// <param name="productNo"></param>
        /// <param name="createSourceType">3:NormalReturn 4:AntiPlating 5:Downgrade</param>
        /// <returns></returns>
        private async Task ProductEnterStore(DisqualifiedProduct entity,int storeId,string locationNo ,DateTime date,string productOrderNo=null,string productNo=null, int createSourceType = 3,int applySourceType=7)
        {
             await ReturnGoodCheck(entity.ReturnOrderNo);

                //var isCanUpdate =
                //    CommonAppService.CheckStoreCanUpdateByLocationNo(locationNo, 2);
                //if (!isCanUpdate)
                //    CheckErrors(IwbIdentityResult.Failed("该库位正在盘点状态,不可进行出入库更新!"));
                productOrderNo = productOrderNo ?? await GetProductionOrderNo();
                productNo = productNo ?? entity.ProductNo;

              
              
             //var currentStore = new CurrentProductStoreHouse()
             //   {
             //       CurrentProductStoreHouseNo = Guid.NewGuid().ToString("N"),
             //       ProductionOrderNo = productOrderNo,
             //       ProductNo = productNo,
             //       StoreHouseId = storeId,
             //       StoreLocationNo = locationNo,
             //       KgWeight = entity.KgWeight,
             //       TimeCreated = date,
             //       CreatorUserId = AbpSession.UserName,
             //       UserIDLastMod = AbpSession.UserName,
             //       Quantity = entity.QuantityPcs,
             //       FreezeQuantity = 0,
             //       PreMonthQuantity = 0,
             //       InventoryCheckState = 1,
             //       ReturnState = 1,
             //       Remark = ""
             //   };
             //   await CurrentProductStoreHouseRepository.InsertAsync(currentStore);
                var fe= new FinshedEnterStore(){
                    ProductionOrderNo= productOrderNo,
                    ProductNo =productNo,
                    PackageApplyNo = $"TH-{entity.DisqualifiedNo}",
                    ApplyStatus = EnterStoreApplyStatusEnum.Applying.ToInt(),
                    ApplySourceType = applySourceType,
                    CreateSourceType = createSourceType,
                    ApplyQuantity2 = entity.QuantityPcs,
                    ApplyQuantity = entity.QuantityWeight,
                    Quantity = entity.QuantityWeight,
                    KgWeight = entity.KgWeight,
                    PackageSpecification=entity.QuantityPcs,
                    ActualPackageCount=1,
                    PackageCount=1,
                    StoreHouseId = storeId,
                    StoreLocationNo = locationNo,
                    AuditDate = date,
                    AuditUser = AbpSession.UserName,
                    CreatorUserId = AbpSession.UserName,
                    TimeCreated = date,
                    UserIDLastMod = AbpSession.UserName,
                    TimeLastMod = date,
                    EnterStoreUser = AbpSession.UserName,
                    EnterStoreDate = date,
                    IsClose = false,
                    ApplyEnterDate = date,

                };
                await FinshedEnterStoreRepository.InsertAsync(fe);
        }

        /// <summary>
        /// 用户加上禁止使用
        /// </summary>
        /// <param name="productOrderNo"></param>
        /// <param name="customerNos"></param>
        /// <returns></returns>
        public async Task DisabledCustomer(string productOrderNo, List<string> customerNos)
        {
            foreach (var no in customerNos)
            {
                if ((await CdpRepository.CountAsync(a=>a.ProductOrderNo==productOrderNo&&a.CustomerNo==no))==0)
                {
                    await CdpRepository.InsertAsync(new CustomerDisabledProduct()
                    {
                        ProductOrderNo = productOrderNo,
                        CustomerNo = no
                    });
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
                
            }
        }

        private async Task<string> GetProductionOrderNo()
        {
            string no = $"T{DateTime.Now.Year.ToString().Substring(2)}{(DateTime.Now.Month < 10 ?  DateTime.Now.Month + "" : Convert.ToString( DateTime.Now.Month, 16).ToUpper())}";
            var productionOrderNo = no;

            var lastEntity =await FinshedEnterStoreRepository.GetAll()
                    .Where(a => a.ProductionOrderNo.StartsWith(productionOrderNo))
                    .OrderByDescending(a => a.TimeCreated).FirstOrDefaultAsync();
            var lastSemiEntity = await SemiEnterStoreRepository.GetAll()
                .Where(a => a.ProductionOrderNo.StartsWith(productionOrderNo))
                .OrderByDescending(a => a.TimeCreated).FirstOrDefaultAsync();
            int index = 0,index1=0, index2 = 0,noLength =3;
            if (lastEntity!=null)
            {
                int.TryParse(lastEntity.ProductionOrderNo.Substring(lastEntity.ProductionOrderNo.Length - noLength), out index1);
            }

            if (lastSemiEntity != null)
            {
                int.TryParse(lastSemiEntity.ProductionOrderNo.Substring(lastSemiEntity.ProductionOrderNo.Length - noLength), out index2);
            }

            index = index1 >= index2 ? ++index1 : ++index2;//成品和半成品中取大的数值加一（避免）

            no += index.LeftPad(noLength);
            //if (await FinshedEnterStoreRepository.CountAsync(a => a.ProductionOrderNo==no)>0)
            //{
            //    no = await GetProductionOrderNo();
            //}
            return no;
        }
    

        ///// <summary>
        ///// 报废处理
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgScrapped),AuditLog("报废处理")]
        //public async Task Scrapped(ReturnOrderDto input)
        //{
        //    var entity = await GetEntity(input);
        //    if (entity.HandleType != DisProductStateDefinition.NoHandle)
        //    {
        //        CheckErrors("已被处理，不能再操作。");
        //    }
        //    entity.HandleType = DisProductStateDefinition.Scrapped;
        //    entity.HandleDate = DateTime.Now;
        //    await ReturnGoodOrder(input,entity.ReturnOrderNo);
        //    await Repository.UpdateAsync(entity);
        //    BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "不合格产品处理",$"检验后决定产品报废[{entity.Id}]",
        //        entity.ProductOrderNo, entity.ProductNo);
        //}

        /// <summary>
        /// 确认报废
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgConfirmScrapped),AuditLog("确认报废")]
        public async Task ConfirmScrapped(EntityDto<int> input)
        {
            var entity = await GetEntity(input);
            if (entity.HandleType != DisProductStateDefinition.Scrapped)
            {
                CheckErrors("该记录未被报废，不能操作。");
            }
            var date = Clock.Now;
            entity.HandleType = DisProductStateDefinition.ScrappedHandled;
            entity.HandleDate = date;
            await Repository.UpdateAsync(entity);
            var se = new ScrapEnterStore()
            {
                Id = Guid.NewGuid().ToString("N"),
                ProductionOrderNo = entity.ProductOrderNo,
                ProductNo = entity.ProductNo,
                ApplyStatus = 3,
                ApplyEnterDate = date,
                StoreHouseId= 6,
                StoreLocationNo="",
                ScrapSourceNo = entity.DisqualifiedNo,
                AuditUser= AbpSession.UserName,
                AuditDate= date,
                EnterStoreUser= AbpSession.UserName,
                EnterStoreDate= date,
                IsClose=false
            };
            if (entity.ProductType == ProductTypeDefinition.Semi) //半成品报废
            {
                se.ProductType = 2;
                se.ScrapSource = 2;
                se.ApplyQuantity = entity.QuantityWeight;
                se.Quantity = entity.QuantityWeight;
            }
            else  if (entity.ProductType == ProductTypeDefinition.Finish) // 成品报废
            {
                await ReturnGoodCheck(entity.ReturnOrderNo);
                se.ProductType = 1;
                se.ScrapSource = 1;
                se.ApplyQuantity = entity.QuantityPcs;
                se.Quantity = entity.QuantityPcs;
            }

            await ScrapEnterStoreRepository.InsertAsync(se);
            BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "不合格产品处理",$"报废处理[{entity.Id}]，报废入库[]",
                entity.ProductOrderNo, entity.ProductNo);
        }

        /// <summary>
        /// 拒绝报废
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgConfirmScrapped),AuditLog("拒绝报废")]
        public async Task UnScrapped(EntityDto<int> input)
        {
            var entity = await GetEntity(input);
            if (entity.HandleType != DisProductStateDefinition.Scrapped)
            {
                CheckErrors("该记录未被报废，不能操作。");
            }
            entity.HandleType = DisProductStateDefinition.ScrappedDowngrade;
            entity.HandleDate = DateTime.Now;
            await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "不合格产品处理",$"拒绝报废[{entity.Id}]",
                entity.ProductOrderNo, entity.ProductNo);
        }

        ///// <summary>
        ///// 确认反镀
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[AbpAuthorize(PermissionNames.PagesProductInspectDisqualifiedProductMgAntiPlating),AuditLog("确认反镀")]
        //public async Task AntiPlating(EntityDto<int> input)
        //{
        //    var entity = await GetEntity(input);
        //    if (entity.HandleType != DisProductStateDefinition.NoHandle)
        //    {
        //        CheckErrors("该记录已被处理，不能操作。");
        //    }
        //    entity.HandleType = DisProductStateDefinition.ScrappedDowngrade;
        //    entity.HandleDate = DateTime.Now;
        //    await Repository.UpdateAsync(entity);
        //    BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "不合格产品处理",$"确认反镀[{entity.Id}]",
        //        entity.ProductOrderNo, entity.ProductNo);
        //}

        /// <summary>
        /// 查询关联排产单
        /// </summary>
        /// <param name="productOrderNo"></param>
        /// <returns></returns>
        public async Task<List<RelatedProductDto>> QueryRelatedProductionOrder(string productOrderNo)
        {
            string no = productOrderNo.Substring(0, 7);
            //实际库存中的批次
            var query1 =  CurrentProductStoreHouseRepository.GetAll().Where(a => a.ProductionOrderNo.StartsWith(no)).Select(a=>new RelatedProductDto()
            {
                ProductOrderNo = a.ProductionOrderNo,
                ProductNo = a.ProductNo,
            });
            var applyStatus = FinshedEnterStoreApplyStatusEnum.EnterStored.ToInt();
            var date = DateTime.Now.AddYears(-1);
            //近一年未完成入库的批次
            var query2 =  FinshedEnterStoreRepository.GetAll().Where(a => a.ProductionOrderNo.StartsWith(no)&& a.ApplyStatus!=applyStatus && a.TimeCreated>date).Select(a=>new RelatedProductDto()
            {
                ProductOrderNo = a.ProductionOrderNo,
                ProductNo = a.ProductNo,
            });
            var list =await query1.Concat(query2).Distinct().ToListAsync();
            var query = from q in list
                join p in ProductRepository.GetAll() on q.ProductNo equals p.Id
                    into rp
                from o in rp.DefaultIfEmpty()
                select new RelatedProductDto()
                {
                    ProductOrderNo = q.ProductOrderNo,
                    ProductNo = q.ProductNo,
                    ProductName = o.ProductName,
                    PartNo = o.PartNo,
                    Model = o.Model,
                    Material = o.Material,
                    SurfaceColor = o.SurfaceColor,
                    Rigidity = o.Rigidity
                };
            var dto =  query.ToList();
            return dto;
        }

        /// <summary>
        /// 填写退货单检验情况
        /// </summary>
        /// <param name="input"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        private async Task  ReturnGoodOrder(ReturnOrderDto input,string no)
        {
            var entity = await RgRepository.FirstOrDefaultAsync(a => a.ReturnOrderNo == no);
            if (entity != null)
            {
                entity.SurveyReason = input.SurveyReason;
                entity.SurveyDetail = input.SurveyDetail;
                entity.Solution = input.Solution;
                await RgRepository.UpdateAsync(entity);
            }
        }
        /// <summary>
        /// 特采(自动生成半成品入库记录，同时备注特采信息)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        private async Task SpecialPurchase(ReturnOrderDto input)
        {
            var entity = Repository.Get(input.Id);
            string semiProductNo = entity.ProductNo;
            decimal quantity = entity.QuantityPcs;
            decimal quantityKeWeight = entity.KgWeight;
            await SemiProductEnterStore(entity.ProductOrderNo, semiProductNo, input.StoreHouseId2 ?? 0,
                input.StoreLocationNo, DateTime.Now, EnterStoreApplySourceEnum.SpecialPurchase, quantity, quantityKeWeight, input.SpecialPurchaseRemark);
        }
        /// <summary>
        /// 更改退货单状态
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        private async Task  ReturnGoodCheck(string no)
        {
            var entity = await RgRepository.FirstOrDefaultAsync(a => a.ReturnOrderNo == no);
            if (entity != null)
            {
                entity.ReturnState = ReturnGoodStateDefinition.HasChecked;
                await RgRepository.UpdateAsync(entity);
            }
        }
    
        public async Task<PagedResultDto<RelatedProductDto>> QueryRelatedProductionOrderPage(IwbPagedRequestDto input,string productOrderNo)
        {
            string no = productOrderNo.Substring(0, 7);
            //实际库存中的批次
            var query1 = ViewCurrentProductStoreHouseRepository.GetAll().Where(a => a.ProductionOrderNo.StartsWith(no));
           string sorting = string.IsNullOrEmpty(input.Sorting) ? "ProductionOrderNo DESC" : input.Sorting.Replace("productOrderNo","ProductionOrderNo");
            var totalCount = await query1.CountAsync();
            var query = query1.OrderBy(sorting).Skip(input.SkipCount).Take(input.MaxResultCount).Select(a=>new RelatedProductDto()
            {
                Id= a.Id,
                ProductOrderNo = a.ProductionOrderNo,
                ProductNo = a.ProductNo,
                Quantity = a.Quantity,
                ProductName = a.ProductName,
                PartNo = a.PartNo,
                Model = a.Model,
                Material = a.Material,
                SurfaceColor = a.SurfaceColor,
                Rigidity = a.Rigidity,
            });
            var dtos = await query.ToListAsync();
            return new PagedResultDto<RelatedProductDto>(totalCount,dtos);
        }

      
    }
}
