using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Lambda;
using ShwasherSys.ProductInfo.Dto;
using Abp.Extensions;
using Abp.Timing;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.BaseSysInfo.SysAttachFiles;
using ShwasherSys.EntityFramework;
using ShwasherSys.ProductInfo.Dto.FileUpload;

namespace ShwasherSys.ProductInfo
{
    [AbpAuthorize]
    public class RmProductAppService : IwbZeroAsyncCrudAppService<RmProduct, RmProductDto, string, IwbPagedRequestDto, RmProductCreateDto, RmProductUpdateDto >, IRmProductAppService
    {
        public RmProductAppService(
			ICacheManager cacheManager,
			IRepository<RmProduct, string> repository, ISqlExecuter sqlExecuter) : base(repository, "Id")
        {
            SqlExecuter = sqlExecuter;
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = false;

        protected ISqlExecuter SqlExecuter { get; }

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

        [AbpAuthorize(PermissionNames.PagesProductInfoRmProductCreate)]
        public override async Task Create(RmProductCreateDto input)
        {
            await CreateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesProductInfoRmProductUpdate)]
        public override async Task Update(RmProductUpdateDto input)
        {
            await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesProductInfoRmProductDelete)]
        public override Task Delete(EntityDto<string> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInfoRmProductQuery)]
        public override async Task<PagedResultDto<RmProductDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<RmProductDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInfoRmProductQuery)]
        public override async Task<RmProductDto> GetDto(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesProductInfoRmProductQuery)]
        public override async Task<RmProductDto> GetDtoById(string id)
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
        [AbpAuthorize(PermissionNames.PagesProductInfoRmProductQuery)]
        public override async Task<RmProductDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesProductInfoRmProductQuery)]
        public override async Task<RmProduct> GetEntity(EntityDto<string> input)
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
        [AbpAuthorize(PermissionNames.PagesProductInfoRmProductQuery)]
        public override async Task<RmProduct> GetEntityById(string id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInfoRmProductQuery)]
        public override async Task<RmProduct> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{RmProduct}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<RmProduct> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<RmProduct>();
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
        //        var exp = objList.GetExp<RmProduct>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<RmProduct> ApplySorting(IQueryable<RmProduct> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<RmProduct> ApplyPaging(IQueryable<RmProduct> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion
        [AbpAuthorize(PermissionNames.PagesProductInfoSemiProductsImportExcel)]
        public bool ImportExcel(FileUploadInfoDto input)
        {
            if (!string.IsNullOrEmpty(input.FileInfo))
            {
                string filePath =
                    $"{SettingManager.GetSettingValue(SettingNames.DownloadPath)}/tmpUpload";
                var lcRetVal = SysAttachFileAppService.Base64ToFile(input.FileInfo,
                    $"{input.FileName}-{DateTime.Now:yyMMddHHmmss}{new Random().Next(1000, 9999)}", input.FileExt,
                    filePath);
                StringBuilder errStringBuilder = new StringBuilder();
                List<RmProduct> resultEntityList = ExcelHelper.ExcelToEntityList<RmProduct>(new Dictionary<string, string>() { { "Id", "编码" }, { "Material", "材质" }, { "Model", "原材料规格" } }, $"{AppDomain.CurrentDomain.BaseDirectory}{lcRetVal}",
                    out errStringBuilder);
                int indexCount = 1;
                if (resultEntityList.Any())
                {
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append("delete from RmProduct;");
                    foreach (var p in resultEntityList)
                    {
                        p.CreationTime = Clock.Now;
                        p.CreatorUserId = AbpSession.UserId;
                        p.ProductName = p.Material+" "+p.Model;
                        p.Model = p.Model;
                        p.Material = p.Material;
                        sbSql.Append(p.InsertSql() + ";\r\n");
                        indexCount++;
                    }
                    if (SqlExecuter.Execute(sbSql.ToString()) > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    CheckErrors(new IwbIdentityResult("文件内容为空(或不符合要求)，请检查！"));

                }
                this.LogError(errStringBuilder.ToString());

            }
            else
            {
                CheckErrors(new IwbIdentityResult("请先上传文件！"));
            }

            return false;
        }

    }
}
