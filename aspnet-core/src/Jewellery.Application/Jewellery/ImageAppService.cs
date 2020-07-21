using Abp.Application.Services;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Jewellery.Jewellery
{

    public class ImageAppService : ApplicationService
    {
        const String folderName = "Images";
        readonly String folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        public ImageAppService()
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

        }
        public async Task<string> Upload(IFormFile file)
        {
            using var fileContentStream = new MemoryStream();
            await file.CopyToAsync(fileContentStream);
            var fileName = Guid.NewGuid().ToString() + ".jpg";

            await File.WriteAllBytesAsync(Path.Combine(folderPath, fileName), fileContentStream.ToArray());

            return fileName;
        }


        public async Task<FileContentResult> Get(string fileName)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                return new FileContentResult(await System.IO.File.ReadAllBytesAsync(filePath), "application/octet-stream")
                {
                    FileDownloadName = fileName
                };
            }
            throw new UserFriendlyException("File Not Found");
        }
    }
}
