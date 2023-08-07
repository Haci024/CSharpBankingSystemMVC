namespace MoneyTransferProject.Extension
{
    public static class PDFLoader
    {
        public static bool IsPDF(this IFormFile file)
        {
            return file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsMore5MB(this IFormFile file)
        {
            return file.Length / 1024 > 5120;
        }

        public static async Task<string> SaveFileAsync(this IFormFile file, string path)
        {
            string filename = Guid.NewGuid().ToString() + file.FileName;
            string fullpath = Path.Combine(path, filename);
            using (FileStream fileStream = new FileStream(fullpath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return filename;
        }
    }
}

