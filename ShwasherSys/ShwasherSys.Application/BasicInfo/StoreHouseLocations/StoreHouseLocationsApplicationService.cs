using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using Microsoft.AspNet.Identity;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BasicInfo.Region.Dto;
using ShwasherSys.BasicInfo.StoreHouseLocations.Dto;

namespace ShwasherSys.BasicInfo.StoreHouseLocations
{
    [AbpAuthorize]
    public class StoreHouseLocationsAppService : ShwasherAsyncCrudAppService<StoreHouseLocation, StoreHouseLocationDto, int, PagedRequestDto, StoreHouseLocationCreateDto, StoreHouseLocationUpdateDto >, IStoreHouseLocationsAppService
    {
        public StoreHouseLocationsAppService(IRepository<StoreHouseLocation, int> repository, IRepository<StoreHouse> storeHouseRepository) : base(repository, "StoreLocationNo")
        {
            StoreHouseRepository = storeHouseRepository;
            KeyIsAuto = false;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesBasicInfoStoreHouseLocations;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesBasicInfoStoreHouseLocations;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesBasicInfoStoreHouseLocationsCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesBasicInfoStoreHouseLocationsUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesBasicInfoStoreHouseLocationsDelete;

        protected IRepository<StoreHouse> StoreHouseRepository { get; }
        public override async Task<StoreHouseLocationDto> Create(StoreHouseLocationCreateDto input)
        {
            CheckCreatePermission();
            var storeHouse = StoreHouseRepository.Get(input.StoreHouseId ?? 0);
            string numPrex = storeHouse.StoreHouseTypeId != StoreHouseType.Finish ? input.StoreHouseId + "-"+ input.StoreAreaCode : input.StoreAreaCode;
            string suffix = "";
            if (storeHouse.StoreHouseTypeId != StoreHouseType.Rm)
            {
                if (input.ShelfNumber.IsNullOrEmpty() || input.ShelfLevel.IsNullOrEmpty() ||
                    input.SequenceNo.IsNullOrEmpty())
                {
                    CheckErrors(new IdentityResult("货架,层次或者列号不能为空！"));
                }
                //suffix = "-"+ input.ShelfNumber + "-" + input.ShelfLevel + "-" + input.SequenceNo;
            }
            suffix = (input.ShelfNumber.IsNullOrEmpty() ? "" : "-" + input.ShelfNumber) + (input.ShelfLevel.IsNullOrEmpty() ? "" : "-" + input.ShelfLevel) + (input.SequenceNo.IsNullOrEmpty() ? "" : "-" + input.SequenceNo);
            input.StoreLocationNo = numPrex+ suffix;
            var entity = await Repository.FirstOrDefaultAsync(i => i.StoreLocationNo == input.StoreLocationNo);
            if (entity != null)
            {
                CheckErrors(IwbIdentityResult.Failed("对应的库位信息已存在！"));
            }
            return await CreateEntity1(input);
        }
        public override async Task<StoreHouseLocationDto> Update(StoreHouseLocationUpdateDto input)
        {
            CheckUpdatePermission();
            //var storeHouse = StoreHouseRepository.Get(input.StoreHouseId ?? 0);
            //string numPrex = storeHouse.StoreHouseTypeId != StoreHouseType.Finish ? input.StoreHouseId + "-" + input.StoreAreaCode : input.StoreAreaCode;
            //string suffix = "";
            //if (storeHouse.StoreHouseTypeId != StoreHouseType.Rm)
            //{
            //    if (input.ShelfNumber.IsNullOrEmpty() || input.ShelfLevel.IsNullOrEmpty() ||
            //        input.SequenceNo.IsNullOrEmpty())
            //    {
            //        CheckErrors(new IdentityResult("货架,层次或者列号不能为空！"));
            //    }
               
            //}
            //suffix = (input.ShelfNumber.IsNullOrEmpty()?"":"-" + input.ShelfNumber) + (input.ShelfLevel.IsNullOrEmpty()?"":"-" + input.ShelfLevel) + (input.SequenceNo.IsNullOrEmpty() ? "":"-" + input.SequenceNo);
            //input.StoreLocationNo = numPrex + suffix;
            var entity = await Repository.FirstOrDefaultAsync(i => i.StoreLocationNo == input.StoreLocationNo);
            if (entity==null)
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到对应的库位信息！"));
            }
            MapToEntity(input, entity);
            await Repository.UpdateAsync(entity);
            return MapToEntityDto(entity);
        }
    }
}
