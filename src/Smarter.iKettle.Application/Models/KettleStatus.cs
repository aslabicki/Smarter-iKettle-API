namespace Smarter.iKettle.Application.Models
{
    public class KettleStatus
    {
        #region Inner class

        public enum StatusType : byte
        {
            Ready = 0x00,
            Heating = 0x01,
            KeepWarm = 0x02,
            CycleFinished = 0x03,
            FormulaCooling = 0x04
        }

        #endregion Inner class

        public StatusType Status { get; set; }

        public int Temperature { get; set; }

        public int WaterSensor { get; set; }

        public decimal WaterPercent { get; set; }

        public bool OnBase => Temperature != 127;
    }
}