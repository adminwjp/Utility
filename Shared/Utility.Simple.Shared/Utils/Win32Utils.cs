#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

using System;
using System.Runtime.InteropServices;

namespace Utility.Utils
{
    /// <summary>
    /// win32 api
    /// </summary>
    public static class Win32Utils
    {
		/// <summary>
        /// 激活，显示在最前
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fAltTab"></param>
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frequency">声音频率（从37Hz到32767Hz）。在windows95中忽略</param>
        /// <param name="duration">声音的持续时间，以毫秒为单位。</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")] //引入命名空间 using System.Runtime.InteropServices;
        public static extern bool Beep(int frequency, int duration);

        /// <summary>
        /// 
        /// </summary>
        public enum MessageBeepType
        {
            /// <summary>
            /// 
            /// </summary>
            Default = -1,
            /// <summary>
            /// 
            /// </summary>
            Ok = 0x00000000,
            /// <summary>
            /// 
            /// </summary>
            Error = 0x00000010,
            /// <summary>
            /// 
            /// </summary>
            Question = 0x00000020,
            /// <summary>
            /// 
            /// </summary>
            Warning = 0x00000030,
            /// <summary>
            /// 
            /// </summary>
            Information = 0x00000040
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MessageBeep(MessageBeepType type);

        /// <summary>
        /// 闪动窗口
        /// </summary>
        /// <param name="hWnd">要闪动的窗口</param>
        /// <param name="bInvert">闪动</param>
        [DllImport("User32")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);
    }
}

#endif

