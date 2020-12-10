using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using IwbZero.Helper;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.CompanyInfo.DeviceInfo.Dto;
namespace ShwasherSys.CompanyInfo.DeviceInfo
{
    [AbpAuthorize, AuditLog("设备维护计划")]
    public class DeviceMgPlanAppService : IwbZeroAsyncCrudAppService<DeviceMgPlan, DeviceMgPlanDto, int, IwbPagedRequestDto, DeviceMgPlanCreateDto, DeviceMgPlanUpdateDto >, IDeviceMgPlanAppService
    {
        public DeviceMgPlanAppService(
			ICacheManager cacheManager,
			IRepository<DeviceMgPlan, int> repository, IRepository<MaintenanceRecord, string> mrRepository, IRepository<Mold> moldRepository, IRepository<FixedAsset> faRepository) : base(repository)
        {
            MrRepository = mrRepository;
            MoldRepository = moldRepository;
            FaRepository = faRepository;
            CacheManager = cacheManager;
            FixedAssetList = null;
            MoldList = null;
        }

        private List<FixedAsset> FixedAssetList { get; set; }
        private List<Mold> MoldList { get; set; }
        protected IRepository<MaintenanceRecord,string> MrRepository { get; }
        protected IRepository<Mold> MoldRepository { get; }
        protected IRepository<FixedAsset> FaRepository { get; }
        protected override bool KeyIsAuto { get; set; } = false;

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

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlan)]
        public override async Task Create(DeviceMgPlanCreateDto input)
        {
            input.No = await MaintainTypeDefinition.GetDeviceMgPlanNo(Repository, input.PlanType);
            input.NextMaintenanceDate = input.MaintenanceDate.AddDays(input.MaintenanceCycle);
            await CreateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlanUpdate)]
        public override async Task Update(DeviceMgPlanUpdateDto input)
        {
            var entity = await GetEntity(input);
            if (entity == null)
            {
                CheckErrors("未查询到记录");
                return;
            }
            entity.NextMaintenanceDate = entity.MaintenanceDate.AddDays(input.MaintenanceCycle);
            MapToEntity(input,entity);
            await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlanDelete)]
        public override Task Delete(EntityDto<int> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlanQuery)]
        public override async Task<PagedResultDto<DeviceMgPlanDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input).OrderByDescending(a=>a.NextMaintenanceDate);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<DeviceMgPlanDto>(totalCount, entities.Select(MapDto).ToList());
            return dtoList;
        }

        private DeviceMgPlanDto MapDto(DeviceMgPlan input)
        {
            var dto = MapToEntityDto(input);
            if (dto.PlanType == MaintainTypeDefinition.Mold)
            {
                MoldList = MoldList ?? MoldRepository.GetAllList();
                dto.Name = MoldList?.FirstOrDefault(a => a.No == dto.DeviceNo)?.Name ?? input.Name;
            }else if (dto.PlanType == MaintainTypeDefinition.Device)
            {
                FixedAssetList = FixedAssetList ?? FaRepository.GetAllList();
                dto.Name = FixedAssetList?.FirstOrDefault(a => a.No == dto.DeviceNo)?.Name ?? input.Name;
            }

            return dto;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlanQuery)]
        public override async Task<DeviceMgPlanDto> GetDto(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlanQuery)]
        public override async Task<DeviceMgPlanDto> GetDtoById(int id)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlanQuery)]
        public override async Task<DeviceMgPlanDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlanQuery)]
        public override async Task<DeviceMgPlan> GetEntity(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlanQuery)]
        public override async Task<DeviceMgPlan> GetEntityById(int id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlanQuery)]
        public override async Task<DeviceMgPlan> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{Device}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<Device> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<Device>();
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
        //        var exp = objList.GetExp<Device>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<Device> ApplySorting(IQueryable<Device> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<Device> ApplyPaging(IQueryable<Device> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceDeviceMgPlanMaintain)]
        public async Task Maintain(MaintainDto input)
        {
            var entity = await GetEntity(input);
            if (entity == null)
            {
                CheckErrors("未查询到记录");
                return;
            }

            await MrRepository.InsertAsync(new MaintenanceRecord()
            {
                Id =await MaintainTypeDefinition.GetMaintainRecordNo(MrRepository,entity.PlanType),
                DeviceMgPlanNo = entity.No,
                DeviceNo = entity.DeviceNo,
                DeviceName = entity.Name,
                MgType = entity.PlanType,
                CompleteState = MaintainStateDefinition.New,
                CompleteDate = null,
                Address = input.Address,
                Description = input.Description,
                PlanDate = input.PlanDate ??
                           entity.NextMaintenanceDate ?? entity.ExpireDate.AddDays(entity.MaintenanceCycle),
            });
        }

        
    }
}
