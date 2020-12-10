using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys
{
     public class EmailSendHelper
    {
        public static int Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailPort"]??"0");
        public static string Host = System.Configuration.ConfigurationManager.AppSettings["EmailHost"];
        public static string UserName = System.Configuration.ConfigurationManager.AppSettings["EmailUserName"];
        public static string Password = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];
        public static string Domain = System.Configuration.ConfigurationManager.AppSettings["EmailDomain"];
        public static bool EnableSsl = System.Configuration.ConfigurationManager.AppSettings["EmailEnableSsl"] =="true";
        public static bool UseDefaultCredentials = System.Configuration.ConfigurationManager.AppSettings["EmailUseDefaultCredentials"] =="true";
        public  static SmtpEmailSenderConfig _config = new SmtpEmailSenderConfig(Host,Port, UserName, Password, Domain, EnableSsl, UseDefaultCredentials);
        /// <summary>
        /// 发送邮件（有问题此方法只能163的邮箱发送成功）
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="from"></param>
        /// <param name="displayName"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="mailPriority"></param>
        /// <param name="smtpEmailSenderConfig"></param>
        public static void SendEmail(string to,string subject,string body, string from="", string displayName="", bool isBodyHtml=false, MailPriority mailPriority = MailPriority.Normal, SmtpEmailSenderConfig smtpEmailSenderConfig=null)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(to);
            if (smtpEmailSenderConfig == null)
            {
                smtpEmailSenderConfig = _config;
            }
            from = string.IsNullOrEmpty(from) ? smtpEmailSenderConfig.UserName : from;
            displayName = string.IsNullOrEmpty(displayName) ? smtpEmailSenderConfig.UserName : from;
            msg.From = new MailAddress(from, displayName);
           
            msg.Subject = subject;
            msg.SubjectEncoding = Encoding.UTF8;
            msg.Body = body;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = isBodyHtml;
            msg.Priority = mailPriority;

            SmtpClient client = new SmtpClient
            {
                UseDefaultCredentials = smtpEmailSenderConfig.UseDefaultCredentials,
                Credentials = new NetworkCredential(smtpEmailSenderConfig.UserName, smtpEmailSenderConfig.Password),
                Port = smtpEmailSenderConfig.Port,
                Host = smtpEmailSenderConfig.Host,
                EnableSsl = smtpEmailSenderConfig.EnableSsl,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            //senderAuthCode不是qq邮箱登录密码，需要在qq邮箱，设置>账户>smtp...，生成授权码
            //QqMail:587(注意：465是无效的)
            // "smtp.qq.com";
            //经过ssl加密
            //client.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                typeof(EmailHelper).LogError(ex);
            }
           
        }




    }


     public class SmtpEmailSenderConfig
    {
        public SmtpEmailSenderConfig()
        {
            
        }
        public SmtpEmailSenderConfig(string host,int port,string userName,string password,string domain,bool enableSsl,bool useDefaultCredentials)
        {
            Host = host;
            Port = port;
            UserName = userName;
            Password = password;
            Domain = domain;
            EnableSsl = enableSsl;
            UseDefaultCredentials = useDefaultCredentials;
        }
        /// <summary>
        /// SMTP Host name/IP.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// SMTP Port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// User name to login to SMTP server.
        /// </summary>
       public string UserName { get; set; }

        /// <summary>
        /// Password to login to SMTP server.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Domain name to login to SMTP server.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Is SSL enabled?
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Use default credentials?
        /// </summary>
        public bool UseDefaultCredentials { get; set; }
    }
}
