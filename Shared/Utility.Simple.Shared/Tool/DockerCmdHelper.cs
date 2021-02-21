using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Utility.Utils;
using Utility.Attributes;
using Utility.Regexs;

namespace Tool
{
    /// <summary>
    /// 
    /// </summary>
    public struct DockerConstant
    {
        /// <summary>
        /// docker 启动
        /// </summary>
        public const string StartDocker = "docker start ";
        /// <summary>
        /// docker 停止
        /// </summary>
        public const string StopDocker = "docker stop ";
        /// <summary>
        /// docker 启动  虚拟机
        /// </summary>
        public const string StartDockerMachine = "docker-machine start  ";
        /// <summary>
        /// docker 停止  虚拟机
        /// </summary>
        public const string StopDockerMachine = "docker-machine stop ";
        /// <summary>
        /// 获取所有Docker镜像
        /// </summary>
        public const string DockerImages = "docker images ";
        /// <summary>
        /// 移除Docker镜像
        /// </summary>
        public const string DockerRemoveImage = "docker rmi ";
        /// <summary>
        /// 获取所有Docker容器
        /// </summary>
        public const string DocokerContainer = "docker ps -a ";
        /// <summary>
        /// 获取所有Docker容器ID
        /// </summary>
        public const string DcokerContainerId = "docker ps -a -q ";
        /// <summary>
        /// 搜索镜像
        /// </summary>
        public const string DcokerSearchImage = "docker search ";
        /// <summary>
        /// 拉取镜像
        /// </summary>
        public const string DcokerPullImage = "docker pull ";
        /// <summary>
        /// 移除Docker容器
        /// </summary>
        public const string DockerRemoveContainer = "docker rm ";
    }

    /// <summary>
    /// 
    /// </summary>

    public enum DockerFlag
    {
        /// <summary>
        /// 启动  docker 容器 或 镜像
        /// </summary>
        Start,
        /// <summary>
        /// 停止  docker 容器 或 镜像
        /// </summary>
        Stop,
        /// <summary>
        /// 启动  docker 虚拟机
        /// </summary>
        StartMachine,
        /// <summary>
        /// 停止  docker 虚拟机
        /// </summary>
        StopMachine,
        /// <summary>
        /// 移除 镜像
        /// </summary>
        Rmi,
        /// <summary>
        /// 移除 容器
        /// </summary>
        Rm,
        /// <summary>
        /// 搜索镜像
        /// </summary>
        Search,
        /// <summary>
        /// 拉取镜像
        /// </summary>
        Pull,
        /// <summary>
        /// 执行命令
        /// </summary>
        Run
    }
    /// <summary>
    /// docker 公共类
    /// </summary>
    public class DockerCmdHelper
    {
      

        private static readonly char[] Row =new char[1] { '\n' };
        private static readonly char[] Space = new char[1] { ' ' };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetContainers<T>()where T: ContainerEntity,new()
        {
            var msg = CmdUtils.RunCmd(DockerConstant.DocokerContainer);
            var strs = msg.Split(Row);
            List<T> result = new List<T>();
            for (int i = 0; i < strs.Length; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                MatchCollection  match = RegexHelper.Matches(strs[i], "(.*?)\\s{2,}");
                if (match.Count == 0)
                {
                    continue;
                }
                T containerEntity = new T();
                containerEntity.ContainerId = match[0].Groups[0].Value.Trim();
                containerEntity.Image = match[1].Groups[0].Value.Trim();
                containerEntity.Command = match[2].Groups[0].Value.Trim();
                containerEntity.Created = match[3].Groups[0].Value.Trim();
                containerEntity.Status = match[4].Groups[0].Value.Trim();
                containerEntity.Ports = match.Count==6? match[5].Groups[0].Value.Trim():string.Empty;
                //yield return containerEntity;
                result.Add(containerEntity);
            }
            return result;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string Run(string cmd,DockerFlag flag)
        {
            var msg = CmdUtils.RunCmd(GetCmd(cmd,flag));
            return msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmds"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static List<string> Run(string[] cmds, DockerFlag flag)
        {
            List<string> msgs = new List<string>(cmds.Length);
            foreach (var item in cmds)
            {
                msgs.Add(GetCmd(item, flag));
            }
            var msg = CmdUtils.RunCmd(msgs.ToArray());
            return msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string GetCmd(string cmd, DockerFlag flag)
        {
            string msg = string.Empty;
            switch (flag)
            {
                case DockerFlag.Start:
                    msg = $"{DockerConstant.StartDocker} {cmd}";
                    break;
                case DockerFlag.Stop:
                    msg = $"{DockerConstant.StopDocker} {cmd}";
                    break;
                case DockerFlag.Rmi:
                    msg = $"{DockerConstant.DockerRemoveImage} {cmd}";
                    break;
                case DockerFlag.Search:
                    msg = $"{DockerConstant.DcokerSearchImage} {cmd}";
                    break;
                case DockerFlag.Pull:
                    msg = $"{DockerConstant.DcokerPullImage} {cmd}";
                    break;
                case DockerFlag.Run:
                    msg = cmd;
                    break;
                case DockerFlag.StartMachine:
                    msg = DockerConstant.StartDockerMachine;
                    break;
                case DockerFlag.StopMachine:
                    msg = DockerConstant.StopDockerMachine;
                    break;
                case DockerFlag.Rm:
                default:
                    msg = $"{DockerConstant.DockerRemoveContainer} {cmd}";
                    break;
            }
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            Console.WriteLine(msg);
#endif
            return msg;
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<ImageEntity> GetImages()
        {
            var msg = CmdUtils.RunCmd(DockerConstant.DockerImages);
            var strs = msg.Split(Row);
            List<ImageEntity> result = new List<ImageEntity>();
            for (int i = 0; i < strs.Length; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                MatchCollection match = RegexHelper.Matches(strs[i], "(.*?)\\s{2,}");
                if (match.Count == 0)
                {
                    continue;
                }
                ImageEntity imageEntity = new ImageEntity();
                imageEntity.Repository = match[0].Groups[0].Value.Trim();
                imageEntity.Tag = match[1].Groups[0].Value.Trim();
                imageEntity.ImageID = match[2].Groups[0].Value.Trim();
                imageEntity.Created = match[3].Groups[0].Value.Trim();
                //yield return imageEntity;
                result.Add(imageEntity);
            }
            return result;
        }
       
    }

    /// <summary>
    /// 
    /// </summary>
    public class ContainerEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [Header(Name = "容器ID")]
        public string ContainerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Header(Name = "镜像")]
        public string Image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Header(Name = "命令")]
        public string Command { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Header(Name = "创建时间")]
        public string Created { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Header(Name = "状态")]
        public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Header(Name = "端口")]
        public string Ports { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ImageEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [Header(Name = "容器名称")]
        public string Repository { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Header(Name = "标签")]
        public string Tag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Header(Name = "镜像ID")]
        public string ImageID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Header(Name = "创建时间")]
        public string Created { get; set; }
    }
}
