using Grpc.Core;

namespace SOP.AgeServer.Services
{
    public class AgerService : Ager.AgerBase
    {
        private readonly ILogger<AgerService> _logger;

        public AgerService(ILogger<AgerService> logger)
        {
            this._logger = logger;
        }

        public override Task<AgeReply> GetAge(AgeRequest request, ServerCallContext context)
        {
            var birthday = DateTime.Parse(request.Birthday);

            var allDays = (DateTime.Now.Year * 365 + DateTime.Now.Month * 30 + DateTime.Now.Day) - 
                (birthday.Year * 365 + birthday.Month * 30 + birthday.Day);

            var years = allDays / 365;
            allDays -= years * 365;

            var months = allDays / 30;
            allDays -= months * 30;

            var days = allDays;

            return Task.FromResult(new AgeReply() { Years = (uint)years, Months = (uint)months, Days = (uint)days });
        }
    }
}
