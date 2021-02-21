#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Utility.Logs;

namespace Utility.Net
{
    /// <summary>
    /// 邮件 帮助类
    /// </summary>
    public class EmailHelper
    {
        private EmailHelper()
        {
            log = new DefaultLog<EmailHelper>();
        }
        private static ILog log;
        /// <summary>
        /// 发送 邮件
        /// </summary>
        /// <param name="email">发送邮件信息</param>
        /// <param name="listTo">发送人</param>
        /// <param name="listCC">抄送人</param>
        /// <param name="priority">邮件优先级</param>
        /// <returns></returns>
        public static bool SendEmail(EmailInfo email, List<string> listTo, List<string> listCC, MailPriority priority = MailPriority.High)
        {
            try
            {
                MailMessage messge = new MailMessage();//发送的电子邮件
                messge.From = new MailAddress("973513569@qq.com", "King", Encoding.Default);//发送的地址
                messge.IsBodyHtml = email.IsBodyHtml;//是否以hmtl格式
                messge.Subject = email.Subject;//主题
                messge.SubjectEncoding = email.SubjectEncod;//主题编码
                listTo.ForEach(item => messge.To.Add(item)); //收件人
                listCC.ForEach(item => messge.CC.Add(item));//抄送人
                messge.Priority = priority;
                SmtpClient clietn = new SmtpClient();
                clietn.Host = email.Host;
                //if (type == SendEmailType.SMTP)
                //{
                //    clietn.Port = email.port;
                //    clietn.Credentials = new NetworkCredential(email.account, email.pwd);
                //}
                //else if (type == SendEmailType.SSLSMTP)
                //{
                //    clietn.Port = email.port;
                //    clietn.Credentials = new NetworkCredential(email.account, email.pwd);
                //    clietn.EnableSsl = email.enableSsl;
                //}
                clietn.Send(messge);
                return true;
            }
            catch (Exception ex)
            {
                log.LogException(LogLevel.Error, "send email error!", ex);
                return false;
            }
        }

        /// <summary>
        /// 发送 邮件
        /// </summary>>
        /// <param name="toEmails">发送人</param>
        /// <param name="bccEmails">抄送人</param>
        /// <param name="formEmail">发送邮件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="host">地址</param>
        /// <param name="encoding">编码</param>
        /// <param name="username">账户</param>
        /// <param name="password">密码</param>
        public static bool SendEamil(List<string> toEmails, List<string> bccEmails, string formEmail, string subject, string body, string host = "smtp.qq.com",
           string encoding = "utf-8", string username = "973513569", string password = "llrqnxuztqkmbffa")
        {
            MailMessage mailMessage = null;
            SmtpClient smtpClient = null;
            try
            {
                MailAddress formMailAddress = new MailAddress(formEmail)
                {

                };
                mailMessage = new MailMessage();
                mailMessage.From = formMailAddress;
                foreach (var item in toEmails)
                {
                    mailMessage.To.Add(item);
                }
                foreach (var item in bccEmails)
                {
                    mailMessage.Bcc.Add(item);
                }
                mailMessage.Body = body;
                mailMessage.Subject = subject;
                mailMessage.BodyEncoding = Encoding.GetEncoding(encoding);
                mailMessage.IsBodyHtml = true;
                smtpClient = new SmtpClient(host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(username, password);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                log.LogException(LogLevel.Error, "send email error!", ex);
                return false;
            }
            finally
            {
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                    mailMessage = null;
                }
                if (smtpClient != null)
                {
                    if(smtpClient is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                    //smtpClient.Dispose();
                    smtpClient = null;
                }
            }
        }
        /// <summary>
        /// 功能：异步发送邮件。Task.Factory.StartNew(() =>{ SendMails()});
        /// </summary>
        /// <param name="MailTo">MailTo为收信人地址</param>
        /// <param name="Subject">Subject为标题</param>
        /// <param name="Body">Body为信件内容</param>
        /// <param name="errorcallback">发送 邮件 失败 事件</param>
        public static void SendMails(string MailTo, string Subject, string Body, Action errorcallback)
        {
            SmtpClient mailclient = new SmtpClient();
            //发送方式
            mailclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp服务器
            mailclient.Host = "smtp.126.com";
            //用户名凭证               
            mailclient.Credentials = new System.Net.NetworkCredential("fnfqp1992", "789632145");
            //邮件信息
            MailMessage message = new MailMessage();
            //发件人
            message.From = new MailAddress("fnfqp1992@126.com");
            //收件人
            message.To.Add(new MailAddress(MailTo, MailTo.ToString(), Encoding.UTF8));
            //邮件标题编码
            message.SubjectEncoding = Encoding.UTF8;
            //主题
            message.Subject = Subject;
            //内容
            message.Body = Body;
            //正文编码
            message.BodyEncoding = Encoding.UTF8;
            //设置为HTML格式
            message.IsBodyHtml = true;
            //优先级
            message.Priority = MailPriority.High;
            //事件注册
            mailclient.SendCompleted += (s1, e1) => { if (e1.Error != null) { errorcallback(); } };
            //异步发送
            mailclient.SendAsync(message, message.To);
        }
    }

    /// <summary>
    ///邮件 信息
    /// </summary>
    public class EmailInfo
    {
        /// <summary>
        /// ssl 默认不启用
        /// </summary>
        public bool EnableSsl { get; set; } = false;
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; } = "测试邮件";
       /// <summary>
       /// 主题编码
       /// </summary>
        public Encoding SubjectEncod { get; set; } = Encoding.UTF8;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; } = "测试邮件，请勿回复!";
        /// <summary>
        /// html 格式 
        /// </summary>
        public bool IsBodyHtml { get; set; } = false;
       /// <summary>
       /// 地址
       /// </summary>
        public string Host { get; set; }
      /// <summary>
      /// 端口
      /// </summary>
        public int Port { get; internal set; }
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; internal set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; internal set; }
    }

}
#endif