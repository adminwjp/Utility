namespace Utility.Page
{
    /// <summary>
    /// 
    /// </summary>
    public class PageHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static int MaxPage = 999;
        /// <summary>
        /// 
        /// </summary>
        public static int MaxSize = 200;
        /// <summary>
        /// 
        /// </summary>
        public static int MinSize = 10;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        public static void Set(ref int? page,ref int? size)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }
            else if (page > MaxPage)
            {
                page = PageHelper.MaxPage;
            }
            if (size == null || size < 1)
            {
                size = PageHelper.MinSize;
            }
            else if (size > MaxSize)
            {
                size = PageHelper.MaxSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        public static void Set(ref int page, ref int size)
        {
            if (page < 1)
            {
                page = 1;
            }
            else if (page > MaxPage)
            {
                page = PageHelper.MaxPage;
            }
            if (size < 1)
            {
                size = PageHelper.MinSize;
            }
            else if (size > MaxSize)
            {
                size = PageHelper.MaxSize;
            }
        }
    }
}
