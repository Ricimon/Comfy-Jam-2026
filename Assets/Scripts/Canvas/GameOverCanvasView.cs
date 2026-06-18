using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverCanvasView : MonoBehaviour
{
    [SerializeField] private UnityEngine.CanvasGroup canvasGroup;
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
        GameContext.World.EntitiesDB.QueryEntities<Score>(GameStatTag.Group).Each((ref Score score) =>
        {
            scoreText.text = $"Score: {score.Value}";
        });
        canvasGroup.DOFade(1, .25f);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.DOFade(0, .25f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    private void OnRetryBtnPress()
    {
        gameObject.SetActive(false);
        onRetryAction?.Invoke();
    }
}
