using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Logging;
using Abp.Runtime.Caching;
using Abp.Timing;
using Abp.UI;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.Helper;
using IwbZero.IdentityFramework;
using IwbZero.Session;
using Quartz.Xml.JobSchedulingData20;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Authorization.Users;
using ShwasherSys.BasicInfo;
using ShwasherSys.CompanyInfo;
using ShwasherSys.EntityFramework;
using ShwasherSys.ProductionOrderInfo.Dto;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.ProductStoreInfo.InventoryCheck
{
    [AbpAuthorize, AuditLog("盘点计划")]
    public class InventoryCheckAppService : IwbZeroAsyncCrudAppService<InventoryCheckInfo, InventoryCheckDto, string, IwbPagedRequestDto, InventoryCheckCreateDto, InventoryCheckUpdateDto >, IInventoryCheckAppService
    {
       

        public InventoryCheckAppService(
			ICacheManager cacheManager,
			IRepository<InventoryCheckInfo, string> repository, IRepository<InventoryCheckRecord, string> inventoryCheckRecordRepository, IRepository<CurrentProductStoreHouse> currentProductStoreHouseRepository, IRepository<CurrentSemiStoreHouse> currentSemiStoreHouseRepository, IRepository<StoreHouse> storeHouseRepository, IRepository<ViewCurrentProductStoreHouse> viewCurrentProductStoreHouseRepository, IRepository<ViewCurrentSemiStoreHouse> viewCurrentSemiStoreHouseRepository, IRepository<ViewInventoryCheckRecordSemi, string> viewInventoryCheckRecordSemiRepository, IRepository<ViewInventoryCheckRecordProduct, string> viewInventoryCheckRecordProductRepository, ISqlExecuter sqlExecuter, IRepository<SysUser, long> sysUseRepository, IRepository<Employee> employeeRepository) : base(repository, "Id")
        {
            InventoryCheckRecordRepository = inventoryCheckRecordRepository;
            CurrentProductStoreHouseRepository = currentProductStoreHouseRepository;
            CurrentSemiStoreHouseRepository = currentSemiStoreHouseRepository;
            CacheManager = cacheManager;
            StoreHouseRepository = storeHouseRepository;
            ViewCurrentProductStoreHouseRepository = viewCurrentProductStoreHouseRepository;
            ViewCurrentSemiStoreHouseRepository = viewCurrentSemiStoreHouseRepository;
            ViewInventoryCheckRecordSemiRepository = viewInventoryCheckRecordSemiRepository;
            ViewInventoryCheckRecordProductRepository = viewInventoryCheckRecordProductRepository;
            SqlExecuter = sqlExecuter;
            SysUseRepository = sysUseRepository;
            EmployeeRepository = employeeRepository;
            EmployeeList = null;
        }

        protected override bool KeyIsAuto { get; set; } = false;

        private List<Employee> EmployeeList { get; set; }
        protected IRepository<InventoryCheckRecord,string> InventoryCheckRecordRepository { get; set; }
        protected IRepository<Employee> EmployeeRepository { get; set; }

        protected IRepository<CurrentProductStoreHouse> CurrentProductStoreHouseRepository { get; set; }
        protected IRepository<CurrentSemiStoreHouse> CurrentSemiStoreHouseRepository { get; set; }

        protected IRepository<ViewCurrentProductStoreHouse> ViewCurrentProductStoreHouseRepository { get; set; }
        protected IRepository<ViewCurrentSemiStoreHouse> ViewCurrentSemiStoreHouseRepository { get; set; }
        protected IRepository<ViewInventoryCheckRecordSemi,string> ViewInventoryCheckRecordSemiRepository { get; set; }
        protected IRepository<ViewInventoryCheckRecordProduct,string> ViewInventoryCheckRecordProductRepository { get; set; }
        protected IRepository<StoreHouse> StoreHouseRepository { get; set; }

        protected ISqlExecuter SqlExecuter { get; }

        protected IRepository<SysUser,long> SysUseRepository { get; set; } 
       

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

        
        public List<LambdaObject> GetFilterLambda(InventoryCheckCreateDto input,int t)
        {
            List<LambdaObject> objList = new List<LambdaObject>()
            {
                new LambdaObject()
                {
                    FieldType = LambdaFieldType.S,
                    FieldName = "StoreAreaCode",
                    FieldValue = input.StoreAreaCode,
                    ExpType = LambdaExpType.Equal
                },
                new LambdaObject()
                {
                    FieldType = LambdaFieldType.S,
                    FieldName = "ShelfNumber",
                    FieldValue = input.ShelfNumber,
                    ExpType = LambdaExpType.Equal
                },
                new LambdaObject()
                {
                    FieldType = LambdaFieldType.I,
                    FieldName = "StoreHouseId",
                    FieldValue = input.StoreHouseId,
                    ExpType = LambdaExpType.Equal
                }
            };
            if (!string.IsNullOrEmpty(input.ShelfLevel))
            {
                var sl = input.ShelfLevel.Split(',');
                objList.Add(new LambdaObject()
                {
                    FieldType = LambdaFieldType.S,
                    FieldName = "ShelfLevel",
                    FieldValue = sl,
                    ExpType = LambdaExpType.Contains
                });
            }
            //if (!string.IsNullOrEmpty(input.SequenceNo))
            //{
            //    objList.Add(new LambdaObject()
            //    {
            //        FieldType = LambdaFieldType.S,
            //        FieldName = "SequenceNo",
            //        FieldValue = input.SequenceNo,
            //        ExpType = LambdaExpType.Equal
            //    });
            //}
            string pn = t == 1 ? "ProductNo" : "SemiProductNo";
            string qn = t == 1 ? "Quantity" : "ActualQuantity";
            if (!string.IsNullOrEmpty(input.ProductNo))
            {
                objList.Add(new LambdaObject()
                {
                    FieldType = LambdaFieldType.S,
                    FieldName = pn,
                    FieldValue = input.ProductNo,
                    ExpType = LambdaExpType.Equal
                });
            }
            objList.Add(new LambdaObject()
            {
                FieldType = LambdaFieldType.Decimal,
                FieldName = qn,
                FieldValue ="0",
                ExpType = LambdaExpType.Greater
            });
            return objList;
        }
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateCreate)]
        public override async Task Create(InventoryCheckCreateDto input)
        {
            input.Id = Guid.NewGuid().ToString("N");
            input.CheckState = InventoryCheckState.New;
            var isExist =await Repository.FirstOrDefaultAsync(i => i.CheckNo == input.CheckNo);
            if (isExist != null)
            {
                CheckErrors("盘点编号重复存在！");
            }
            var houseType = StoreHouseRepository.Get(input.StoreHouseId).StoreHouseTypeId;
            var cpshs = new List<string>();
            List<LambdaObject> lambdaObjects = GetFilterLambda(input, houseType ?? 0);
            string updateStoreSql =
                "Update CurrentProductStoreHouse set InventoryCheckState=2 where CurrentProductStoreHouseNo in (select CurrentStoreHouseNo from InventoryCheckRecord where CheckNo = '" + input.CheckNo + "');";
            if (houseType == StoreHouseType.Finish)
            {
                var query = ViewCurrentProductStoreHouseRepository.GetAll();
                var exp = lambdaObjects.GetExp<ViewCurrentProductStoreHouse>();
                query = exp != null ? query.Where(exp) : query;
                var hasExistCheckRecord = query.FirstOrDefault(i => i.InventoryCheckState == 2);
                if (hasExistCheckRecord!=null)
                {

                    CheckErrors(new IwbIdentityResult($"创建的盘点库存记录已处于盘点中！{hasExistCheckRecord.CurrentProductStoreHouseNo}"),LogSeverity.Info);
                    
                }
                cpshs = query.Select(i => i.CurrentProductStoreHouseNo).ToList();

            }
            if (houseType == StoreHouseType.SemiFinish)
            {
                var query = ViewCurrentSemiStoreHouseRepository.GetAll();
                var exp = lambdaObjects.GetExp<ViewCurrentSemiStoreHouse>();
                query = exp != null ? query.Where(exp) : query;
                var hasExistCheckRecord = query.FirstOrDefault(i => i.InventoryCheckState == 2);
                if (hasExistCheckRecord != null)
                {
                    CheckErrors(new IwbIdentityResult($"创建的盘点库存记录已处于盘点中！{hasExistCheckRecord.CurrentSemiStoreHouseNo}"), LogSeverity.Info);
                }
                cpshs = query.Select(i => i.CurrentSemiStoreHouseNo).ToList();
                updateStoreSql = "Update CurrentSemiStoreHouse set InventoryCheckState=2 where CurrentSemiStoreHouseNo in (select CurrentStoreHouseNo from InventoryCheckRecord where CheckNo = '" + input.CheckNo + "');";
            }
            StringBuilder recordSql = new StringBuilder();
            foreach (var cpsh in cpshs)
            {
                InventoryCheckRecord r = new InventoryCheckRecord();
                r.Id = Guid.NewGuid().ToString("N");
                r.CheckNo = input.CheckNo;
                r.CurrentStoreHouseNo = cpsh;
                r.CreatorUserId = AbpSession.UserId;
                r.CreationTime = Clock.Now;
                recordSql.Append(r.InsertSql());
                //await InventoryCheckRecordRepository.InsertAsync(r);
            }

           
            if (!recordSql.ToString().IsNullOrEmpty())
            {
                await SqlExecuter.ExecuteAsync(recordSql.ToString()+"\r\n"+ updateStoreSql);
                
            }
            await CreateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateUpdate)]
        public override async Task Update(InventoryCheckUpdateDto input)
        {
            var entity = Repository.Get(input.Id);
            entity.PlanEndDate = input.PlanEndDate;
            entity.PlanStartDate = input.PlanStartDate;
            await Repository.UpdateAsync(entity);
        }

        //[AbpAuthorize(PermissionNames.PagesMgInventoryCheckMgDelete)]
        public override Task Delete(EntityDto<string> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateQuery)]
        public override async Task<PagedResultDto<InventoryCheckDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<InventoryCheckDto>(totalCount, entities.Select(MapInventoryCheckDto).ToList());
            return dtoList;
        }

        private InventoryCheckDto MapInventoryCheckDto(InventoryCheckInfo input)
        {
            if (EmployeeList == null)
            {
                EmployeeList = EmployeeRepository.GetAllList();
            }
            var dto = MapToEntityDto(input);
            dto.CheckUserName = EmployeeList.FirstOrDefault(a => a.No == input.CheckUser)?.Name ?? "";
            return dto;
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckQuery)]
        public async Task<PagedResultDto<InventoryCheckDto>> GetAllToEmployee(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var user =await SysUseRepository.FirstOrDefaultAsync(AbpSession.UserId ?? 0);
            string employeeNo = AbpSession.GetClaimValue(IwbClaimTypes.EmployeeNo);
            if (employeeNo.IsNullOrEmpty())
            {
                CheckErrors("该账号未绑定员工账号！");
            }

            query = query.Where(i => i.CheckUser == employeeNo);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<InventoryCheckDto>(totalCount, entities.Select(MapInventoryCheckDto).ToList());
            return dtoList;
        }

        #region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateQuery)]
        public override async Task<InventoryCheckDto> GetDto(EntityDto<string> input)
        {
            var entity = await GetEntity(input);
            return MapInventoryCheckDto(entity);
        }

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateQuery)]
        public override async Task<InventoryCheckDto> GetDtoById(string id)
        {
            var entity = await GetEntityById(id);
            return MapInventoryCheckDto(entity);
        }

        /// <summary>
        /// 查询实体Dto（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateQuery)]
        public override async Task<InventoryCheckDto> GetDtoByNo(string no)
        {
            var entity = await GetEntityByNo(no);
            return MapInventoryCheckDto(entity);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
       [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateQuery)]
        public override async Task<InventoryCheckInfo> GetEntity(EntityDto<string> input)
        {
            var entity = await GetEntityById(input.Id);
            return entity;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DisableAuditing]
       // [AbpAuthorize(PermissionNames.PagesMgInventoryCheckMgQuery)]
        public override async Task<InventoryCheckInfo> GetEntityById(string id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
      //  [AbpAuthorize(PermissionNames.PagesMgInventoryCheckMgQuery)]
        public override async Task<InventoryCheckInfo> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{InventoryCheck}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<InventoryCheck> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<InventoryCheck>();
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
        //        var exp = objList.GetExp<InventoryCheck>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<InventoryCheck> ApplySorting(IQueryable<InventoryCheck> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<InventoryCheck> ApplyPaging(IQueryable<InventoryCheck> query, IwbPagedRequestDto input)
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
        /// 变更状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateChangeState, PermissionNames.PagesInventoryCheckInfoInventoryCheckChangeState)]
        public async Task ChangeState(CheckStateDto input)
        {
            var entity = Repository.Get(input.Id);
            entity.CheckState = input.CheckState;
            if (entity.CheckState == InventoryCheckState.Finish||entity.CheckState==InventoryCheckState.Closed)
            {
                string updateStoreSql =
                    "Update CurrentProductStoreHouse set InventoryCheckState=1 where CurrentProductStoreHouseNo in (select CurrentStoreHouseNo from InventoryCheckRecord where CheckNo = '" + entity.CheckNo + "');";
                if (entity.StoreHouseId!=1)
                {
                    updateStoreSql = "Update CurrentSemiStoreHouse set InventoryCheckState=1 where CurrentSemiStoreHouseNo in (select CurrentStoreHouseNo from InventoryCheckRecord where CheckNo = '" + entity.CheckNo + "');";
                }

                entity.FinishDate = Clock.Now;
                await SqlExecuter.ExecuteAsync(updateStoreSql);
            }
            await Repository.UpdateAsync(entity);
        }

        /// <summary>
        /// 根据盘点条件查询库存记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateQuery)]
        public async Task<PagedResultDto<CurrentStoreItemDto>> QueryCheckStoreItems(IwbPagedRequestDto input)
        {
            var pagedInput = input as IIwbPagedRequest;
            int houseType=0;
            List<LambdaObject> objList = new List<LambdaObject>();
            string shelfLevel = "";
            if (pagedInput.SearchList != null && pagedInput.SearchList.Count > 0)
            {
                foreach (var o in pagedInput.SearchList)
                {
                    if (string.IsNullOrEmpty(o.KeyWords))
                        continue;
                    object keyWords = o.KeyWords;
                    if (o.KeyField == "shelfLevel")
                    {
                        keyWords = o.KeyWords.Split(',');
                        //continue;
                    }
                    if (o.KeyField == "storeHouseId")
                    {
                        int storeId = Convert.ToInt16(keyWords);
                        houseType = StoreHouseRepository.Get(storeId).StoreHouseTypeId ?? 0;
                    }
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
            }
            int totalCount = 0;
            List<CurrentStoreItemDto> resutList = new List<CurrentStoreItemDto>();
            if (houseType == StoreHouseType.Finish)
            {
                var query = ViewCurrentProductStoreHouseRepository.GetAll();
                var exp = objList.GetExp<ViewCurrentProductStoreHouse>();
                query = exp != null ? query.Where(exp) : query;
                query = query.Where(i => i.Quantity > 0);
                
                totalCount = await AsyncQueryableExecuter.CountAsync(query);
                query = query.OrderByDescending(i=>i.TimeCreated);
                query = _ApplyPaging(query, input);
                var entities = await AsyncQueryableExecuter.ToListAsync(query);
                resutList = ObjectMapper.Map<List<CurrentStoreItemDto>>(entities);
            }
            if (houseType == StoreHouseType.SemiFinish)
            {
                var query = ViewCurrentSemiStoreHouseRepository.GetAll();
                var pQuery = objList.Find(i => i.FieldName.ToUpper() == "PRODUCTNO");
                if (pQuery!=null)
                {
                    objList.Remove(pQuery);
                    objList.Add(new LambdaObject
                    {
                        FieldType = LambdaFieldType.S,
                        FieldName = "SemiProductNo",
                        FieldValue = pQuery.FieldValue,
                        ExpType = LambdaExpType.Equal
                    });
                }
                var exp = objList.GetExp<ViewCurrentSemiStoreHouse>();
                query = exp != null ? query.Where(exp) : query;
                query = query.Where(i => i.ActualQuantity > 0 );
                totalCount = await AsyncQueryableExecuter.CountAsync(query);
                query = query.OrderByDescending(i => i.TimeCreated);
                query = _ApplyPaging(query, input);
                var entities = await AsyncQueryableExecuter.ToListAsync(query);
                resutList = ObjectMapper.Map<List<CurrentStoreItemDto>>(entities);
            }
            var dtoList = new PagedResultDto<CurrentStoreItemDto>(totalCount, resutList);
            return dtoList;
        }


        

        #region extend
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateQuery, PermissionNames.PagesInventoryCheckInfoInventoryCheckQuery)]
        public async Task<PagedResultDto<ViewInventoryCheckRecordProduct>> GetCheckRecordProduct(IwbPagedRequestDto input)
        {
            var pagedInput = input as IIwbPagedRequest;
            var query = ViewInventoryCheckRecordProductRepository.GetAll();
            if (pagedInput.SearchList != null && pagedInput.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in pagedInput.SearchList)
                {
                    if (string.IsNullOrEmpty(o.KeyWords))
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
                var exp = objList.GetExp<ViewInventoryCheckRecordProduct>();
                query = exp != null ? query.Where(exp) : query;
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = _ApplySorting(query, input);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ViewInventoryCheckRecordProduct>(totalCount, entities);
            return dtoList;
        }
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateQuery, PermissionNames.PagesInventoryCheckInfoInventoryCheckQuery)]
        public async Task<PagedResultDto<ViewInventoryCheckRecordSemi>> GetCheckRecordSemi(IwbPagedRequestDto input)
        {
            var pagedInput = input as IIwbPagedRequest;
            var query = ViewInventoryCheckRecordSemiRepository.GetAll();
            if (pagedInput.SearchList != null && pagedInput.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in pagedInput.SearchList)
                {
                    if (string.IsNullOrEmpty(o.KeyWords))
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
                var exp = objList.GetExp<ViewInventoryCheckRecordSemi>();
                query = exp != null ? query.Where(exp) : query;
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = _ApplySorting(query, input);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ViewInventoryCheckRecordSemi>(totalCount, entities);
            return dtoList;
        }
        #endregion

        #region Record
        /// <summary>
        /// 查询盘点单记录-成品
        /// </summary>
        /// <param name="checkNo"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateQuery, PermissionNames.PagesInventoryCheckInfoInventoryCheckQuery)]
        public async Task<List<ViewInventoryCheckRecordProduct>> GetCheckRecordProductNotPage(string checkNo)
        {
            var query = ViewInventoryCheckRecordProductRepository.GetAll().Where(i => i.CheckNo == checkNo);
            query = query.OrderByDescending(i => i.CreationTime);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return entities;
        }
        /// <summary>
        /// 查询盘点单记录-半成品
        /// </summary>
        /// <param name="checkNo"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateQuery, PermissionNames.PagesInventoryCheckInfoInventoryCheckQuery)]
        public async Task<List<ViewInventoryCheckRecordSemi>> GetCheckRecordSemiNotPage(string checkNo)
        {
            var query = ViewInventoryCheckRecordSemiRepository.GetAll().Where(i => i.CheckNo == checkNo);
            query = query.OrderByDescending(i => i.CreationTime);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return entities;
        }

        [AbpAuthorize(PermissionNames.PagesInventoryCheckInfoInventoryCheckCheckData, PermissionNames.PagesInventoryCheckInfoInventoryCheckCreateCheckData)]
        public async Task CheckData(CheckDataDto input)
        {
            var entity = InventoryCheckRecordRepository.Get(input.Id);
            var checkInfo = await Repository.FirstOrDefaultAsync(i => i.CheckNo == entity.CheckNo);
            if (checkInfo.CheckState != InventoryCheckState.Checking)
            {
                CheckErrors("当前盘点任务未开始或已结束！！");
            }
            entity.CheckQuantity = input.CheckQuantity;
            await InventoryCheckRecordRepository.UpdateAsync(entity);
        }

        #endregion


        public async Task<List<InventoryReportItem>> QueryStaticsInventoryItems(QueryInventoryReportDto input)
        {
            var startDate = input.Year.GetDateByType(input.Month, out var endDate, out var dateStr);

            //查询外购的排产单进行统计(不是新建状态的)
            var query = Repository.GetAll().Where(a => a.PlanStartDate >= startDate && a.PlanStartDate < endDate );
            if (input.CheckState.HasValue)
            {
                query = query.Where(i => i.CheckState == input.CheckState);
            }
            if (input.EmployeeId.HasValue)
            {
                query = query.Where(i => i.CheckUser == input.EmployeeId.ToString());
            }

            if (input.HouseType.HasValue)
            {
                var storeHouseIds = await StoreHouseRepository.GetAll()
                    .Where(i => i.StoreHouseTypeId == input.HouseType).Select(i => i.Id).ToListAsync();
                query = query.Where(i => storeHouseIds.Contains(i.StoreHouseId));
            }
            //联合查询半成品信息
            IQueryable<InventoryReportItem> itemQuery = from a in query
                join sp in StoreHouseRepository.GetAll() on a.StoreHouseId equals sp.Id join em in EmployeeRepository.GetAll() on a.CheckUser equals em.No 
                select new InventoryReportItem()
                {
        StoreHouseId = a.StoreHouseId,
        StoreHouseName = sp.StoreHouseName,
        StoreAreaCode = a.StoreAreaCode,
         ShelfNumber = a.ShelfNumber,
       ShelfLevel= a.ShelfLevel,
        SequenceNo  = a.SequenceNo,
       CheckNo = a.CheckNo,
         PlanStartDate  = a.PlanStartDate,
         PlanEndDate = a.PlanEndDate,
      
         CheckUser = a.CheckUser, CheckUserName = em.Name,
         PublishUser = a.PublishUser,
         FinishDate = a.FinishDate,

         CheckState = a.CheckState
    };
            var items = await itemQuery.OrderByDescending(i=>i.PlanStartDate).ToListAsync();
            
            return items;
        }

    }
}
