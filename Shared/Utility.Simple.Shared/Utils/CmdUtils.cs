using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Utility.Utils
{
    /// <summary>
    /// 命令公共类
    /// </summary>
    public class CmdUtils
    {
        /// <summary>
        /// 启动  服务
        /// </summary>
        public const string StartService = "net start ";
        /// <summary>
        /// 关闭  服务
        /// </summary>
        public const string StopService = "net stop ";

        /// <summary>
        /// 删除 服务
        /// </summary>
        public const string DeleteService = "sc delete ";
        private const string CMD_PATH = @"C:\Windows\System32\cmd.exe";
        /// <summary>
        /// 执行cmd命令
        /// </summary>
        /// <param name="cmd">cmd命令</param>
        /// <param name="cmdpath">cmd执行命令路径</param>
        /// <returns>执行cmd命令结果</returns>
        public static string RunCmd(string cmd, string cmdpath = CMD_PATH)
        {
            return RunCmd(new string[] { cmd }, cmdpath)[0];
        }

        /// <summary>
        /// 执行cmd命令
        /// </summary>
        /// <param name="cmds">cmd命令</param>
        /// <param name="cmdpath">cmd执行命令路径</param>
        /// <returns>执行cmd命令结果</returns>
        public static List<string> RunCmd(string[] cmds, string cmdpath = CMD_PATH)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            using (Process process = new Process())
            {
                process.StartInfo.FileName = cmdpath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Arguments = $"/k {cmds[0]}&exit";
                process.Start();
                List<string> msg = new List<string>();
                // process.StandardInput.WriteLine($"cd c: {cmd}&exit");
                //process.StandardInput.WriteLine($"{cmd}&exit");
                process.StandardInput.AutoFlush = true;
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                msg.Add(output);
                for (int i =1; i < cmds.Length; i++)
                {
                    process.StartInfo.Arguments = $"/k {cmds[i]}&exit";
                    process.Start();
                    output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                    msg.Add(output);
                }
                process.WaitForExit();
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
                process.Close();
#else
                process.Dispose();
#endif

                return msg;
            }
#else
            return null;
#endif
        }
    }
}
