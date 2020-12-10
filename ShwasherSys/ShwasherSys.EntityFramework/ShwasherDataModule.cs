using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using ShwasherSys.EntityFramework;

namespace ShwasherSys
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(ShwasherCoreModule))]
    public class ShwasherDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<ShwasherDbContext>(null);
        }
    }
}
