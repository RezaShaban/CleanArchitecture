using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class TimeService : ITimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
