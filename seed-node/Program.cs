using Common.Services;

namespace SeedNode
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceRunner.Run(() => new SeedNodeService());
        }
    }
}
