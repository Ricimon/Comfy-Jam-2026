using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvasView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button retryBtn;


    private void Start()
    {
        retryBtn.onClick.AddListener(OnRetryBtnPress);
    }

    public void Show()
    {
        var (score, count) = GameContext.World.EntitiesDB.QueryEntities<Score>(GameStatTag.Group);
    }
    private void OnRetryBtnPress()
    {
        Debug.Log("GameReset");
    }
}
