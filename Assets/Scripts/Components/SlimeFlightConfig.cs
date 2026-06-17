using Svelto.DataStructures.Experimental;
using Svelto.ECS;

public struct SlimeFlightConfig : IEntityComponent
{
    public ValueIndex FlightCurveId;
    public ValueIndex FlightRotationCurveId;
}

public class SlimeFlightConfigEntity : GenericEntityDescriptorAndGroup<SlimeFlightConfig> { }