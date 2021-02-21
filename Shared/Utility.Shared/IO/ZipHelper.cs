#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if  NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using ICSharpCode.SharpZipLib.Zip;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.IO;

namespace Utility.IO
{
    ///<sumary>zip文件帮助类</sumary>
    public class ZipHelper
    {

        /// <summary>  功能：解压zip格式的文件。  </summary>  
        /// <param name="zipFilePath">压缩文件路径</param>  
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>  
        /// <returns>解压是否成功</returns>  
        public static bool UnZip(string zipFilePath, string unZipDir)
        {
            try
            {
                if (zipFilePath == string.Empty)
                {
                    return false;
                }
                if (!File.Exists(zipFilePath))
                {
                    return false;
                }
                //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
                if (unZipDir == string.Empty)
                    unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
                if (!unZipDir.EndsWith("/"))
                    unZipDir += "/";
                if (!Directory.Exists(unZipDir))
                    Directory.CreateDirectory(unZipDir);
                using (var s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (!string.IsNullOrEmpty(directoryName))
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (directoryName != null && !directoryName.EndsWith("/"))
                        {
                        }
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                            {
                                int size;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>压缩所有的文件 </summary>
        /// <param name="filesPath"></param>
        /// <param name="zipFilePath"></param>
        public static void CreateZipFile(string filesPath, string zipFilePath)
        {
            if (!Directory.Exists(filesPath))
            {
                return;
            }
            ZipOutputStream stream = new ZipOutputStream(File.Create(zipFilePath));
            stream.SetLevel(9); // 压缩级别 0-9
            byte[] buffer = new byte[4096]; //缓冲区大小
            string[] filenames = Directory.GetFiles(filesPath, "*.*", SearchOption.AllDirectories);
            foreach (string file in filenames)
            {
                ZipEntry entry = new ZipEntry(file.Replace(filesPath, ""));
                entry.DateTime = DateTime.Now;
                stream.PutNextEntry(entry);
                using (FileStream fs = File.OpenRead(file))
                {
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        stream.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }
            }
            stream.Finish();
            stream.Close();
        }

        /// <summary>
        /// 解压 rar 文件
        /// </summary>
        /// <param name="srcUrl"></param>
        /// <param name="targetUrl"></param>
        public static void UnRAR(string srcUrl, string targetUrl)
        {
            using (Stream stream = File.OpenRead(srcUrl))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                       ExtractionOptions option = new ExtractionOptions() { Overwrite = true, ExtractFullPath = true };
                        reader.WriteEntryToDirectory(targetUrl, option);
                    }
                }
            }
        }

        /// <summary> 压缩文件夹 不支持 出错 </summary>
        /// <param name="targetFile">压缩文件夹路径</param>
        /// <param name="zipFile">压缩后路径</param>
        public static void Zips(string targetFile, string zipFile)
        {
            
            using (var archive = ZipArchive.Create())
            {
                ZipRecursion(targetFile, archive);
                FileStream fs_scratchPath = new FileStream(zipFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                byte[] bytes = new byte[fs_scratchPath.Length];
                fs_scratchPath.Read(bytes, 0, bytes.Length);
                fs_scratchPath.Close();
                MemoryStream ms = new MemoryStream(bytes);
                archive.SaveTo(ms, CompressionType.GZip);
                fs_scratchPath.Close();
                fs_scratchPath.Dispose();
            }
        }

        /// <summary> 压缩递归</summary>
        /// <param name="fullName">压缩文件夹目录</param>
        /// <param name="archive">压缩实体</param>
        public static void ZipRecursion(string fullName, ZipArchive archive)
        {
            DirectoryInfo di = new DirectoryInfo(fullName);//获取需要压缩的文件夹信息
            foreach (var fi in di.GetDirectories())
            {
                if (Directory.Exists(fi.FullName))
                {
                    ZipRecursion(fi.FullName, archive);
                }
            }
            foreach (var f in di.GetFiles())
            {
                archive.AddEntry(f.FullName, f.OpenRead());//添加文件夹中的文件
            }
        }

    }
}
#endif