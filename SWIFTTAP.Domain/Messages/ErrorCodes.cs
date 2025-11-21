namespace SWIFTTAP.Domain.Messages;
public static class ErrorCodes
{
    public enum Application
    {
        AccessDenied
    }

    public enum Authorization
    {
        Failure,
        InvalidMobileApiKey,
        NoMobileApiKeyProvided
    }

    public enum Authentication
    {
        Failed,
        LockedOut,
        TwoFactorCodeExpired
    }

    public enum Card
    {
        AlreadyExists,
        CannotCreate,
        InvalidUniqueName,
        NotFound
    }

    public enum CardVisitStatistic
    { 
        InvalidCardId,
        NotFound
    }

    public enum CardVisitGeoStatistic
    {
        IpAddressGetFailed,
        GeolocationLookupFailed,
        InvalidCardId,
        InvalidCity,
        InvalidCountryCode,
        NotFound,
    }

    public enum DeletedUser
    {
        InvalidName,
        InvalidReason,
        InvalidEmail,
        NotFound
    }

    public enum Email
    {
        InvalidMessageId,
        InvalidFrom,
        InvalidTo,
        InvalidSubject,
        NotFound
    }

    public enum EmailSend
    {
        CannotSend,
        NotFound,
        UserNotAllowed
    }

    public enum Link
    { 
        InvalidName,
        InvalidUrl,
        InvalidType,
        InvalidCardId,
        NotFound,
        OrderAlreadyExists
    }

    public enum LinkIcon
    {
        InvalidLink,
        InvalidLinkKind,
        NotFound
    }

    public enum LinkVisitStatistic
    { 
        InvalidLinkId,
        NotFound
    }

    public enum Logo
    { 
        NotFound,
        NotDefined
    }

    public enum Role
    {
        NotFound
    }

    public enum RefreshToken
    {
        InvalidValue,
        InvalidExpiresAt,
        InvalidUserId
    }

    public enum Theme
    { 
        InvalidName,
        InvalidPrimaryColor,
        InvalidSecondaryColor,
        NotFound
    }

    public enum Translation
    {
        NotFound,
        InvalidName,
        InvalidLanguage,
    }

    public enum VcfDownloadStatistic
    { 
        InvalidCardId,
        NotFound
    }

    public enum User
    {
        AlreadyExists,
        CannotChangePassword,
        CannotConfirmEmail,
        InvalidEmail,
        InvalidName,
        InvalidUniqueName,
        NotAuthenticated,
        NotFound,
        RegistrationFailed,
        UpdateFailed
    }

    public enum ContactForm
    {
        EmailInvalid,
        NameInvalid,
        PhoneInvalid,
        CompanyInvalid,
        QuantityInvalid,
        MessageInvalid
    }
}
