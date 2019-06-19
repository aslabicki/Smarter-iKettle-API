namespace Smarter.iKettle.Application.Helpers
{
    public static class WaterPercentHelper
    {
        public static decimal Calculate(int currentWaterSensor, int waterSensorMax, int waterSensorMin)
        {
            var current = waterSensorMax - currentWaterSensor;
            var range = waterSensorMax - waterSensorMin;

            var waterPercent = 1 - decimal.Divide(current, range);

            return waterPercent < 0 ? 0m : decimal.Round(waterPercent, 2);
        }
    }
}