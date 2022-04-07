using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ShopOnline.Core.Helpers
{
    public static class UserHelper
    {
        public static string GetNewRandomPassword()
        {
            Random rnd = new();
            string value = "";
            for (int i = 0; i < 6; i++)
            {
                value += rnd.Next(0, 9).ToString();
            }
            return value;
        }

        public static async Task<string> UploadImageAvatarHanlderAsync(IFormFile uploadAvt, IWebHostEnvironment hostEnvironment)
        {
            string wwwRootPath = hostEnvironment.WebRootPath;
            string fileName;
            string extension;
            string avatarPath;

            fileName = Path.GetFileNameWithoutExtension(uploadAvt.FileName);
            extension = Path.GetExtension(uploadAvt.FileName);
            avatarPath = fileName += extension;
            string path1 = Path.Combine(wwwRootPath + "/img/Avatar/", fileName);
            using (var fileStream = new FileStream(path1, FileMode.Create))
            {
                await uploadAvt.CopyToAsync(fileStream);
            }

            return avatarPath;
        }
    }
}
