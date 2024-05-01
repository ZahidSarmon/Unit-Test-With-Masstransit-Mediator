using MassTransit;
using MasstransitConsumer.Services;

namespace MasstransitConsumer.Consumers
{
    public class TestConsumer : IConsumer<TestDTO>
    {
        private readonly ITestService _service;
        public TestConsumer(ITestService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<TestDTO> context)
        {
            var randomNumbers = _service.GetRangedNumbers(context.Message.Limit);

            await context.RespondAsync(new TestResponse
            {
                Data = randomNumbers,
            });
        }
    }
}
