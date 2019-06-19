using Smarter.iKettle.Application.Models.Enums;

namespace Smarter.iKettle.Application.Models
{
    public class Details
    {
        public KettleStatus Status { get; set; }

        public int Temperature { get; set; }

        public int WaterSensor { get; set; }

        public decimal WaterPercent { get; set; }

        public bool OnBase => Temperature != 127;
    }
}