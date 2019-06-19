using Smarter.iKettle.Application.Models;
using System.Threading.Tasks;

namespace Smarter.iKettle.Application.Interfaces
{
    public interface IKettleClient
    {
        Task<bool> Boil();

        Task<KettleStatus> GetStatus();

        Task<bool> Heat(int temperature, int keepWarmMinutes);

        Task<bool> HeatFormula(int temperature);

        Task<bool> Interrupt();
    }
}