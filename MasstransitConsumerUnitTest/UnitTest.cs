using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using MasstransitConsumer.Consumers;
using MasstransitConsumer.Services;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace MasstransitConsumerUnitTest
{
    [ExcludeFromCodeCoverage]
    public class UnitTest
    {
        private readonly ITestService testServiceMoq;
        public UnitTest()
        {
            testServiceMoq = Substitute.For<ITestService>();
        }

        [Test]
        public async Task Get_Random_Numbers_Should_Return_List()
        {
            //////////Arrange

            int limit = 100;

            var numbers = Enumerable.Range(0, limit);

            testServiceMoq.GetRangedNumbers(limit).Returns(numbers);

            //Configure masstransit consumer with masstransit harness

            await using var provider = new ServiceCollection()
                .AddScoped(provider => testServiceMoq)
                .AddMassTransitTestHarness(opt =>
                {
                    opt.AddConsumer<TestConsumer>();
                })
                .BuildServiceProvider(true);

            //Register the Test service
            var harness = provider.GetRequiredService<ITestHarness>();

            await harness.Start();

            //Request with Test DTO
            var client = harness.GetRequestClient<TestDTO>();

            ////////////Act

            //Response 
            var response = await client.GetResponse<TestResponse>(new TestDTO
            {
                Limit = limit
            });

            ///////////Assert
            response.Message.Should().NotBeNull();
            response.Message.Data.Should().HaveCount(limit);

            await harness.Stop();
            await provider.DisposeAsync();
        }
    }
}