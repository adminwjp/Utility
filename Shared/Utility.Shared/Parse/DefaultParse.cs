#if !(NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4)
namespace Utility.Parse
{
    using HtmlAgilityPack;
    using System.Collections.Generic;
#if !(NET45 || NET451 || NET452 || NET46)
    //using DotnetSpider.HtmlAgilityPack.Css;

    using HtmlAgilityPack.CssSelectors.NetCore;
 #else

#endif
    /// <summary>
    /// 默认 使用  HtmlDocument 解析
    /// </summary>
    public class DefaultParse : HtmlDocument
    {
        /// <summary>
        /// 
        /// </summary>
        public object entity;
        /// <summary>
        /// 
        /// </summary>
        public DefaultParse()
        {
      
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlNode GetRootNode(string html)
        {
            DefaultParse defaultParse = new DefaultParse();
            defaultParse.LoadHtml(html);
            var rootNode = defaultParse.DocumentNode;
            return rootNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static DefaultParse GetDefaultParse(string html)
        {
            DefaultParse defaultParse = new DefaultParse();
            defaultParse.LoadHtml(html);
            return defaultParse;
        }
        /// <summary>
        /// 获取节点
        /// </summary>
        private HtmlNode _htmlNode => base.DocumentNode;
        /// <summary>
        /// 根据规则解析集合
        /// </summary>
        /// <param name="xpath">规则</param>
        /// <returns></returns>
        public virtual IEnumerable<HtmlNode> QuerySelectorAll(string xpath) => 
#if (NET45 || NET451 || NET452 || NET46)
        this.QuerySelectorAll(xpath);
#else
        _htmlNode == null ? null : _htmlNode.QuerySelectorAll(xpath);
#endif

        /// <summary>
        /// 根据规则解析
        /// </summary>
        /// <param name="xpath">规则</param>
        /// <returns></returns>
        public virtual HtmlNode QuerySelector(string xpath) => 
#if (NET45 || NET451 || NET452 || NET46)
        this.QuerySelector(xpath);
#else
        _htmlNode == null ? null : _htmlNode.QuerySelector(xpath);
#endif
        /// <summary>
        /// 根据规则解析
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="xpath">规则</param>
        /// <returns></returns>
        public virtual string QueryInnerText(HtmlNode node, string xpath) =>
#if (NET45 || NET451 || NET452 || NET46)
       node == null ? string.Empty : (node = this.QuerySelector(xpath)) == null ? string.Empty : node.InnerText;
#else
        node == null ? string.Empty : (node = node.QuerySelector(xpath)) == null ? string.Empty : node.InnerText;
#endif

        /// <summary>
        /// 根据规则解析
        /// </summary>
        /// <param name="xpath">规则</param>
        /// <returns></returns>
        public virtual IList<HtmlNode> SelectNodes(string xpath) => _htmlNode == null ? null : _htmlNode.SelectNodes(xpath);
        /// <summary>
        /// 根据规则解析
        /// </summary>
        /// <param name="xpath">规则</param>
        /// <returns></returns>
        public virtual HtmlNode SelectSingleNode(string xpath) => _htmlNode == null ? null : _htmlNode.SelectSingleNode(xpath);
    }
}
#endif
