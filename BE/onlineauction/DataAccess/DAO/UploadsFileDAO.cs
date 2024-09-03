using System;
using System.IO;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class UploadsFileDAO
    {
        private readonly string destinationFolderPath;

        public UploadsFileDAO(string baseDirectory)
        {
            destinationFolderPath = Path.Combine(baseDirectory, "uploads");
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }
        }

        public async Task<Stream> ReadFileAsync(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

            var filePath = Path.Combine(destinationFolderPath, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File không tồn tại.", fileName);
            }

            // Sử dụng `FileStream` với `using` để đảm bảo tài nguyên được giải phóng
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);

            return fileStream; // Trả về `FileStream` dưới dạng `Stream`
        }
    }
}
