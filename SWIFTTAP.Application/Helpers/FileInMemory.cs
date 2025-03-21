using Microsoft.AspNetCore.Http;

namespace SWIFTTAP.Application.Helpers;
public sealed class FileInMemory : ICloneable, IEquatable<FileInMemory>
{
    private const string DefaultContentType = "application/octet-stream";

    private readonly byte[] _data;
    private readonly string _name;
    private readonly string _contentType;

    public byte[] Data => _data;
    public int Size => _data.Length;
    public string Name => _name;
    public string ContentType => _contentType;

    public Stream OpenReadStream() => new MemoryStream(_data);

    public string ToBase64() => $"data:{ContentType};base64,{Convert.ToBase64String(_data)}";

    public static bool operator ==(FileInMemory? a, FileInMemory? b) =>
        ReferenceEquals(a, b) || (a is not null && b is not null && a.Equals(b));

    public static bool operator !=(FileInMemory? a, FileInMemory? b) => !(a == b);

    public object Clone() => new FileInMemory(_data.ToArray(), _name, _contentType);

    public bool Equals(FileInMemory? other)
    {
        if (other is null) return false;
        return _name == other._name && _contentType == other._contentType && _data.SequenceEqual(other._data);
    }

    public override bool Equals(object? obj) => obj is FileInMemory other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_name, _contentType, _data.Length);

    private FileInMemory(byte[] data, string name, string contentType)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _contentType = contentType ?? DefaultContentType;
    }

    public static FileInMemory CreateFromFormFile(IFormFile formFile)
    {
        if (formFile is null) throw new ArgumentNullException(nameof(formFile));

        using var memoryStream = new MemoryStream();
        formFile.CopyTo(memoryStream);
        return new FileInMemory(memoryStream.ToArray(), formFile.FileName ?? string.Empty, formFile.ContentType ?? DefaultContentType);
    }

    public static FileInMemory CreateFromStream(Stream stream, string name, string contentType = DefaultContentType)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return new FileInMemory(memoryStream.ToArray(), name, contentType);
    }

    public static FileInMemory CreateFromBytes(byte[] data, string name, string contentType = DefaultContentType)
    {
        if (data is null) throw new ArgumentNullException(nameof(data));
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

        return new FileInMemory(data, name, contentType);
    }

    public static FileInMemory CreateFromBase64(string srcBase64, string name)
    {
        if (string.IsNullOrWhiteSpace(srcBase64)) throw new ArgumentException("Base64 string cannot be null or empty", nameof(srcBase64));
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

        const string base64ContentTypeLeft = "data:";
        const string base64ContentTypeRight = ";base64,";

        var contentTypeFrom = srcBase64.IndexOf(base64ContentTypeLeft) + base64ContentTypeLeft.Length;
        var contentTypeTo = srcBase64.IndexOf(base64ContentTypeRight);

        if (contentTypeFrom <= 0 || contentTypeTo <= 0)
            throw new InvalidOperationException("Invalid base64 encoding format.");

        var contentType = srcBase64[contentTypeFrom..contentTypeTo].Trim();

        if (string.IsNullOrWhiteSpace(contentType))
            throw new InvalidOperationException("Invalid base64 encoding format.");

        var data = Convert.FromBase64String(srcBase64.Split(',')[1]);

        return new FileInMemory(data, name, contentType);
    }

    public static FileInMemory CreateFromFileInMemory(FileInMemory file, string? name = null)
    {
        if (file is null) throw new ArgumentNullException(nameof(file));

        return new FileInMemory(file._data, name ?? file._name, file._contentType);
    }    
}

