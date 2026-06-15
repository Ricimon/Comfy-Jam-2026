using TMPro;
using UnityEngine;

public class GameCanvasView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        GameContext.World.EntitiesDB.QueryEntities<Score>(GameStatTag.Group).Each((ref Score score) =>
        {
            scoreText.text = $"Score: {score.Value}";
        });
        GameContext.World.EntitiesDB.QueryEntities<Lives>(GameStatTag.Group).Each((ref Lives lives) =>
        {
            livesText.text = $"Lives: {lives.Value}";
        });
    }
}
