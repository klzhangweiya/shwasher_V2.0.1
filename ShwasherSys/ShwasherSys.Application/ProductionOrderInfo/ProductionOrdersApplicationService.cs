using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Timing;
using Abp.UI;
using Castle.Components.DictionaryAdapter;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.Helper;
using IwbZero.IdentityFramework;
using JetBrains.Annotations;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BasicInfo.OutFactory;
using ShwasherSys.Common;
using ShwasherSys.CompanyInfo;
using ShwasherSys.Lambda;
using ShwasherSys.ProductionOrderInfo.Dto;
using ShwasherSys.ProductInfo;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.SemiProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo.Dto;
using ShwasherSys.RmStore;
using LambdaExpType = ShwasherSys.Lambda.LambdaExpType;
using LambdaFieldType = ShwasherSys.Lambda.LambdaFieldType;
using LambdaObject = ShwasherSys.Lambda.LambdaObject;

namespace ShwasherSys.ProductionOrderInfo
{
    [AbpAuthorize, AuditLog("半成品排产出入库维护")]
    public class ProductionOrdersAppService : ShwasherAsyncCrudAppService<ProductionOrder, ProductionOrderDto, int, PagedRequestDto, ProductionOrderCreateDto, ProductionOrderUpdateDto>
        , IProductionOrdersAppService
    {

        protected IRepository<CurrentRmStoreHouse,string> CrsRepository { get; }
        protected IRepository<RmEnterStore,string> ErsRepository { get; }
        protected IRepository<RmOutStore,string> OrsRepository { get; }
        protected IRepository<EmployeeWorkPerformance> PerformanceRepository { get; }
        protected IRepository<BusinessLog> LogRepository;
        protected IRepository<ProductionLog> ProductLogRepository;

        protected IRepository<SemiEnterStore> SemiEnterStoreRepository;
        protected IRepository<ViewSemiEnterStore> ViewSemiEnterStoreRepository;
        protected IRepository<SemiOutStore> SemiOutStoreRepository;
        protected IRepository<CurrentSemiStoreHouse> CurrentSemiStoreHouseRepository;
        protected IRepository<ViewSemiOutStore> ViewSemiOutStoreRepository;
        protected IRepository<SemiProducts, string> SemiProductRepository; 
        protected IRepository<OutFactory,string> OutFactoryRepository;
        protected IRepository<ViewProductOutStore> ViewProductOutStoreRepository { get; }
        protected IRepository<ProductOutStore> ProductOutStoreRepository { get; }
        protected IRepository<Product,string> ProductRepository { get; }
        protected IRepository<CurrentProductStoreHouse> CurrentProductStoreHouseRepository { get; }
        protected ICommonAppService CommonAppService { get; }
        public ProductionOrdersAppService(IRepository<ProductionOrder, int> repository, IRepository<BusinessLog> logRepository, IRepository<SemiEnterStore> semiEnterStoreRepository, IRepository<SemiOutStore> semiOutStoreRepository, IRepository<ViewSemiEnterStore> viewSemiEnterStoreRepository, IRepository<CurrentSemiStoreHouse> currentSemiStoreHouseRepository, IRepository<ViewSemiOutStore> viewSemiOutStoreRepository, IRepository<SemiProducts, string> semiProductRepository, IRepository<OutFactory, string> outFactoryRepository, IRepository<ProductionLog> productLogRepository, IRepository<EmployeeWorkPerformance> performanceRepository, IRepository<RmOutStore, string> orsRepository, IRepository<RmEnterStore, string> ersRepository, IRepository<CurrentRmStoreHouse, string> crsRepository, ICommonAppService commonAppService, IRepository<ViewProductOutStore> viewProductOutStoreRepository, IRepository<ProductOutStore> productOutStoreRepository, IRepository<CurrentProductStoreHouse> currentProductStoreHouseRepository, IRepository<Product, string> productRepository) : base(repository, "ProductionOrderNo")
        {
            LogRepository = logRepository;
            SemiEnterStoreRepository = semiEnterStoreRepository;
            SemiOutStoreRepository = semiOutStoreRepository;
            ViewSemiEnterStoreRepository = viewSemiEnterStoreRepository;
            CurrentSemiStoreHouseRepository = currentSemiStoreHouseRepository;
            ViewSemiOutStoreRepository = viewSemiOutStoreRepository;
            SemiProductRepository = semiProductRepository;
            OutFactoryRepository = outFactoryRepository;
            ProductLogRepository = productLogRepository;
            PerformanceRepository = performanceRepository;
            OrsRepository = orsRepository;
            ErsRepository = ersRepository;
            CrsRepository = crsRepository;
            CommonAppService = commonAppService;
            ViewProductOutStoreRepository = viewProductOutStoreRepository;
            ProductOutStoreRepository = productOutStoreRepository;
            CurrentProductStoreHouseRepository = currentProductStoreHouseRepository;
            ProductRepository = productRepository;
        }
		protected override bool KeyIsAuto { get; set; } = false;
		protected override string GetPermissionName { get; set; } = PermissionNames.PagesProductionInfoProductionOrderMg;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesProductionInfoProductionOrderMg;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesProductionInfoProductionOrderMgCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesProductionInfoProductionOrderMgUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesProductionInfoProductionOrderMgDelete;

        #region 排产单
        [DisableAuditing]
        public override async Task<PagedResultDto<ProductionOrderDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();
            bool hasEnd = false;
            var query = CreateFilteredQuery(input);
            //var semiProductQuery = SemiProductRepository.GetAll();
            var outFacQuery = OutFactoryRepository.GetAll();
            //var result = query.Join(semiProductQuery, i => i.SemiProductNo, u => u.Id, (i, u) => new ProductionOrderDto()
            //{
            //    Model = u.Model ?? "",
            //    CarNo = i.CarNo,
            //    EnterQuantity = i.EnterQuantity,
            //    Id = i.Id,
            //    ProductionType = i.ProductionType,
            //    OutsourcingFactory = i.OutsourcingFactory,
            //    CreatorUserId = i.CreatorUserId,
            //    IsChecked = i.IsChecked,
            //    IsLock = i.IsLock,
            //    Material = u.Material ?? "",
            //    PartNo = u.PartNo ?? "",
            //    PlanProduceDate = i.PlanProduceDate,
            //    RawMaterials = i.RawMaterials,
            //    SurfaceColor = u.SurfaceColor ?? "",
            //    Rigidity = u.Rigidity ?? "",
            //    Remark = i.Remark,
            //    UserIDLastMod = i.UserIDLastMod,
            //    TimeCreated = i.TimeCreated,
            //    TimeLastMod = i.TimeLastMod,
            //    Size = i.Size,
            //    ProcessingType = i.ProcessingType,
            //    ProcessingLevel = i.ProcessingLevel,
            //    SourceProductionOrderNo = i.SourceProductionOrderNo,
            //    StoveNo = i.StoveNo,
            //    Quantity = i.Quantity,
            //    ProductionOrderNo = i.ProductionOrderNo,
            //    ProductionOrderStatus = i.ProductionOrderStatus,
            //    SemiProductNo = i.SemiProductNo,
            //    SemiProductName = u.SemiProductName,
            //    KgWeight = i.KgWeight
            //});

            var result = from u in query
                         join s in SemiProductRepository.GetAll() on u.SemiProductNo equals s.Id  into l
                         from luq in l.DefaultIfEmpty()
                         join o in outFacQuery on u.OutsourcingFactory equals o.Id into j
                         from oj in j.DefaultIfEmpty() 
                         select new ProductionOrderDto
                         {
                             Model = luq.Model ?? "",
                             CarNo = u.CarNo,
                             EnterQuantity = u.EnterQuantity,
                             Id = u.Id,
                             ProductionType = u.ProductionType,
                             OutsourcingFactory = u.OutsourcingFactory,
                             CreatorUserId = u.CreatorUserId,
                             IsChecked = u.IsChecked,
                             IsLock = u.IsLock,
                             Material = luq.Material ?? "",
                             PartNo = luq.PartNo ?? "",
                             PlanProduceDate = u.PlanProduceDate,
                             RawMaterials = u.RawMaterials,
                             SurfaceColor = luq.SurfaceColor ?? "",
                             Rigidity = luq.Rigidity ?? "",
                             Remark = u.Remark,
                             UserIDLastMod = u.UserIDLastMod,
                             TimeCreated = u.TimeCreated,
                             TimeLastMod = u.TimeLastMod,
                             Size = u.Size,
                             ProcessingType = u.ProcessingType,
                             ProcessingLevel = u.ProcessingLevel,
                             SourceProductionOrderNo = u.SourceProductionOrderNo,
                             StoveNo = u.StoveNo,
                             Quantity = u.Quantity,
                             ProductionOrderNo = u.ProductionOrderNo,
                             ProductionOrderStatus = u.ProductionOrderStatus,
                             SemiProductNo = u.SemiProductNo,
                             SemiProductName = luq.SemiProductName,
                             KgWeight = u.KgWeight,
                             OutsourcingFactoryName = oj.OutFactoryName,
                             EnterDate = u.EnterDate,
                             InspectDate = u.InspectDate,
                             HasExported = u.HasExported
                         };
            var property = typeof(ProductionOrder).GetProperty("IsLock");
            if (property != null)
            {
                LambdaObject objLambdaObject = new LambdaObject()
                {
                    FieldType = LambdaFieldType.S,
                    FieldName = "IsLock",
                    FieldValue = "N",
                    ExpType = LambdaExpType.Equal
                };
                var expIsLock = objLambdaObject.GetExp<ProductionOrderDto>();
                result = result.Where(expIsLock);
            }

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
                    if (o.KeyField?.ToLower() == "productionorderstatus" && o.KeyWords == ProductionOrderStatusEnum.End.ToInt() + "")
                    {
                        hasEnd = true;
                    }
                }
                var exp = objList.GetExp<ProductionOrderDto>();
                result = result.Where(exp);
            }
            if (!hasEnd)
            {
                var endState = ProductionOrderStatusEnum.End.ToInt();
                result = result.Where(a => (a.ProductionOrderStatus != endState));
            }


            var totalCount = await AsyncQueryableExecuter.CountAsync(result);

            result = result.OrderByDescending(i=>i.TimeCreated);
            result = result.Skip(input.SkipCount).Take(input.MaxResultCount);
        
            var dtos = new PagedResultDto<ProductionOrderDto>(
                totalCount,
                result.ToList()
            );
            return dtos;
        }

        private ProductionOrderDto TrunProductionOrderDto(ProductionOrder p,SemiProducts s)
        {
            var productionOrderDto = MapToEntityDto(p);
            productionOrderDto.Model = s.Model ?? "";
            productionOrderDto.Material = s.Material ?? "";
            productionOrderDto.PartNo = s.PartNo ?? "";
            productionOrderDto.SurfaceColor = s.SurfaceColor ?? "";
            productionOrderDto.Rigidity = s.Rigidity ?? "";
            /*var productionOrderDto = new ProductionOrderDto(p)
            {
                Model = s.Model ?? "",
                Material = s.Material ?? "",
                PartNo = s.PartNo ?? "",
                SurfaceColor = s.SurfaceColor ?? "",
                Rigidity = s.Rigidity ?? "",
            };*/
            return productionOrderDto;
        }
        /// <summary>
        /// 获取新建排产单的编号
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<string> GetNewProductionOrderNo()
        {
            return await GetNewProductionOrderNo(0);
        }
        /// <summary>
        /// 获取新建排产单（外购单）的编号
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<string> GetNewProductionOrderNo(int isOutsourcing)
        {
          
            CheckGetAllPermission();
            string lcRetVal;
            DateTime loTiem = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
            //loTiem = loTiem.AddSeconds(-1);
            var orders = (await Repository.GetAllListAsync(i => i.TimeCreated >= loTiem && i.ProcessingLevel == "1")).OrderByDescending(i => i.Id).ToList();
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
            DateTime loDate = DateTime.Today;
            //string lcMonth = liMonth < 10 ? liMonth + "" : Convert.ToString(liMonth, 16);
            lcRetVal = loDate.Date.Year + GetMonthString(isOutsourcing) + lcRetVal;
            lcRetVal = lcRetVal.Substring(2, lcRetVal.Length - 2);
            return lcRetVal;
        }

        /// <summary>
        /// 转换月份
        /// </summary>
        /// <param name="isOutsourcing"></param>
        /// <returns></returns>
        private string GetMonthString(int isOutsourcing)
        {
            DateTime loDate = DateTime.Today;
            int liMonth = loDate.Date.Month;
            if (isOutsourcing == 0)
            {
                return liMonth < 10 ? liMonth + "" : Convert.ToString(liMonth, 16).ToUpper();
            }

            string[] scource = {"", "G", "H", "W", "J", "K", "L", "M", "N", "T", "P","Q","R"};
            return scource[liMonth];
        }

        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOrderMgUpdate), AuditLog("变更排产状态")]
        public async Task<ProductionOrderDto> ChangeProductionOrderStatus(ChangeProductionOrderStatusDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ProductionOrderStatus > ProductionOrderStatusEnum.Producting.ToInt()&&entity.ProductionOrderStatus!= ProductionOrderStatusEnum.HangUp.ToInt() && entity.ProductionOrderStatus != ProductionOrderStatusEnum.Audited.ToInt() && entity.IsChecked==0)
            {
                CheckErrors(IwbIdentityResult.Failed("排产单已进行入库，不可进行更改！"));
            }
            entity.ProductionOrderStatus = input.ProductionOrderStatus;
            //ProductionOrderLogs loLogs = new ProductionOrderLogs()
            //{
            //    ProductionOrderNo = loProductionOrder.ProductionOrderNo,
            //    CreatorUserId = AbpSession.UserName,
            //    OperatorTitle = $"排产单:{loProductionOrder.ProductionOrderNo} 状态变更",
            //    OperatorConent = $"排产单:{loProductionOrder.ProductionOrderNo} 状态变更为{input.ProductionOrderStatus}",
            //    TimeCreated = Clock.Now
            //};
            //await ProductionOrderLogRepository.InsertAsync(loLogs);


            entity = await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半产品排产",
                $"排产单状态变更为[{(ProductionOrderStatusEnum)input.ProductionOrderStatus}]",
                entity.ProductionOrderNo);
            return MapToEntityDto(entity);

        }
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOrderMgUpdate), AuditLog("排产进入已入库状态状态")]
        public async Task<ProductionOrderDto> ConfirmEnterStore(ConfirmEnterStoreDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ProductionOrderStatus != ProductionOrderStatusEnum.Storeing.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("排查单未进行入库操作！"));
            }
            entity.ProductionOrderStatus = ProductionOrderStatusEnum.EnterStore.ToInt();
            //ProductionOrderLogs loLogs = new ProductionOrderLogs()
            //{
            //    ProductionOrderNo = loProductionOrder.ProductionOrderNo,
            //    CreatorUserId = AbpSession.UserName,
            //    OperatorTitle = $"排产单:{loProductionOrder.ProductionOrderNo} 状态变更",
            //    OperatorConent = $"排产单:{loProductionOrder.ProductionOrderNo} 状态变更为{input.ProductionOrderStatus}",
            //    TimeCreated = Clock.Now
            //};
            //await ProductionOrderLogRepository.InsertAsync(loLogs);

            entity.EnterDate = Clock.Now;
            entity = await Repository.UpdateAsync(entity);
            if (string.IsNullOrEmpty(input.CurrentRmHouseId))
            {
                BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半产品排产",
                    $"排产单状态变更为[{ProductionOrderStatusEnum.EnterStore}]。");
            }
            else
            {
                var rmCurrent= await CreateRwStore(input, entity.ProductionOrderNo);
                BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半产品排产确认入库（车间生产）",$"排产单状态变更为[{ProductionOrderStatusEnum.EnterStore}]。原材料[{rmCurrent.RmProductNo}],领用{input.TotalQuantity}kg,剩余{input.LaveQuantity}kg,使用后库存:{rmCurrent.Quantity}kg",entity.ProductionOrderNo);
            }
            return MapToEntityDto(entity);

        }


               /// <summary>
        /// 生成原材料出入库记录
        /// </summary>
        /// <param name="input"></param>
        /// <param name="productOrderNo"></param>
        /// <returns></returns>
        private async Task<CurrentRmStoreHouse> CreateRwStore(ConfirmEnterStoreDto input,string productOrderNo)
        {
            
            
            var current = await CrsRepository.FirstOrDefaultAsync(a => a.Id == input.CurrentRmHouseId);
            if (current == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到库存记录!"));
                return null; 
            }



            if (current.Quantity < 0 || (current.Quantity- current.FreezeQuantity - input.TotalQuantity + input.LaveQuantity) < 0) 
            {
                CheckErrors(IwbIdentityResult.Failed("库存不足!"));
                return null; 
            }

            current.Quantity = current.Quantity - input.TotalQuantity + input.LaveQuantity;
            await CrsRepository.UpdateAsync(current);
            var date = DateTime.Now;
            var oStore = new RmOutStore
            {
                Id = Guid.NewGuid().ToString("N"),
                CurrentRmStoreHouseNo = current.Id,
                IsConfirm= true,
                ProductionOrderNo = productOrderNo,
                ProductBatchNum = current.ProductBatchNum,
                RmProductNo = current.RmProductNo,
                ApplyStatus = RmEnterOutStatusEnum.Stored.ToInt(),
                CreateSourceType =  CreateSourceType.Normal.ToInt(),
                StoreHouseId = current.StoreHouseId,
                ActualQuantity = input.TotalQuantity,
                Quantity = input.TotalQuantity,
                ApplyOutDate = date,
                AuditUser = AbpSession.UserName,
                AuditDate = date,
                OutStoreDate = date,
                OutStoreUser = AbpSession.UserName,
            };
            await OrsRepository.InsertAsync(oStore);
            if (input.LaveQuantity > 0)
            {
                var eStore = new RmEnterStore
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ProductionOrderNo = productOrderNo,
                    ProductBatchNum = current.ProductBatchNum,
                    RmProductNo = current.RmProductNo,
                    ApplyStatus = RmEnterOutStatusEnum.Stored.ToInt(),
                    CreateSourceType =  CreateSourceType.Normal.ToInt(),
                    StoreHouseId = current.StoreHouseId,
                    StoreLocationNo = current.StoreLocationNo,
                    ApplyQuantity = input.LaveQuantity,
                    Quantity = input.LaveQuantity,
                    ApplyEnterDate = date,
                    AuditUser = AbpSession.UserName,
                    AuditDate = date,
                    EnterStoreDate = date,
                    EnterStoreUser = AbpSession.UserName,
                };
                await ErsRepository.InsertAsync(eStore);
            }
            return current;
        }

        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOrderMgCreate), AuditLog("创建排产单")]
        public override async Task<ProductionOrderDto> Create(ProductionOrderCreateDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.ProductionOrderNo == input.ProductionOrderNo);
            if (entity != null)
            {
                CheckErrors(IwbIdentityResult.Failed("排产单号已存在，请稍后再试！"));
                return null;
            }
            var dto = await base.Create(input);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半产品排产",
                $"创建排产单,信息为：[{dto.Obj2String()}]",
                dto.ProductionOrderNo);
            return dto;
        }
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOrderMgUpdate), AuditLog("修改排产单")]
        public override async Task<ProductionOrderDto> Update(ProductionOrderUpdateDto input)
        {
            CheckUpdatePermission();
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ProductionOrderStatus > ProductionOrderStatusEnum.Producting.ToInt()&& entity.ProductionOrderStatus != ProductionOrderStatusEnum.Audited.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("排产单已进行入库，不可进行修改！"));
            }
            MapToEntity(input, entity);
            entity.TimeLastMod = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserId + "";
            entity= await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半产品排产",
                $"修改排产单,信息为：[{entity.Obj2String()}]",
                entity.ProductionOrderNo);
            return MapToEntityDto(entity);
        }
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOrderMgDelete), AuditLog("删除排产单")]
        public override async Task Delete(EntityDto<int> input)
        {
            CheckDeletePermission();
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ProductionOrderStatus > ProductionOrderStatusEnum.Producting.ToInt()&&entity.ProductionOrderStatus!=ProductionOrderStatusEnum.Audited.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("排产单已进行入库，不可进行删除！"));
            }
           
            entity.IsLock = "Y";
            entity=await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半产品排产",
                $"删除排产单,信息为：[{entity.Obj2String()}]",
                entity.ProductionOrderNo);
        }

        /// <summary>
        /// 创建外协排产单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOrderMgCreate), AuditLog("创建外协排产单")]
        public async Task<ProductionOrderDto> CreateOutProductionOrder(CreateOutProductionOrderDto input)
        {
            string sourceProductionOrderNo = input.SourceProductionOrderNo;
            string processType = input.ProcessingType;
            string processTypeNo = input.ProcessingTypeNo;
            string productionOrderNo = GetOutProductionOrderNo(sourceProductionOrderNo, processType, processTypeNo);
            var isExistObj = Repository.FirstOrDefault(i => i.ProductionOrderNo == productionOrderNo);
            if (isExistObj != null)
            {
                throw new UserFriendlyException("该流转单编号已存在!同一批次产品入库分两次外协加工,编号不能重复填写！");
            }

            if (input.IsReplating == 1)
            {
                var outStore = await ProductOutStoreRepository.FirstOrDefaultAsync(a => a.Id == input.OutStoreId);
                outStore.IsClose = true;
                outStore.IsConfirm = true;
                await ProductOutStoreRepository.UpdateAsync(outStore);
                var currentStore = await CurrentProductStoreHouseRepository.FirstOrDefaultAsync(a =>
                    a.CurrentProductStoreHouseNo == outStore.CurrentProductStoreHouseNo);
                currentStore.FreezeQuantity -= outStore.Quantity;
                currentStore.Quantity -= outStore.Quantity;
                await CurrentProductStoreHouseRepository.UpdateAsync(currentStore);
            }
            else
            {
                var semiOutStore = await SemiOutStoreRepository.FirstOrDefaultAsync(a => a.Id == input.OutStoreId);
                semiOutStore.IsClose = true;
                await SemiOutStoreRepository.UpdateAsync(semiOutStore);
            }
            

            var semiProduct = await SemiProductRepository.FirstOrDefaultAsync(a => a.Id == input.SemiProductNo);

            ProductionOrderCreateDto objOrderCreateDto = new ProductionOrderCreateDto()
            {
                Material = semiProduct.Material,
                ProductionOrderNo = productionOrderNo,
                ProcessingLevel = "2",
                OutsourcingFactory = input.OutsourcingFactory,
                PlanProduceDate = input.PlanProduceDate,
                Model = semiProduct.Model,
                Quantity = input.Quantity,
                SurfaceColor = semiProduct.SurfaceColor,
                ProductionOrderStatus = ProductionOrderStatusEnum.Start.ToInt(),
                ProcessingType = processType,
                SourceProductionOrderNo = sourceProductionOrderNo,
                SemiProductNo = input.SemiProductNo,
                Rigidity = semiProduct.Rigidity,
                Remark = input.Remark,
            };
            //返镀标识
            if (input.IsReplating == 1)
            {
                objOrderCreateDto.Remark = $"[返镀]{objOrderCreateDto.Remark}";
            }
            var dto = await CreateEntity(objOrderCreateDto);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半产品排产",
                $"创建外协排产单[{dto.Id}],信息为：[{input.Obj2String()}]",
                dto.ProductionOrderNo, dto.SourceProductionOrderNo);

            return dto;
        }
        [AbpAuthorize(PermissionNames.PagesProductionInfoOutProductionOrderMgUpdate), AuditLog("修改外协排产单")]
        public async Task<ProductionOrderDto> UpdateOutProductionOrder(UpdateOutProductionOrderDto input)
        {
            //string productionOrderNo = GetOutProductionOrderNo(input.SourceProductionOrderNo, input.ProcessingType, input.ProcessingTypeNo);
            var entity = Repository.FirstOrDefault(i => i.ProductionOrderNo == input.ProductionOrderNo);
            if (entity == null)
            {
                throw new UserFriendlyException("该流转单编号不存在！");
            }
            //var semiProduct =await SemiProductRepository.FirstOrDefaultAsync(a=>a.Id == input.SemiProductNo);
            entity.Quantity = input.Quantity;
            entity.PlanProduceDate = input.PlanProduceDate;
            entity.TimeLastMod = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserName;
            entity.Remark = input.Remark;
            entity.KgWeight = input.KgWeight??0;
            entity.OutsourcingFactory = input.OutsourcingFactory;
            entity.SemiProductNo = input.SemiProductNo;


            entity = await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半产品排产",
                $"修改外协排产单[{entity.Id}],信息为：[{input.Obj2String()}]",
                entity.ProductionOrderNo, entity.SourceProductionOrderNo);

            return MapToEntityDto(entity);
        }

        [AbpAuthorize(PermissionNames.PagesProductionInfoOutProductionOrderMgDelete), AuditLog("删除外协排产单")]
        public async Task DeleteOutProductionOrder(EntityDto<int> input)
        {
            //string productionOrderNo = GetOutProductionOrderNo(input.SourceProductionOrderNo, input.ProcessingType, input.ProcessingTypeNo);
            var entity = Repository.FirstOrDefault(i => i.Id == input.Id);
            if (entity == null)
            {
                throw new UserFriendlyException("该流转单编号不存在！");
            }
            if (entity.ProductionOrderStatus >= ProductionOrderStatusEnum.Producting.ToInt()&& entity.ProductionOrderStatus!= ProductionOrderStatusEnum.Audited.ToInt())
            {
                CheckErrors(IwbIdentityResult.Failed("排产单已进行入库，不可进行删除！"));
            }
            entity.IsLock = "Y";
            await Repository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半产品排产",
                $"删除外协排产单[{entity.Id}]",
                entity.ProductionOrderNo, entity.SourceProductionOrderNo);

        }

        private string GetOutProductionOrderNo(string sourceProductionOrderNo, string processType,
            string processTypeNo)
        {
            string resultNo = sourceProductionOrderNo.Length > 7
                ? sourceProductionOrderNo
                : sourceProductionOrderNo + "0000";
            if (processType == ProductionOrderProcessTypeEnum.HeatTreatment.ToInt() + "")
            {
                resultNo = resultNo.Substring(0, resultNo.Length - 2) + processTypeNo;
            }
            else if (processType == ProductionOrderProcessTypeEnum.SurfaceTreatment.ToInt() + "")
            {
                string lastTwoLength = resultNo.Substring(resultNo.Length - 2, 2);
                resultNo = resultNo.Substring(0, resultNo.Length - 4) + processTypeNo + lastTwoLength;
            }
            return resultNo;
        }

        #endregion

        #region 半成品入库

        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionEnterStoreApplyMg), DisableAuditing]
        public PagedResultDto<ViewSemiEnterStore> GetSemiEnterStoreApply(PagedRequestDto input)
        {
            var query = ViewSemiEnterStoreRepository.GetAll();
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
            var totalCount = query.Count();

            query = query.OrderByDescending(i => i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = query.ToList();

            var dtos = new PagedResultDto<ViewSemiEnterStore>(
                totalCount, entities
            );
            return dtos;
        }


        /// <summary>
        /// 半成品入库申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOrderMgUpdate), AuditLog("创建入库申请")]
        public async Task<SemiEnterStoreDto> CreateEnterStoreApply(CreateEnterStoreApplyDto input)
        {
            var loProductionOrder = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            loProductionOrder.ProductionOrderStatus = ProductionOrderStatusEnum.Storeing.ToInt();
            loProductionOrder.KgWeight = input.KgWeight;
            //loProductionOrder.ProductionOrderStatus = ProductionOrderStatusEnum.Producting.ToInt();
            var applySource = loProductionOrder.ProcessingLevel == "1"
                ? (loProductionOrder.ProductionType == "0"
                    ? EnterStoreApplySourceEnum.InnerCar
                    : EnterStoreApplySourceEnum.Out)
                : EnterStoreApplySourceEnum.OutProduct;
            SemiEnterStore entity = new SemiEnterStore
            {
                ApplyEnterDate = Clock.Now,
                ApplySource =applySource.ToInt().ToString(),
                ApplyStatus = EnterStoreApplyStatusEnum.Applying.ToInt().ToString(),
                ProductionOrderNo = loProductionOrder.ProductionOrderNo,
                SemiProductNo = loProductionOrder.SemiProductNo,
                Quantity = input.EnterStoreQuantity,
                ActualQuantity = 0,
                AuditDate = Clock.Now,
                AuditUser = AbpSession.UserName,
                TimeCreated = Clock.Now,
                CreatorUserId = AbpSession.UserName,
                TimeLastMod = Clock.Now,
                UserIDLastMod = AbpSession.UserName,
                Remark = input.Remark,
                StoreHouseId = input.StoreHouseId,
                KgWeight = input.KgWeight,
            };
            await SemiEnterStoreRepository.InsertAsync(entity);
            if (loProductionOrder.ProductionType == "0" &&
                input.ProductUser != null && input.ProductUser.Any()) 
            {
                if (string.IsNullOrEmpty(input.CarNo))
                {
                    CheckErrors(IwbIdentityResult.Failed("车号不能为空，请检查后再试。"));
                }
                if (string.IsNullOrEmpty(loProductionOrder.CarNo))
                {
                    loProductionOrder.CarNo = input.CarNo;
                }
                else if (input.CarNo!=loProductionOrder.CarNo)
                {
                    CheckErrors(IwbIdentityResult.Failed("车号与上一次入库不一致，请检查后再试。"));
                }


                await CreateProductionLog(input, loProductionOrder.ProductionOrderNo);

            }

            await Repository.UpdateAsync(loProductionOrder);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品入库",
                $"创建半成品入库申请[{entity.Id}],信息为：[{input.Obj2String()}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiEnterStoreDto>(entity);
        }

        private async Task CreateProductionLog(CreateEnterStoreApplyDto input,string productOrderNo)
        {
            int index= await GetLogIndex(productOrderNo);
            decimal q1 = Math.Round(input.EnterStoreQuantity / input.ProductUser.Count,2),
                q2 =  Math.Round(input.EnterStoreQuantity2 / input.ProductUser.Count,2);
            foreach (var user in input.ProductUser)
            {
                index++;
                string logNo = $"{productOrderNo}-{index}";
                var log = new ProductionLog()
                {
                    ProductionNo = logNo,
                    ProductOrderNo = productOrderNo,
                    QuantityWeight = q1,
                    QuantityPcs = q2,
                    KgWeight = input.KgWeight,
                    CarNo = input.CarNo,
                    EmployeeId = user,
                };
                await ProductLogRepository.InsertAsync(log);
                var p = new EmployeeWorkPerformance()
                {
                    PerformanceNo =await WorkTypeDefinition.GetPerformanceNo(PerformanceRepository, WorkTypeDefinition.Product),
                    ProductOrderNo = productOrderNo,
                    RelatedNo = logNo,
                    EmployeeId = user,
                    WorkType =  WorkTypeDefinition.Product,
                    Performance = q2,
                    PerformanceUnit = "千件",
                    PerformanceDesc =
                        $"{q2}千件,总重:{q1}kg,千件重：{input.KgWeight}",
                };
                await PerformanceRepository.InsertAsync(p);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }
         private  async Task<int> GetLogIndex(string productOrderNo)
         {
             var log = await ProductLogRepository.GetAll().Where(a => a.ProductionNo.Contains(productOrderNo + "-")).OrderByDescending(a=>a.CreationTime).FirstOrDefaultAsync();
            if (log==null)
            {
                return 0;
            }

            var no = log.ProductionNo.Substring(log.ProductionNo.IndexOf("-", StringComparison.Ordinal) + 1);
            return Convert.ToInt32(no);
         }
      
        /// <summary>
        /// 确认入库数量（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionEnterStoreApplyMgConfirm), AuditLog("确认入库申请")]
        public async Task<SemiEnterStoreDto> ConfirmSemiEnterStoreQuantity(EntityDto<int> input)
        {
            var entity = await SemiEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == EnterStoreApplyStatusEnum.EnterStored.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("已入库不能再操作!"));
            }
            if (entity.ApplyStatus != EnterStoreApplyStatusEnum.Checked.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("还未检验,不能入库!"));
            }
           

            entity.ApplyStatus = EnterStoreApplyStatusEnum.EnterStored.ToInt() + "";
            var date = Clock.Now;
            entity.EnterStoreDate = date;
            entity.EnterStoreUser = AbpSession.UserName;
            await SemiEnterStoreRepository.UpdateAsync(entity);
            var currentStore = CurrentSemiStoreHouseRepository.GetAll().FirstOrDefault(i => i.ProductionOrderNo == entity.ProductionOrderNo&& i.StoreHouseId==entity.StoreHouseId&&i.StoreLocationNo==entity.StoreLocationNo);
            if (currentStore != null)
            {
                var isCanUpdate =
                    CommonAppService.CheckStoreRecordCanUpdate(currentStore.CurrentSemiStoreHouseNo, 2);
                if (!isCanUpdate)
                    CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
                currentStore.ActualQuantity += entity.ActualQuantity;
                currentStore.TimeLastMod = date;
                currentStore.UserIDLastMod = AbpSession.UserName;
                currentStore.KgWeight = entity.KgWeight;
                await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
            }
            else
            {
                string lcJson = JsonConvert.SerializeObject(entity);
                currentStore = lcJson.GetModel<CurrentSemiStoreHouse>();
                var isCanUpdate =
                    CommonAppService.CheckStoreCanUpdateByLocationNo(currentStore.StoreLocationNo, 2);
                if (!isCanUpdate)
                    CheckErrors(IwbIdentityResult.Failed("该库位正在盘点状态,不可进行出入库更新!"));
                currentStore.CurrentSemiStoreHouseNo = Guid.NewGuid().ToString("N");
                currentStore.TimeCreated = date;
                currentStore.UserIDLastMod = AbpSession.UserName;
                currentStore.ActualQuantity = entity.ActualQuantity;
                currentStore.FreezeQuantity = 0;
                await CurrentSemiStoreHouseRepository.InsertAsync(currentStore);
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
           BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品入库",
                $"确认半成品入库数量[{entity.Id}],排产单入库信息：[确认数量:{entity.ActualQuantity},库存:{currentStore.ActualQuantity},冻结:{currentStore.FreezeQuantity}]。",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiEnterStoreDto>(entity);
        }

 
        /// <summary>
        /// 取消入库申请（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionEnterStoreApplyMgCancel), AuditLog("取消入库申请")]
        public async Task<SemiEnterStoreDto> CancelSemiEnterStoreApplyStatus(EntityDto<int> input)
        {
            var entity = await SemiEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == EnterStoreApplyStatusEnum.EnterStored.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("已入库不能再操作!"));
            }
            entity.ApplyStatus = EnterStoreApplyStatusEnum.Canceled.ToInt() + "";
            await SemiEnterStoreRepository.UpdateAsync(entity);
           
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品入库",
                $"取消半成品入库申请[{entity.Id}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiEnterStoreDto>(entity);
        }
        /// <summary>
        /// 关闭入库申请 （by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionEnterStoreApplyMgClose), AuditLog("关闭入库申请")]
        public async Task<SemiEnterStoreDto> CloseEnterStoreApply(EntityDto<int> input)
        {
            var entity = await SemiEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            entity.IsClose = true;

            //var productionOrder =
            //    await Repository.FirstOrDefaultAsync(a => a.ProductionOrderNo == entity.ProductionOrderNo);
            //if (productionOrder == null)
            //{
            //    CheckErrors(IwbIdentityResult.Failed("未发现排产单!"));
            //    return null;
            //}

            //if (productionOrder.ProductionOrderStatus != ProductionOrderStatusEnum.EnterStore.ToInt()&& productionOrder.ProductionOrderStatus != ProductionOrderStatusEnum.End.ToInt())
            //{
            //    productionOrder.ProductionOrderStatus = ProductionOrderStatusEnum.Producting.ToInt();
            //}
            //await Repository.UpdateAsync(productionOrder);
          
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品入库",
                $"关闭半成品入库申请[{entity.Id}]",
                entity.ProductionOrderNo);
            await SemiEnterStoreRepository.UpdateAsync(entity);
            return ObjectMapper.Map<SemiEnterStoreDto>(entity);
        }

        /// <summary>
        /// 恢复入库申请（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionEnterStoreApplyMgRecovery), AuditLog("恢复入库申请")]
        public async Task<SemiEnterStoreDto> RecoverySemiEnterStoreApplyStatus(EntityDto<int> input)
        {
            var entity = await SemiEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == EnterStoreApplyStatusEnum.EnterStored.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("已入库不能再操作!"));
            }
            entity.ApplyStatus = EnterStoreApplyStatusEnum.Applying.ToInt() + "";
            await SemiEnterStoreRepository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品入库",
                $"恢复半成品入库申请[{entity.Id}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiEnterStoreDto>(entity);
        }

   
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionEnterStoreApplyMgUpdate), AuditLog("修改入库申请")]
        public async Task<SemiEnterStoreDto> UpdateEnterStoreApply(UpdateSemiEnterStoreDto input)
        {
            var entity = await SemiEnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            entity.Quantity = input.Quantity;
            entity.KgWeight = input.KgWeight;
            await SemiEnterStoreRepository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品入库",
                $"修改半成品入库申请数量[{entity.Id}],数量修改为[{input.Quantity}],千斤重修改为[{input.KgWeight}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiEnterStoreDto>(entity);
        }

        #endregion

        #region 半成品出库

        [DisableAuditing,AbpAuthorize(PermissionNames.PagesProductionInfoProductionOutStoreApplyMg)]
        public PagedResultDto<ViewSemiOutStore> GetSemiOutStoreApply(PagedRequestDto input)
        {
            int type = OutStoreApplyTypeEnum.OutAssistant.ToInt();
            var query = ViewSemiOutStoreRepository.GetAll().Where(a => a.ApplyTypes== type);
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
      
        [DisableAuditing,AbpAuthorize(PermissionNames.PagesProductionInfoProductionOutStoreApplyMg)]
        public ViewSemiOutStore GetSemiOutStoreApplyById(int id)
        {
            var outStore = ViewSemiOutStoreRepository.Get(id);

            return outStore;
        }


        /// <summary>
        /// 半成品出库申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOutStoreApplyMgCreate), AuditLog("创建出库申请")]
        public async Task<SemiOutStoreDto> CreateOutStoreApply(SemiOutStoreCreateDto input)
        {
            var currentStore =
                CurrentSemiStoreHouseRepository.FirstOrDefault(i =>
                    i.CurrentSemiStoreHouseNo == input.CurrentSemiStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }
            #region 检查是否可出入库
            var isCanUpdate =
                CommonAppService.CheckStoreRecordCanUpdate(currentStore.CurrentSemiStoreHouseNo, 2);
            if (!isCanUpdate)
                CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
            #endregion
            var canUseQuantit = currentStore.ActualQuantity - currentStore.FreezeQuantity;
            if (canUseQuantit < input.Quantity)
            {
                CheckErrors(IwbIdentityResult.Failed("可用库存量不够，请检查出库数量!"));
                return null;
            }
            currentStore.ActualQuantity -= input.Quantity;
            await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
            input.ApplyOutDate = Clock.Now;
            var entity = ObjectMapper.Map<SemiOutStore>(input);
            entity.ApplyOutStoreSource = OutStoreApplyTypeEnum.OutAssistant.ToInt() + "";
            entity.ApplyTypes = OutStoreApplyTypeEnum.OutAssistant.ToInt();
            entity.ApplyStatus = OutStoreApplyStatusEnum.OutStored.ToInt() + "";
            entity.IsConfirm = true;
            entity.AuditDate = Clock.Now;
            entity.AuditUser = AbpSession.UserName;
            entity.ActualQuantity = input.Quantity;
            entity.TimeCreated = Clock.Now;
            entity.CreatorUserId = AbpSession.UserName;
            entity.TimeLastMod = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserName;
            int id = await SemiOutStoreRepository.InsertAndGetIdAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品出库",
                $"创建出库申请(外协加工)[{entity.Id}],信息为:[{entity.Obj2String()}]",
                entity.ProductionOrderNo);
            var dto=ObjectMapper.Map<SemiOutStoreDto>(entity);
            dto.Id = id;
            return dto;
        }
       
        ///// <summary>
        ///// 半成品出库申请
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[AbpAuthorize(PermissionNames.PagesProductionInfoProductionOutStoreApplyMgCreate), AuditLog("创建出库申请")]
        //public async Task<SemiOutStoreDto> CreateOutStoreApply(SemiOutStoreCreateDto input)
        //{
        //    var currentStore =
        //        CurrentSemiStoreHouseRepository.FirstOrDefault(i =>
        //            i.CurrentSemiStoreHouseNo == input.CurrentSemiStoreHouseNo);
        //    if (currentStore == null)
        //    {
        //        CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
        //        return null;
        //    }
        //    var canUseQuantit = currentStore.ActualQuantity - currentStore.FreezeQuantity;
        //    if (canUseQuantit < input.Quantity)
        //    {
        //        CheckErrors(IwbIdentityResult.Failed("可用库存量不够，请检查出库数量!"));
        //        return null;
        //    }
        //    currentStore.FreezeQuantity += input.Quantity;
        //    await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
        //    input.ApplyOutDate = Clock.Now;
        //    var entity = ObjectMapper.Map<SemiOutStore>(input);
        //    entity.ApplyOutStoreSource = OutStoreApplyTypeEnum.OutAssistant.ToInt() + "";
        //    entity.ApplyTypes = OutStoreApplyTypeEnum.OutAssistant.ToInt();
        //    entity.IsConfirm = false;
        //    entity.AuditDate = Clock.Now;
        //    entity.AuditUser = AbpSession.UserName;
        //    entity.ActualQuantity = 0;
        //    entity.TimeCreated = Clock.Now;
        //    entity.CreatorUserId = AbpSession.UserName;
        //    entity.TimeLastMod = Clock.Now;
        //    entity.UserIDLastMod = AbpSession.UserName;
        //    entity = await SemiOutStoreRepository.InsertAsync(entity);
        //    BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品出库",
        //        $"创建出库申请(外协加工)[{entity.Id}],信息为:[{entity.Obj2String()}]",
        //        entity.ProductionOrderNo);
        //    return ObjectMapper.Map<SemiOutStoreDto>(entity);
        //}
       
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOutStoreApplyMgUpdate), AuditLog("修改出库申请")]
        public async Task<SemiOutStoreDto> UpdateOutStoreApply(SemiOutStoreUpdateDto input)
        {
            var entity = await SemiOutStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == OutStoreApplyStatusEnum.Audited + ""|| entity.ApplyStatus == OutStoreApplyStatusEnum.OutStored + "")
            {
                CheckErrors(IwbIdentityResult.Failed("出库申请已处理，不可操作!"));
                return null;
            } 
            var currentStore =
                CurrentSemiStoreHouseRepository.FirstOrDefault(i =>
                    i.CurrentSemiStoreHouseNo == input.CurrentSemiStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }
            if (entity.ApplyStatus == OutStoreApplyStatusEnum.Applying.ToInt()+"")
            {
                var canUseQuantit = currentStore.ActualQuantity + entity.Quantity - currentStore.FreezeQuantity;
                if (canUseQuantit < input.Quantity)
                {
                    CheckErrors(IwbIdentityResult.Failed("可用库存量不够，请检查出库数量!"));
                    return null;
                }
                currentStore.FreezeQuantity -= entity.Quantity;
                currentStore.FreezeQuantity += input.Quantity;
                await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
                entity.Quantity = input.Quantity;
            }


            entity.TimeLastMod = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserName;
            entity.Remark = input.Remark;
            await SemiOutStoreRepository.UpdateAsync(entity);
          BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品出库",
                $"修改半成品出库申请数量[{entity.Id}],[数量修改为{input.Quantity},冻结数量:{currentStore.FreezeQuantity},库存数量:{currentStore.ActualQuantity}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiOutStoreDto>(entity);
        }

        /// <summary>
        /// 确认出库数量（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionEnterStoreApplyMgConfirm), AuditLog("确认出库申请")]
        public async Task<SemiOutStoreDto> ConfirmSemiOutStoreQuantity(EntityDto<int> input)
        {
            var entity =await SemiOutStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);

            #region 检查是否可出入库
            var isCanUpdate =
                CommonAppService.CheckStoreRecordCanUpdate(entity.CurrentSemiStoreHouseNo, 2);
            if (!isCanUpdate)
                CheckErrors(IwbIdentityResult.Failed("该库存处于退货或者正在盘点状态,不可进行出入库更新!"));
            #endregion

            if (entity.ApplyStatus == OutStoreApplyStatusEnum.OutStored.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("已出库不能再操作!"));
            }
            entity.ApplyStatus = OutStoreApplyStatusEnum.OutStored.ToInt() + "";
            entity.IsConfirm = true;
            var date = Clock.Now;
            entity.OutStoreDate = date;
            entity.OutStoreUser = AbpSession.UserName;
            await SemiOutStoreRepository.UpdateAsync(entity);
            var currentStore = await
                CurrentSemiStoreHouseRepository.FirstOrDefaultAsync(i =>
                    i.CurrentSemiStoreHouseNo == entity.CurrentSemiStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }
            currentStore.FreezeQuantity -= entity.ActualQuantity;
            currentStore.ActualQuantity -= entity.ActualQuantity;
            await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品出库",
                $"确认半成品出库数量[{entity.Id}],[出库数量:{entity.ActualQuantity},冻结数量:{currentStore.FreezeQuantity},库存数量:{currentStore.ActualQuantity}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiOutStoreDto>(entity);
        }


        /// <summary>
        /// 取消出库申请（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOutStoreApplyMgCancel), AuditLog("取消出库申请")]
        public async Task<SemiOutStoreDto> CancelSemiOutStoreApplyStatus(EntityDto<int> input)
        {
            var entity = await SemiOutStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            var currentStore =
                CurrentSemiStoreHouseRepository.FirstOrDefault(i =>
                    i.CurrentSemiStoreHouseNo == entity.CurrentSemiStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }
            currentStore.FreezeQuantity -= entity.Quantity;
            await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
            entity.ApplyStatus = OutStoreApplyStatusEnum.Canceled.ToInt() + "";
            await SemiOutStoreRepository.UpdateAsync(entity);
         
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品出库",
                $"取消半成品出库申请[{entity.Id}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiOutStoreDto>(entity);
        }

        /// <summary>
        ///  关闭出库申请 （by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOutStoreApplyMgClose), AuditLog("关闭出库申请")]
        public async Task<SemiOutStoreDto> CloseOutStoreApply(EntityDto<int> input)
        {
            var entity = await SemiOutStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            entity.IsClose = true;
            await SemiOutStoreRepository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品出库",
                $"关闭半成品出库申请[{entity.Id}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiOutStoreDto>(entity);
        }

        /// <summary>
        /// 恢复出库申请（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOutStoreApplyMgRecovery),AuditLog("恢复出库申请")]
        public async Task<SemiOutStoreDto> RecoverySemiOutStoreApplyStatus(EntityDto<int> input)
        {
            var entity = await SemiOutStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            var currentStore =
                CurrentSemiStoreHouseRepository.FirstOrDefault(i =>
                    i.CurrentSemiStoreHouseNo == entity.CurrentSemiStoreHouseNo);
            if (currentStore == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                return null;
            }

            if (currentStore.ActualQuantity- currentStore.FreezeQuantity < entity.Quantity)
            {
                CheckErrors(IwbIdentityResult.Failed("可用库存量不够，请检查出库数量!"));
                return null;
            }
            currentStore.FreezeQuantity += entity.Quantity;
            await CurrentSemiStoreHouseRepository.UpdateAsync(currentStore);
            entity.ApplyStatus = OutStoreApplyStatusEnum.Applying.ToInt() + "";
            await SemiOutStoreRepository.UpdateAsync(entity);
          
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品出库",
                $"恢复半成品出库申请[{entity.Id}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<SemiOutStoreDto>(entity);
        }


        #endregion
        #region 成品改镀
        [DisableAuditing, AbpAuthorize(PermissionNames.PagesProductionInfoRePlatingOutStoreApplyMg)]
        public PagedResultDto<ViewProductOutStore> GetRePlatingOutStoreApply(PagedRequestDto input)
        {
            int type = OutStoreApplyTypeEnum.RePlating.ToInt();
            var query = ViewProductOutStoreRepository.GetAll().Where(a => a.ApplyOutStoreSourceType == type);
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

            var totalCount = query.Count();

            query = query.OrderByDescending(i => i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = query.ToList();

            var dtos = new PagedResultDto<ViewProductOutStore>(
                totalCount, entities
            );
            return dtos;
        }

        [AbpAuthorize(PermissionNames.PagesProductionInfoRePlatingOutStoreApplyMgCancel), AuditLog("拒绝成品改镀出库申请")]
        public async Task<ProductOutStoreDto> CancelFinishOutStoreApply(EntityDto<int> input)
        {
            var entity = await ProductOutStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            entity.IsClose = true;
            entity.ApplyStatus = OutStoreApplyStatusEnum.Refused.ToInt();
            await ProductOutStoreRepository.UpdateAsync(entity);
            var currentStore = await CurrentProductStoreHouseRepository.FirstOrDefaultAsync(a =>
                a.CurrentProductStoreHouseNo == entity.CurrentProductStoreHouseNo);
            currentStore.FreezeQuantity -= entity.Quantity;//释放冻结
            await CurrentProductStoreHouseRepository.UpdateAsync(currentStore);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "成品出库",
                $"关闭成品出库申请[{entity.Id}]",
                entity.ProductionOrderNo);
            return ObjectMapper.Map<ProductOutStoreDto>(entity);
        }
       [AbpAuthorize(PermissionNames.PagesProductionInfoRePlatingOutStoreApplyMgExport), AuditLog("导出改镀出库申请")]
        public async Task<string> RePlatingExportApply(EntityDto<int> input)
        {
            var entity = await ProductOutStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到排产单信息。"));
                return null;
            }
            var productEntity = ProductRepository.Get(entity.ProductNo);
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources/ProductionOrderTemplate/RePlatingBillTemplate.xls";
            var work = ExcelHelper.CreateWorkBook03(path);
           
            var sheet1 = work.GetSheet("Sheet1");
            sheet1.GenerateCell(3, 5).SetCellValue(entity.ProductionOrderNo);
            sheet1.GenerateCell(4, 2).SetCellValue($"{productEntity.PartNo}");
            sheet1.GenerateCell(4, 5).SetCellValue(productEntity.ProductName);
            sheet1.GenerateCell(5, 2).SetCellValue(productEntity.Material);
            sheet1.GenerateCell(5, 4).SetCellValue(productEntity.Rigidity);
            sheet1.GenerateCell(5, 6).SetCellValue(productEntity.Model);
            sheet1.GenerateCell(6, 2).SetCellValue(productEntity.SurfaceColor + "");
            sheet1.GenerateCell(9, 1).SetCellValue($"日期：{Clock.Now:yyyy-MM-dd}");


            var savePath = "Download/Excel/ProductionOrder";
            var fileName = $"改镀单[{entity.Id}]-{Clock.Now:yyyyMMddHHmmss}.xls";
            var result = work.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"{savePath}/{fileName}";



        }
        #endregion
        #region 数据统计

        /// <summary>
        /// 生产统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductionReportDto> QueryProductionReport(QueryProductionReportDto input)
        {
            var startDate = input.Year.GetDateByType(input.Month,out var endDate ,out var dateStr);

            //查询指定周期员工生产日志
            var query = ProductLogRepository.GetAllIncluding(a => a.EmployeeInfo)
                .Where(a => a.CreationTime >= startDate && a.CreationTime < endDate).OrderBy(a=>a.CreationTime);

            IQueryable<ProductionReportItem> itemQuery = input.EmployeeId != null
                ?
                //查询指定员工的统计(关联半成品入库视图获取半成品信息)
                query.Where(a => a.EmployeeId == input.EmployeeId)
                    .Join(ViewSemiEnterStoreRepository.GetAll(),
                    a => a.ProductOrderNo, s => s.ProductionOrderNo, (a, s) => new ProductionReportItem()
                    {
                        ProductDate = a.CreationTime,
                        ProductionOrderNo = a.ProductOrderNo,
                        ProductNo = s.SemiProductNo,
                        ProductName = s.SemiProductName,
                        CarNo = a.CarNo,
                        PartNo = s.PartNo,
                        Model = s.Model,
                        Material = s.Material,
                        SurfaceColor = s.SurfaceColor,
                        Rigidity = s.Rigidity,
                        KgQuantity = a.QuantityWeight,
                        PcsQuantity = a.QuantityPcs,
                        KgWeight = a.KgWeight,
                        EmployeeId = a.EmployeeId,
                        EmployeeNo = a.EmployeeInfo.No,
                        EmployeeName = a.EmployeeInfo.Name
                    }).Distinct()
                :
                // 查询所有员工的统计
                query.Select(a => new ProductionReportItem()
                {
                    ProductDate= a.CreationTime,
                    ProductionOrderNo = a.ProductOrderNo,
                    ProductNo = a.ProductionNo,
                    KgQuantity = a.QuantityWeight,
                    PcsQuantity = a.QuantityPcs,
                    KgWeight = a.KgWeight,
                    EmployeeId = a.EmployeeId,
                    EmployeeNo = a.EmployeeInfo.No,
                    EmployeeName = a.EmployeeInfo.Name
                });

            var items = await itemQuery.ToListAsync();
            var dto = new ProductionReportDto(dateStr, items,input.EmployeeId);
            return dto;
        }

       /// <summary>
       /// 外购统计
       /// </summary>
       /// <param name="input"></param>
       /// <returns></returns>
        public async Task<ProductionReportDto> QueryOutsourcingReport(QueryProductionReportDto input)
        {
            var startDate = input.Year.GetDateByType(input.Month,out var endDate ,out var dateStr);
            
            //查询外购的排产单进行统计(不是新建状态的)
            var query = Repository.GetAll().Where(a => a.TimeCreated >= startDate && a.TimeCreated < endDate && a.ProductionType=="1" && a.ProductionOrderStatus!=1).OrderBy(a=>a.TimeCreated);
          
            //联合查询半成品信息
            IQueryable<ProductionReportItem> itemQuery = from a in query
                join sp in SemiProductRepository.GetAll() on a.SemiProductNo equals sp.Id into l
                from s in l.DefaultIfEmpty()
                select new ProductionReportItem()
                {
                    ProductDate = a.TimeCreated,
                    ProductionOrderNo = a.ProductionOrderNo,
                    ProductNo = s.Id,
                    ProductName = s.SemiProductName,
                    PartNo = s.PartNo,
                    Model = s.Model,
                    Material = s.Material,
                    SurfaceColor = s.SurfaceColor,
                    Rigidity = s.Rigidity,
                    KgQuantity = a.Quantity * a.KgWeight,
                    PcsQuantity = a.Quantity,
                    KgWeight = a.KgWeight,
                };
            var items = await itemQuery.ToListAsync();
            var dto = new ProductionReportDto(dateStr, items,0);
            return dto;
        }

       /// <summary>
       /// 导出外购报表
       /// </summary>
       /// <param name="input"></param>
       /// <returns></returns>
         public async Task<string> ExportOutsourcingReport(QueryProductionReportDto input)
        {
            var startDate = input.Year.GetDateByType(input.Month,out var endDate ,out var dateStr);


            var dto = await QueryOutsourcingReport(input);
            if (dto.Items == null || !dto.Items.Any())
            {
                CheckErrors(IwbIdentityResult.Failed("未查询采购信息。"));
                return null;
            }
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources/ProductionOrderTemplate/OutsourcingReport.xls";
            var work = ExcelHelper.CreateWorkBook03(path);

            var sheet1 = work.GetSheet("Sheet1");
            sheet1.GenerateCell(2, 10).SetCellValue(dateStr);
            int index = 4, count = dto.Items.Count;
            sheet1.InsertRows(index, count-1);
            foreach (var item in dto.Items)
            {
                sheet1.GenerateCell(index, 1).SetValue<int>(index - 3);
                sheet1.GenerateCell(index, 2).SetValue(item.ProductionOrderNo);
                sheet1.GenerateCell(index, 3).SetValue(item.ProductName);
                sheet1.GenerateCell(index, 4).SetValue(item.PartNo);
                sheet1.GenerateCell(index, 5).SetValue(item.Model);
                sheet1.GenerateCell(index, 6).SetValue<decimal>(item.PcsQuantity);
                sheet1.GenerateCell(index, 7).SetValue<decimal>(item.KgWeight);
                sheet1.GenerateCell(index, 8).SetValue<decimal>(item.KgQuantity);
                sheet1.GenerateCell(index, 9).SetValue<DateTime>(item.ProductDate);
                sheet1.GenerateCell(index, 10).SetValue(item.Material);
                sheet1.GenerateCell(index, 11).SetValue(item.SurfaceColor);

                index++;
            }
            sheet1.GenerateCell(index, 2).SetValue($"总重量：{dto.KgTotal} kg");
            sheet1.GenerateCell(index, 4).SetValue($"总件数：{dto.PcsTotal} 千件");
            var savePath = "Download/Excel/ProductionOrder/OutsourcingReport";
            var fileName = $"采购统计-{dateStr}-{Clock.Now:yyMMddHHmmss}.xls";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }

        
        #endregion

        #region ExcelExport
        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionOrderMgExport), AuditLog("导出排产单Excel")]
        public async Task<string> ExcelExport(ExportDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到排产单信息。"));
                return null;
            }

            var semi = await SemiProductRepository.FirstOrDefaultAsync(a => a.Id == entity.SemiProductNo);
            if (semi == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到排产单中半成品信息。"));
                return null;
            }
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources/ProductionOrderTemplate/ProductionOrderTemplate.xls";
            var work = ExcelHelper.CreateWorkBook03(path);

            var sheet1 = work.GetSheet("Template1");
            sheet1.GenerateCell(2, 10).SetCellValue(entity.ProductionOrderNo);
            sheet1.GenerateCell(4, 11).SetCellValue($"{entity.ProductionOrderNo}{Clock.Now:MMdd}");
            sheet1.GenerateCell(5, 4).SetCellValue(semi.PartNo);
            sheet1.GenerateCell(5, 7).SetCellValue(semi.SemiProductName);
            sheet1.GenerateCell(5, 11).SetCellValue(semi.Model);
            sheet1.GenerateCell(6, 4).SetCellValue(semi.Material);
            sheet1.GenerateCell(6, 11).SetCellValue(entity.Quantity + "");
            sheet1.GenerateCell(8, 4).SetCellValue(semi.ProductDesc);
            sheet1.GenerateCell(8, 11).SetCellValue(semi.Rigidity);
            sheet1.GenerateCell(21, 7).SetCellValue(semi.SurfaceColor);
            var sheet2 = work.GetSheet("Template2");
            sheet2.GenerateCell(1, 17).SetCellValue(entity.ProductionOrderNo);
            sheet2.GenerateCell(3, 16).SetCellValue($"{entity.ProductionOrderNo}{Clock.Now:MMdd}");
            sheet2.GenerateCell(4, 3).SetCellValue(semi.PartNo);
            sheet2.GenerateCell(4, 9).SetCellValue(semi.SemiProductName);
            sheet2.GenerateCell(4, 16).SetCellValue(semi.Model);
            sheet2.GenerateCell(5, 3).SetCellValue(semi.Material);
            sheet2.GenerateCell(7, 3).SetCellValue(semi.ProductDesc);
            var sheet3 = work.GetSheet("Template3");
            sheet3.GenerateCell(2, 10).SetCellValue(entity.ProductionOrderNo);
            sheet3.GenerateCell(4, 4).SetCellValue(semi.PartNo);
            sheet3.GenerateCell(4, 11).SetCellValue($"{entity.ProductionOrderNo}{Clock.Now:MMdd}");
            //sheet3.GenerateCell(5, 4).SetCellValue(semi.PartNo);
            sheet3.GenerateCell(5, 7).SetCellValue(semi.SemiProductName);
            sheet3.GenerateCell(5, 11).SetCellValue(semi.Model);
            sheet3.GenerateCell(6, 4).SetCellValue(semi.Material);
            sheet3.GenerateCell(6, 11).SetCellValue(entity.Quantity + "");
            sheet3.GenerateCell(8, 4).SetCellValue(semi.ProductDesc);
            sheet3.GenerateCell(8, 11).SetCellValue(semi.Rigidity);
            var savePath= "Download/Excel/ProductionOrder";
            var fileName = $"排产单[{entity.ProductionOrderNo}]-{Clock.Now:yyyyMMddHHmmss}.xls";
            var result= work.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }

            entity.HasExported = true;
            await Repository.UpdateAsync(entity);
            return $"/{savePath}/{fileName}";
        }
        [AbpAuthorize(PermissionNames.PagesProductionInfoOutProductionOrderMgExportOut), AuditLog("导出外协排产单Excel")]
        public async Task<string> ExcelExportOut(EntityDto<string> input)
        {
            string selectIds = input.Id;
            var ids = selectIds.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            if (!ids.Any())
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到排产单信息."));
                return null;
            }
           // List<ProductionOrder> sheetProductionOrders = new List<ProductionOrder>();
           Dictionary<string, List<ProductionOrder>> dicSheet = new Dictionary<string, List<ProductionOrder>>();
            foreach (var id in ids)
            {
                int.TryParse(id, out int enId);
                var entity = await Repository.FirstOrDefaultAsync(a => a.Id == enId);
                if (entity == null)
                {
                    CheckErrors(IwbIdentityResult.Failed("未查询到排产单信息。"));
                    return null;
                }

                if (entity.ProductionOrderStatus == ProductionOrderStatusEnum.Start.ToInt())
                {
                    CheckErrors(IwbIdentityResult.Failed("存在排产单未确认审核，不可导出！"));
                    return null;
                }
                string enKey = entity.OutsourcingFactory + "@@" + entity.ProcessingType;
                if (dicSheet.ContainsKey(enKey))
                {
                    dicSheet[enKey].Add(entity);
                }
                else
                {
                    List<ProductionOrder> productionOrders = new List<ProductionOrder>();
                    productionOrders.Add(entity);
                    dicSheet.Add(enKey, productionOrders);
                }
                
                entity.HasExported = true;
                await Repository.UpdateAsync(entity);
            }
            string path3 = AppDomain.CurrentDomain.BaseDirectory + "Resources/ProductionOrderTemplate/HotProcess.xls";
            string path2 = AppDomain.CurrentDomain.BaseDirectory + "Resources/ProductionOrderTemplate/SurfaceColorProcess.xls";
            //List<string> savePathList = new List<string>();
            string cSavePath = "Download/Excel/ProductionOrder";
            string fileDir= "Out" + DateTime.Now.ToString("yyMMddHHmmss") + "-" + new Random().Next(1000);
            var savePath = "Download/Excel/ProductionOrder/"+ fileDir;
            foreach (var ds in dicSheet)
            {
                string[] ot = ds.Key.Split("@@");
                string type = ot[1];
                string outFactoryId = ds.Value[0].OutsourcingFactory;
                var outFactory = await OutFactoryRepository.FirstOrDefaultAsync(a => a.Id == outFactoryId);
                HSSFWorkbook work = null;
                if (type == "2")
                {
                    work = ExcelHelper.CreateWorkBook03(path2);
                  
                    var sheet1 = work.GetSheet("Sheet1");
                    sheet1.GenerateCell(4, 8).SetCellValue("单号:"+DateTime.Now.ToString("yyMMddHHmmss")+new Random().Next(1000));
                    sheet1.GenerateCell(7, 2).SetCellValue(outFactory?.OutFactoryName);
                    sheet1.GenerateCell(7, 9).SetCellValue(outFactory?.Address);
                    sheet1.GenerateCell(8, 2).SetCellValue(outFactory?.Telephone);
                    sheet1.GenerateCell(8, 9).SetCellValue(outFactory?.LinkMan);
                    int index = 1;
                    foreach (var e in ds.Value)
                    {
                        var semi = await SemiProductRepository.FirstOrDefaultAsync(a => a.Id == e.SemiProductNo);
                        sheet1.GenerateCell(10+index, 1).SetCellValue(index);
                        sheet1.GenerateCell(10 + index, 2).SetCellValue(semi?.PartNo);
                        sheet1.GenerateCell(10 + index, 3).SetCellValue(semi?.SemiProductName);
                        sheet1.GenerateCell(10 + index, 4).SetCellValue(semi?.Model);
                        sheet1.GenerateCell(10 + index, 7).SetCellValue(semi?.SurfaceColor);
                        sheet1.GenerateCell(10 + index, 11).SetCellValue(e?.ProductionOrderNo);
                        index++;
                    }
                }
                else if (type == "3")
                {
                    work = ExcelHelper.CreateWorkBook03(path3);
                    var sheet1 = work.GetSheet("Sheet1");
                  
                    sheet1.GenerateCell(4, 8).SetCellValue("单号:" + DateTime.Now.ToString("yyMMddHHmmss") + new Random().Next(1000));
                    sheet1.GenerateCell(7, 2).SetCellValue(outFactory?.OutFactoryName);
                    sheet1.GenerateCell(7, 9).SetCellValue(outFactory?.Address);
                    sheet1.GenerateCell(8, 2).SetCellValue(outFactory?.Telephone);
                    sheet1.GenerateCell(8, 9).SetCellValue(outFactory?.LinkMan);
                    int index = 1;
                    foreach (var e in ds.Value)
                    {
                        var semi = await SemiProductRepository.FirstOrDefaultAsync(a => a.Id == e.SemiProductNo);
                        sheet1.GenerateCell(10 + index, 1).SetCellValue(index);
                        sheet1.GenerateCell(10 + index, 2).SetCellValue(semi?.PartNo);
                        sheet1.GenerateCell(10 + index, 3).SetCellValue(semi?.SemiProductName);
                        sheet1.GenerateCell(10 + index, 4).SetCellValue(semi?.Model);
                        sheet1.GenerateCell(10 + index, 6).SetCellValue(semi?.Material);
                        sheet1.GenerateCell(10 + index, 8).SetCellValue(semi?.SurfaceColor);
                        sheet1.GenerateCell(10 + index, 10).SetCellValue(semi?.Rigidity);
                        sheet1.GenerateCell(10 + index, 11).SetCellValue(e?.ProductionOrderNo);
                        index++;
                    }
                }
                var fileName = $"外协排产单[{ ds.Value[0].ProductionOrderNo}]-{Clock.Now:HHmmss}.xls";
                //string lcPath = $"{AppDomain.CurrentDomain.BaseDirectory}{savePath}" + "\\" + fileName;
                var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
                if (!result.IsNullOrEmpty())
                {
                    CheckErrors(IwbIdentityResult.Failed(result));
                    return null;
                }
                //savePathList.Add(lcPath);
            }

            ZipHelper.ZipDirectory($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", $"{AppDomain.CurrentDomain.BaseDirectory}{cSavePath}/{fileDir}.zip" );
            
            return $"/{cSavePath}/{fileDir}.zip";
        }


        #endregion

    }
}

