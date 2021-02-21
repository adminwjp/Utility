using System;

namespace Utility.Randoms
{
    /// <summary> 随机数 公共类 </summary>
    public class RandomHelper
    {
        /// <summary> 数字加小写字母</summary>
        const string NUMBER_LOWER_LETTER="1234567890abcdefghijklmnopqrstuvwxyz";
        /// <summary> 数字加大写字母 </summary>
        const string NUMBER_TOUPPER_LETTER="1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>小写字母加大写字母 </summary>
        const string LOWER_TOUPPER_LETTER="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>数字加小写字母再加大写字母 </summary>
        const string NUMBER_LOWER_TOUPPER_LETTER="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary> 小写字母 </summary>
        const string LOWER_LETTER="abcdefghijklmnopqrstuvwxyz";
        /// <summary>大写字母</summary>
        const string TOUPPER_LETTER="ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>数字 </summary>
        const string NUMBER="1234567890";

        #region 随机数公共类

        /// <summary> 随机数 </summary>
        public  static readonly Random Random = new Random();

        /// <summary> 获取随机数 </summary>
        /// <returns></returns>
        public static double RandomNonice { get { return Random.NextDouble(); } }

        /// <summary> 产生随机标识 (Guid) </summary>
        /// <returns></returns>
        public static string Id => Guid.NewGuid().ToString("N");

        /// <summary>产生随机标识 (Guid) +产生随机数,0-9数字 4位</summary>
        /// <returns></returns>
        public static string RandomGuid => $"{Guid.NewGuid().ToString("N") }{RandomInt(4)}".ToLower();

        /// <summary> 产生随机数 </summary>
        /// <param name="num">随机个数</param>
        /// <returns>0-9数字和26个字母的随机数</returns>
        public static string RandomStr(int num) => RandomStr(NUMBER_LOWER_LETTER, num);

        /// <summary> 产生随机数,0-9数字</summary>
        /// <param name="num">随机个数</param>
        /// <returns>0-9个数字的随机数</returns>
        public static string RandomInt(int num) => RandomStr(NUMBER, num);

        /// <summary>产生随机数,26个字母 </summary>
        /// <param name="num">随机个数</param>
        /// <returns>26个字母的随机数</returns>
        public static string RandomString(int num) => RandomStr(LOWER_LETTER, num);

        /// <summary>生成随机数 </summary>
        /// <param name="num"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string RandomInStrLower(int num, RandomType type)
        {
            switch (type)
            {
                case RandomType.RANDOM_NUM: return RandomStr(NUMBER, num);//数字
                case RandomType.RANDOM_UPPERCASE: return RandomStr(TOUPPER_LETTER, num);//大写
                case RandomType.RANDOM_LOWERCASE://小写
                    return RandomStr(LOWER_LETTER, num);
                case RandomType.RANDOM_NUM_UPPERCASE://数字+大写
                    return RandomStr(NUMBER_TOUPPER_LETTER, num);
                case RandomType.RANDOM_NUM_LOWERCASE://数字+小写
                    return RandomStr(NUMBER_LOWER_LETTER, num);
                case RandomType.RANDOM_UPPERCASE_LOWERCASE://小写+大写
                    return RandomStr(LOWER_TOUPPER_LETTER, num);
                default:
                    return RandomStr(NUMBER_LOWER_TOUPPER_LETTER, num);
            }
        }

        /// <summary>
        /// 时间 前缀 + 随机数 生成 订单
        /// </summary>
        public static string OrderId=> $"{DateTime.Now.ToString("yyyyMMddhhmmssfff")}{Random.Next(int.MaxValue - 1147483647, int.MaxValue)}";

        /// <summary> 根据当前字符串产生随机字符串 </summary>
        /// <param name="str">需要产生字符串原始数据</param>
        /// <param name="num">产生的个数</param>
        /// <returns></returns>
        public static string RandomStr(string str, int num)
        {
            if (string.IsNullOrEmpty(str) && str.Length == 0&&num<1)
                return string.Empty;
            char[] chars=new char[num];
            for (int i = 0; i < num; i++)
            {
                chars[i]=str[Random.Next(0, str.Length)];
            }
            return new string(chars);
        }
        #endregion 随机数公共类
    }

    /// <summary> 随机数类型 </summary>
    public enum RandomType : int
    {
        /// <summary>
        /// 数字
        /// </summary>
        RANDOM_NUM,

        /// <summary>
        /// 大写
        /// </summary>
        RANDOM_UPPERCASE,

        /// <summary>
        /// 小写
        /// </summary>
        RANDOM_LOWERCASE,

        /// <summary>
        /// 数字+大写
        /// </summary>
        RANDOM_NUM_UPPERCASE,

        /// <summary>
        /// 数字+小写
        /// </summary>
        RANDOM_NUM_LOWERCASE,

        /// <summary>
        /// 大写+小写
        /// </summary>
        RANDOM_UPPERCASE_LOWERCASE
    }
}

