using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitDisposedExceptionDemo.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitDisposedExceptionDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IBus _bus;

        public TestController(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        [Produces("application/json")]
        [HttpGet("[action]")]
        public async Task<IActionResult> Do()
        {
            await _bus.Send(new MyFirstMessage(DateTimeOffset.Now)).ConfigureAwait(false);

            return Ok();
        }
    }
}
