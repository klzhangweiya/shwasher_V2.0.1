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
using Abp.Runtime.Caching;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using IwbZero.Helper;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.CompanyInfo.MaintenanceRecordInfo.Dto;
namespace ShwasherSys.CompanyInfo.MaintenanceRecordInfo
{
    [AbpAuthorize, AuditLog("机模维护记录")]
    public class MaintainRecordAppService : IwbZeroAsyncCrudAppService<MaintenanceRecord, MaintenanceRecordDto, string, IwbPagedRequestDto, MaintenanceRecordCreateDto, MaintenanceRecordUpdateDto >, IMaintainRecordAppService
    {
        public MaintainRecordAppService(
			ICacheManager cacheManager,
			IRepository<MaintenanceRecord, string> repository, IRepository<MaintenanceMember, string> mmRepository, IRepository<EmployeeWorkPerformance> performanceRepository, IRepository<Mold> moldRepository, IRepository<DeviceMgPlan> deviceRepository, IRepository<FixedAsset> faRepository) : base(repository, "Id")
        {
            MmRepository = mmRepository;
            PerformanceRepository = performanceRepository;
            MoldRepository = moldRepository;
            DeviceRepository = deviceRepository;
            FaRepository = faRepository;
            CacheManager = cacheManager;
            FixedAssetList = null;
            MoldList = null;
        }
        
        private List<FixedAsset> FixedAssetList { get; set; }
        private List<Mold> MoldList { get; set; }
        protected IRepository<FixedAsset> FaRepository { get; }
        protected IRepository<Mold>  MoldRepository { get; }
        protected IRepository<DeviceMgPlan>  DeviceRepository { get; }
        protected IRepository<EmployeeWorkPerformance>  PerformanceRepository { get; }
        protected IRepository<MaintenanceMember,string>  MmRepository { get; }
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

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordCreate)]
        public override async Task Create(MaintenanceRecordCreateDto input)
        {
            input.Id = await MaintainTypeDefinition.GetMaintainRecordNo(Repository,MaintainTypeDefinition.Other);
            input.MgType = MaintainTypeDefinition.Other;
            
            input.CompleteState = MaintainStateDefinition.New;
            await CreateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordUpdate)]
        public override async Task Update(MaintenanceRecordUpdateDto input)
        {
            input.CompleteState = MaintainStateDefinition.New;
            await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordDelete)]
        public override Task Delete(EntityDto<string> input)
        {

            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordQuery)]
        public override async Task<PagedResultDto<MaintenanceRecordDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<MaintenanceRecordDto>(totalCount, entities.Select(MapDto).ToList());
            return dtoList;
        }
        
        private MaintenanceRecordDto MapDto(MaintenanceRecord input)
        {
            var dto = MapToEntityDto(input);
            if (dto.MgType == MaintainTypeDefinition.Mold)
            {
                MoldList = MoldList ?? MoldRepository.GetAllList();
                dto.DeviceName = MoldList?.FirstOrDefault(a => a.No == dto.DeviceNo)?.Name ?? input.DeviceName;
            }else if (dto.MgType == MaintainTypeDefinition.Device)
            {
                FixedAssetList = FixedAssetList ?? FaRepository.GetAllList();
                dto.DeviceName = FixedAssetList?.FirstOrDefault(a => a.No == dto.DeviceNo)?.Name ?? input.DeviceName;
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordQuery)]
        public override async Task<MaintenanceRecordDto> GetDto(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordQuery)]
        public override async Task<MaintenanceRecordDto> GetDtoById(string id)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordQuery)]
        public override async Task<MaintenanceRecordDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordQuery)]
        public override async Task<MaintenanceRecord> GetEntity(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordQuery)]
        public override async Task<MaintenanceRecord> GetEntityById(string id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordQuery)]
        public override async Task<MaintenanceRecord> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{MaintenanceRecord}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<MaintenanceRecord> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<MaintenanceRecord>();
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
        //        var exp = objList.GetExp<MaintenanceRecord>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<MaintenanceRecord> ApplySorting(IQueryable<MaintenanceRecord> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<MaintenanceRecord> ApplyPaging(IQueryable<MaintenanceRecord> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion


        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordComplete)]
        public async Task Complete(CompleteDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors("未查询到记录！");
                return;
            }

            var ms = await MmRepository.GetAllIncluding(a=>a.EmployeeInfo).Where(a => a.MaintenanceNo == entity.Id).ToListAsync();
            if (ms == null || !ms.Any())
            {
                CheckErrors("未查询维护成员记录,请检查后再操作！");
                return;
            }
            entity.CompleteState = MaintainStateDefinition.Complete;
            entity.CompleteDate = input.CompleteDate ?? DateTime.Now;
            await Repository.UpdateAsync(entity);
            if (entity.MgType == MaintainTypeDefinition.Mold)
            {
                var mold = await MoldRepository.FirstOrDefaultAsync(a => a.No == entity.DeviceMgPlanNo);
                if (mold != null)
                {
                   // mold.MaintenanceDate = entity.CompleteDate ?? DateTime.Now;
                   // mold.NextMaintenanceDate = mold.MaintenanceDate.AddDays(mold.MaintenanceCycle);
                    await MoldRepository.UpdateAsync(mold);
                }
            }else if (entity.MgType == MaintainTypeDefinition.Device)
            {
                var device = await DeviceRepository.FirstOrDefaultAsync(a => a.No == entity.DeviceMgPlanNo);
                if (device != null)
                {
                    device.MaintenanceDate = entity.CompleteDate ?? DateTime.Now;
                    device.NextMaintenanceDate = device.MaintenanceDate.AddDays(device.MaintenanceCycle);
                    await DeviceRepository.UpdateAsync(device);
                }
            }

            foreach (var m in ms)
            {
                await ConfirmMember(m, entity.MgType);
            }
           
        }


        #region Member

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainMember)]
        public  async Task<PagedResultDto<MemberDto>> GetAllMember(IwbPagedRequestDto input)
        {
            var q = MmRepository.GetAllIncluding(a=>a.EmployeeInfo);
            q = ApplyFilter(q, input);
            var query = q.Select(a => new MemberDto()
            {
                Id = a.Id,
                MaintenanceNo = a.MaintenanceNo,
                EmployeeId = a.EmployeeId,
                EmployeeNo = a.EmployeeInfo.No,
                Name = a.EmployeeInfo.Name,
                StartDateTime = a.StartDateTime,
                EndDateTime = a.EndDateTime,
                WorkHour = a.WorkHour,
                WorkDesc = a.WorkDesc
            });
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<MemberDto>(totalCount, entities);
            return dtoList;
        }

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainRecordAddMember)]
        public async Task AddMember(MemberDto input)
        {
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors("未查询到维护记录");
                return;
            }
            if (entity.CompleteState != MaintainStateDefinition.New&&entity.CompleteState != MaintainStateDefinition.Start )
            {
                CheckErrors("维护信息完成，不能操作。");
                return;
            }
            if (entity.CompleteState == MaintainStateDefinition.New)
            {
                entity.CompleteState = MaintainStateDefinition.Start;
                await Repository.UpdateAsync(entity);
            }
            string id = await GetMemberNo(input.Id);
            if (input.EndDateTime == null && input.StartDateTime <= input.EndDateTime)
            {
                CheckErrors("结束时间不能早于开始时间");
            }
            await MmRepository.InsertAsync(new MaintenanceMember()
            {
                Id = id,
                MaintenanceNo = input.Id,
                IsConfirm = false,
                EmployeeId = input.EmployeeId,
                StartDateTime = input.StartDateTime,
                EndDateTime = input.EndDateTime,
                WorkDesc = input.WorkDesc,
                WorkHour = input.WorkHour ?? 0,
            });
        }

        private async Task<string> GetMemberNo(string mNo)
        {
            var lastEntity = await MmRepository.GetAll().Where(a => a.Id.StartsWith(mNo)).OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            int noLength=3,index = 0;
            if (lastEntity!=null)
            {
                var entityNo = lastEntity.Id;
                index = Convert.ToInt32(entityNo.Substring(entityNo.Length - noLength));
            }
            index++;
            string no = $"{mNo}-{index.LeftPad(noLength)}";
            if ((await MmRepository.CountAsync(a=>a.Id==no)) > 0) 
            {
                no = await GetMemberNo(mNo);
            }
            return no;
        }

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainMemberUpdate)]
        public async Task UpdateMember(MemberDto input)
        {
            var entity = await MmRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors("未查询到记录！");
                return;
            }
            if (entity.IsConfirm)
            {
                CheckErrors("维护信息已确认，不能修改。");
                return;
            }
            if (input.EndDateTime == null && input.StartDateTime <= input.EndDateTime)
            {
                CheckErrors("结束时间不能早于开始时间");
            }
            entity.StartDateTime = input.StartDateTime;
            entity.EndDateTime = input.EndDateTime;
            entity.WorkHour = input.WorkHour ?? 0;
            entity.WorkDesc = input.WorkDesc;
            await MmRepository.UpdateAsync(entity);
        }

        [AbpAuthorize(PermissionNames.PagesCompanyDieMaintenanceMaintainMemberDelete)]
        public async Task DeleteMember(EntityDto<string> input)
        {
            var entity = await MmRepository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity == null)
            {
                CheckErrors("未查询到记录！");
                return;
            }
            if (entity.IsConfirm)
            {
                CheckErrors("维护信息已确认，不能修改。");
                return;
            }
            
            await MmRepository.DeleteAsync(entity);
        }

        private async Task ConfirmMember(MaintenanceMember entity,int type)
        {
            if ( entity.WorkHour <= 0)
            {
                CheckErrors($"{entity.EmployeeInfo.Name}(工号:{entity.EmployeeInfo.No})的工时不能小于等于0");

            }
            if (entity.IsConfirm)
            {
                return;
            }
            entity.IsConfirm = true;
            await MmRepository.UpdateAsync(entity);
            var pType = type == MaintainTypeDefinition.Mold ? WorkTypeDefinition.MoldMg : WorkTypeDefinition.DeviceMg;
            var p = new EmployeeWorkPerformance()
            {
                PerformanceNo =await WorkTypeDefinition.GetPerformanceNo(PerformanceRepository,pType),
                ProductOrderNo = "",
                RelatedNo = entity.Id,
                EmployeeId = entity.EmployeeId,
                WorkType =  pType,
                Performance = entity.WorkHour,
                PerformanceUnit = "小时",
                PerformanceDesc =
                    $"维护设备{entity.WorkHour}小时",
            };
            await PerformanceRepository.InsertAsync(p);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

       
        #endregion

    }
}
