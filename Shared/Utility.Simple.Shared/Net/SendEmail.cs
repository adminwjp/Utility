#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
namespace Utility.Net
{
    /// <summary>
    /// 发送 邮件
    /// </summary>
    public class SendEmail
    {
        /// <summary>
        /// 发送 邮件
        /// </summary>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="password"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="bcc"></param>
        /// <returns></returns>
        public static bool Send(string body,bool isBodyHtml,string password,MailAddress from,IList<MailAddress> to,IList<MailAddress> bcc)
        {
            SmtpClient client = new SmtpClient();
            try
            {
                //using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = true;// 指定 System.Net.Mail.SmtpClient 是否使用安全套接字层 (SSL) 加密连接,必须在实例身份前面设置
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(from.Address, password);//设置发件人身份的票据 
                    MailMessage message = new MailMessage();
                    message.From = from;
                    message.Body = body;
                    message.IsBodyHtml = isBodyHtml;
                    foreach (MailAddress send in to)
                    {
                        message.To.Add(send);
                    }
                    if (bcc != null && bcc.Count > 0)
                    {
                        foreach (MailAddress sendCopy in bcc)
                        {
                            message.Bcc.Add(sendCopy);
                        }
                    }
                    client.Send(message);
                    return true;
                }
            }
            finally
            {
                if (client is IDisposable) ((IDisposable)client).Dispose();
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool Send(Email email)
        {
            SmtpClient client = new SmtpClient();
            try
            {
                //using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = true;// 指定 System.Net.Mail.SmtpClient 是否使用安全套接字层 (SSL) 加密连接,必须在实例身份前面设置
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(email.From.Address, email.Password);//设置发件人身份的票据 
                    MailMessage message = new MailMessage();
                    var from = new MailAddress(email.From.Address, email.From.DisplayName, email.From.DisplayNameEncoding);
                    message.From = from;
                    message.Body = email.Body;
                    message.IsBodyHtml = email.IsBodyHtml;
                    foreach (var send in email.To)
                    {
                        var to = new MailAddress(send.Address, send.DisplayName, send.DisplayNameEncoding);
                        message.To.Add(to);
                    }
                    if (email.Bcc != null && email.Bcc.Count > 0)
                    {
                        foreach (var sendCopy in email.Bcc)
                        {
                            var bcc = new MailAddress(sendCopy.Address, sendCopy.DisplayName, sendCopy.DisplayNameEncoding);
                            message.Bcc.Add(bcc);
                        }
                    }
                    client.Send(message);
                    return true;
                }
            }
            finally
            {
                if (client is IDisposable) ((IDisposable)client).Dispose();
            }
           
        }
        /// <summary>
        /// 发送 邮件
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public static bool Send(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            try
            {
                //using (SmtpClient client = new SmtpClient())
                {
                    MailMessage message = new MailMessage();
                    client.Send(mail);
                    return true;
                }
            }
            finally
            {
                if (client is IDisposable) ((IDisposable)client).Dispose();
            }
        }
       
    }

    /// <summary>
    /// 邮箱 信息
    /// </summary>
    public class Email
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// body 是否 html
        /// </summary>
        public bool IsBodyHtml { get; set; }
        
        /// <summary>
        /// body 内容
        /// </summary>
        public string Body { get; set; } = "测试邮件，请勿回复!";
        
        /// <summary>
        /// 发送人 发送给谁
        /// </summary>
        public IList<EmailMessage> To { get; set; }
        
        /// <summary>
        /// 抄送人 抄送给谁
        /// </summary>
        public IList<EmailMessage> Bcc { get; set; }
        
        /// <summary>
        /// 发送人 谁发送
        /// </summary>
        public EmailMessage From { get; set; }
    }

    /// <summary>
    /// 邮件信息
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// 显示名称编码
        /// </summary>
        public Encoding DisplayNameEncoding { get; set; }


    }
}
#endif