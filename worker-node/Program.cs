using Common.Services;

namespace WorkerNode
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceRunner.Run(() => new WorkerNodeService());
        }
    }
}
