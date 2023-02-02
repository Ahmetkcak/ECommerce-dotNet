using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface IFileService
    {
        Task<List<(string fileName, string path)>> UploadAsycn(string path,IFormFileCollection formFiles);
        Task<bool> CopyFileAsync(string path,IFormFile formFile);
    }
}
