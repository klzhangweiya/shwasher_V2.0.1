using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace ShwasherSys
{
    public static class CompressHelper
    {
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static bool UnCompress(this string sourcePath, string targetPath)
        {
            try
            {
                if (sourcePath.EndsWith(".zip"))
                {
                    var archive = ArchiveFactory.Open(sourcePath);
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            Console.WriteLine(entry.Key);
                            entry.WriteToDirectory(targetPath, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                        }
                    }
                }
                else if (sourcePath.EndsWith(".rar"))
                {
                    using (Stream stream = File.OpenRead(sourcePath))
                    {
                        var reader = ReaderFactory.Open(stream);
                        while (reader.MoveToNextEntry())
                        {
                            if (!reader.Entry.IsDirectory)
                            {
                                reader.WriteEntryToDirectory(targetPath, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                typeof(CompressHelper).LogError(e);
            }
            return false;
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static bool Compress(this string sourcePath, string targetPath)
        {
            try
            {
                using (var archive = ZipArchive.Create())
                {
                    archive.AddAllFromDirectory(sourcePath);
                    archive.SaveTo(targetPath, new WriterOptions(CompressionType.LZip));
                }
                return true;
            }
            catch (Exception e)
            {
                typeof(CompressHelper).LogError(e);
            }
            return false;
        }

        ///// <summary>
        ///// 单文件压缩（生成的压缩包和第三方的解压软件兼容）
        ///// </summary>
        ///// <param name="sourceFilePath"></param>
        ///// <returns></returns>
        //public static string CompressSingle( string sourceFilePath)
        //{
        //    string zipFileName = sourceFilePath + ".gz";
        //    using (FileStream sourceFileStream = new FileInfo(sourceFilePath).OpenRead())
        //    {
        //        using (FileStream zipFileStream = File.Create(zipFileName))
        //        {
        //            using (GZipStream zipStream = new GZipStream(zipFileStream, CompressionMode.Compress))
        //            {
        //                sourceFileStream.CopyTo(zipStream);
        //            }
        //        }
        //    }
        //    return zipFileName;
        //}
        ///// <summary>
        ///// 自定义多文件压缩（生成的压缩包和第三方的压缩文件解压不兼容）
        ///// </summary>
        ///// <param name="sourceFileList">文件列表</param>
        ///// <param name="saveFullPath">压缩包全路径</param>
        //public static void CompressMulti(this string[] sourceFileList, string saveFullPath)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    foreach (string filePath in sourceFileList)
        //    {
        //        Console.WriteLine(filePath);
        //        if (File.Exists(filePath))
        //        {
        //            string fileName = Path.GetFileName(filePath);
        //            byte[] fileNameBytes = Encoding.UTF8.GetBytes(fileName ?? throw new InvalidOperationException());
        //            byte[] sizeBytes = BitConverter.GetBytes(fileNameBytes.Length);
        //            ms.Write(sizeBytes, 0, sizeBytes.Length);
        //            ms.Write(fileNameBytes, 0, fileNameBytes.Length);
        //            byte[] fileContentBytes = File.ReadAllBytes(filePath);
        //            ms.Write(BitConverter.GetBytes(fileContentBytes.Length), 0, 4);
        //            ms.Write(fileContentBytes, 0, fileContentBytes.Length);
        //        }
        //    }
        //    ms.Flush();
        //    ms.Position = 0;
        //    using (FileStream zipFileStream = File.Create(saveFullPath))
        //    {
        //        using (GZipStream zipStream = new GZipStream(zipFileStream, CompressionMode.Compress))
        //        {
        //            ms.Position = 0;
        //            ms.CopyTo(zipStream);
        //        }
        //    }
        //    ms.Close();
        //}

        ///// <summary>
        ///// 多文件压缩解压
        ///// </summary>
        ///// <param name="zipPath">压缩文件路径</param>
        ///// <param name="targetPath">解压目录</param>
        ///// <param name="fileName"></param>
        //public static void DeCompressMulti(this string zipPath, string targetPath,string fileName=null)
        //{
        //    byte[] fileSize = new byte[4];
        //    if (File.Exists(zipPath))
        //    {
        //        using (FileStream fStream = File.Open(zipPath, FileMode.Open))
        //        {
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                using (GZipStream zipStream = new GZipStream(fStream, CompressionMode.Decompress))
        //                {
        //                    zipStream.CopyTo(ms);
        //                }
        //                ms.Position = 0;
        //                while (ms.Position != ms.Length)
        //                {
        //                    ms.Read(fileSize, 0, fileSize.Length);
        //                    int fileNameLength = BitConverter.ToInt32(fileSize, 0);
        //                    byte[] fileNameBytes = new byte[fileNameLength];
        //                    ms.Read(fileNameBytes, 0, fileNameBytes.Length);
        //                    fileName = fileName ?? Encoding.UTF8.GetString(fileNameBytes);
        //                    string fileFulleName = targetPath + fileName;
        //                    ms.Read(fileSize, 0, 4);
        //                    int fileContentLength = BitConverter.ToInt32(fileSize, 0);
        //                    byte[] fileContentBytes = new byte[fileContentLength];
        //                    ms.Read(fileContentBytes, 0, fileContentBytes.Length);
        //                    using (FileStream childFileStream = File.Create(fileFulleName))
        //                    {
        //                        childFileStream.Write(fileContentBytes, 0, fileContentBytes.Length);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}


    }


    /// <summary>   
    /// 只适用于ZIP压缩   
    /// </summary>   
    public static class ZipHelper
    {
        #region 压缩  

        /// <summary>   
        /// 压缩文件或文件夹   
        /// </summary>   
        /// <param name="fileToZip">要压缩的路径</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <param name="password">密码</param>   
        /// <returns>压缩结果</returns>   
        public static bool Zip(string fileToZip, string zipedFile, string password = null)
        {
            bool result = false;
            if (Directory.Exists(fileToZip))
                result = ZipDirectory(fileToZip, zipedFile, password);
            else if (File.Exists(fileToZip))
                result = ZipFile(fileToZip, zipedFile, password);

            return result;
        }

        /// <summary>   
        /// 压缩文件夹    
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹路径</param>   
        /// <param name="zipedFile">压缩文件完整路径</param>   
        /// <param name="password">密码</param>   
        /// <returns>是否压缩成功</returns>   
        public static bool ZipDirectory(string folderToZip, string zipedFile, string password = null)
        {
            if (!Directory.Exists(folderToZip))
                return false;

            ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile));
            zipStream.SetLevel(6);
            if (!string.IsNullOrEmpty(password)) zipStream.Password = password;

            var result = ZipDirectory(folderToZip, zipStream, "");

            zipStream.Finish();
            zipStream.Close();

            return result;
        }

        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="fileToZip">要压缩的文件全名</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <param name="password">密码</param>   
        /// <returns>压缩结果</returns>   
        public static bool ZipFile(string fileToZip, string zipedFile, string password = null)
        {
            bool result = true;
            ZipOutputStream zipStream = null;
            FileStream fs = null;

            if (!File.Exists(fileToZip))
                return false;

            try
            {
                fs = File.OpenRead(fileToZip);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                fs = File.Create(zipedFile);
                zipStream = new ZipOutputStream(fs);
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                var ent = new ZipEntry(Path.GetFileName(fileToZip));
                zipStream.PutNextEntry(ent);
                zipStream.SetLevel(6);

                zipStream.Write(buffer, 0, buffer.Length);

            }
            catch
            {
                result = false;
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }

                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            GC.Collect();
            GC.Collect(1);

            return result;
        }

        /// <summary>   
        /// 递归压缩文件夹的内部方法   
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹路径</param>   
        /// <param name="zipStream">压缩输出流</param>   
        /// <param name="parentFolderName">此文件夹的上级文件夹</param>   
        /// <returns></returns>   
        private static bool ZipDirectory(string folderToZip, ZipOutputStream zipStream, string parentFolderName)
        {
            bool result = true;
            FileStream fs = null;
            Crc32 crc = new Crc32();

            try
            {
                var ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/"));
                zipStream.PutNextEntry(ent);
                zipStream.Flush();

                var files = Directory.GetFiles(folderToZip);
                foreach (string file in files)
                {
                    fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ent = new ZipEntry(Path.Combine(parentFolderName,
                        Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file)))
                    {
                        DateTime = DateTime.Now,
                        Size = fs.Length
                    };

                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    ent.Crc = crc.Value;
                    zipStream.PutNextEntry(ent);
                    zipStream.Write(buffer, 0, buffer.Length);
                }

            }
            catch
            {
                result = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }

                GC.Collect();
                GC.Collect(1);
            }

            var folders = Directory.GetDirectories(folderToZip);
            foreach (string folder in folders)
                if (!ZipDirectory(folder, zipStream, folderToZip))
                    return false;

            return result;
        }

     

        #endregion

        #region 解压  

        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <param name="password">密码</param>   
        /// <returns>解压结果</returns>   
        public static bool UnZip(this string fileToUnZip, string zipedFolder, string password = null)
        {
            bool result = true;
            FileStream fs = null;
            ZipInputStream zipStream = null;

            if (!File.Exists(fileToUnZip))
                return false;

            if (!Directory.Exists(zipedFolder))
                Directory.CreateDirectory(zipedFolder);

            try
            {
                zipStream = new ZipInputStream(File.OpenRead(fileToUnZip));
                if (!string.IsNullOrEmpty(password))
                    zipStream.Password = password;
                ZipEntry ent;
                while ((ent = zipStream.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(ent.Name))
                    {
                        var fileName = Path.Combine(zipedFolder, ent.Name);
                        fileName = fileName.Replace('/', '\\');//change by Mr.HopeGi   

                        if (fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        fs = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[size];
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                                fs.Write(data, 0, data.Length);
                            else
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                typeof(ZipHelper).LogError(e);
                result = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (zipStream != null)
                {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                GC.Collect();
                GC.Collect(1);
            }
            return result;
        }



        #endregion
    }
}