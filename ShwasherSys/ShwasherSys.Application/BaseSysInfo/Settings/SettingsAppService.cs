using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using ShwasherSys.BaseSysInfo.Settings.Dto;
using ShwasherSys.Lambda;
using Castle.Core.Internal;
using ShwasherSys.Authorization.Permissions;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.IdentityFramework;
using IwbZero.Setting;

namespace ShwasherSys.BaseSysInfo.Settings
{
    [AbpAuthorize, AuditLog("系统配置", "配置")]
    public class SettingsAppService : ShwasherAsyncCrudAppService<SysSetting, SettingDto, int, PagedRequestDto, SettingCreateDto, SettingUpdateDto>, ISettingsAppService
    {
        protected ISettingManager AbpSettingManager;
        public const string ApplicationSettingsCacheKey = "ApplicationSettings";
    
        public SettingsAppService(ICacheManager cacheManager, IIwbSettingManager settingManager, IRepository<SysSetting, int> repository, ISettingManager abpSettingManager) : base(repository, "SettingNo")
        {
            SettingManager = settingManager;
            CacheManager = cacheManager;
           
            AbpSettingManager = abpSettingManager;
        }

        protected override string GetPermissionName { get; set; } = PermissionNames.PagesSystemSysSetting;
        protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesSystemSysSetting;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesSystemSysSettingCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesSystemSysSettingUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesSystemSysSettingDelete;

        [DisableAuditing]
        public override async Task<PagedResultDto<SettingDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input).Where(a => a.SettingType != 0);
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
                var exp = objList.GetExp<SysSetting>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<SettingDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
            return dtos;
        }
        public override async Task<SettingDto> Create(SettingCreateDto input)
        {
            CheckCreatePermission();
            input.SettingType = input.SettingType == 0 ? 1 : input.SettingType;
            if ((await Repository.FirstOrDefaultAsync(a => a.Code == input.Code)) != null)
                CheckErrors(IwbIdentityResult.Failed("系统配置已存在，请检查后再试！"));
            var entity = MapToEntity(input);
            entity.SettingNo = Guid.NewGuid().ToString("N");
            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            await Refresh();
            return MapToEntityDto(entity);
        }
        public override async Task<SettingDto> Update(SettingUpdateDto input)
        {
            CheckUpdatePermission();
            //input.SettingType = input.SettingType == 0 ? 1 : input.SettingType;
            var entity = await Repository.FirstOrDefaultAsync(a => a.Code == input.Code);
            if (entity == null)
                CheckErrors(IwbIdentityResult.Failed("系统配置不存在，请检查后再试！"));
            else
            {
                input.SettingNo = entity.SettingNo;
                input.Code = entity.Code;
                input.SettingType = entity.SettingType;
            }
            MapToEntity(input, entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            await SettingManager.ChangeSettingForApplicationAsync(input.Code, input.Value);
            //await AbpSettingManager.ChangeSettingForApplicationAsync(input.Code, input.Value);
            await Refresh();
            return MapToEntityDto(entity);
        }
        public override async Task Delete(EntityDto<int> input)
        {
            CheckDeletePermission();
            await Repository.DeleteAsync(input.Id);
            await CurrentUnitOfWork.SaveChangesAsync();
            await Refresh();
        }
        [AbpAuthorize(PermissionNames.PagesSystemSysSettingRefresh)]
        public async Task Refresh()
        {
            foreach (var cach in CacheManager.GetAllCaches())
            {
                await cach.ClearAsync();
            }
           
            await SettingManager.RefreshAsync();
            await Refresh2();
        }

        public async Task<Dictionary<string, SettingInfo>> Refresh2()
        {
            var list = Repository.GetAll();
            return await CacheManager.GetApplicationSettingsCache().GetAsync(ApplicationSettingsCacheKey, factory: async () => {
                var dictionary = new Dictionary<string, SettingInfo>();
                foreach (var sysSetting in list)
                {
                    var settingInfo = new SettingInfo()
                    {
                        TenantId = 1,
                        Name = sysSetting.Code,
                        Value = sysSetting.Value,
                        UserId = 1
                    };
                    dictionary[sysSetting.Code] = settingInfo;
                }
                return dictionary;
            });

        }
    }
}
