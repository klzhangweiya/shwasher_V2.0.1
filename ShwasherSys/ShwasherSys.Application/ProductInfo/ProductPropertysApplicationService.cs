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
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.ProductInfo.Dto;
namespace ShwasherSys.ProductInfo
{
    [AbpAuthorize]
    public class ProductPropertyAppService : IwbZeroAsyncCrudAppService<ProductProperty, ProductPropertyDto, int, IwbPagedRequestDto, ProductPropertyCreateDto, ProductPropertyUpdateDto >, IProductPropertyAppService
    {
        public ProductPropertyAppService(
			ICacheManager cacheManager,
			IRepository<ProductProperty, int> repository) : base(repository, "Id")
        {
            CacheManager = cacheManager;
        }

        protected override bool KeyIsAuto { get; set; } = true;

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


        #region 编号维护
        /// <summary>
        /// 十进制转32进制
        /// </summary>
        /// <param name="inputNum"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static string DecimalCalcTo32(int inputNum, int maxSize)
        {
            int max = 0;
            var result = new string[20];
            var displayArr = new string[]
            {
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M",
                "N", "P", "Q", "R", "T", "U", "V", "W", "X", "Y", "Z"
            };
            int ten = inputNum;
            int arrSize = displayArr.Length;
            string lResult = "";
            do
            {
                var sixteen = ten % arrSize;
                ten = ten / arrSize;
                result[max] = displayArr[sixteen];
                lResult = result[max] + lResult;
                max++;
            } while (ten != 0);
            lResult = lResult.PadLeft(maxSize, '0');
            return lResult;
        }
        /// <summary>
        /// 32进制转10进制
        /// </summary>
        /// <param name="inputNum"></param>
        /// <returns></returns>
        public static double Cal32ToDecimal(string inputNum)
        {
            if (string.IsNullOrEmpty(inputNum))
            {
                return -1;
            }
            var displayStr = "0123456789ABCDEFGHJKMNPQRTUVWXYZ";
            int disLength = displayStr.ToArray().Length;
            double numResult = 0;
            var inputArr = inputNum.ToArray().Reverse().ToList();

            for (int i = 0; i < inputArr.Count; i++)
            {
                int index = displayStr.IndexOf(inputArr[i]);
                if (index < 0)
                {
                    return -1;
                }
                numResult += index * Math.Pow(disLength, i);
            }

            return numResult;
        }
        /// <summary>
        /// 上一个32位编码
        /// </summary>
        /// <param name="inputNum"></param>
        /// <returns></returns>
        private string GetNextNum(string inputNum,int maxSize)
        {
            var preNum = Cal32ToDecimal(inputNum);
            var newNum = preNum + 1;
            return DecimalCalcTo32((int) newNum, maxSize);
        }

        private int GetMaxSizeByType(string type)
        {
            int size = 4;
            switch (type)
            {
                case "1":
                    size = 4;
                    break;
                case "2":
                    size = 1;
                    break;
                case "3":
                    size = 2;
                    break;
                case "4":
                    size = 3;
                    break;
            }

            return size;
        }
        #endregion
        #region CURD



        [AbpAuthorize(PermissionNames.PagesProductInfoProductPropertyCreate)]
        public override async Task Create(ProductPropertyCreateDto input)
        {
            var preEntity = (await Repository.GetAllListAsync(i => i.PropertyType == input.PropertyType))
                .OrderByDescending(i => i.CreationTime).FirstOrDefault();
            input.PropertyNo = GetNextNum(preEntity?.PropertyNo, GetMaxSizeByType(preEntity?.PropertyType));

            await CreateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesProductInfoProductPropertyUpdate)]
        public override async Task Update(ProductPropertyUpdateDto input)
        {
            await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesProductInfoProductPropertyDelete)]
        public override Task Delete(EntityDto<int> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInfoProductPropertyQuery)]
        public override async Task<PagedResultDto<ProductPropertyDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            query = ApplyFilter(query, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ProductPropertyDto>(totalCount, entities.Select(MapToEntityDto).ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInfoProductPropertyQuery)]
        public override async Task<ProductPropertyDto> GetDto(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesProductInfoProductPropertyQuery)]
        public override async Task<ProductPropertyDto> GetDtoById(int id)
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
        //[AbpAuthorize(PermissionNames.PagesMgProductPropertyMgQuery)]
        public override async Task<ProductPropertyDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesProductInfoProductPropertyQuery)]
        public override async Task<ProductProperty> GetEntity(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesProductInfoProductPropertyQuery)]
        public override async Task<ProductProperty> GetEntityById(int id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesProductInfoProductPropertyQuery)]
        public override async Task<ProductProperty> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{ProductProperty}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<ProductProperty> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<ProductProperty>();
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
        //        var exp = objList.GetExp<ProductProperty>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<ProductProperty> ApplySorting(IQueryable<ProductProperty> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<ProductProperty> ApplyPaging(IQueryable<ProductProperty> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion

    }
}
