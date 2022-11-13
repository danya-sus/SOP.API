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
            var years = DateTime.Now.Year - birthday.Year;
            var months = DateTime.Now.Month - birthday.Month;
            var days = DateTime.Now.Day - birthday.Day;

            return Task.FromResult(new AgeReply() { Years = (uint)years, Months = (uint)months, Days = (uint)days });
        }
    }
}
