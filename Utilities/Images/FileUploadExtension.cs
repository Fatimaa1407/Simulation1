namespace Simulation1.Utilities.Images
{
    public static class FileUploadExtension
    {
        public static string SaveImage(this IFormFile formFile, IWebHostEnvironment env, string folder)
        {
            string path = Path.Combine(env.WebRootPath, folder);
            string fileName = Guid.NewGuid().ToString()+ formFile.FileName;
            string fullPath  = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
                {
                Directory.CreateDirectory(path);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                formFile.CopyTo(stream);
                return fileName;
            }
        }
    }
}
