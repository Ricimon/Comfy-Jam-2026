using System;
using Svelto.ECS;

namespace ECS
{
    public class BaseEntityDescriptor : IEntityDescriptor
    {
        public virtual IComponentBuilder[] componentsToBuild => Array.Empty<IComponentBuilder>();
    }
}