using Akka.Actor;
using Akka.Cluster;
using System;

namespace PoisonNode.Actors
{
    public class PoisonActor : ReceiveActor
    {
        public PoisonActor()
        {
            Ready();
        }

        protected override void PreStart()
        {
            Console.WriteLine("Poison actor is listening for MemberUp event");
            Cluster.Get(Context.System).Subscribe(Self, new[] { typeof(ClusterEvent.MemberUp) });
        }

        protected override void PostStop()
        {
            Console.WriteLine("Poison actor is closing, unsubscribing from all events");
            Cluster.Get(Context.System).Unsubscribe(Self);
        }

        private void Ready()
        {
            Receive<ClusterEvent.MemberUp>(memberUp =>
            {
                var newMember = memberUp.Member;
                Console.WriteLine($"Poison actor sees MemberUp: {newMember}");
                if (!newMember.HasRole("worker")) return;

                var cluster = Cluster.Get(Context.System);
                var allMembers = cluster.State.Members;
                foreach (var member in allMembers)
                {
                    if (member.HasRole("worker") && !member.Equals(newMember))
                    {
                        Console.WriteLine($"Poison actor is downing {member}");
                        cluster.Down(member.Address);
                    }
                }
            });
        }
    }
}
