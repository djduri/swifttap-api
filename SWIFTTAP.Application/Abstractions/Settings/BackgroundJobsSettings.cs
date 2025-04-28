using NCrontab;
using SWIFTTAP.Domain.Common;
using System.Data;

namespace SWIFTTAP.Application.Abstractions.Settings;

public sealed class BackgroundJobsSettings : IValidatableSettings
{
    public bool IsEnabled { get; init; }
    public string? ConnectionString { get; init; }
    public bool IsDashboardEnabled { get; init; }
    public CronJobSchedules CronJobSchedules { get; init; } = new CronJobSchedules();

    public bool Valid()
    {
        if (string.IsNullOrEmpty(ConnectionString)) return false;
        if (!CronJobSchedules.IsValid()) return false;

        return true;
    }
}

public sealed class CronJobSchedules
{
    public string RefreshTokenExpiredCleanupSchedule { get; set; } = string.Empty;
    public string UserCountStatisticSchedule { get; set; } = "28 10 * * *";

    public bool IsValid()
    {
        if (!IsValidCronExpression(RefreshTokenExpiredCleanupSchedule)) return false;
        if (!IsValidCronExpression(UserCountStatisticSchedule)) return false;
        return true;
    }

    private bool IsValidCronExpression(string cronExpression)
    {
        try
        {
            var schedule = CrontabSchedule.Parse(cronExpression);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}