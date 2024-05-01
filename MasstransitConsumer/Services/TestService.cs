namespace MasstransitConsumer.Services
{
    public interface ITestService
    {
        IEnumerable<int> GetRangedNumbers(int number);
    }
    
    public class TestService : ITestService
    {
        public TestService() { 

        }

        public IEnumerable<int> GetRangedNumbers(int number)
        {
            return Enumerable.Range(0, number);
        }
    }
}
