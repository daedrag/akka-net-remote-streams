﻿using Akka.Actor;
using Akka.Cluster;
using Common.Config;
using System;

namespace Common.Services
{
    public abstract class ServiceBase
    {
        public string ServiceName { get; }
        public string Description { get; }

        public string ClusterName { get; }
        public string HoconPath { get; }
        public ActorSystem ClusterSystem { get; private set; }

        public event EventHandler OnClusterShutdown;
        private bool isShuttingDown;

        public ServiceBase(string clusterName, string hoconPath, string serviceName, string description = null)
        {
            ServiceName = serviceName;
            Description = description ?? string.Empty;
            ClusterName = clusterName;
            HoconPath = hoconPath;
        }

        public bool Start()
        {
            PreStart();
            // create actor system
            var config = HoconLoader.ParseConfig(HoconPath);
            ClusterSystem = ActorSystem.Create(ClusterName, config);

            var cluster = Cluster.Get(ClusterSystem);
            // call PostStart only after joined cluster successfully
            cluster.RegisterOnMemberUp(() => PostStart());
            // aggressive solution: call Stop immediately when left cluster
            cluster.RegisterOnMemberRemoved(() => {
                Console.WriteLine($"Current member is removed from cluster, member info: {cluster.SelfMember}");
                Stop();
            });
            return true;
        }

        public virtual void PreStart() { }
        public virtual void PostStart() { }

        public bool Stop()
        {
            // add check if Stop has already been called by MemberRemoved
            if (isShuttingDown) return true;
            isShuttingDown = true;

            PreStop();

            // trigger coordinated shutdown before calling PostStop
            Console.WriteLine("Coordinated shutdown starting...");
            CoordinatedShutdown.Get(ClusterSystem)
                .Run(CoordinatedShutdown.ClrExitReason.Instance)
                .ContinueWith(t => PostStop())  // call PostStop after shutdown successfully
                .ContinueWith(t => NotifyClusterShutdown())  // notify for topself stop
                .Wait();
            return true;
        }

        public virtual void PreStop() { }
        public virtual void PostStop() { }

        private void NotifyClusterShutdown()
        {
            OnClusterShutdown?.Invoke(this, null);
        }
    }
}
