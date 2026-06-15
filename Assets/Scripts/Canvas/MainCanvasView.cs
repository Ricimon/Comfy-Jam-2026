using UnityEngine;

public class MainCanvasView : MonoBehaviour
{
    [SerializeField] private GameOverCanvasView gameOverCanvas;
    [SerializeField] private GameCanvasView gameCanvas;

    private CanvasType activeCanvas;
    private void Start()
    {
        gameOverCanvas.Init(ShowGame);

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
    private void ShowGame()
    {
        //Should be in some System/Controller
        GameContext.World.EntitiesDB.QueryEntities<Lives>(GameStatTag.Group).Each((ref Lives lives) =>
        {
            lives.Value = 3;
        });
        gameOverCanvas.Hide();
        gameCanvas.Show();
        activeCanvas = CanvasType.Game;
    }

    private void ShowGameOver()
    {
        gameCanvas.Hide();
        gameOverCanvas.Show();

        activeCanvas = CanvasType.GameOver;
    }
}
