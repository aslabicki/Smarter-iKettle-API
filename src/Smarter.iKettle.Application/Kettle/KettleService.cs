using Smarter.iKettle.Application.Interfaces;
using Smarter.iKettle.Application.Models;
using System.Threading.Tasks;

namespace Smarter.iKettle.Application.Kettle
{
    public class KettleService : IKettleService
    {
        private readonly IKettleClient kettleClient;

        public KettleService(IKettleClient kettleClient)
        {
            this.kettleClient = kettleClient;
        }

        public async Task Boil()
            => await kettleClient.Boil();

        public async Task<KettleStatus> GetStatus()
            => await kettleClient.GetStatus();

        public async Task Heat(int temperature, int keepWarmMinutes)
            => await kettleClient.Heat(temperature, keepWarmMinutes);

        public async Task HeatFormula(int temperature)
            => await kettleClient.HeatFormula(temperature);

        public async Task Interrupt()
            => await kettleClient.Interrupt();
    }
}