using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Services.Interfaces;

public interface IDataSeederService : IScopedAppService
{
	Task Seed();
}