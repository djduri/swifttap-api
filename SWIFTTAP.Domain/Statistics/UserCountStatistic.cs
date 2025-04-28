using SWIFTTAP.Domain.Base;

namespace SWIFTTAP.Domain.Statistics;
public sealed class UserCountStatistic : Entity
{
    public DateOnly Date { get; private set; }
    public int Counter { get; private set; }

    public UserCountStatistic SetCounter(int counter)
    {
        Counter = counter;
        return this;
    }

    private UserCountStatistic()
    {
        Counter = 0;
        Date = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static class Factory
    {
        public static UserCountStatistic Create(int counter)
        {
            return new UserCountStatistic()
                .SetCounter(counter);
        }
    }
}
