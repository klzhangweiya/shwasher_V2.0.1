using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Timing;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.SysAttachFiles;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;
using ShwasherSys.Common;
using ShwasherSys.EntityFramework;
using ShwasherSys.ProductInfo.Dto;
using ShwasherSys.ProductInfo.Dto.FileUpload;

namespace ShwasherSys.ProductInfo
{
    [AbpAuthorize]
    public class SemiProductsAppService : ShwasherAsyncCrudAppService<SemiProducts, SemiProductDto, string, PagedRequestDto, SemiProductCreateDto, SemiProductUpdateDto >, ISemiProductsAppService
    {
        public IRepository<SysAttachFile> AttachRepository { get; }
        public ISqlExecuter SqlExecuter { get; }
        public SemiProductsAppService(IRepository<SemiProducts, string> repository, IIwbSettingManager settingManager, IRepository<SysAttachFile> attachRepository, ISqlExecuter sqlExecuter) : base(repository)
        {
            AttachRepository = attachRepository;
            SqlExecuter = sqlExecuter;
            SettingManager = settingManager;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesProductInfoSemiProducts;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesProductInfoSemiProducts;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesProductInfoSemiProductsCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesProductInfoSemiProductsUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesProductInfoSemiProductsDelete;


        public override async Task<SemiProductDto> Create(SemiProductCreateDto input)
        {
            CheckCreatePermission();

            var resultEntity = await CreateEntity(input);
           
            if (!string.IsNullOrEmpty(input.FileInfo))
            {
                SysAttachFileCreateDto fileCreateDto = new SysAttachFileCreateDto()
                {
                    AttachNo = Guid.NewGuid().ToString("N"),
                    TableName = "SemiProducts",
                    ColumnName = "SemiProductNo",
                    SourceKey = input.Id,
                    FileInfo = input.FileInfo,
                    FileExt = input.FileExt,
                    FileName = input.FileName
                };
                await CreateAttach(fileCreateDto);
            }
            return resultEntity;
        }
        public override async Task<SemiProductDto> Update(SemiProductUpdateDto input)
        {
            CheckCreatePermission();

            var resultEntity = await UpdateEntity(input);

            if (!string.IsNullOrEmpty(input.FileInfo))
            {
                SysAttachFileCreateDto fileCreateDto = new SysAttachFileCreateDto()
                {
                    AttachNo = Guid.NewGuid().ToString("N"),
                    TableName = "SemiProducts",
                    ColumnName = "SemiProductNo",
                    SourceKey = input.Id,
                    FileInfo = input.FileInfo,
                    FileExt = input.FileExt,
                    FileName = input.FileName
                };
                await AttachRepository.DeleteAsync(i =>
                    i.TableName == "SemiProducts" && i.ColumnName == "SemiProductNo" && i.SourceKey == input.Id);
                await CreateAttach(fileCreateDto);
            }
            return resultEntity;
        }
        private async Task CreateAttach(SysAttachFileCreateDto input)
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
                        IwbIdentityResult.Failed(lcRetVal.Split(new[] { '@' },
                            StringSplitOptions.RemoveEmptyEntries)[1]));
                    return;
                }
                input.FilePath = lcRetVal;
                var entity = ObjectMapper.Map<SysAttachFile>(input);
                entity = await AttachRepository.InsertAsync(entity);
               
                return;
            }

            CheckErrors(IwbIdentityResult.Failed("文件类型不合法，请上传合法文件。"));
        }
        private async Task<bool> IsValidFileType(string fileName, bool isName = false)
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
                List<SemiProducts> resultEntityList = ExcelHelper.ExcelToEntityList<SemiProducts>(new Dictionary<string, string>() { { "Id", "半成品编码" }, { "PartNo", "零件号" }, { "Model", "规格" }, { "Material", "材质" }, { "Rigidity", "硬度" }, { "SurfaceColor", "表色" }, { "IsStandard", "是否标件" },{ "SemiProductName", "半成品名称"} }, $"{AppDomain.CurrentDomain.BaseDirectory}{lcRetVal}",
                    out errStringBuilder);
                int indexCount = 1;
               
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("delete from SemiProducts;");
                foreach (var semiProductse in resultEntityList)
                {
                    semiProductse.IsLock = "N";
                    semiProductse.TimeLastMod = Clock.Now;
                    semiProductse.TimeCreated = Clock.Now;
                    semiProductse.UserIDLastMod = AbpSession.UserName;
                    if (semiProductse.IsStandard == "是"|| semiProductse.IsStandard == "否")
                    {
                        semiProductse.IsStandard = semiProductse.IsStandard == "是" ? "Y" : "N";
                    }
                    semiProductse.SemiProductName = semiProductse.SemiProductName;
                    semiProductse.Sequence = indexCount;
                    semiProductse.Model = semiProductse.Model == "-" ? "" : semiProductse.Model;
                    semiProductse.Material = semiProductse.Material == "-" ? "" : semiProductse.Material;
                    semiProductse.Rigidity = semiProductse.Rigidity == "-" ? "" : semiProductse.Rigidity;
                    semiProductse.SurfaceColor = semiProductse.SurfaceColor == "-" ? "" : semiProductse.SurfaceColor;
                    semiProductse.PartNo = semiProductse.PartNo == "-" ? "" : semiProductse.PartNo;
                    sbSql.Append(semiProductse.InsertSql()+"\r\n");
                    indexCount++;
                }

                if (SqlExecuter.Execute(sbSql.ToString()) > 0)
                {
                    return true;
                }
                this.LogError(errStringBuilder.ToString());
            }
            else
            {
                CheckErrors(new IwbIdentityResult("请先上传文件！"));
            }

            return false;
        }
        [AbpAllowAnonymous]
        public async Task<string> ExportExcel()
        {
            var query = Repository.GetAll();
          
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
           
            string downloadUrl = await SettingManager.GetSettingValueAsync("SYSTEMDOWNLOADPATH");
            string lcFilePath = System.Web.HttpRuntime.AppDomainAppPath + "\\" +
                                downloadUrl;
            var exportEntity = new Dictionary<string, string>()
            {
                {"Id", "半成品编码"},
                {"Model", "规格"},
                {"Material", "材质"},
                {"SurfaceColor", "表色"},
                {"Rigidity", "硬度"},
                {"IsStandard", "是否标件"},
                {"PartNo", "零件号"},
                {"SemiProductName", "半成品名称"},
                
            };
           
            string lcResultFileName = ExcelHelper.EntityListToExcel2003(exportEntity, entities, "sheet", lcFilePath);
            return Path.Combine(downloadUrl, lcResultFileName);
        }
    }
}
