using ECS;
using UnityEngine;

public class AudioClipEntityInitializer : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip Button;
    public AudioClip Correct;
    public AudioClip Death;
    public AudioClip Disguise;
    public AudioClip Drop;
    public AudioClip Pickup;
    public AudioClip Wrong;

    public void Start()
    {
        GameContext.World.AddSystem(new AudioClipSystem(audioSource, GameContext.AudioClipResourceManager));

        var buttonId = GameContext.AudioClipResourceManager.Add(Button);
        var correctId = GameContext.AudioClipResourceManager.Add(Correct);
        var deathId = GameContext.AudioClipResourceManager.Add(Death);
        var disguiseId = GameContext.AudioClipResourceManager.Add(Disguise);
        var dropId = GameContext.AudioClipResourceManager.Add(Drop);
        var pickupId = GameContext.AudioClipResourceManager.Add(Pickup);
        var wrongId = GameContext.AudioClipResourceManager.Add(Wrong);

        GameContext.World.RemoveEntitiesFromGroup(AudioClipGroup.Group);

        var entity = GameContext.World.Entity<AudioClipEntityDescriptor>(AudioClipGroup.Group);

        entity.Init(new AudioClips 
        {
            ButtonId = buttonId,
            CorrectId = correctId,
            DeathId = deathId,
            DisguiseId = disguiseId,
            DropId = dropId,
            PickupId = pickupId,
            WrongId = wrongId
        });
    }

}
