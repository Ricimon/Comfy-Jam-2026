using Svelto.DataStructures.Experimental;
using Svelto.ECS;
using UnityEngine;

public struct AudioClips : IEntityComponent
{
    public ValueIndex ButtonId;
    public ValueIndex CorrectId;
    public ValueIndex DeathId;
    public ValueIndex DisguiseId;
    public ValueIndex DropId;
    public ValueIndex PickupId;
    public ValueIndex WrongId;

    public ValueIndex GetClipId(SFX type)
    {
        switch (type)
        {
            case SFX.Button: return ButtonId;
            case SFX.Correct: return CorrectId;
            case SFX.Death: return DeathId;
            case SFX.Disguise: return DisguiseId;
            case SFX.Drop: return DropId;
            case SFX.Pickup: return PickupId;
            case SFX.Wrong: return WrongId;
            default: return ButtonId;
        }
    }
}

public enum SFX
{
    Button,
    Correct,
    Death,
    Disguise,
    Drop,
    Pickup,
    Wrong
}