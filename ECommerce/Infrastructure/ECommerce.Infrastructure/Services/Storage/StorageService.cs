using ECommerce.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {

        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; }

        public Task DeleteAsycn(string pathOrContainer, string fileName)
        {
            return _storage.DeleteAsycn(pathOrContainer, fileName); 
        }

        public List<string> GetFiles(string pathOrContainerName)
        {
            return _storage.GetFiles(pathOrContainerName);
        }

        public bool HasFile(string pathOrContainerName, string fileName)
        {
            return _storage.HasFile(pathOrContainerName, fileName);
        }

        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsycn(string pathOrContainerName, IFormFileCollection files)
        {
            return _storage.UploadAsycn(pathOrContainerName, files);
        }
    }
}
