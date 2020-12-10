using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;

namespace IwbZero
{
    [DependsOn(typeof(AbpEntityFrameworkModule))]
    public class IwbZeroEntityFrameworkModule : AbpModule
    {

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
        }

    }
}
