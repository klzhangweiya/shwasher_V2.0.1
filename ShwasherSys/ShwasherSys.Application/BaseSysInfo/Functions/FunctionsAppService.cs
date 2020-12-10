using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Castle.Core.Internal;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.Functions.Dto;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.Lambda;
using IwbZero;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.Authorization.Permissions;
using IwbZero.IdentityFramework;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.BaseSysInfo.Functions
{
    [AbpAuthorize, AuditLog("系统功能菜单", "菜单")]
    public class FunctionsAppService : ShwasherAsyncCrudAppService<SysFunction, FunctionDto, int, PagedRequestDto, FunctionCreateDto, FunctionUpdateDto>, IFunctionsAppService
    {
        
        private readonly IStatesAppService _statesAppService;
        public FunctionsAppService(IStatesAppService statesAppService, ICacheManager cacheManager, IRepository<SysFunction, int> repository) 
            : base(repository, "FunctionNo")
        {
            CacheManager = cacheManager;
            _statesAppService = statesAppService;
            KeyIsAuto = false;
        }

        protected override string ExistMessage { get; set; } = "功能菜单已存在，请检查后重试！";
        protected override string NotExistMessage { get; set; } = "功能菜单不存在，请检查后重试！";
        protected override string GetPermissionName { get; set; } = PermissionNames.PagesSystemSysFunction;
        protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesSystemSysFunction;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesSystemSysFunctionCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesSystemSysFunctionUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesSystemSysFunctionDelete;

        #region SelectList

        /// <summary>
        /// 获取 Function 下拉框选项
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<List<SelectListItem>> GetFunctionSelect()
        {
            var list = await Repository.GetAllListAsync();
            var sList = new List<SelectListItem>();
            foreach (var l in list)
            {
                sList.Add(new SelectListItem() { Value = l.FunctionNo, Text = l.FunctionName });
            }

            return sList;
        }

        /// <summary>
        /// 获取 Function 下拉框选项
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<string> GetFunctionSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string options = "";
            foreach (var f in list)
            {
                options += "<option value=\"" + f.FunctionNo + "\">" + f.FunctionName + "</option>";
            }

            return options;
        }

        ///// <summary>
        ///// 获取 Function 下拉框选项
        ///// </summary>
        ///// <returns></returns>
        //public async Task<string> GetLogSel()
        //{
        //    var list = (await Repository.GetAllListAsync()).Where(a => a.FunctionType == 1 && !a.Controller.IsNullOrEmpty());
        //    string lcRetval = "";
        //    foreach (var f in list)
        //    {
        //        lcRetval += "<option value=\"" + f.Controller + "\">" + f.FunctionName + "</option>";
        //    }

        //    return lcRetval;
        //}


        #endregion

        #region CURD

        [DisableAuditing]
        public async Task<SysFunction> GetFunByPermissionName(string name)
        {
            var fun = await Repository.FirstOrDefaultAsync(a => a.PermissionName == name);
            return fun;
        }

        [DisableAuditing]
        public override async Task<PagedResultDto<FunctionDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input).Where(a => a.FunctionType != 0);
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
                var exp = objList.GetExp<SysFunction>();
                query = query.Where(exp);
            }

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<FunctionDto>(
                totalCount,
                entities.Select(a => new FunctionDto()
                {
                    Id = a.Id,
                    FunctionNo = a.FunctionNo,
                    ParentNo = a.ParentNo,
                    FunctionName = a.FunctionName,
                    PermissionName = a.PermissionName,
                    FunctionType = a.FunctionType,
                    FunctionPath = a.FunctionPath,
                    Action = a.Action,
                    Controller = a.Controller,
                    Url = a.Url,
                    Icon = a.Icon,
                    Class = a.Class,
                    Script = a.Script,
                    Sort = a.Sort,
                    Depth = a.Depth,
                    FunctionTypeName = _statesAppService.GetDisplayValue("SysFunction", "FunctionType", a.FunctionType.ToString())
                }).ToList()
            );

            return dtos;
        }
        public override async Task<FunctionDto> Create(FunctionCreateDto input)
        {
            input.ParentNo = input.ParentNo.IsNullOrEmpty() ? "0" : input.ParentNo;
            input.FunctionPath = input.FunctionPath + "," + input.FunctionNo;
            input.PermissionName = input.PermissionName + "." + input.FunctionNo;
            var dto = await CreateEntity1(input);
            await Refresh();
            return dto;
        }
        public override async Task<FunctionDto> Update(FunctionUpdateDto input)
        {
            input.ParentNo = input.ParentNo.IsNullOrEmpty() ? "0" : input.ParentNo;
            var entity = await GetEntityByIdAsync(input.Id);
            string sysUser = AbpSession.UserName.ToUpper();
            if (sysUser != "ADMIN" && sysUser != "SYSTEM")
            {
                input.FunctionType = entity.FunctionType;
                input.PermissionName = entity.PermissionName;
                input.Action = entity.Action;
                input.Controller = entity.Controller;
                input.FunctionPath = entity.FunctionPath;
                input.Script = entity.Script;
                input.Class = entity.Class;
                input.Url = entity.Url;
            }

            MapToEntity(input, entity);
            var dto = await UpdateEntity1(input);
            await CacheManager.GetCache(IwbZeroConsts.SysFunctionItemCache).RemoveAsync(dto.FunctionNo);
            return dto;
        }
        public override async Task Delete(EntityDto<int> input)
        {
            CheckDeletePermission();
            var entity = await Repository.FirstOrDefaultAsync(a => a.Id == input.Id);
            if ((await Repository.GetAllListAsync(a => a.ParentNo == entity.FunctionNo)).Any())
            {
                CheckErrors(IwbIdentityResult.Failed("此菜单下还有子菜单，不能删除"));
            }
            await Repository.DeleteAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            await CacheManager.GetCache(IwbZeroConsts.SysFunctionItemCache).RemoveAsync(entity.FunctionNo);
        }

        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesSystemSysFunctionMoveUp), AuditLog("上移菜单")]
        public async Task MoveUp(MoveUpFunctionDto input)
        {
            var fun = await Repository.GetAsync(input.Id);
            int sort = fun.Sort;
            var prevFun = await Repository.GetAsync(input.PrevId);
            int prevSort = prevFun.Sort;
            fun.Sort = prevSort;
            prevFun.Sort = sort;
            await CurrentUnitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesSystemSysFunctionMoveDown), AuditLog("下移菜单")]
        public async Task MoveDown(MoveDownFunctionDto input)
        {
            var fun = await Repository.GetAsync(input.Id);
            int sort = fun.Sort;
            var nextFun = await Repository.GetAsync(input.NextId);
            int nextSort = nextFun.Sort;
            fun.Sort = nextSort;
            nextFun.Sort = sort;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 强制刷新
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesSystemSysFunctionRefresh), AuditLog("强制刷新")]
        public async Task Refresh()
        {
            await CacheManager.GetCache(IwbZeroConsts.SysFunctionCache).ClearAsync();
            await CacheManager.GetCache(IwbZeroConsts.SysFunctionItemCache).ClearAsync();
        }

        /// <summary>
        /// 重写排序方法
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<SysFunction> ApplySorting(IQueryable<SysFunction> query, PagedRequestDto input)
        {
            return query.OrderBy(a => a.FunctionType).ThenBy(a => a.Sort);
        }

        #endregion
    }
}
