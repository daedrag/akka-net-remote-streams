using Common.Services;

namespace PoisonNode
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceRunner.Run(() => new PoisonNodeService());
        }
    }
}
