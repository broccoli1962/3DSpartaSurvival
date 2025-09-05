using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : UIBase
{
    public Button restartButton;

    protected void Awake()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
        }
    }
}