using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace Tool
{
    /// <summary>
    /// 
    /// </summary>
    public class DockerHelper
    {
        /// <summary>
        /// 有时会卡 bug
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="ids"></param>
        /// <param name="container"></param>
        public static void RunCmd(DockerFlag flag, List<string> ids, bool container = true)
        {
            var tip = "容器ID或镜像ID不能为空,无法启动!";
            if (ids.Count == 0)
            {
                switch (flag)
                {

                    case DockerFlag.Start:
                        break;
                    case DockerFlag.Stop:
                        tip = "容器ID或镜像ID不能为空,无法停止!";
                        break;
                    case DockerFlag.Rmi:
                        tip = "镜像ID不能为空,无法删除!";
                        break;
                    case DockerFlag.Rm:
                    default:
                        tip = "容器ID不能为空,无法删除!";
                        break;
                }
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
                Console.WriteLine(tip);
#endif
                return;
            }
            var msg = DockerCmdHelper.Run(ids.ToArray(), flag);
            var error = string.Empty;
            for (int i = 0; i < ids.Count; i++)
            {
                if (!msg[i].Replace("\n", "").Equals(ids[i]))
                {
                    error += ids[i] + ",";
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
                    Console.WriteLine(ids[i]);
#endif
                }
            }
            var failId = error.TrimEnd(new char[] { ',' });
            tip = error == string.Empty ? "容器或镜像启动成功!" : $"容器或镜像ID {failId}启动失败!";
            switch (flag)
            {
                case DockerFlag.Start:
                    break;
                case DockerFlag.Stop:
                    tip = error == string.Empty ? (container ? "容器停止成功!" : "镜像停止成功!") : container ? $"容器ID {failId}停止失败!" : $"镜像ID {failId}停止失败!";
                    break;
                case DockerFlag.Rmi:
                    tip = error == string.Empty ? "镜像移除成功!" : $"镜像ID {failId}移除失败!";
                    break;
                case DockerFlag.Rm:
                default:
                    tip = error == string.Empty ? "容器移除成功!" : $"容器ID {failId}移除失败!";
                    break;
            }
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
        Console.WriteLine(tip);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StartDocker()
        {
            var msg = DockerCmdHelper.Run(string.Empty, DockerFlag.StartMachine);
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            if (string.IsNullOrEmpty(msg))
            {
                Console.WriteLine("Docker启动失败!");
            }
            else
            {
                Console.WriteLine($"Docker启动成功! {msg}");
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StopDocker()
        {
            var msg = DockerCmdHelper.Run(string.Empty, DockerFlag.StopMachine);
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            if (string.IsNullOrEmpty(msg))
            {
                Console.WriteLine($"Docker停止失败!");
            }
            else
            {
                Console.WriteLine($"Docker停止成功! {msg}");
            }
#endif
        }
    }
}
