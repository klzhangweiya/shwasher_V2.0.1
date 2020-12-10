using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.SysAttachFiles;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;
using ShwasherSys.Common;
using ShwasherSys.CompanyInfo.LicenseInfo.Dto;
namespace ShwasherSys.CompanyInfo.LicenseInfo
{
    [AbpAuthorize, AuditLog("证照信息维护")]
    public class LicenseDocumentAppService : IwbZeroAsyncCrudAppService<LicenseDocument, LicenseDocumentDto, int, IwbPagedRequestDto, LicenseDocumentCreateDto, LicenseDocumentUpdateDto >, ILicenseDocumentAppService
    {
        public LicenseDocumentAppService(
			ICacheManager cacheManager,
			IRepository<LicenseDocument, int> repository, IRepository<SysAttachFile> attachRepository, IRepository<BusinessLog> logRepository) : base(repository)
        {
            AttachRepository = attachRepository;
            LogRepository = logRepository;
            CacheManager = cacheManager;
        }
        public IRepository<SysAttachFile> AttachRepository { get; }
        public IRepository<BusinessLog> LogRepository { get; }
        protected override bool KeyIsAuto { get; set; } = false;

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

        [AbpAuthorize(PermissionNames.PagesCompanyLicenseDocumentCreate)]
        public override async Task Create(LicenseDocumentCreateDto input)
        {
            var entity =
                await Repository.FirstOrDefaultAsync(a => a.No == input.No && a.LicenseType == input.LicenseType);
            if (entity != null)
            {
                CheckErrors("证照编码不能重复，请检查后再试！");
            }
            var dto = await CreateEntity(input);
            if (input.AttachFiles != null && input.AttachFiles.Any())
            {
                foreach (var attach in input.AttachFiles)
                {
                    attach.AttachNo = Guid.NewGuid().ToString("N");
                    attach.TableName = "License";
                    attach.ColumnName = "License";
                    attach.SourceKey = dto.No;
                    await CreateAttach(attach);
                }
            }
        }

        [AbpAuthorize(PermissionNames.PagesCompanyLicenseDocumentUpdate)]
        public override async Task Update(LicenseDocumentUpdateDto input)
        {
            var dto = await UpdateEntity(input);
            if (input.AttachFiles != null && input.AttachFiles.Any())
            {
                await AttachRepository.DeleteAsync( a => a.ColumnName == "License" && a.TableName == "License" && a.SourceKey == dto.No);
                foreach (var attach in input.AttachFiles)
                {
                    attach.AttachNo = Guid.NewGuid().ToString("N");
                    attach.TableName = "License";
                    attach.ColumnName = "License";
                    attach.SourceKey = dto.No;
                    await CreateAttach(attach);
                }
            }
        }

        [AbpAuthorize(PermissionNames.PagesCompanyLicenseDocumentDelete)]
        public override Task Delete(EntityDto<int> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyLicenseDocumentQuery)]
        public override async Task<PagedResultDto<LicenseDocumentDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<LicenseDocumentDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyLicenseDocumentQuery)]
        public override async Task<LicenseDocumentDto> GetDto(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyLicenseDocumentQuery)]
        public override async Task<LicenseDocumentDto> GetDtoById(int id)
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
        [AbpAuthorize(PermissionNames.PagesCompanyLicenseDocumentQuery)]
        public override async Task<LicenseDocumentDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesCompanyLicenseDocumentQuery)]
        public override async Task<LicenseDocument> GetEntity(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesCompanyLicenseDocumentQuery)]
        public override async Task<LicenseDocument> GetEntityById(int id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesCompanyLicenseDocumentQuery)]
        public override async Task<LicenseDocument> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{LicenseDocument}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<LicenseDocument> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<LicenseDocument>();
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
        //        var exp = objList.GetExp<LicenseDocument>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<LicenseDocument> ApplySorting(IQueryable<LicenseDocument> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<LicenseDocument> ApplyPaging(IQueryable<LicenseDocument> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion


        private  async Task CreateAttach(SysAttachFileCreateDto input)
        {
            if (await IsValidFileType(input.FileExt))
            {
                string filePath =
                    $"{SettingManager.GetSettingValue(SettingNames.DownloadPath)}/{input.TableName}/{input.ColumnName}";
                var lcRetVal = SysAttachFileAppService.Base64ToFile(input.FileInfo,
                    $"{input.FileName}-{DateTime.Now:yyMMddHHmmss}{new Random().Next(1000, 9999)}", input.FileExt,
                    filePath);
                if (lcRetVal.StartsWith("error@"))
                {
                    CheckErrors(
                        IwbIdentityResult.Failed(lcRetVal.Split(new[] {'@'},
                            StringSplitOptions.RemoveEmptyEntries)[1]));
                    return;
                }
                input.FilePath = lcRetVal;
                var entity = ObjectMapper.Map<SysAttachFile>(input);
                entity = await AttachRepository.InsertAsync(entity);
                BusinessLogTypeEnum.License.WriteLog(LogRepository, "证照上传",
                    $"添加附件：[{entity.AttachNo}]-{entity.FileTitle}-{entity.FileName}.{entity.FileExt}",
                    entity.SourceKey, entity.AttachNo);
                return;
            }

            CheckErrors(IwbIdentityResult.Failed("文件类型不合法，请上传合法文件。"));
        }
        
        private async Task<bool> IsValidFileType(string fileName ,bool isName=false)
        {
            string ext = isName ? GetFileExt(fileName) : fileName;
            string lcExts = await SettingManager.GetSettingValueAsync(SettingNames.UploadFileExt);
            string[] loList = lcExts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var lcExt in loList)
            {
                if (ext.ToLower() == lcExt.ToLower())
                    return true;
            }
            this.LogError("上传的文件非法:" + fileName);
            return false;
        }

        private string GetFileExt(string fileName)
        {
            string fileExt = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal) + 1, fileName.Length - fileName.LastIndexOf(".", StringComparison.Ordinal) - 1);
            return fileExt.ToLower();
        }


    }
}
