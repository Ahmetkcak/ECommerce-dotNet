using ECommerce.Application.Services;
using ECommerce.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment= webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile formFile)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 10214, useAsync: false);
                await formFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task<string> FileRenameAsync(string path, string fileName,bool first = true)
        {
            string newFileName = await Task.Run<string>(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string newFileName = string.Empty;
                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
                }
                else
                {
                    newFileName = fileName;
                    int indexNo1 = newFileName.IndexOf("-");
                    if (indexNo1 == -1)
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    else
                    {
                        int lastIndex = 0;
                        while (true)
                        {
                            lastIndex = indexNo1;
                            indexNo1 = newFileName.IndexOf("-", indexNo1 + 1);
                            if (indexNo1 == -1)
                            {
                                indexNo1 = lastIndex;
                                break;
                            }
                        }

                        int indexNo2 = newFileName.IndexOf(".");
                        string fileNo = newFileName.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);

                        if (int.TryParse(fileNo, out int _fileNo))
                        {
                            _fileNo++;
                            newFileName = newFileName.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                                                .Insert(indexNo1 + 1, _fileNo.ToString());
                        }
                        else
                            newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";

                    }
                }

                if (File.Exists($"{path}\\{newFileName}"))
                    return await FileRenameAsync(path, newFileName, false);
                else
                    return newFileName;
            });

            return newFileName;
        }

        public async Task<List<(string fileName, string path)>> UploadAsycn(string path, IFormFileCollection formFiles)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if(!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);

            }

            List<(string fileName, string path)> datas = new();
            List<bool> results = new();

            foreach(IFormFile file in formFiles) 
            {
                string fileNewName = await FileRenameAsync(uploadPath,file.FileName);
                bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}",file);
                datas.Add((fileNewName, $"{uploadPath}\\{fileNewName}"));
                results.Add(result);
            }

            if (results.TrueForAll(r => r.Equals(true)))
                return datas;

            return null; ;
        }
    }
}
