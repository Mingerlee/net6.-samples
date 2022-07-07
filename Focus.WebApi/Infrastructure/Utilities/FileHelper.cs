using Infrastructure.Config;
using Infrastructure.Models;
using Infrastructure.UserManager;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities
{
    public class FileHelper
    {
        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="folderName">文件夹名称</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<ResultUploadFile> SaveToFile(IFormFile formFile, string folderName="", CancellationToken token = default)
        {
            ResultUploadFile result = new ResultUploadFile();
            if (formFile != null)
            {
                string fullFileName = BuilderFullFileName(new FileInfo(formFile.FileName).Extension, folderName);
                using (var uploadFile = File.OpenWrite(fullFileName))
                {
                    try
                    {
                        // 打开文件流
                        var stream = formFile.OpenReadStream();
                        var buffer = new byte[4 * 1096];
                        int bytesRead = 0;
                        // 开始读取文件
                        while ((bytesRead = await stream.ReadAsync(buffer, token)) > 0)
                        {
                            await uploadFile.WriteAsync(buffer.AsMemory(0, bytesRead), token);
                        }

                        result.FileSize = formFile.Length;
                        result.FileName = formFile.FileName;
                        result.ContentType = new FileInfo(fullFileName).Extension;
                        result.FilePath = fullFileName;
                        result.Status = Enums.UploadFileStatus.Success;
                        return result;
                    }
                    catch (Exception ex)
                    {
                        return new ResultUploadFile(Enums.UploadFileStatus.Failure, ex.Message);
                    }
                }
            }
            return new ResultUploadFile(Enums.UploadFileStatus.NotFound);
        }

        /// <summary>
        /// 构建文件保存全路径
        /// </summary>
        /// <param name="fileExtension">文件扩展名</param>
        /// <param name="folderName">文件夹名称</param>
        /// <returns></returns>
        private static string BuilderFullFileName(string fileExtension, string folderName = "")
        {
            string userCode;
            if (string.IsNullOrEmpty(folderName))
                folderName = DateTime.Now.ToString("yyyyMMddHHmmss");
            try
            {
                userCode = UserContext.Current.UserInfo.UserCode;
                if (string.IsNullOrEmpty(userCode))  userCode = "temp";
            }
            catch
            {
                userCode = "temp";
            }
            //根目录默认为wwwroot
            string rootDirectory = $"{Directory.GetCurrentDirectory()}/wwwroot";
            if (AppSetting.GetConfigBoolean("UploadFile:EnableBasePath") && !string.IsNullOrEmpty(AppSetting.GetConfig("UploadFile:BasePath")))
                rootDirectory = AppSetting.GetConfig("UploadFile:BasePath");
            //设置文件上传路径
            string fileHead = $"{rootDirectory}/FileUpload/{folderName}";
            //string fullFileName = string.Format("{0}/{1}", fileHead, Path.GetFileName(formFile.FileName));
            string fullFileName = string.Format("{0}/{1}/{2}{3}", fileHead, userCode, Guid.NewGuid().GetNextGuid(), fileExtension);
            // 文件保护，如果文件存在则先删除
            if (System.IO.File.Exists(fullFileName))
            {
                try
                {
                    System.IO.File.Delete(fullFileName);
                }
                catch (Exception ex)
                {
                    
                }
            }
            var folder = Path.GetDirectoryName(fullFileName);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return fullFileName;
        }
    }
}
