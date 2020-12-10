using System.Reflection;
using System.Web.Http;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.MailKit;
using Abp.Modules;
using Abp.Quartz;
using Abp.Quartz.Configuration;
using Quartz;
using ShwasherSys.Authorization.Users;
using ShwasherSys.BaseSysInfo.Functions.Dto;
using ShwasherSys.BaseSysInfo.Roles.Dto;
using ShwasherSys.BaseSysInfo.Users.Dto;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.Help.Dto;
using ShwasherSys.Common;
using ShwasherSys.Inspection;
using ShwasherSys.Inspection.Dto;
using ShwasherSys.Invoice;
using ShwasherSys.Invoice.Dto;
using ShwasherSys.Order;
using ShwasherSys.Order.Dto;
using ShwasherSys.OrderSendInfo;
using ShwasherSys.OrderSendInfo.Dto;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ProductionOrderInfo.Dto;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys
{
    [DependsOn(typeof(ShwasherCoreModule), typeof(AbpAutoMapperModule), typeof(AbpQuartzModule), typeof(AbpMailKitModule))]
    public class ShwasherApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
          
        }

        public override void PostInitialize()
        {
            var jobHelper = IocManager.Resolve<JobTaskHelp>();
            jobHelper.StartJob<PreMonthJob>("PreMonthJob", "StoreHouseGroup", "0 37 16 * * ?");
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            // TODO: Is there somewhere else to store these, with the dto classes
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                //cfg.CreateMap<Permission, string>().ConvertUsing(r => r.Name);
                //cfg.CreateMap<RolePermissionSetting, string>().ConvertUsing(r => r.Name);

                cfg.CreateMap<SysUser, UserDto>().ForMember(x => x.RoleNames, o => o.Ignore());
                cfg.CreateMap<SysUser, UserCreateDto>().ForMember(x => x.RoleNames, o => o.Ignore());
                cfg.CreateMap<SysUser, UserUpdateDto>().ForMember(x => x.RoleNames, o => o.Ignore());
                cfg.CreateMap<UserCreateDto, SysUser>().ForMember(x => x.Roles, o => o.Ignore());
                cfg.CreateMap<UserUpdateDto, SysUser>().ForMember(x => x.Roles, o => o.Ignore());
                cfg.CreateMap<Permission, PermissionDto>()
                    .ForMember(r => r.PermDisplayName, o => o.Ignore())
                    .ForMember(r => r.Sort, o => o.Ignore())
                    .ForMember(r => r.IsAuth, o => o.Ignore());

                cfg.CreateMap<SysFunction, FunctionDto>().ForMember(x => x.FunctionTypeName, o => o.Ignore());

                cfg.CreateMap<SysHelp, SysHelpDto>().ForMember(x => x.ClassificationShow, o => o.Ignore());

                cfg.CreateMap<OrderHeader, OrderHeaderDto>().ForMember(x => x.CustomerSendName, o => o.Ignore());
                cfg.CreateMap<OrderHeader, OrderHeaderDto>().ForMember(x => x.SendAdress, o => o.Ignore());
                cfg.CreateMap<OrderSendBill, OrderSendBillCreateDto>().ForMember(x => x.OrderSendIds, o => o.Ignore());
                cfg.CreateMap<OrderStickBill, OrderStickBillCreateDto>().ForMember(x => x.OrderSendIds, o => o.Ignore());
                cfg.CreateMap<OrderStickBill, OrderStickBillDto>().ForMember(x => x.CustomerName, o => o.Ignore());
                cfg.CreateMap<ProductInspectInfo, ProductInspectCreateDto>().ForMember(x => x.ReportContent, o => o.Ignore()).ForMember(x => x.AttachFiles, o => o.Ignore());
                cfg.CreateMap<ProductInspectInfo, ProductInspectUpdateDto>().ForMember(x => x.ReportContent, o => o.Ignore()).ForMember(x => x.AttachFiles, o => o.Ignore());

                cfg.CreateMap<ProductionLog, ProductionLogDto>().ForMember(x => x.EmployeeNo, o => o.MapFrom(a=>a.EmployeeInfo.No));
                cfg.CreateMap<ProductionLog, ProductionLogDto>().ForMember(x => x.EmployeeName, o => o.MapFrom(a=>a.EmployeeInfo.Name));
                cfg.CreateMap<ViewCurrentSemiStoreHouse, CurrentStoreItemDto>().ForMember(x => x.ProductNo, o => o.MapFrom(a => a.SemiProductNo)).ForMember(x=>x.Quantity,o=>o.MapFrom(a=>a.ActualQuantity));

            });
        }
    }
}
