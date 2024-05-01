using MassTransit.Mediator;
using MasstransitConsumer.Consumers;
using Microsoft.AspNetCore.Mvc;

namespace MasstransitConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRandomNumbers([FromQuery] TestDTO query)
        {
            var response = await _mediator.CreateRequestClient<TestDTO>()
                            .GetResponse<TestResponse>(new TestDTO
                            {
                                Limit = query.Limit,
                            });

            return Ok(response.Message);
        }

    }
}
