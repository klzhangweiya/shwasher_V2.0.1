using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using ShwasherSys.BaseSysInfo.States.Dto;
using IwbZero.AppServiceBase;

namespace ShwasherSys.BaseSysInfo.States
{
    public interface IStatesAppService : IIwbAsyncCrudAppService<StateDto, int, PagedRequestDto, StateCreateDto, StateUpdateDto>
    {
        List<SelectListItem> GetSelectLists(QueryStateDisplayValue input, Expression<Func<SysState, bool>> exp = null);
        List<SelectListItem> GetSelectLists(string tableName, string columnName, Expression<Func<SysState, bool>> exp = null);
        string GetSelectListStrs(QueryStateDisplayValue input, Expression<Func<SysState, bool>> exp = null);
        string GetSelectListStrs(string tableName, string columnName, Expression<Func<SysState, bool>> exp = null);
        List<StateDisplayDto> GetStateList(QueryStateDisplayValue input, Expression<Func<SysState, bool>> exp = null);
        List<StateDisplayDto> GetStateList(string tableName, string columnName, Expression<Func<SysState, bool>> exp = null);
        string GetDisplayValue(QueryStateDisplayValue input);
        string GetDisplayValue(string tableName, string columnName, string codeValue);
        Task<string> GetDisplayValueAsync(QueryStateDisplayValue input);
        Task<string> GetDisplayValueAsync(string tableName, string columnName, string codeValue);
    }
}
