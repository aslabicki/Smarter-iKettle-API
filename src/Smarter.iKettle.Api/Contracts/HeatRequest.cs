namespace Smarter.iKettle.Api.Contracts
{
    public class HeatRequest
    {
        public int Temperature { get; set; }

        public int KeepWarmMinutes { get; set; }
    }
}