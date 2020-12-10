using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.Functions.Dto;
using ShwasherSys.BasicInfo.Region.Dto;
using ShwasherSys.Lambda;

namespace ShwasherSys.BasicInfo.Region
{
    [AbpAuthorize]
    public class RegionsAppService : ShwasherAsyncCrudAppService<Regions, RegionDto, string, PagedRequestDto, RegionCreateDto, RegionUpdateDto >, IRegionsAppService
    {
        public RegionsAppService(IRepository<Regions, string> repository) : base(repository)
        {
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesBasicInfoRegions;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesBasicInfoRegions;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesBasicInfoRegionsCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesBasicInfoRegionsUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesBasicInfoRegionsDelete;

        public override async Task<RegionDto> Create(RegionCreateDto input)
        {
            CheckCreatePermission();
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if (entity!=null)
            {
                CheckErrors(IwbIdentityResult.Failed("此编号已经被使用！请更换其它编号！"));
            }
            var loFather =await Repository.GetAsync(input.FatherRegionID);
            loFather.IsLeaf = "N";
            input.FatherRegionID = loFather.Id;
            input.Depth = loFather.Depth + 1;
            input.IsLeaf = "Y";
            input.Path = loFather.Path + input.Id+",";
            input.Sort = input.Sort;
            await Repository.UpdateAsync(loFather);
            return await CreateEntity(input);
        }

        public override async Task Delete(EntityDto<string> input)
        {
            CheckDeletePermission();
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if ((await Repository.GetAllListAsync(a => a.FatherRegionID == entity.Id&&a.IsLock=="N")).Any())
            {
                CheckErrors(IwbIdentityResult.Failed("此菜单下还有子区域，不能删除"));
            }

            entity.IsLock = "Y";
            await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task<string> GetRegionSelectStrs()
        {
            var options = "";
            var list = await Repository.GetAllListAsync(a => a.IsLock == "N");
            foreach (var l in list)
            {
                string parent = l.FatherRegionID == "0" ? "" : $" parent=\"{l.FatherRegionID}\"";
                options += $"<option value=\"{l.Id}\"{parent}>{l.RegionName}</option>\r\n";
            }

            return options;
        }

        /*public override async Task<PagedResultDto<RegionDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input).Where(a => a.IsLock == "N");
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
                var exp = objList.GetExp<Regions>();
                query = query.Where(exp);
            }

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<RegionDto>(
                totalCount, ObjectMapper.Map<List<RegionDto>>(entities)
            );

            return dtos;
        }*/
    }
}
