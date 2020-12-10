using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Extensions;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Common.Dto;
using ShwasherSys.Lambda;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo;
using ShwasherSys.StoreQuery.Dto;

namespace ShwasherSys.StoreQuery
{
    [AbpAuthorize]
    public class StoreStatisticsApplicationService: ShwasherAsyncCrudAppService<ViewEnterOutProductStore, ViewEnterOutProductStoreDto, string, PagedRequestDto, ViewEnterOutProductStoreDto, ViewEnterOutProductStoreDto>, IStoreStatisticsApplicationService
    {
        public  IRepository<ViewCurrentStoreTotal,string> ViewCurrentStoreTotalRepository { get; }
        public  IRepository<ViewEnterOutLogCus, string> ViewEnterOutLogCusRepository { get; }
        public  IRepository<ViewEnterOutSemiProductStore, string> ViewEnterOutSemiProductStoreRepository { get; }
        public  IRepository<ViewCurrentSemiStoreTotal, string> ViewCurrentSemiStoreTotalRepository { get; }
        public StoreStatisticsApplicationService(IRepository<ViewEnterOutProductStore, string> repository,IRepository<ViewCurrentStoreTotal, string> viewCurrentStoreTotalRepository,IIwbSettingManager settingManager, IRepository<ViewEnterOutLogCus, string> viewEnterOutLogCusRepository, IRepository<ViewEnterOutSemiProductStore, string> viewEnterOutSemiProductStoreRepository, IRepository<ViewCurrentSemiStoreTotal, string> viewCurrentSemiStoreTotalRepository):base(repository)
        {
            ViewCurrentStoreTotalRepository = viewCurrentStoreTotalRepository;
            ViewEnterOutLogCusRepository = viewEnterOutLogCusRepository;
            ViewEnterOutSemiProductStoreRepository = viewEnterOutSemiProductStoreRepository;
            ViewCurrentSemiStoreTotalRepository = viewCurrentSemiStoreTotalRepository;
            SettingManager = settingManager;
        }
       [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoEnterOutStoreHouseQueryMg),AuditLog("库存出入库记录")]
        public async Task<PagedResultDto<ViewEnterOutLogCus>> QueryEnterOutRecord(PagedRequestDto input)
        {
            var query = ViewEnterOutLogCusRepository.GetAll();
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
                var exp = objList.GetExp<ViewEnterOutLogCus>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderByDescending(i => i.ProductNo).ThenByDescending(i=>i.DateTiem);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var result = new PagedResultDto<ViewEnterOutLogCus>(totalCount, entities);
            return result;
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoEnterOutStoreHouseQueryMg), AuditLog("库存出入库记录")]
        public async Task<List<ViewEnterOutProductStore>> QueryEnterOutRecordTmp(List<MultiSearchDtoExt> input)
        {
            var query = Repository.GetAll();
            if (input != null && input.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input)
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
                var exp = objList.GetExp<ViewEnterOutProductStore>();
                query = query.Where(exp);
            }
          
            query = query.OrderByDescending(i => i.ProductNo).ThenByDescending(i => i.DateTiem);
         
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
          
            return entities;
        }
        public async Task<List<ViewEnterOutProductStore>> QueryProductEnterOutRecord(string productNo)
        {
            return (await Repository.GetAllListAsync(i => i.ProductNo == productNo)).OrderByDescending(i=>i.DateTiem).ToList();
        }

        #region 进出库记录导出
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoEnterOutStoreHouseQueryMgExportExcel), AuditLog("库存出入库记录导出excel")]
        public async Task<string> ExportExcel(List<MultiSearchDtoExt> input)
        {
            var query = ViewEnterOutLogCusRepository.GetAll();
            if (input != null && input.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input)
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
                var exp = objList.GetExp<ViewEnterOutLogCus>();
                query = query.Where(exp);
            }
            query = query.OrderByDescending(i => i.ProductNo).ThenByDescending(i => i.DateTiem);
            var r = query.Select(i => new
            {
                i.ProductNo,
                i.ProductName,
                i.Model,
                i.SurfaceColor,
                i.Material,
                i.Rigidity,
                i.Quantity,
                EnterOutFlag = i.EnterOutFlag==1?"入库":"出库",
                i.DateTiem,
                i.UserIDLastMod,
                i.CustomerId,
                i.CustomerName,
            }).ToList();
            string downloadUrl = await SettingManager.GetSettingValueAsync("SYSTEMDOWNLOADPATH");
            string lcFilePath = System.Web.HttpRuntime.AppDomainAppPath + "\\" +
                                downloadUrl;
            List<ToExcelObj> columnsList = new List<ToExcelObj>()
            {
                new ToExcelObj(){MapColumn = "ProductNo",ShowColumn = "产品编号"},
                new ToExcelObj(){MapColumn = "ProductName",ShowColumn = "产品名称"},
                new ToExcelObj(){MapColumn = "Model",ShowColumn = "规格"},
                new ToExcelObj(){MapColumn = "SurfaceColor",ShowColumn = "表色"},
                new ToExcelObj(){MapColumn = "Material",ShowColumn = "材质"},
                new ToExcelObj(){MapColumn = "Rigidity",ShowColumn = "硬度"},
                new ToExcelObj(){MapColumn = "Quantity",ShowColumn = "数量(千件)"},
                new ToExcelObj(){MapColumn = "EnterOutFlag",ShowColumn = "进出库"},
                new ToExcelObj(){MapColumn = "DateTiem",ShowColumn = "日期"},
                new ToExcelObj(){MapColumn = "UserIDLastMod",ShowColumn = "操作人"},
                new ToExcelObj(){MapColumn = "CustomerId",ShowColumn = "客户编号"},
                new ToExcelObj(){MapColumn = "CustomerName",ShowColumn = "客户名称"},
            };
           string lcResultFileName = ExcelHelper.ToExcel2003(columnsList, r, "sheet", lcFilePath);
          
            return Path.Combine(downloadUrl, lcResultFileName);
        }

        #endregion


        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentStoreHouseQueryMg), AuditLog("库存信息查询")]
        public async Task<PagedResultDto<ViewCurrentStoreTotal>> QueryCurrentStoreTotal(PagedRequestDto input)
        {
            var query = ViewCurrentStoreTotalRepository.GetAll();
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
                var exp = objList.GetExp<ViewCurrentStoreTotal>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderByDescending(i => i.Id);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = new PagedResultDto<ViewCurrentStoreTotal>(
                totalCount,
                entities
            );
            return dtos;
        }
        [AbpAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentStoreHouseQueryMg), AuditLog("库存信息查询")]
        public async Task<ViewCurrentStoreTotal> QueryStoreTotalByProduct(string pcProductNo)
        {
            //throw new NotImplementedException();
            var query =await ViewCurrentStoreTotalRepository.FirstOrDefaultAsync(i=>i.Id==pcProductNo);
            return query;
        }

        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoCurrentSemiStoreHouseMgQuery), AuditLog("半成品库存进出信息查询")]
        public async Task<List<ViewEnterOutSemiProductStore>> QuerySemiEnterOutRecord(string productNo)
        {
            return (await ViewEnterOutSemiProductStoreRepository.GetAllListAsync(i => i.SemiProductNo == productNo)).OrderByDescending(i => i.DateTiem).ToList();
        }

        [AbpAuthorize(PermissionNames.PagesSemiProductStoreInfoCurrentSemiStoreHouseMgQuery), AuditLog("半成品库存统计信息查询")]
        public async Task<PagedResultDto<ViewCurrentSemiStoreTotal>> QuerySemiCurrentStoreTotal(PagedRequestDto input)
        {
            var query = ViewCurrentSemiStoreTotalRepository.GetAll();
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
                var exp = objList.GetExp<ViewCurrentSemiStoreTotal>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderByDescending(i => i.Id);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = new PagedResultDto<ViewCurrentSemiStoreTotal>(
                totalCount,
                entities
            );
            return dtos;
        }

        public async Task<ViewCurrentSemiStoreTotal> QuerySemiCurrentStoreTotalByProduct(string pcProductNo)
        {
            //throw new NotImplementedException();
            var query = await ViewCurrentSemiStoreTotalRepository.FirstOrDefaultAsync(i => i.Id == pcProductNo);
            return query;
        }
    }
}
