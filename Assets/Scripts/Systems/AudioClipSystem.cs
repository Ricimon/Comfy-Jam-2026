using ECS;
using Svelto.ECS;
using UnityEngine;


public class AudioClipSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly AudioSource audioSource;
    private readonly AudioClipResourceManager resourceManager;
    public AudioClipSystem(AudioSource audioSource, AudioClipResourceManager resourceManager)
    {
        this.audioSource = audioSource;
        this.resourceManager = resourceManager;
    }

    public static void PlaySFX(SFX sfx)
    {
        var soundEffectEntity = GameContext.World.Entity<SoundEffectEntityDescriptor>(SoundEffectGroup.Group);
        var audioClips = GameContext.World.EntitiesDB.GetSingletonComponent<AudioClips>(AudioClipGroup.Group);
        soundEffectEntity.Init(new SoundEffect()
        {
            SoundId = audioClips.GetClipId(sfx)
        });
    }

    public void Ready()
    {
    }

    public void Update()
    {
        entitiesDB.QueryEntities<SoundEffect>(SoundEffectGroup.Group)
    .Each((uint id, ref SoundEffect effect) =>
    {
        audioSource.PlayOneShot(resourceManager[effect.SoundId]);
        GameContext.World.RemoveEntity<SoundEffectEntityDescriptor>(new EGID(id, SoundEffectGroup.Group));
    });
    }
}
