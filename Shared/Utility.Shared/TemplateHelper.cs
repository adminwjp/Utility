//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Utility.Crawl
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class TemplateHelper
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public static readonly List<CrawlModel> CrawlModels = new List<CrawlModel>();
//        /// <summary>
//        /// 
//        /// </summary>
//        public static void Init()
//        {
//            CrawlModel crawlModel = new CrawlModel() { Name= "百度",Url= "http://www.baidu.com/" };
//            RequestModel requestModel = new RequestModel() { Url = "http://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=0&rsv_idx=1&tn=baidu&wd={0}" ,Page=true };
//            crawlModel.RequestModels.Add(requestModel);
//            NodeModel nodeModel = new NodeModel() {  CssSelector = "div#page>a",Index=-2, Flag= CssFlag.QuerySelectorAll };
//            requestModel.PageModel = nodeModel;
//            nodeModel = new NodeModel() { CssSelector = "div#content_left>div.c-container", Flag = CssFlag.QuerySelectorAll,While=true };
//            requestModel.NodeModels.Add(nodeModel);
//            nodeModel.Next = new NodeModel();
//        }
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    public class CrawlModel
//    {
//        /// <summary>抓取网站名称 </summary>
//        public string Name { get; set; }
//        /// <summary>抓取网站地址 </summary>
//        public string Url { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public readonly List<RequestModel> RequestModels = new List<RequestModel>();
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    public class RequestModel
//    {
//        /// <summary>请求url </summary>
//        public string Url { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public object[] Formats { get; set; }

//        /// <summary>解析 </summary>
//        public bool Parse { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public ParseFlag Flag { get; set; } = ParseFlag.Html;
//        /// <summary>
//        /// 
//        /// </summary>
//        public bool Page { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public NodeModel PageModel { get; set; }


      
//        /// <summary>
//        /// 
//        /// </summary>
//        public readonly List<NodeModel> NodeModels = new List<NodeModel>();
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    public class NodeModel
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public string CssSelector { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public bool Single { get; set; } = true;
//        /// <summary>
//        /// 
//        /// </summary>
//        public int Index { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public CssFlag Flag { get; set; } = CssFlag.None;
//        /// <summary>
//        /// 
//        /// </summary>
//        public NodeModel Next { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public bool While { get; set; }
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    public enum CssFlag
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        None,
//        /// <summary>
//        /// 
//        /// </summary>
//        QuerySelectorAll,
//        /// <summary>
//        /// 
//        /// </summary>
//        QuerySelector
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    public enum ParseFlag
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        None,

//        /// <summary>
//        /// 
//        /// </summary>
//        Html,
//        /// <summary>
//        /// 
//        /// </summary>
//        Regex
//    }
//}
