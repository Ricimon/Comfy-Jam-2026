using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvasView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button retryBtn;


    private Action onRetryAction;

    public void Init(Action onRetryAction)
    {
        this.onRetryAction = onRetryAction;
    }

    private void Start()
    {
        retryBtn.onClick.AddListener(OnRetryBtnPress);
    }

    public void Show()
    {
       GameContext.World.EntitiesDB.QueryEntities<Score>(GameStatTag.Group).Each((ref Score score)=> 
       {
           scoreText.text = $"Score: {score.Value}";
       });
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnRetryBtnPress()
    {
        gameObject.SetActive(false);
        onRetryAction?.Invoke();
    }
}
