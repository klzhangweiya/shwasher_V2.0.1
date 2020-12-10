using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.BasicInfo.StoreHouses.Dto;
using ShwasherSys.Lambda;

namespace ShwasherSys.BasicInfo.StoreHouses
{
    [AbpAuthorize]
    public class StoreHousesAppService : ShwasherAsyncCrudAppService<StoreHouse, StoreHouseDto, int, PagedRequestDto, StoreHouseCreateDto, StoreHouseUpdateDto >, IStoreHousesAppService
    {
        protected IStatesAppService StatesAppService { get; set; }
        public StoreHousesAppService(IRepository<StoreHouse, int> repository, IStatesAppService statesAppService) : base(repository, "StoreHouseName")
        {
            StatesAppService = statesAppService;
            KeyIsAuto = false;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesBasicInfoStoreHouses;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesBasicInfoStoreHouses;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesBasicInfoStoreHousesCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesBasicInfoStoreHousesUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesBasicInfoStoreHousesDelete;


        [AbpAuthorize(PermissionNames.PagesBasicInfoStoreHouses),DisableAuditing]
        public override async Task<PagedResultDto<StoreHouseDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input).Where(i=>i.IsLock=="N");
            
           
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
                var exp = objList.GetExp<StoreHouse>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var storeHouseDtos = ObjectMapper.Map<List<StoreHouseDto>>(entities);
            foreach (var storeHouseDto in storeHouseDtos)
            {
                storeHouseDto.StoreHouseTypeName =
                    StatesAppService.GetDisplayValue("StoreHouse", "StoreHouseType", storeHouseDto.StoreHouseTypeId + "");
            }

            var dtos = new PagedResultDto<StoreHouseDto>(totalCount,storeHouseDtos);
            return dtos;
        }

    }
}
