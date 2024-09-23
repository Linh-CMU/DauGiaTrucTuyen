using System;
using System.IO;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadsFileDAO
    {
        /// <summary>
        /// The destination folder path
        /// </summary>
        private readonly string destinationFolderPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadsFileDAO"/> class.
        /// </summary>
        /// <param name="baseDirectory">The base directory.</param>
        public UploadsFileDAO(string baseDirectory)
        {
            destinationFolderPath = Path.Combine(baseDirectory, "uploads");
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }
        }

        /// <summary>
        /// Reads the file asynchronous.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">File name cannot be null or empty. - fileName</exception>
        /// <exception cref="System.IO.FileNotFoundException">File không tồn tại.</exception>
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
