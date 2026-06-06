using UnityEngine;

namespace ECS.Example
{
    public class ExampleRunner : MonoBehaviour
    {
        private World world;

        private void Start()
        {
            world = new World();

            world.AddSystem(new ExampleSystem());

            var entity1 = world.Entity<ExampleEntityDescriptor>();
            entity1.Init(new ExampleComponent
            {
                AddToFilter = true,
            });

            var entity2 = world.Entity<ExampleEntityDescriptor>();
            entity2.Init(new ExampleComponent
            {
                AddToFilter = false,
            });
        }

        private void Update()
        {
            world.Progress();
        }

        private void OnDestroy()
        {
            world.Dispose();
        }
    }
}