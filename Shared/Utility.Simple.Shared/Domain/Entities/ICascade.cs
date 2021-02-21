using System;
using System.Collections.Generic;
#if !(NET20 || NET30 || NET10 || NET11)
using System.Linq;
#endif

namespace Utility.Domain.Entities
{
    /// <summary>
    /// 树形 接口
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public interface ICascade<Entity,Key> where Entity : ICascade<Entity,Key>
    {
        /// <summary>
        /// 排序的位置
        /// </summary>
        int OrderNo { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        Key Id { get; set; }
        /// <summary>
        /// 父键 
        /// </summary>
        Key ParentId { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        ICollection<Entity> Children { get; set; }
    }

    /// <summary>
    /// 统一 使用 ICollection -  List 
    /// </summary>
    public class CascadeHelper
    {

        /// <summary>
        /// 递归去除自引用
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="data"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static List<Entity> DataParseIfWhileReference<Entity, Key>(List<Entity> data, bool where = false) where Entity : class, ICascade<Entity, Key>
        {
            var has = data != null&&data.Count>0;
            if(has)
            //if (data.Any())
            {
#if !(NET20 || NET30 || NET10 || NET11)
                //var ids = data.Select(it => it.Id).ToList();
#endif
                var ids =new  List<Key>();
                foreach (var item in data)
                {
                    ids.Add(item.Id);
                }
                data = GetEntities(data, ids);
                //手动排序，位置不对
                RecursionOrderBy<Entity, Key>(data);
            }
            return data;
        }

        /// <summary>
        /// 递归 排序
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="result"></param>
        public static void RecursionOrderBy<Entity, Key>(List<Entity> result) where Entity : class, ICascade<Entity, Key>
        {
            result.Sort((Entity x, Entity y) => {
                if (x.OrderNo == y.OrderNo) return 0;
                else if (x.OrderNo > y.OrderNo) return 1;
                return -1;
            });
            foreach (var item in result)
            {
                if (item.Children != null && item.Children.Count>0)
                {
                    RecursionOrderBy<Entity,Key>(new List<Entity>(item.Children));
                }
            }
        }

        /// <summary>
        /// 整理树形列表
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="result"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<Entity> GetEntities<Entity, Key>(List<Entity> result, List<Key> ids = null) where Entity : class, ICascade<Entity, Key>
        {
            List<Entity> childs = new List<Entity>();
           // SortedSet<int> indexs = new SortedSet<int>();//存储子集索引,可能是打乱的顺序
            for (int i = 0; i < result.Count; i++)
            {
                var item = result[i];
                //T!=T 语法不支持 equal 引用类型才能比较 1==1 false .所以统一stirng类型比较
                int index = ids.FindIndex(it => it.ToString() == item.ParentId?.ToString());
                if (index > -1)
                {
                    //找到了父集  添加子集
                    result[index].Children = result[index].Children ?? new List<Entity>();
                    result[index].Children.Add(item);
                    continue;
                }
                else
                {
                    //未找到
                    //1.如果是子集, 父集打乱的 暂时找不到怎么办(不可能,因为所以id都拿到了)
                    //throw new Exception("unkow error !");

                }
                childs.Add(item);
            }
            return childs;
        }

    }
}
