using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Helpers;

namespace SWIFTTAP.Application.Services.Interfaces;

public interface IImageService : IScopedAppService
{
    FileInMemory FitImageToSize(FileInMemory imageFile, int maxWidth, int maxHeight);
    FileInMemory BlankImage(int maxWidth, int maxHeight);
}