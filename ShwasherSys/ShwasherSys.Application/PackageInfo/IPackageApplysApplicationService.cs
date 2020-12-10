using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.PackageInfo.Dto;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.PackageInfo
{
    public interface IPackInfoApplyAppService : IIwbAsyncCrudAppService<PackageApplyDto, int, PagedRequestDto, PackageApplyCreateDto, PackageApplyUpdateDto >
    {

        /// <summary>
        /// 确认包装申请，创建包装明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreatePackInfo(CreatePackInfosDto input);

        /// <summary>
        /// 拒绝包装申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task RefusePackInfoApply(RefusePackInfoDto input);

        /// <summary>
        /// 关闭包装申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ClosePackInfoApply(EntityDto<int> input);

        #region 包装后成品入库

        /// <summary>
        /// 查询包装明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgQuery)]
        PagedResultDto<FinshedEnterStore> GetFinishedEnterStoreApply(PagedRequestDto input);

        /// <summary>
        /// 修改包装明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgUpdate)]
        Task UpdatePackInfo(UpdatePackInfoDto input);

        /// <summary>
        /// 添加包装明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgUpdate)]
        Task AddPackInfo(CreatePackInfoDto input);

        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgDelete)]
        Task DeletePackInfo(EntityDto input);

        /// <summary>
        /// 包装入库申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgEnter)]
        Task CreateProductApply(EntityDto input);

        /// <summary>
        /// 包装完成入库申请批量
        /// </summary>
        /// <param name="applyNo"></param>
        /// <returns></returns>
        Task CreateProductApplyBatch(string applyNo);

        /// <summary>
        /// 确认入库数量（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgConfirm)]
        Task ConfirmProductApply(EntityDto<int> input);

        /// <summary>
        /// 取消入库申请（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesPackInfoPackInfoMgPackageInfoMgCancel)]
        Task CancelProductApply(EntityDto<int> input);

        /// <summary>
        /// 关闭入库申请 （by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CloseProductApply(EntityDto<int> input);

        /// <summary>
        /// 恢复入库申请（by yue.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task RecoveryProductApply(EntityDto<int> input);

        #endregion

        Task<FinshedEnterStore> GetHasExistProductionOrderNo(string pcProductionOrderNo);
       // Task<FinshedEnterStore> GetProductionOrderHasCreate(string pcProductionOrderNo, string pcProductNo);
        Task<ViewProductEnterStore> GetHasExistProductionOrderNoView(string pcProductionOrderNo);

        Task<PackageDayDateDto> QueryPackageDaily(DateTime date);
        Task<List<SelectListItem>> GetSelectList();
		Task<string> GetSelectStr();

		#region Get
		Task<PackageApply> GetEntityById(int id);
		Task<PackageApply> GetEntityByNo(string no);
		Task<PackageApplyDto> GetDtoById(int id);
		Task<PackageApplyDto> GetDtoByNo(string no);
        #endregion

    }
}
