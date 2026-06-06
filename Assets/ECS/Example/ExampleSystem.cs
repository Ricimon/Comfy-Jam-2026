using Svelto.ECS;
using UnityEngine;

namespace ECS.Example
{
    public class ExampleSystem : ISystem, IQueryingEntitiesEngine,
        IReactOnAddEx<ExampleComponent>, IReactOnRemoveEx<ExampleComponent>
    {
        public EntitiesDB entitiesDB { get; set; }

        private EntitiesDB.SveltoFilters filters;

        public void Ready()
        {
            filters = entitiesDB.GetFilters();
            filters.GetOrCreateTransientFilter<ExampleComponent>(ExampleFilters.ExampleFilter);
        }

        public void Update()
        {
            var (components, entityIDs, count) = entitiesDB.QueryEntities<ExampleComponent>(World.DefaultGroup);
            var exampleFilter = filters.GetTransientFilter<ExampleComponent>(ExampleFilters.ExampleFilter);
            Debug.Log($"ExampleSystem found {count} example entities, {exampleFilter.ComputeFinalCount()} filtered entities");
        }

        public void Add((uint start, uint end) rangeOfEntities, in EntityCollection<ExampleComponent> entities,
            ExclusiveGroupStruct groupID)
        {
            var (components, entityIDs, _) = entities;
            var count = rangeOfEntities.end - rangeOfEntities.start;

            Debug.Log($"ExampleSystem received event of {count} entities added");

            var exampleFilter = filters.GetTransientFilter<ExampleComponent>(ExampleFilters.ExampleFilter);
            for (var i = rangeOfEntities.start; i < rangeOfEntities.end; i++)
            {
                if (components[i].AddToFilter)
                {
                    exampleFilter.Add(new EGID(entityIDs[i], groupID), i);
                }
            }
        }

        public void Remove((uint start, uint end) rangeOfEntities, in EntityCollection<ExampleComponent> entities,
            ExclusiveGroupStruct groupID)
        {
            var (_, entityIDs, _) = entities;
            var count = rangeOfEntities.end - rangeOfEntities.start;

            Debug.Log($"ExampleSystem received event of {count} entities removed");
        }
    }
}