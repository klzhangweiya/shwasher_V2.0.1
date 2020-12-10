using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.States.Dto;
using IwbZero;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;

namespace ShwasherSys.BaseSysInfo.States
{
    [AbpAuthorize, AuditLog("系统字典", "字典")]
    public class StatesAppService : ShwasherAsyncCrudAppService<SysState, StateDto, int, PagedRequestDto, StateCreateDto, StateUpdateDto>, IStatesAppService
    {

        public StatesAppService(ICacheManager cacheManager, IRepository<SysState, int> repository) : base(repository, "StateNo")
        {
            CacheManager = cacheManager;
        }

        protected override string GetPermissionName { get; set; } = PermissionNames.PagesSystemSysState;
        protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesSystemSysState;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesSystemSysStateCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesSystemSysStateUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesSystemSysStateDelete;

        #region GetSelectList

        [DisableAuditing, AllowAnonymous]
        public List<SelectListItem> GetSelectLists(QueryStateDisplayValue input, Expression<Func<SysState, bool>> exp = null)
        {
            return GetSelectLists(input.TableName, input.ColumnName, exp);
        }
        [DisableAuditing, AllowAnonymous]
        public List<SelectListItem> GetSelectLists(string tableName, string columnName, Expression<Func<SysState, bool>> exp = null)
        {
            var slist = new List<SelectListItem>();
            var list = GetStateList(tableName, columnName, exp);
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.DisplayValue, Value = l.CodeValue });
            }
            return slist;
        }
        [DisableAuditing, AllowAnonymous]
        public string GetSelectListStrs(QueryStateDisplayValue input, Expression<Func<SysState, bool>> exp = null)
        {
            return GetSelectListStrs(input.TableName, input.ColumnName, exp);
        }
        [DisableAuditing, AllowAnonymous]
        public string GetSelectListStrs(string tableName, string columnName, Expression<Func<SysState, bool>> exp = null)
        {
            var options = "";
            var list = GetStateList(tableName, columnName, exp);
            foreach (var l in list)
            {
                options += $"<option value=\"{l.CodeValue}\" >{l.DisplayValue}</option>\r\n";
            }
            return options;
        }

        [DisableAuditing, AllowAnonymous]
        public List<StateDisplayDto> GetStateList(QueryStateDisplayValue input, Expression<Func<SysState, bool>> exp = null)
        {
            return GetStateList(input.TableName, input.ColumnName, exp);
        }
        [DisableAuditing, AllowAnonymous]
        public List<StateDisplayDto> GetStateList(string tableName, string columnName, Expression<Func<SysState, bool>> exp = null)
        {
            var list = Repository.GetAll().Where(a => a.TableName == tableName && a.ColumnName == columnName);
            if (exp != null)
            {
                list = list.Where(exp);
            }
            var dtos = list.Select(a => new StateDisplayDto()
            {
                CodeValue = a.CodeValue,
                DisplayValue = a.DisplayValue
            }).ToList();
            return dtos;
        }

        #endregion

        #region GetDisplayValue
        [DisableAuditing]
        public async Task<string> GetDisplayValueAsync(QueryStateDisplayValue input)
        {
            return await GetDisplayValueAsync(input.TableName, input.ColumnName, input.CodeValue);
        }
        [DisableAuditing]
        public async Task<string> GetDisplayValueAsync(string tableName, string columnName, string codeValue)
        {
            return await CacheManager.GetCache(IwbZeroConsts.SysStateCache).GetAsync(
                 tableName + "." + columnName + "." + codeValue, () => DisplayValueAsync(tableName, columnName, codeValue));
        }
        private async Task<string> DisplayValueAsync(string tableName, string columnName, string codeValue)
        {
            var state = await Repository.FirstOrDefaultAsync(a =>
                a.TableName == tableName && a.ColumnName == columnName && a.CodeValue == codeValue);
            return state?.DisplayValue;
        }
        [DisableAuditing]
        public string GetDisplayValue(QueryStateDisplayValue input)
        {
            return GetDisplayValue(input.TableName, input.ColumnName, input.CodeValue);
        }
        [DisableAuditing]
        public string GetDisplayValue(string tableName, string columnName, string codeValue)
        {
            return CacheManager.GetCache(IwbZeroConsts.SysStateCache).Get(
                 tableName + "." + columnName + "." + codeValue, () => DisplayValue(tableName, columnName, codeValue));
        }
        private string DisplayValue(string tableName, string columnName, string codeValue)
        {
            var state = Repository.FirstOrDefault(a =>
               a.TableName == tableName && a.ColumnName == columnName && a.CodeValue == codeValue);
            return state?.DisplayValue;
        }

        #endregion

        public override async Task<StateDto> Update(StateUpdateDto input)
        {
            CheckUpdatePermission();
            var dto = await UpdateEntity1(input);
            await CacheManager.GetCache(IwbZeroConsts.SysStateCache)
                .RemoveAsync(input.TableName + "." + input.ColumnName + "." + input.CodeValue);
            return dto;
        }

        public override async Task<StateDto> Create(StateCreateDto input)
        {
            CheckCreatePermission();
            var dto = await CreateEntity1(input);
            return dto;
        }
    }
}
