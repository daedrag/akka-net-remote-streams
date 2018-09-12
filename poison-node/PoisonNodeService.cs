using Akka.Actor;
using Common.Services;
using PoisonNode.Actors;
using System;

namespace PoisonNode
{
    class PoisonNodeService : ServiceBase
    {
        public PoisonNodeService(): base("MyCluster", "PoisonNode.hocon", "PoisonNode")
        {
        }

        public override void PreStart()
        {
            Console.WriteLine($"PreStart: {ServiceName} running...");
        }

        public override void PostStart()
        {
            Console.WriteLine($"PostStart: {ServiceName} running...");
            ClusterSystem.ActorOf(Props.Create<PoisonActor>(), "poison-actor");
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
