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
using Abp.Json;
using Abp.Runtime.Caching;
using Abp.Timing;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using IwbZero.Helper;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.SysAttachFiles;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;
using ShwasherSys.BasicInfo.OutFactory;
using ShwasherSys.Common;
using ShwasherSys.Lambda;
using ShwasherSys.Inspection.Dto;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ProductionOrderInfo.Dto;
using ShwasherSys.ProductInfo;
using ShwasherSys.SemiProductStoreInfo;
using LambdaExpType = ShwasherSys.Lambda.LambdaExpType;
using LambdaFieldType = ShwasherSys.Lambda.LambdaFieldType;
using LambdaObject = ShwasherSys.Lambda.LambdaObject;

namespace ShwasherSys.Inspection
{
    [AbpAuthorize, AuditLog("技术检验信息")]
    public class ProductInspectAppService : ShwasherAsyncCrudAppService<ProductInspectInfo, ProductInspectDto, int, PagedRequestDto, ProductInspectCreateDto, ProductInspectUpdateDto >
        , IProductInspectAppService
    {
        public ProductInspectAppService(
            IRepository<SysAttachFile> attachRepository,
            IRepository<BusinessLog> logRepository,
            IRepository<SemiEnterStore> enterStoreRepository,
            IRepository<TemplateInfo> templateRepository,
            IRepository<ProductInspectReportContent> reportContentRepository,
            IRepository<ProductionOrder> productionOrderRepository,
            IIwbSettingManager settingManager, 
			ICacheManager cacheManager,
			IRepository<ProductInspectInfo, int> repository, IRepository<SemiProducts, string> semiProductRepository, IRepository<DisqualifiedProduct> disProductRepository, IRepository<ViewSemiEnterStore> viewSemiEnterStoreRepository, IQueryAppService queryAppService, IRepository<ProductInspectReport> reportRepository, IRepository<OutFactory, string> outFactoryRepository) : base(repository, "ProductInspectNo")
        {
			SettingManager = settingManager;
            CacheManager = cacheManager;
            AttachRepository = attachRepository;
            LogRepository = logRepository;
            EnterStoreRepository = enterStoreRepository;
            TemplateRepository = templateRepository;
            ReportContentRepository = reportContentRepository;
            ProductionOrderRepository = productionOrderRepository;
            SemiProductRepository = semiProductRepository;
            DisProductRepository = disProductRepository;
            ViewSemiEnterStoreRepository = viewSemiEnterStoreRepository;
            QueryAppService = queryAppService;
            ReportRepository = reportRepository;
            OutFactoryRepository = outFactoryRepository;
        }

        protected override bool KeyIsAuto { get; set; } = false;
        public IQueryAppService QueryAppService { get; }
        public IRepository<SysAttachFile> AttachRepository { get; }
        public IRepository<BusinessLog> LogRepository { get; }
        public IRepository<SemiEnterStore> EnterStoreRepository { get; }
        public IRepository<TemplateInfo> TemplateRepository { get; }
        public IRepository<ProductInspectReport> ReportRepository { get; }
        public IRepository<ProductInspectReportContent> ReportContentRepository { get; }
        public IRepository<ProductionOrder> ProductionOrderRepository { get; }
        public IRepository<SemiProducts,string> SemiProductRepository { get; }
        public IRepository<ViewSemiEnterStore> ViewSemiEnterStoreRepository { get; }
        public IRepository<DisqualifiedProduct> DisProductRepository { get; }
        public IRepository<OutFactory,string> OutFactoryRepository { get; }

        #region GetSelect

        [DisableAuditing]
        public async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var slist = new List<SelectListItem> {new SelectListItem {Text = @"请选择...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.ProductInspectNo, Value = l.InspectContent });
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
                str += $"<option value=\"{l.ProductInspectNo}\">{l.InspectContent}</option>";
            }
            return str;
        }

        #endregion

        #region CURD
        
		#region Get

		[DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectProductInspectMg)]
        public  Task<ProductInspectInfo> GetEntityById(int id)
        {
            return Repository.GetAsync(id);
        }
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectProductInspectMg)]
        public  Task<ProductInspectInfo> GetEntityByNo(string no)
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
            var exp = obj.GetExp<ProductInspectInfo>();
            return Repository.FirstOrDefaultAsync(exp);
        }

		[DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectProductInspectMg)]
        public  async Task<ProductInspectDto> GetDtoById(int id)
        {
            var entity = await GetEntityById(id);
            return MapToEntityDto(entity);
        }
		[DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectProductInspectMg)]
        public  async Task<ProductInspectDto> GetDtoByNo(string no)
        {
            var entity = await GetEntityByNo(no);
            return MapToEntityDto(entity);
        }

      

        #endregion

        #region 创建报告

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectProductInspectMgQuery)]
        public async Task<PagedResultDto<ProductionOrderDto>> GetAllInspect(PagedRequestDto input)
        {
            var query = ProductionOrderRepository.GetAll();
            var querySemiPro = SemiProductRepository.GetAll();
            var queryOutFactory = OutFactoryRepository.GetAll();
            var queryEntity = from u in query
                join s in querySemiPro on u.SemiProductNo equals s.Id into pu
                from luq in pu.DefaultIfEmpty() join ofo in queryOutFactory on u.OutsourcingFactory equals ofo.Id into fu from osfu in fu.DefaultIfEmpty()
                select new ProductionOrderDto
                {
                    Model = luq.Model ?? "",
                    CarNo = u.CarNo,
                    EnterQuantity = u.EnterQuantity,
                    Id = u.Id,
                    CreatorUserId = u.CreatorUserId,
                    IsChecked = u.IsChecked,
                    IsLock = u.IsLock,
                    Material = luq.Material ?? "",
                    PartNo = luq.PartNo ?? "",
                    SemiProductName = luq.SemiProductName??"",
                    PlanProduceDate = u.PlanProduceDate,
                    RawMaterials = u.RawMaterials,
                    SurfaceColor = luq.SurfaceColor ?? "",
                    Rigidity = luq.Rigidity ?? "",
                    Remark = u.Remark,
                    UserIDLastMod = u.UserIDLastMod,
                    TimeCreated = u.TimeCreated,
                    TimeLastMod = u.TimeLastMod,
                    Size = u.Size,
                    ProductionType = u.ProductionType,
                    ProcessingType = u.ProcessingType,
                    ProcessingLevel = u.ProcessingLevel,
                    SourceProductionOrderNo = u.SourceProductionOrderNo,
                    StoveNo = u.StoveNo,
                    Quantity = u.Quantity,
                    ProductionOrderNo = u.ProductionOrderNo,
                    ProductionOrderStatus = u.ProductionOrderStatus,
                    SemiProductNo = u.SemiProductNo,
                    OutsourcingFactory = u.OutsourcingFactory,
                    OutsourcingFactoryName = osfu.OutFactoryName
                };
            int enterStoredState = ProductionOrderStatusEnum.EnterStore.ToInt();
            int storingState = ProductionOrderStatusEnum.Storeing.ToInt();
            //入库中和已经入库的可以生成报告
            queryEntity = queryEntity
                .Where(a => a.IsChecked != 1 && (a.ProductionOrderStatus == storingState||a.ProductionOrderStatus== enterStoredState));
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
                var exp = objList.GetExp<ProductionOrderDto>();
                queryEntity = queryEntity.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(queryEntity);

            queryEntity = queryEntity.OrderByDescending(a => a.TimeCreated);
            queryEntity = queryEntity.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await AsyncQueryableExecuter.ToListAsync(queryEntity);

            var dtos = new PagedResultDto<ProductionOrderDto>(
                totalCount,
                entities
            );
            return dtos;
        }



        [AbpAuthorize(PermissionNames.PagesProductInspectProductInspectMgTemplate),AuditLog("报告模板")]
        public  async Task Template(ProductInspectTemplateDto input)
        {
            if (input.ReportTemplate.IsNullOrEmpty())
            {
               CheckErrors(IwbIdentityResult.Failed("模板内容不能为空"));
            }
            var entity = await TemplateRepository.FirstOrDefaultAsync(a => a.TemplateNo == ShwasherConsts.InspectReportTemplateName);
            if (entity == null)
            {
                entity = new TemplateInfo
                {
                    TemplateNo = ShwasherConsts.InspectReportTemplateName,
                    Name = "检验报告单模板",
                    Content = input.ReportTemplate,
                    Type = 0
                };
                entity = await TemplateRepository.InsertAsync(entity);
            }
            else
            {
                entity.Content = input.ReportTemplate;
                entity = await TemplateRepository.UpdateAsync(entity);
            }
            await CacheManager.GetCache(ShwasherConsts.TemplateCache)
                .SetAsync(ShwasherConsts.InspectReportTemplateName, entity.Content);
        }
        [AbpAuthorize(PermissionNames.PagesProductInspectProductInspectMgCreate), AuditLog("生成报告")]
        public async Task<ProductInspectDto> CreateInspect(ProductInspectCreateDto input)
        {
            if (input.ReportContent.IsNullOrEmpty())
            {
                CheckErrors(IwbIdentityResult.Failed("报告内容不能为空！"));
                return null;
            }
            //var productOrder =
            //    await ProductionOrderRepository.FirstOrDefaultAsync(a =>
            //        a.ProductionOrderNo == input.ProductionOrderNo);
            //if (productOrder==null)
            //{
            //    CheckErrors(IwbIdentityResult.Failed("未发现排产单！"));
            //    return null;
            //}

            //productOrder.IsChecked = 1;
            //productOrder.InspectDate = Clock.Now;
            //var enterStores =
            //    await EnterStoreRepository.GetAllListAsync(a => a.ProductionOrderNo == productOrder.ProductionOrderNo);
            //if (enterStores!=null&& enterStores.Any())
            //{
            //    foreach (var store in enterStores)
            //    {
            //        store.IsClose = true;
            //        await EnterStoreRepository.UpdateAsync(store);
            //        BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "技术检验",
            //            $"完成检验。关闭入库申请,检验结果为[{result}]", productOrder.ProductionOrderNo);
                   
            //    }
            //}
            //await ProductionOrderRepository.UpdateAsync(productOrder);
            var report =
                await ReportRepository.FirstOrDefaultAsync(a => a.ProductionOrderNo == input.ProductionOrderNo);
            if (report == null)
            {
                report= new ProductInspectReport()
                {
                    ProductInspectReportNo = await    InspectConfirmStateDefinition.GetReportNo(ReportRepository),
                    ProductionOrderNo = input.ProductionOrderNo,
                    ConfirmStatus = InspectConfirmStateDefinition.New,
                    InspectCount = 1,
                    SemiProductNo = input.SemiProductNo
                };
                await ReportRepository.InsertAsync(report);
            }
            else
            {
                if (report.ConfirmStatus ==  InspectConfirmStateDefinition.Confirm)
                {
                    CheckErrors(IwbIdentityResult.Failed("检测报告已最终确认生成，不能再添加记录！"));

                }
                report.InspectCount++;
                await ReportRepository.UpdateAsync(report);
            }

            input.InspectMember = AbpSession.UserName;
            input.ProductInspectReportNo = report.ProductInspectReportNo;
            input.ProductInspectNo = $"{report.ProductInspectReportNo}-{report.InspectCount}";
            await CurrentUnitOfWork.SaveChangesAsync();
            var dto=  await CreateEntity(input);
            var reportContent =
                await ReportContentRepository.FirstOrDefaultAsync(a =>
                    a.PtoductInspectNo == report.ProductInspectReportNo);
            if (reportContent == null)
            {
                
                await ReportContentRepository.InsertAsync(new ProductInspectReportContent()
                {
                    ProductionOrderNo = dto.ProductionOrderNo,
                    PtoductInspectNo = report.ProductInspectReportNo,
                    ReportContent = input.ReportContent
                });
            }
            else
            {
                reportContent.ReportContent = input.ReportContent;
                await ReportContentRepository.UpdateAsync(reportContent);
            }


            await ReportContentRepository.InsertAsync(new ProductInspectReportContent()
            {
                ProductionOrderNo = dto.ProductionOrderNo,
                PtoductInspectNo = dto.ProductInspectNo,
                ReportContent = input.ReportContent
            });
            string result= input.InspectResult == 0 ? "不合格" : "合格";
            BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "生成检验报告",
                $"检验项目[{dto.InspectSubject}],检验结果为[{result}],检验报告内容：[{input.ReportContent}]", input.ProductionOrderNo,report.ProductInspectReportNo,dto.ProductInspectNo);
            if (input.AttachFiles!=null && input.AttachFiles.Any())
            {
                foreach (var attach in input.AttachFiles)
                {
                    attach.AttachNo = Guid.NewGuid().ToString("N");
                    attach.TableName = "Product";
                    attach.ColumnName = "Inspect";
                    attach.SourceKey = report.ProductInspectReportNo;
                    await CreateAttach(attach);
                }
            }
            return dto;
        }
        /// <summary>
        /// 最终确认报告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesProductInspectInspectReportConfirmReport), AuditLog("确认报告")]
        public async Task ConfirmReport(ProductReportConfirmDto input)
        {
            var entity =
                await ReportRepository.FirstOrDefaultAsync(
                    a => a.ProductInspectReportNo == input.ProductInspectReportNo);
            if (entity == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到检测报告记录！"));
                return;
            }
            if (  entity.ConfirmStatus == InspectConfirmStateDefinition.Confirm)
            {
                CheckErrors(IwbIdentityResult.Failed("检测报告已确认，请勿重复操作！"));
                return;
            }
            var productOrder =
                await ProductionOrderRepository.FirstOrDefaultAsync(a =>
                    a.ProductionOrderNo == entity.ProductionOrderNo);
            if (productOrder == null)
            {
                CheckErrors(IwbIdentityResult.Failed("未发现排产单！"));
                return ;
            }

            productOrder.IsChecked = 1;
            productOrder.InspectDate = Clock.Now;
            await ProductionOrderRepository.UpdateAsync(productOrder);
            entity.ConfirmDate=DateTime.Now;
            entity.ConfirmUser = AbpSession.UserName;
            entity.ConfirmStatus = InspectConfirmStateDefinition.Confirm;
            entity.InspectContent = input.InspectContent;
            await ReportRepository.UpdateAsync(entity);
            var reportContent =
                await ReportContentRepository.FirstOrDefaultAsync(a =>
                    a.PtoductInspectNo == entity.ProductInspectReportNo);
            if (reportContent == null)
            {
                
                await ReportContentRepository.InsertAsync(new ProductInspectReportContent()
                {
                    ProductionOrderNo = entity.ProductionOrderNo,
                    PtoductInspectNo = entity.ProductInspectReportNo,
                    ReportContent = input.ReportContent
                });
            }
            else
            {
                reportContent.ReportContent = input.ReportContent;
                await ReportContentRepository.UpdateAsync(reportContent);
            }
            
            BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "生成检验报告",
                $"最终确认报告,检验报告内容：[{input.ReportContent}]", entity.ProductionOrderNo,entity.ProductInspectReportNo);
            if (input.AttachFiles!=null && input.AttachFiles.Any())
            {
                foreach (var attach in input.AttachFiles)
                {
                    attach.AttachNo = Guid.NewGuid().ToString("N");
                    attach.TableName = "Product";
                    attach.ColumnName = "Inspect";
                    attach.SourceKey = entity.ProductInspectReportNo;
                    await CreateAttach(attach);
                }
            }
        }

        

        #endregion
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectProductInspectMgQuery)]
        public override async Task<PagedResultDto<ProductInspectDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            var queryOrder = ProductionOrderRepository.GetAll();
            var querySemiPro = SemiProductRepository.GetAll();
            var queryEntity = from u in query
                join s in querySemiPro on u.SemiProductNo equals s.Id into pu
                from luq in pu.DefaultIfEmpty() 
                join o in queryOrder on u.ProductionOrderNo equals o.ProductionOrderNo  into ou
                from ouq in ou.DefaultIfEmpty() 
                select new ProductInspectDto
                {
                    Model = luq.Model ?? "",
                    Id = u.Id,
                    CreatorUserId = u.CreatorUserId,
                    IsLock = u.IsLock,
                    Material = luq.Material ?? "",
                    SurfaceColor = luq.SurfaceColor ?? "",
                    Rigidity = luq.Rigidity ?? "",
                    UserIDLastMod = u.UserIDLastMod,
                    TimeCreated = u.TimeCreated,
                    TimeLastMod = u.TimeLastMod,
                    ProductionOrderNo = u.ProductionOrderNo,
                    SemiProductNo = u.SemiProductNo,
                    SemiProductName = luq.SemiProductName,
                    //InspectStatus = u.InspectStatus,
                    InspectSubject = u.InspectSubject,
                    InspectResult = u.InspectResult,
                    InspectDate = u.InspectDate,
                    InspectMember = u.InspectMember,
                    InspectContent = u.InspectContent,
                    ProductInspectNo = u.ProductInspectNo,
                    ProcessingLevel = ouq.ProcessingLevel,
                    ProductionType = ouq.ProductionType
                };
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
                var exp = objList.GetExp<ProductInspectDto>();
                queryEntity = queryEntity.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(queryEntity);

            queryEntity = queryEntity.OrderByDescending(i => i.InspectDate);
            queryEntity = queryEntity.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await AsyncQueryableExecuter.ToListAsync(queryEntity);

            var dtos = new PagedResultDto<ProductInspectDto>(
                totalCount,
                entities
            );
            return dtos;
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInspectProductInspectMgQuery)]
        public async Task<PagedResultDto<ProductInspectReportDto>> GetAllReport(PagedRequestDto input)
        {
            var query = ReportRepository.GetAll();
            var queryProductionOrder = ProductionOrderRepository.GetAll();
            var querySemiPro = SemiProductRepository.GetAll();
            var queryEntity = from a in query
                join s in querySemiPro on a.SemiProductNo equals s.Id into pu
                from luq in pu.DefaultIfEmpty()
                join pp in queryProductionOrder on a.ProductionOrderNo equals pp.ProductionOrderNo  into ppp
                from p in ppp.DefaultIfEmpty()
                select new ProductInspectReportDto
                {
                    ProductInspectReportNo = a.ProductInspectReportNo,
                    ProductionOrderNo = a.ProductionOrderNo,
                    ConfirmStatus = a.ConfirmStatus,
                    ConfirmDate = a.ConfirmDate,
                    ConfirmUser = a.ConfirmUser,
                    InspectCount = a.InspectCount,
                    SemiProductNo = a.SemiProductNo,
                    SemiProductName = luq.SemiProductName??"",
                    PartNo = luq.PartNo ?? "",
                    Model = luq.Model ?? "",
                    Material = luq.Material ?? "",
                    SurfaceColor = luq.SurfaceColor ?? "",
                    Rigidity = luq.Rigidity ?? "",
                    ProductionType = p.ProductionType,
                    ProcessingType = p.ProcessingType,
                    ProcessingLevel = p.ProcessingLevel,
                   
                };
           

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
                var exp = objList.GetExp<ProductInspectReportDto>();
                if (exp != null)
                {
                    queryEntity = queryEntity.Where(exp);
                }
            }

            int total = await queryEntity.CountAsync();
            queryEntity = queryEntity.OrderBy(a => a.ConfirmStatus).ThenBy(a => a.ProductInspectReportNo);
            queryEntity = queryEntity.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await queryEntity.ToListAsync();
            return new PagedResultDto<ProductInspectReportDto>(total,entities.Select(ObjectMapper.Map<ProductInspectReportDto>).ToList());
        }

       //[AbpAuthorize(PermissionNames.PagesProductInspectInspectReportUpdate), AuditLog("修改报告")]
       // public  async Task<ProductInspectDto> UpdateInspect(ProductInspectUpdateDto input)
       // {
       //     if (input.ReportContent.IsNullOrEmpty())
       //     {
       //         CheckErrors(IwbIdentityResult.Failed("报告内容不能为空！"));
       //         return null;
       //     }
       //     var dto= await UpdateEntity(input);
       //     var entity = await ReportContentRepository.FirstOrDefaultAsync(a => a.PtoductInspectNo == input.ProductInspectNo);
       //     if (entity==null)
       //     {
       //         entity = new ProductInspectReportContent()
       //         {
       //             PtoductInspectNo = dto.ProductInspectNo,
       //             ReportContent = input.ReportContent
       //         };
       //         await ReportContentRepository.InsertAsync(entity);
       //     }
       //     else
       //     {
       //         entity.ReportContent = input.ReportContent;
       //         await ReportContentRepository.UpdateAsync(entity);
       //     }

       //     string result = input.InspectResult == 0 ? "不合格" : "合格";
       //     BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "修改检验报告",
       //         $"修改结果为[{result}],检验报告内容：[{entity.ReportContent}]", dto.ProductionOrderNo,
       //         entity.PtoductInspectNo);
       //     if (input.AttachFiles != null && input.AttachFiles.Any())
       //     {
       //         foreach (var attach in input.AttachFiles)
       //         {
       //             attach.AttachNo = Guid.NewGuid().ToString("N");
       //             attach.TableName = "Product";
       //             attach.ColumnName = "Inspect";
       //             attach.SourceKey = dto.ProductInspectNo;
       //             await CreateAttach(attach);
       //         }
       //     }
       //     return dto;
       // }
      


        [AbpAuthorize(PermissionNames.PagesProductInspectInspectReportQueryReport),DisableAuditing]
        public  async Task<string> QueryReport(string no,int isProduct)
        {
            string template = "";
            if (no.IsNullOrEmpty())
            {
                CheckErrors(IwbIdentityResult.Failed("检验遍码不能为空！"));
                return template;
            }
            if (no.ToLower() == "new")
            {
                template = (string) await CacheManager.GetCache(ShwasherConsts.TemplateCache).GetOrDefaultAsync(ShwasherConsts.InspectReportTemplateName);
                if (template.IsNullOrEmpty())
                {
                    template = (await TemplateRepository.FirstOrDefaultAsync(a =>
                        a.TemplateNo == ShwasherConsts.InspectReportTemplateName))?.Content;
                }
                return template;
            }
            if (isProduct==0)
            {
                var report = await ReportContentRepository.FirstOrDefaultAsync(a => a.PtoductInspectNo == no);
                if (report == null)
                {
                    CheckErrors(IwbIdentityResult.Failed("检验报告不存在！"));
                    return "";
                }
                template = report.ReportContent;
            }
            else
            {
                var productInspect = await ReportRepository.FirstOrDefaultAsync(a => a.ProductionOrderNo == no);
                if (productInspect != null)
                    template = (await ReportContentRepository.FirstOrDefaultAsync(a =>
                        a.PtoductInspectNo == productInspect.ProductInspectReportNo))?.ReportContent;

                if (template.IsNullOrEmpty())
                {
                    string proNo = no.Substring(0, 7);//先找他前7位排查单的报告
                    //LogHelper.LogError(this,"前7位编码"+proNo);
                    var productInspectPre = (await ReportRepository.GetAllListAsync(a => a.ProductionOrderNo == proNo)).OrderByDescending(i=>i.CreationTime).FirstOrDefault();
                   //LogHelper.LogError(this, "productInspectPre" + productInspectPre?.ToJsonString());
                    if (productInspectPre != null)
                        template = (await ReportContentRepository.FirstOrDefaultAsync(a =>
                            a.PtoductInspectNo == productInspectPre.ProductInspectReportNo))?.ReportContent;
                   //LogHelper.LogError(this, "template" + template);
                    if (template.IsNullOrEmpty())
                    {
                        template = (string)await CacheManager.GetCache(ShwasherConsts.TemplateCache).GetOrDefaultAsync(ShwasherConsts.InspectReportTemplateName);
                        if (template.IsNullOrEmpty())
                        {
                            template = (await TemplateRepository.FirstOrDefaultAsync(a =>
                                a.TemplateNo == ShwasherConsts.InspectReportTemplateName))?.Content;
                        }
                    }
                  
                }
            }
            return template;
        }
        /// <summary>
        /// 查询附件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<SysAttachFileDto>> QueryAttach(QueryAttachDto input)
        {
            var report = await ReportRepository.FirstOrDefaultAsync(a => a.ProductionOrderNo == input.Key);
            if (report==null)
            {
                return null;
            }
            var entities = await AttachRepository.GetAllListAsync(a =>
                a.TableName == input.TableName && a.ColumnName == input.ColName && a.SourceKey.StartsWith(report.ProductInspectReportNo));
            return entities.Select(ObjectMapper.Map<SysAttachFileDto>).ToList();
        }

        public override Task Delete(EntityDto<int> input)
        {
            return Task.CompletedTask;
        }

        public async Task DeleteAttach(string attachNo)
        {
            var entity = await AttachRepository.FirstOrDefaultAsync(a => a.AttachNo == attachNo);
            if (entity==null)
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到附件。"));
                return;
            }
            await AttachRepository.DeleteAsync(a => a.AttachNo == attachNo);
            BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "修改检验报告",
                $"删除附件：[{entity.AttachNo}]-{entity.FileTitle}-{entity.FileName}.{entity.FileExt}", entity.SourceKey, entity.AttachNo);
        }
        private  async Task CreateAttach(SysAttachFileCreateDto input)
        {
            if (await IsValidFileType(input.FileExt))
            {
                string filePath =
                    $"{SettingManager.GetSettingValue(SettingNames.DownloadPath)}/{input.TableName}/{input.ColumnName}";
                var lcRetVal = SysAttachFileAppService.Base64ToFile(input.FileInfo,
                    $"{input.FileName}-{DateTime.Now:yyMMddHHmmss}{new Random().Next(1000, 9999)}", input.FileExt,
                    filePath);
                if (lcRetVal.StartsWith("error@"))
                {
                    CheckErrors(
                        IwbIdentityResult.Failed(lcRetVal.Split(new[] {'@'},
                            StringSplitOptions.RemoveEmptyEntries)[1]));
                    return;
                }
                input.FilePath = lcRetVal;
                    var entity = ObjectMapper.Map<SysAttachFile>(input);
                    entity = await AttachRepository.InsertAsync(entity);
                    BusinessLogTypeEnum.Inspect.WriteLog(LogRepository, "修改检验报告",
                        $"添加附件：[{entity.AttachNo}]-{entity.FileTitle}-{entity.FileName}.{entity.FileExt}",
                        entity.SourceKey, entity.AttachNo);
                    return;
            }

            CheckErrors(IwbIdentityResult.Failed("文件类型不合法，请上传合法文件。"));
        }

        private async Task<bool> IsValidFileType(string fileName ,bool isName=false)
        {
            string ext = isName ? GetFileExt(fileName) : fileName;
            string lcExts = await SettingManager.GetSettingValueAsync(SettingNames.UploadFileExt);
            string[] loList = lcExts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var lcExt in loList)
            {
                if (ext.ToLower() == lcExt.ToLower())
                    return true;
            }
            this.LogError("上传的文件非法:" + fileName);
            return false;
        }

        private string GetFileExt(string fileName)
        {
            string fileExt = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal) + 1, fileName.Length - fileName.LastIndexOf(".", StringComparison.Ordinal) - 1);
            return fileExt.ToLower();
        }


        //protected override IQueryable<ProductInspectInfo> ApplySorting(IQueryable<ProductInspectInfo> query, PagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<ProductInspectInfo> ApplyPaging(IQueryable<ProductInspectInfo> query, PagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #region SemiEnterStoreCheck

        [AbpAuthorize(PermissionNames.PagesProductionInfoProductionEnterStoreApplyMg), DisableAuditing]
        public PagedResultDto<ViewSemiEnterStore> GetSemiEnterStoreCheck(PagedRequestDto input)
        {
            var checkState = EnterStoreApplyStatusEnum.Audited.ToInt() + "";
            var query = ViewSemiEnterStoreRepository.GetAll().Where(a => a.ApplyStatus == checkState);
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


        [AbpAuthorize(PermissionNames.PagesProductInspectProductItemInspectMgCheck), AuditLog("入库检验合格")]
        public async Task Check(EntityDto<int> input)
        {
            var entity = await EnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == EnterStoreApplyStatusEnum.EnterStored.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("已入库不能再操作!"));
            }
            if (entity.ApplyStatus != EnterStoreApplyStatusEnum.Audited.ToInt() + "" && entity.ApplyStatus != EnterStoreApplyStatusEnum.UnChecked.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("还未审核,不能检验!"));
            }

            entity.ApplyStatus = EnterStoreApplyStatusEnum.Checked.ToInt() + "";
            await EnterStoreRepository.UpdateAsync(entity);
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品检验",
                $"半成品检验合格，可以入库[{entity.Id}]", $"检验人：{entity.CreatorUserId}",
                entity.ProductionOrderNo);
        }
        [AbpAuthorize(PermissionNames.PagesProductInspectProductItemInspectMgUnCheck), AuditLog("入库检验不合格")]
        public async Task UnCheck(EntityDto<int> input)
        {
            var entity = await EnterStoreRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity.ApplyStatus == EnterStoreApplyStatusEnum.EnterStored.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("已入库不能再操作!"));
            }
            if (entity.ApplyStatus != EnterStoreApplyStatusEnum.Audited.ToInt() + "")
            {
                CheckErrors(IwbIdentityResult.Failed("还未审核,不能检验!"));
            }
            entity.ApplyStatus = EnterStoreApplyStatusEnum.UnChecked.ToInt() + "";
            await EnterStoreRepository.UpdateAsync(entity);
            DateTime date= DateTime.Now;
            await DisProductRepository.InsertAsync(new DisqualifiedProduct()
            {
                DisqualifiedNo = await ProductTypeDefinition.GetDisProductNo(DisProductRepository,ProductTypeDefinition.Semi),
                ProductOrderNo = entity.ProductionOrderNo,
                ProductNo = entity.SemiProductNo,
                ProductName = await QueryAppService.GetSemiProductName(entity.SemiProductNo),
                ProductType = ProductTypeDefinition.Semi,
                QuantityWeight = entity.ActualQuantity,
                KgWeight = entity.KgWeight,
                QuantityPcs = entity.KgWeight != 0 ? Math.Floor(entity.ActualQuantity / entity.KgWeight) : 0,
                CheckUser = AbpSession.UserName,
                CheckDate = date,
                HandleType = DisProductStateDefinition.NoHandle,
                HandleDate = date,
                HandleUser = AbpSession.UserName,
                DisqualifiedType = DisqualifiedType.ProductionCheck
            });
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "半成品检验",
                $"半成品检验不合格，不能入库[{entity.Id}]", $"检验人：{entity.CreatorUserId}",
                entity.ProductionOrderNo);
        }

        #endregion
    }
}
