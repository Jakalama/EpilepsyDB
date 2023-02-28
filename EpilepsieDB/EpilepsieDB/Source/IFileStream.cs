using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace EpilepsieDB.Source
{
    public interface IFileStream
    {
        string Name { get; }

        string FileName { get; }

        string ContentType { get; }

        long Length { get; }

        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
    }
}
