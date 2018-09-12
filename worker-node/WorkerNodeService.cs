using Common.Services;
using System;

namespace WorkerNode
{
    class WorkerNodeService : ServiceBase
    {
        public WorkerNodeService(): base("MyCluster", "WorkerNode.hocon", "WorkerNode")
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
