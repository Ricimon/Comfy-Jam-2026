using System;
using System.Collections.Generic;
using Svelto.ECS;
using Svelto.ECS.Schedulers;

namespace ECS
{
    public class World : IDisposable
    {
        public static ExclusiveGroup DefaultGroup = new();

        private readonly EnginesRoot enginesRoot;
        private readonly EntitiesSubmissionScheduler entitiesSubmissionScheduler;
        private readonly IEntityFactory entityFactory;
        private readonly List<ISystem> systems = new();
        private readonly IdPool idPool = new();

        public World()
        {
            entitiesSubmissionScheduler = new EntitiesSubmissionScheduler();
            enginesRoot = new(entitiesSubmissionScheduler);

            entityFactory = enginesRoot.GenerateEntityFactory();
            var entityFunctions = enginesRoot.GenerateEntityFunctions();
        }

        public void Dispose()
        {
            enginesRoot.Dispose();
        }

        public void AddSystem(ISystem system)
        {
            enginesRoot.AddEngine(system);
            systems.Add(system);
        }

        public EntityInitializer Entity<T>() where T : IEntityDescriptor, new()
        {
            var id = idPool.Get();
            var initializer = entityFactory.BuildEntity<T>(new EGID(id, DefaultGroup));
            return initializer;
        }

        public void Progress()
        {
            entitiesSubmissionScheduler.SubmitEntities();
            foreach(var system in systems)
            {
                system.Update();
            }
        }
    }
}