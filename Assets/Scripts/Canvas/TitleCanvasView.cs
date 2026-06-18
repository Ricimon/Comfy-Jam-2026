using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TitleCanvasView : MonoBehaviour
{
    [SerializeField] private Button gameStartBtn;
    [SerializeField] private UnityEngine.CanvasGroup canvasGroup;


    private Action onStartAction;
    public void Init(Action onStartAction)
    {
        this.onStartAction = onStartAction;
        gameStartBtn.onClick.AddListener(OnGameStartBtnPress);
    }
    public void Show()
    {
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



    private void OnGameStartBtnPress()
    {
        onStartAction?.Invoke();
    }


}
