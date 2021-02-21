using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Net.Http
{
    /// <summary>
    /// 统一 htpp <see cref="IHttpCollect"/> 接口
    /// </summary>
    public interface IHttpCollect
    {
        /// <summary>
        /// 通用 请求 带cookie
        /// </summary>
        /// <param name="httpEntity"></param>
        /// <returns></returns>
        HttpResult Http(HttpEntity httpEntity);

        /// <summary>
        /// 通用请求方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <param name="param">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="encoding">编码</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        string Http(string url, string param, string referer,  HttpMethod method, string contentType, Encoding encoding);

        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        string GetString(string url, string referer = "");

        /// <summary>
        ///http post form 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <returns></returns>
        string PostString(string url, string param, string referer = "");

        /// <summary>
        /// /http post form 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
       string PostString(string url, Dictionary<string, string> param, string referer = "");

        /// <summary>
        /// post json
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="json">post请求参数</param>
        /// <param name="referer">referer地址</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        string PostJson(string url, string json, string referer = "");

        /// <summary>
        /// /http put json 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="json">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        string PutJson(string url, string json, string referer = "");

        /// <summary>
        /// down
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        byte[] DownFile(string url, string referer = "");
    }
}
