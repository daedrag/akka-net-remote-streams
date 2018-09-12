using Common.Services;
using System;

namespace SeedNode
{
    class SeedNodeService : ServiceBase
    {
        public SeedNodeService(): base("MyCluster", "SeedNode.hocon", "SeedNode")
        {
        }

        public override void PreStart()
        {
            Console.WriteLine($"PreStart: {ServiceName} running...");
        }

        public override void PostStart()
        {
            Console.WriteLine($"PostStart: {ServiceName} running...");
        }

        public override void PreStop()
        {
            Console.WriteLine($"PreStop: {ServiceName} running...");
        }

        public override void PostStop()
        {
            Console.WriteLine($"PostStop: {ServiceName} running...");
        }
    }
}
