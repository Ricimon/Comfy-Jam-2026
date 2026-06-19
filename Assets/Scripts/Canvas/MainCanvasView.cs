using Svelto.ECS;
using UnityEngine;

public class MainCanvasView : MonoBehaviour
{
    [SerializeField] private GameOverCanvasView gameOverCanvas;
    [SerializeField] private GameCanvasView gameCanvas;
    [SerializeField] private TitleCanvasView titleCanvas;

    private CanvasType activeCanvas;
    private void Start()
    {
        gameOverCanvas.Init(StartGame);
        titleCanvas.Init(StartGame);

        activeCanvas = CanvasType.Game;
    }

    private void Update()
    {
        GameContext.World.EntitiesDB.QueryEntities<Lives>(GameStatTag.Group).Each((ref Lives lives) =>
        {
            if(activeCanvas == CanvasType.Game && lives.Value <= 0)
            {
                ShowGameOver();
            }
        });
    }
    private void StartGame()
    {
        Debug.Log("START");
        //Should be in some System/Controller
        GameContext.World.EntitiesDB.QueryEntities<Lives>(GameStatTag.Group).Each((ref Lives lives) =>
        {
            lives.Value = 3;
        });
        GameContext.World.EntitiesDB.QueryEntities<Score>(GameStatTag.Group).Each((ref Score score) =>
        {
            score.Value = 0;
        });

        GameStatSystem.ResetTimer(GameContext.World);
        SlimeSpawnerSystem.DisposeAllSlimes(GameContext.World);
        var (pause, scoreCount) = GameContext.World.EntitiesDB.QueryEntities<Pause>(GameStatTag.Group);
        pause[0].IsPaused = false;

        titleCanvas.Hide();
        gameOverCanvas.Hide();
        gameCanvas.Show();
        activeCanvas = CanvasType.Game;
    }

    private void ShowGameOver()
    {
        titleCanvas.Hide();
        gameCanvas.Hide();
        gameOverCanvas.Show();

        activeCanvas = CanvasType.GameOver;
    }
}
