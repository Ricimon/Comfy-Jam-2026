using Svelto.ECS;

namespace ECS.Example
{
    public static class ExampleFilters
    {
        public static CombinedFilterID ExampleFilter = new(0, id);

        private static readonly FilterContextID id = FilterContextID.GetNewContextID();
    }
}