using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smarter.iKettle.Api.Contracts;
using Smarter.iKettle.Application.Kettle;
using Smarter.iKettle.Application.Models;
using System.Threading.Tasks;

namespace Smarter.iKettle.Api.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/kettle")]
    [ApiController]
    public class KettleController : ControllerBase
    {
        private readonly IKettleService kettleService;

        public KettleController(IKettleService kettleService)
        {
            this.kettleService = kettleService;
        }

        /// <summary>
        /// Gets full details about kettle.
        /// </summary>
        /// <returns>Kettle status, temperature, value from the water sensor, percentage of filling, position.</returns>
        [HttpGet("details")]
        [ProducesResponseType(typeof(Details), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Details>> GetDetails()
        {
            var result = await kettleService.GetDetails();

            if(result is null)
            {
                BadRequest("Unable to get kettle details");
            }

            return Ok(result);
        }

        /// <summary>
        /// Turns boiling.
        /// </summary>
        [HttpPost("boil")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Boil()
        {
            await kettleService.Boil();

            return NoContent();
        }

        /// <summary>
        /// Turns heating.
        /// </summary>
        /// <param name="request">Parameters for heating action.</param>
        [HttpPost("heat")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Heat([FromBody] HeatRequest request)
        {
            if(request.Temperature < 20 || request.Temperature > 100)
            {
                return BadRequest("Temperature cannot be lower than 20°C and greater than 100°C");
            }
            else if(request.KeepWarmMinutes < 0 || request.KeepWarmMinutes > 30)
            {
                return BadRequest("Keep warm time cannot be lower than 0 and greater than 30 minutes");
            }

            await kettleService.Heat(request.Temperature, request.KeepWarmMinutes);

            return NoContent();
        }

        /// <summary>
        /// Turns formula heating.
        /// </summary>
        /// <param name="request">Parameters for formula heating action.</param>
        [HttpPost("heatformula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> HeatFormula([FromBody] HeatFormulaRequest request)
        {
            if(request.Temperature < 20 || request.Temperature > 100)
            {
                return BadRequest("Temperature cannot be lower than 20°C and greater than 100°C");
            }

            await kettleService.HeatFormula(request.Temperature);

            return NoContent();
        }

        /// <summary>
        /// Stops the kettle actions.
        /// </summary>
        [HttpPost("interrupt")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Interrupt()
        {
            await kettleService.Interrupt();

            return NoContent();
        }
    }
}