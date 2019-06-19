namespace Smarter.iKettle.Application.Models.Enums
{
    public enum KettleStatus : byte
    {
        Ready = 0x00,
        Heating = 0x01,
        KeepWarm = 0x02,
        CycleFinished = 0x03,
        FormulaCooling = 0x04
    }
}
