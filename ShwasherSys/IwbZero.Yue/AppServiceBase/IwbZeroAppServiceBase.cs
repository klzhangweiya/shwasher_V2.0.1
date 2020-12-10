using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Application.Services;
using Abp.Runtime.Caching;
using Abp.UI;
using IwbZero.Helper;
using IwbZero.Session;

namespace IwbZero.AppServiceBase
{

    public abstract class IwbZeroAppServiceBase : ApplicationService
    {
        public ICacheManager CacheManager { get; set; }
        public new IIwbSession AbpSession{ get; set; }

        protected IwbZeroAppServiceBase()
        {

            LocalizationSourceName = IwbZeroConsts.IwbZeroLocalizationSourceName;
        }

        
        protected virtual IQueryable<T> ApplyFilter<T>(IQueryable<T> query, IIwbPagedRequest input)
        {
            if (!string.IsNullOrEmpty(input.KeyWords))
            {
                object keyWords = input.KeyWords;
                LambdaObject obj = new LambdaObject()
                {
                    FieldType = (LambdaFieldType)input.FieldType,
                    FieldName = input.KeyField,
                    FieldValue = keyWords,
                    ExpType = (LambdaExpType)input.ExpType
                };
                var exp = obj.GetExp<T>();
                query = query.Where(exp);
            }
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (string.IsNullOrEmpty(o.KeyWords))
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
                var exp = objList.GetExp<T>();
                query = query.Where(exp);
            }

            return query;
        }



        //protected virtual void CheckErrors(IdentityResult identityResult)
        //{
        //    identityResult.CheckErrors(LocalizationManager);
        //}
        protected virtual void CheckErrors(string error)
        {
            throw new UserFriendlyException(error);
        }
        /// <summary>
        /// 抛出错误
        /// </summary>
        /// <param name="err"></param>
        /// <param name="isLocalization">是否要本地化</param>
        protected virtual void ThrowError(string err, bool isLocalization = true)
        {
            CheckErrors(isLocalization ? L(err) : err);
        }
    }
}
