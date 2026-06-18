using ECS;
using Svelto.ECS;
using UnityEngine;

public class GameStatSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    public static int Score { get; private set; }
    public static int Lives { get; private set; }
    public void Ready()
    {
    }

    public static void ResetTimer(World world)
    {
        var (elapsedTime, count) = world.EntitiesDB.QueryEntities<ElapsedTime>(GameStatTag.Group);
        elapsedTime[0].ValueSeconds = 0;
    }

    public void Update()
    {
        var (score, scoreCount) = entitiesDB.QueryEntities<Score>(GameStatTag.Group);
        Score = score[0].Value;

        var (lives, livesCount) = entitiesDB.QueryEntities<Lives>(GameStatTag.Group);
        Lives = lives[0].Value;
    }
}
