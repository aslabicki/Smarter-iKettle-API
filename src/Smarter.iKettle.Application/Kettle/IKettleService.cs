using Smarter.iKettle.Application.Models;
using System.Threading.Tasks;

namespace Smarter.iKettle.Application.Kettle
{
    public interface IKettleService
    {
        Task Boil();

        Task<Details> GetDetails();

        Task Heat(int temperature, int keepWarmMinutes);

        Task HeatFormula(int temperature);

        Task Interrupt();
    }
}