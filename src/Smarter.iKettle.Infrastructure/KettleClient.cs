using Microsoft.Extensions.Options;
using Smarter.iKettle.Application.Helpers;
using Smarter.iKettle.Application.Interfaces;
using Smarter.iKettle.Application.Models;
using Smarter.iKettle.Infrastructure.Settings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Smarter.iKettle.Application.Models.Enums;

namespace Smarter.iKettle.Infrastructure
{
    public class KettleClient : IKettleClient
    {
        private const byte HeatCommand = 21;
        private const byte HeatFormulaCommand = 25;
        private const byte InterruptCommand = 22;

        private readonly KettleSettings settings;

        public KettleClient(IOptions<KettleSettings> options)
        {
            settings = options.Value;
        }

        public async Task<bool> Heat(int temperature, int keepWarmMinutes)
        {
            var response = await GetResponse(new byte[] { HeatCommand, (byte)temperature, (byte)keepWarmMinutes });

            return EnsureSuccessResponse(response);
        }

        public async Task<bool> HeatFormula(int temperature)
        {
            var response = await GetResponse(new byte[] { HeatFormulaCommand, (byte)temperature, 0 });

            return EnsureSuccessResponse(response);
        }

        public async Task<bool> Boil()
        {
            var response = await GetResponse(new byte[] { HeatCommand, 100, 0 });

            return EnsureSuccessResponse(response);
        }

        public async Task<bool> Interrupt()
        {
            var response = await GetResponse(new byte[] { InterruptCommand });

            return EnsureSuccessResponse(response);
        }

        public async Task<Details> GetDetails()
        {
            var response = await GetResponse();

            if(response.First() != 0x14)
            {
                return null;
            }

            var responseArray = response.ToArray();
            var waterSensor = (responseArray[3] << 8) + responseArray[4];

            var details = new Details
            {
                Status = (KettleStatus)responseArray[1],
                Temperature = responseArray[2],
                WaterSensor = waterSensor,
                WaterPercent = WaterPercentHelper.Calculate(waterSensor, settings.WaterSensorMax, settings.WaterSensorMin)
            };

            return details;
        }

        private async Task<byte[]> GetResponse(byte[] command = null)
        {
            var response = new List<byte>();

            using(var client = new TcpClient(settings.Host, settings.Port))
            using(var networkStream = client.GetStream())
            using(var binaryReader = new BinaryReader(networkStream))
            {
                if(command != null && command.Length > 0)
                {
                    await networkStream.WriteAsync(command, 0, command.Length);
                }

                while(true)
                {
                    var readByte = binaryReader.ReadByte();

                    if(readByte == 0x7e)
                    {
                        break;
                    }

                    response.Add(readByte);
                }
            }

            return response.ToArray();
        }

        private static bool EnsureSuccessResponse(IEnumerable<byte> response)
            => response.SequenceEqual(new byte[] { 3, 0 });
    }
}