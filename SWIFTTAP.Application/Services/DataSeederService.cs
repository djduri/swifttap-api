using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Authorization.Interfaces;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Services;
internal sealed class DataSeederService : IDataSeederService
{
	private readonly UserManager<User> _userManager;
    private readonly IRepository<Card> _cardRepository;
	private readonly DataSeederSettings _dataSeederSettings;
	private readonly IAuthorizationSeederService _authorizationSeederService;

    public DataSeederService(UserManager<User> userManager,
                          IRepository<Card> cardRepository,
                          IOptions<DataSeederSettings> dataSeederSettings,
                          IAuthorizationSeederService authorizationSeederService)
    {
        _userManager = userManager;
        _cardRepository = cardRepository;
        _dataSeederSettings = dataSeederSettings.Value;
        _authorizationSeederService = authorizationSeederService;
    }

    public async Task Seed()
    {
        await SeedAdminUserAsync();
        await _authorizationSeederService.SeedAsync();
    }

    private async Task SeedAdminUserAsync()
    {
        var adminUserEmail = _dataSeederSettings.AdminUser?.Email;
        var adminUserPassword = _dataSeederSettings.AdminUser?.Password;

        if (string.IsNullOrEmpty(adminUserEmail) || string.IsNullOrEmpty(adminUserPassword))
            return;


        var user = await _userManager.FindByEmailAsync(adminUserEmail);

        if (user is null)
        {
            var newCard = Card.Factory.Create("SuperAdmin");
            _cardRepository.Add(newCard);

            var admin = User.Factory.CreateWithConfirmedEmail("SuperAdmin", adminUserEmail, newCard);
            await _userManager.CreateAsync(admin, adminUserPassword);
        }
    }
}
