using System;
using System.Collections.Generic;
using Svelto.ECS;
using Svelto.ECS.Schedulers;

namespace ECS
{
    public class World : IDisposable
    {
        private class DbSystem : ISystem, IQueryingEntitiesEngine
        {
            public EntitiesDB entitiesDB { get; set; }

            public void Ready() { }

            public void Update() { }
        }

        public EntitiesDB EntitiesDB => dbSystem.entitiesDB;

        private readonly EnginesRoot enginesRoot;
        private readonly EntitiesSubmissionScheduler entitiesSubmissionScheduler;
        private readonly IEntityFactory entityFactory;
        private readonly IEntityFunctions entityFunctions;
        private readonly DbSystem dbSystem = new();
        private readonly List<ISystem> systems = new();
        private readonly IdPool idPool = new();

        public World()
        {
            entitiesSubmissionScheduler = new EntitiesSubmissionScheduler();
            enginesRoot = new(entitiesSubmissionScheduler);

            entityFactory = enginesRoot.GenerateEntityFactory();
            entityFunctions = enginesRoot.GenerateEntityFunctions();

            AddSystem(dbSystem);
        }

        public bool IsValid() => enginesRoot.IsValid();

        public void Dispose()
        {
            foreach (var system in systems)
            {
                if (system is IDisposable d)
                {
                    d.Dispose();
                }
            }
            enginesRoot.Dispose();
        }

        public void AddSystem(ISystem system)
        {
            enginesRoot.AddEngine(system);
            systems.Add(system);
        }

        public EntityInitializer Entity<T>(ExclusiveGroupStruct groupID) where T : IEntityDescriptor, new()
        {
            var id = idPool.Get();
            var initializer = entityFactory.BuildEntity<T>(new EGID(id, groupID));
            return initializer;
        }

        public void RemoveEntity<T>(EGID egid) where T : IEntityDescriptor, new()
        {
            entityFunctions.RemoveEntity<T>(egid);
        }

        public void RemoveEntity<T>(uint id, ExclusiveGroupStruct groupID) where T : IEntityDescriptor, new()
        {
            entityFunctions.RemoveEntity<T>(id, groupID);
        }

        public void RemoveEntitiesFromGroup(ExclusiveGroupStruct group)
        {
            entityFunctions.RemoveEntitiesFromGroup(group);
        }

        public void Progress()
        {
            entitiesSubmissionScheduler.SubmitEntities();
            foreach (var system in systems)
            {
                system.Update();
            }
        }
    }
}