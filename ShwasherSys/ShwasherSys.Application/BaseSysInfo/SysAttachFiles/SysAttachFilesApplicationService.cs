using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Caching;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;
using ShwasherSys.Lambda;

namespace ShwasherSys.BaseSysInfo.SysAttachFiles
{
    [AbpAuthorize]
    public class SysAttachFileAppService : ShwasherAsyncCrudAppService<SysAttachFile, SysAttachFileDto, int, PagedRequestDto, SysAttachFileCreateDto, SysAttachFileUpdateDto >
        , ISysAttachFileAppService
    {
        public SysAttachFileAppService(
			IIwbSettingManager settingManager, 
			ICacheManager cacheManager,
			IRepository<SysAttachFile, int> repository) : base(repository, "AttachNo")
        {
			SettingManager = settingManager;
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = true;

        #region GetSelect

        [DisableAuditing]
        public async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var slist = new List<SelectListItem> {new SelectListItem {Text = @"请选择...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.AttachNo, Value = l.FileTitle });
            }
            return slist;
        }
        [DisableAuditing]
        public async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string str = "<option value=\"\" selected>请选择...</option>";
            foreach (var l in list)
            {
                str += $"<option value=\"{l.AttachNo}\">{l.FileTitle}</option>";
            }
            return str;
        }


        [DisableAuditing]
        public async Task<List<SelectListItem>> GetTableSelectList(string tableName, string colName)
        {
            var list = await Repository.GetAllListAsync(a => a.TableName == tableName && a.ColumnName == colName);
            var slist = new List<SelectListItem> { new SelectListItem { Text = @"请选择...", Value = "", Selected = true } };
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.AttachNo, Value = l.FileTitle });
            }
            return slist;
        }

        [DisableAuditing]
        public async Task<string> GetTableSelectStr(string tableName,string colName )
        {
            var list = await Repository.GetAllListAsync(a => a.TableName == tableName && a.ColumnName == colName);
            string str = "<option value=\"\" selected>请选择...</option>";
            foreach (var l in list)
            {
                str += $"<option value=\"{l.AttachNo}\">{l.FileTitle}</option>";
            }
            return str;
        }

        #endregion

        #region CURD
        /// <summary>
        /// 查询附件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<SysAttachFileDto>> QueryAttach(QueryAttachDto input)
        {
            var entities = await Repository.GetAllListAsync(a =>
                a.TableName == input.TableName && a.ColumnName == input.ColName && a.SourceKey == input.Key);
            return entities.Select(MapToEntityDto).ToList();
        }

        [DisableAuditing]
        public override async Task<PagedResultDto<SysAttachFileDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
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
                var exp = objList.GetExp<SysAttachFile>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<SysAttachFileDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
            return dtos;
        }

        public override async Task<SysAttachFileDto> Create(SysAttachFileCreateDto input)
        {
            if (await IsValidFileType(input.FileExt))
            {
                string filePath = $"{SettingManager.GetSettingValue(SettingNames.DownloadPath)}/{input.TableName}/{input.ColumnName}";
                var lcRetVal= Base64ToFile(input.FileInfo,$"{input.FileName}-{DateTime.Now:yyMMddHHmmss}{new Random().Next(1000, 9999)}", input.FileExt,filePath);
                if (lcRetVal.StartsWith("error@"))
                {
            CheckErrors(IwbIdentityResult.Failed(lcRetVal.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries)[1]));
                    return null;
                }
                input.FilePath = lcRetVal;
                return await CreateEntity(input);
            }
            CheckErrors(IwbIdentityResult.Failed("文件类型不合法，请上传合法文件。"));
            return null;
        }

        public override async Task<SysAttachFileDto> Update(SysAttachFileUpdateDto input)
        {
            if (await IsValidFileType(input.FileExt))
            {
                string filePath = $"{SettingManager.GetSettingValue(SettingNames.DownloadPath)}/{input.TableName}/{input.ColumnName}";
                var lcRetVal= Base64ToFile(input.FileInfo, $"{input.FileName}-{DateTime.Now:yyMMddHHmmss}{new Random().Next(1000, 9999)}", input.FileExt,filePath);
                if (lcRetVal.StartsWith("error@"))
                {
            CheckErrors(IwbIdentityResult.Failed(lcRetVal.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries)[1]));
                    return null;
                }
                input.FilePath = lcRetVal;
                return await UpdateEntity(input);
            }
            CheckErrors(IwbIdentityResult.Failed("文件类型不合法，请上传合法文件。"));
            return null;
        }

        public override Task Delete(EntityDto<int> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        //protected override IQueryable<SysAttachFile> ApplySorting(IQueryable<SysAttachFile> query, PagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<SysAttachFile> ApplyPaging(IQueryable<SysAttachFile> query, PagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion


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



        public static  string Base64ToFile( string base64Str, string fileName, string fileExt, string filePath)
        {
            string lcRetVal = "error@";
            try
            {
                fileName = $"{fileName}.{fileExt}";
                filePath = filePath.StartsWith("/") ? filePath : ("/" + filePath);
                filePath = filePath.EndsWith("/") ? filePath : (filePath + "/");

                string path = $"{AppDomain.CurrentDomain.BaseDirectory}{filePath}";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                byte[] bytes = Convert.FromBase64String(base64Str);
                using (FileStream fs = new FileStream($"{path}{fileName}", FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }

                lcRetVal = filePath + fileName;
            }
            catch (Exception e)
            {
                typeof(SysAttachFileAppService).LogError(e);
                lcRetVal += "文件上传异常。";
            }

            return lcRetVal;
        }

    }
}
