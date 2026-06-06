using Svelto.ECS;

namespace ECS
{
    public interface ISystem : IEngine
    {
        public void Update();
    }
}