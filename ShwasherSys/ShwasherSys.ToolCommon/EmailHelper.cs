using System;
using System.Text;
using System.Threading;
using System.Web.Mail;

namespace ShwasherSys
{
    public class EmailHelper
    {
        public static int Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailPort"]??"0");
        public static string Host = System.Configuration.ConfigurationManager.AppSettings["EmailHost"];
        public static string UserName = System.Configuration.ConfigurationManager.AppSettings["EmailUserName"];
        public static string Password = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];
        public static string Domain = System.Configuration.ConfigurationManager.AppSettings["EmailDomain"];
        public static bool EnableSsl = System.Configuration.ConfigurationManager.AppSettings["EmailEnableSsl"] =="true";
        public static bool UseDefaultCredentials = System.Configuration.ConfigurationManager.AppSettings["EmailUseDefaultCredentials"] =="true";
        public  static SmtpEmailSenderConfig _config = new SmtpEmailSenderConfig(Host,Port, UserName, Password, Domain, EnableSsl, UseDefaultCredentials);
        public static void SendEmail(string to, string title, string body, bool isHtml= false,SmtpEmailSenderConfig config=null)
        {
            config = config ?? _config;
            try
            {
                var mMsg = new MailMessage();
                mMsg.Subject = title.Trim();//邮件主题

                mMsg.BodyFormat =isHtml? MailFormat.Html:MailFormat.Text;
                mMsg.Body = body;// 邮件正文
                mMsg.BodyEncoding = Encoding.UTF8;//正文编码
                mMsg.Priority = MailPriority.Normal;//优先级
 
                mMsg.From = config.UserName;//发件者邮箱地址
                mMsg.To = to;//收件人收箱地址
                mMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                //登陆名  
                mMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", config.UserName);
                //登陆密码  
                mMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", config.Password);
                mMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", config.Port);//端口 
                mMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
                SmtpMail.SmtpServer = config.Host;
                SmtpMail.Send(mMsg);
            }
            catch (Exception ex)
            {
                typeof(EmailHelper).LogError(ex);
            }

        }

    }
}