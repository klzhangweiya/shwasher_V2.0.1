using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Timing;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BasicInfo.ExpressInfo.Dto;
using ShwasherSys.BasicInfo.Factory.Dto;

namespace ShwasherSys.BasicInfo.ExpressInfo
{
    [AbpAuthorize]
    public class ExpressAppService : ShwasherAsyncCrudAppService<ExpressLogistics, ExpressLogisticsDto, int, PagedRequestDto, ExpressLogisticsCreateDto, ExpressLogisticsUpdateDto>, IExpressAppService
    {
        protected IRepository<ExpressProviderMapper> ExpressProviderMapperRepository;
        protected IRepository<ExpressServiceProvider> ExpressServiceProviderRepository;
        public ExpressAppService(IRepository<ExpressLogistics, int> repository, IRepository<ExpressProviderMapper> expressProviderMapperRepository, IRepository<ExpressServiceProvider> expressServiceProviderRepository) : base(repository)
        {
            ExpressProviderMapperRepository = expressProviderMapperRepository;
            ExpressServiceProviderRepository = expressServiceProviderRepository;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesBasicInfoExpress;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesBasicInfoExpress;
		protected override string CreatePermissionName { get; set; } = PermissionNames.PagesBasicInfoExpressCreate;
		protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesBasicInfoExpressUpdate;
		protected override string DeletePermissionName { get; set; } = PermissionNames.PagesBasicInfoExpressDelete;

        [DisableAuditing]
        public List<SelectListItem> GetExpressSelects()
        {
            var slist = new List<SelectListItem>();
            var list = Repository.GetAll();
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.ExpressName, Value = l.Id+"" });
            }
            return slist;
        }

        public override async Task<ExpressLogisticsDto> Create(ExpressLogisticsCreateDto input)
        {
            CheckCreatePermission();
            var entity = MapToEntity(input);
            if (Repository.FirstOrDefault(i => i.Code == input.Code)!=null)
            {
                CheckErrors(new IwbIdentityResult("快递公司编码已存在！"));
            }
            entity.IsLock = "N";
            entity.TimeCreated = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserName;
            entity.TimeLastMod = Clock.Now;
            var id = await Repository.InsertAndGetIdAsync(entity);
            entity.Id = id;
            var mappers = input.ExpressProviderMapper;
            foreach (var mapper in mappers)
            {
                ExpressProviderMapper mapperEntity = new ExpressProviderMapper()
                {
                    ProviderId = mapper.ProviderId,
                    ExpressId = id,
                    MapperCode = mapper.MapperCode,
                    ExtendInfo = mapper.ExtendInfo,
                    QueryUrl = mapper.QueryUrl,
                    ActiveStatus = mapper.ActiveStatus
                };
               await ExpressProviderMapperRepository.InsertAsync(mapperEntity);
            }
            return MapToEntityDto(entity);
        }


        public override async Task<ExpressLogisticsDto> Update(ExpressLogisticsUpdateDto input)
        {
            CheckUpdatePermission();
            var entity = Repository.Get(input.Id);
            entity.ExpressName = input.ExpressName;
            entity.Sort = input.Sort;
            entity.Code = input.Code;
            await ExpressProviderMapperRepository.DeleteAsync(i => i.ExpressId == input.Id);
            foreach (var mapper in input.ExpressProviderMapper)
            {
                ExpressProviderMapper mapperEntity = new ExpressProviderMapper()
                {
                    ProviderId = mapper.ProviderId,
                    ExpressId = input.Id,
                    MapperCode = mapper.MapperCode,
                    ExtendInfo = mapper.ExtendInfo,
                    QueryUrl = mapper.QueryUrl,
                    ActiveStatus = mapper.ActiveStatus
                };
                await ExpressProviderMapperRepository.InsertAsync(mapperEntity);
            }

            return MapToEntityDto(entity);
        }

        public override async Task Delete(EntityDto<int> input)
        {
            CheckDeletePermission();
            var entity = Repository.Get(input.Id);
            entity.IsLock = "Y";
            await Repository.UpdateAsync(entity);
        }

        //public async Task<string> GetProviderOptions()
        //{
        //    string result = "";
        //    var ps = await ExpressServiceProviderRepository.GetAll();
        //}

        public ExpressLogisticsDto GetExpressDtoById(EntityDto<int> input)
        {
            var entity = Repository.Get(input.Id);
            var ms = ExpressProviderMapperRepository.GetAllIncluding(i => i.ExpressServiceProvider,i=>i.ExpressLogistics).Where(i=>i.ExpressId==input.Id).ToList();
          
            var dto = MapToEntityDto(entity);
            dto.ExpressProviderMapper = ms;
            return dto;
        }
    }
}
