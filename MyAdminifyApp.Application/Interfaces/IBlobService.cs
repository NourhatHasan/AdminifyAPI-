using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAdminifyApp.Application.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
        Task DeleteFileAsync(string fileName);
        Task<bool> FileExistsAsync(string fileName);
    }

}
