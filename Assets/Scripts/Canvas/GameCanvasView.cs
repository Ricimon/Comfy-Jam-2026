using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameCanvasView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private UnityEngine.CanvasGroup canvasGroup;

    public void Show()
    {
        canvasGroup.DOFade(1,.25f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.DOFade(0, .25f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
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
