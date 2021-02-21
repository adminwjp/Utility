#if  ! (NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Utility.Logs;

namespace Utility.Net
{
    /// <summary>
    /// ftp 请求 公共类 不支持netstandard 1.0 - 1.6  netcoreapp 1.0 -1.2
    /// </summary>
    public class FtpHelper
    {
        /// <summary>
        /// 内部类
        /// </summary>
        class InnerFtp
        {
            /// <summary> 声明FtpUtils对象并初始化 </summary>
            public readonly static FtpHelper FtpObject = new FtpHelper() { log=new DefaultLog<FtpHelper>() };
        }
        /// <summary> 懒汉式 单例模式 </summary>
        public static FtpHelper Instance
        {
            get
            {
                return InnerFtp.FtpObject;
            }
        }
        private ILog<FtpHelper> log;

        /// <summary>默认 账号 ftpadmin</summary>
        public string UserName { get; set; }// = "ftpadmin";
        /// <summary>默认 密码 wjp930514. </summary>
        public string Password { get; set; }// = "wjp930514.";

        /// <summary> 上传文件  </summary>
        /// <param name="url">上传路径</param>
        /// <param name="file">文件路径</param>
        /// <param name="timeout">超时时间</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public virtual bool UploadFile(string url, string file, int timeout = 5000)
        {

            return UploadFile(url, new FileStream(file, System.IO.FileMode.Open),timeout);
        }

        /// <summary> 上传文件  </summary>
        /// <param name="url">上传路径</param>
        /// <param name="fileStream">文件流</param>
        /// <param name="timeout">超时时间</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public virtual bool UploadFile(string url, Stream fileStream, int timeout = 5000)
        {

            FtpWebRequest ftpWeb = null;
            try
            {
                ftpWeb = (FtpWebRequest)FtpWebRequest.Create(url);
                SetFtpWebRequest(ftpWeb, WebRequestMethods.Ftp.UploadFile,UserName,Password, timeout);
                using (Stream fileStream1 = fileStream)
                {
                    using (Stream stream = ftpWeb.GetRequestStream())
                    {
                        int line = 0;
                        byte[] buffer = new byte[1024];
                        while ((line = fileStream1.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stream.Write(buffer, 0, line);
                        }
                        return true;
                    }
                }
            }
            catch (System.Exception e)
            {
                log.LogException(LogLevel.Error, $"url:{url} upload file error!", e);
                return false;
            }
            finally
            {
                if (ftpWeb != null)
                {
                    ftpWeb.Abort();
                    ftpWeb = null;
                }
            }
        }

        /// <summary> 设置FtpWebRequest 请求属性  </summary>
        /// <param name="ftpWeb">FtpWebRequest</param>
        /// <param name="method">ftp 方法</param>
        /// <param name="password"></param>
        /// <param name="userName"></param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public virtual void SetFtpWebRequest(FtpWebRequest ftpWeb,string method,string userName,string password, int timeout = 5000)
        {
            ftpWeb.Timeout = timeout;
            if(!(string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password)))
            {
                ftpWeb.Credentials = new NetworkCredential(userName, password);
            }
            ftpWeb.Method = method;
            ftpWeb.UseBinary = true;
            ftpWeb.KeepAlive = false;
        }

        /// <summary> 获取ftp 信息  </summary>
        /// <param name="url">路径</param>
        /// <param name="method">请求方法</param>
        /// <param name="timeout">超时时间</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public virtual string GetInfo(string url, string method, int timeout = 5000)
        {
            using (System.IO.Stream stream = GetStream(url, method, timeout))
            {
                if (stream == null)
                {
                    return string.Empty;
                }
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        /// <summary> 获取ftp 信息  </summary>
        /// <param name="url">路径</param>
        /// <param name="method">请求方法</param>
        /// <param name="timeout">超时时间</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public virtual Stream GetStream(string url, string method, int timeout = 5000)
        {
            FtpWebRequest ftpWeb = null;
            try
            {
                ftpWeb = (FtpWebRequest)FtpWebRequest.Create(url);
                SetFtpWebRequest(ftpWeb, method,UserName,Password, timeout);
                using (FtpWebResponse response = (FtpWebResponse)ftpWeb.GetResponse())
                {
                    return response.GetResponseStream();
                }
            }
            catch (System.Exception e)
            {
                log.LogException(LogLevel.Error, $"url:{url} , method : {method}, get file stream error!", e);
                return null;
            }
            finally
            {
                if (ftpWeb != null)
                {
                    ftpWeb.Abort();
                    ftpWeb = null;
                }
            }
        }

        /// <summary>获取ftp 目录  </summary>
        /// <param name="url">路径</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public virtual string ListDirectory(string url, int timeout = 5000)
        {
            return GetInfo(url, WebRequestMethods.Ftp.ListDirectory, timeout);
        }
    }
}
#endif