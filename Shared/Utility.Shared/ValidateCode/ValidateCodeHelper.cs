#if !( NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 ||  NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Utility.Randoms;

namespace Utility.ValidateCode
{
    /// <summary>
    /// validate code helper
    /// </summary>
    public class ValidateCodeHelper
    {
        /// <summary> create   validate code</summary>
        /// <param name="length">validate code length</param>
        /// <returns></returns>
        public static string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            int beginSeek = (int)RandomHelper.Random.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = RandomHelper.Random.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                int numPosition = RandomHelper.Random.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }

        /// <summary> 创建验证码的图片 </summary>
        /// <param name="validateCode">验证码</param>
        public static Bitmap CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = RandomHelper.Random.Next(image.Width);
                    int x2 = RandomHelper.Random.Next(image.Width);
                    int y1 = RandomHelper.Random.Next(image.Height);
                    int y2 = RandomHelper.Random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("monaco", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = RandomHelper.Random.Next(image.Width);
                    int y = RandomHelper.Random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(RandomHelper.Random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                return image;
            }
            finally
            {
                g.Dispose();
               // image.Dispose();
            }
        }
    }
}
#endif