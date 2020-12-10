using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Quartz;
using Quartz;
using ShwasherSys.EntityFramework;
using ShwasherSys.ProductStoreInfo;

namespace ShwasherSys.Common
{
    public class PreMonthJob : JobBase, ITransientDependency
    {
        protected ICommonAppService CommonAppService;

        public PreMonthJob( ICommonAppService commonAppService)
        {
            CommonAppService = commonAppService;
        }
       
        public override  Task Execute(IJobExecutionContext context)
        {
            return CommonAppService.PreMonth();
        }
    }
}
