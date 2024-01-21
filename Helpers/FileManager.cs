using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CarVilla.Helpers
{
    public static  class FileManager
    {
        public static bool CheckImage(this IFormFile file)
        {
            return file.ContentType.Contains("image") && file.Length / 1024 / 1024 <= 3;
        }

        public static string Upload(this IFormFile file,string web,string folderPath)
        {
            string uploadPath = web + folderPath;
            var fileName = Guid.NewGuid().ToString() + file.FileName;
            var filePath = uploadPath+fileName;
            if(!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }
    }
}
