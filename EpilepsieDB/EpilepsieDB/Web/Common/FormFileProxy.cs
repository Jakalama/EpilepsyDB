using EpilepsieDB.Source;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EpilepsieDB.Web.Common
{
    public class FormFileProxy : IFileStream
    {
        private readonly IFormFile file;

        public FormFileProxy(IFormFile file)
        {
            this.file = file;
        }

        public string Name => file.Name;

        public string FileName => file.FileName;

        public string ContentType => file.ContentType;

        public long Length => file.Length;

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            await file.CopyToAsync(target, cancellationToken);
        }
    }
}
