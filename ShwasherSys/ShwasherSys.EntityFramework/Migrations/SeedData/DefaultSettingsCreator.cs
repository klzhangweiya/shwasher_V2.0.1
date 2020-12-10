using System;
using System.Linq;
using Abp.Localization;
using Abp.Net.Mail;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.EntityFramework;
using IwbZero.Setting;

namespace ShwasherSys.Migrations.SeedData
{
    public class DefaultSettingsCreator
    {
        private readonly ShwasherDbContext _context;

        public DefaultSettingsCreator(ShwasherDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
           
            //Languages
            AddSettingIfNotExists("默认语言", LocalizationSettingNames.DefaultLanguage, "zh-Hans");
            AddSettingIfNotExists("平台名称", SettingNames.AdminSystemName, "上海上垫垫圈管理系统", "平台名称", 1);
            AddSettingIfNotExists("平台版本", SettingNames.AdminSystemVersion, "1.0.0", "管理平台版本", 1);
            AddSettingIfNotExists("文件路径", SettingNames.DownloadPath, "Download/AttachFiles", "上传文件路径", 1);
            AddSettingIfNotExists("文件类型", SettingNames.UploadFileExt, "jpg,png,pdf,xlsx,xls,doc,docx,zip,rar", "系统允许上传的文件类型", 1);
            AddSettingIfNotExists("用户默认密码", SettingNames.UserDefaultPassword, "111222", "新创建用户初始密码", 1);
            AddSettingIfNotExists("页面底部", SettingNames.HtmlPagesFooter, @"<strong>Copyright © 2019 <a href='#'>Iwb.Net</a>.</strong> All Rights Reserved.", "页面底部内容", 1);
            AddSettingIfNotExists("订单创建通知", ShwasherSettingNames.DINGDANLRMSG, "wangyy", "订单创建后需要通知人员", 1);
            AddSettingIfNotExists("订单审核后发送消息", ShwasherSettingNames.DINGDANSHMSG, "shenjianfang,menghanming,jiangjingeng", "订单审核后需要发送消息的用户", 1);
            AddSettingIfNotExists("订单修改发送消息", ShwasherSettingNames.DINGDANXGMSG, "shenjianfang,wangyy,menghanming,jiangjingeng", "订单修改的时候需要发送消息的用户", 1);
            AddSettingIfNotExists("订单修改金额发送消息", ShwasherSettingNames.DINGDANXGJEMSG, "shenjianfang,wangyy,menghanming,jiangjingeng", "订单修改金额的时候需要发送消息的用户", 1);
            AddSettingIfNotExists("送货单公司地址", ShwasherSettingNames.SENDADDRESS, "地址:上海市金山区朱泾镇(新农)新利路51号", "送货单上显示的公司地址", 1);
            AddSettingIfNotExists("送货单大标题", ShwasherSettingNames.SENDBILLTITLE, "送货单", "送货单上显示的大标题", 1);
            AddSettingIfNotExists("送货单电话", ShwasherSettingNames.SENDTELEPHONE, @"电话:57317386, &nbsp;&nbsp;&nbsp; 57338737 &nbsp;&nbsp;&nbsp;&nbsp; 传真:57321201&nbsp;&nbsp;&nbsp;&nbsp; 仓库电话:57340420", "送货单上显示的电话传真号码", 1);
            AddSettingIfNotExists("送货单公司名称", ShwasherSettingNames.SysUserCop, "上海上垫垫圈有限公司", "", 1);
            AddSettingIfNotExists("可查看订单价格的角色", ShwasherSettingNames.CanShowOrderItemPrice, "11,15", "可查看订单价格的角色", 1);
            AddSettingIfNotExists("订单明细价格税率", ShwasherSettingNames.OrderItemPriceTaxRate, "13", "订单明细价格税率(百分比)", 1);
            //AddSettingIfNotExists("发送邮件地址", Abp.Net.Mail.EmailSettingNames.DefaultFromAddress, "zhangwy@iwbnet.com", "发送邮件地址");
            //AddSettingIfNotExists("发送邮件地址显示名称", Abp.Net.Mail.EmailSettingNames.DefaultFromDisplayName, "zhangwy@iwbnet.com", "发送邮件地址显示名称");
            //AddSettingIfNotExists("Smtp服务域名", Abp.Net.Mail.EmailSettingNames.Smtp.Host, "smtp.exmail.qq.com", "Smtp服务域名");
            //AddSettingIfNotExists("SMTP服务端口", Abp.Net.Mail.EmailSettingNames.Smtp.Port, "465", "SMTP服务端口");
            //AddSettingIfNotExists("用户名称", Abp.Net.Mail.EmailSettingNames.Smtp.UserName, "zhangwy@iwbnet.com", "邮件验证用户名称");
            //AddSettingIfNotExists("发送邮件密码", Abp.Net.Mail.EmailSettingNames.Smtp.Password, "Abc1234%", "发送邮件密码");
            //AddSettingIfNotExists("smtp域名", Abp.Net.Mail.EmailSettingNames.Smtp.Domain, "zhangwy@iwbnet.com", " Domain for the username, if the SMTP server requires authentication");
            //AddSettingIfNotExists("SMTP server开启SSL", Abp.Net.Mail.EmailSettingNames.Smtp.EnableSsl, "false", " SMTP server uses SSL");
            //AddSettingIfNotExists("是否使用默认凭据", Abp.Net.Mail.EmailSettingNames.Smtp.UseDefaultCredentials, "false", "如果为true，则使用默认凭据代替提供的用户名和密码（“ true”或“ false”。默认值：“ true”");

            AddSettingIfNotExists("发送邮件地址", EmailSettingNames.DefaultFromAddress, "zhangwy@iwbnet.com", "发送邮件地址", 1);
            AddSettingIfNotExists("发送邮件地址显示名称", EmailSettingNames.DefaultFromDisplayName, "zhangwy@iwbnet.com", "发送邮件地址显示名称", 1);
            AddSettingIfNotExists("邮件服务器Smtp端口", EmailSettingNames.Smtp.Port, "465", "Smtp端口", 1);
            AddSettingIfNotExists("SmtpUserHost", EmailSettingNames.Smtp.Host, "smtp.exmail.qq.com", "邮件服务器SmtpUserHost", 1);
            AddSettingIfNotExists("邮件服务器SmtpUserName", EmailSettingNames.Smtp.UserName, "zhangwy@iwbnet.com", "邮件服务器SmtpUserName", 1);
            AddSettingIfNotExists("邮件服务器SmtpPwd", EmailSettingNames.Smtp.Password, "Abc1234%", "邮件服务器SmtpPwd", 1);
            AddSettingIfNotExists("邮件服务器SmtpDomain", EmailSettingNames.Smtp.Domain, "", "邮件服务器SmtpDomain", 1);
            AddSettingIfNotExists("邮件服务器SmtpEnableSsl", EmailSettingNames.Smtp.EnableSsl, "true", "邮件服务器SmtpEnableSsl", 1);
            AddSettingIfNotExists("邮件服务器SmtpUseDefaultCredentials", EmailSettingNames.Smtp.UseDefaultCredentials, "false", "邮件服务器SmtpUseDefaultCredentials", 1);

            AddSettingIfNotExists("订单金额修改发送部门消息", ShwasherSettingNames.DINGDANJEXGTOD, "销售部,财务部", "订单金额修改发送部门消息", 1);
            AddSettingIfNotExists("订单数量修改发送消息到部门", ShwasherSettingNames.DINGDANSLXGTOD, "销售部,财务部,生产部,成品仓库,半成品包装部", "订单数量修改发送消息到部门", 1);
            AddSettingIfNotExists("订单修改发送消息到部门", ShwasherSettingNames.DINGDANXGTOD, "销售部,财务部,生产部", "订单修改发送消息到对应通知的部门人员", 1);
            AddSettingIfNotExists("仓库包装人员信息", ShwasherSettingNames.CKBZRY, "1133,1015,1131,1095,1050,1065", "仓库包装人员信息", 1);

        }

        private void AddSettingIfNotExists(string name, string code, string value, string desc = "", int? type = null)
        {
            if (_context.Settings.Any(s => s.Code == code))
                return;
            _context.Settings.Add(new SysSetting()
            {
                SettingNo = Guid.NewGuid().ToString("N"),
                SettingName = name,
                Code = code,
                Value = value,
                SettingType = type ?? 0,
                Description = desc
            });
            _context.SaveChanges();
        }
    }
}