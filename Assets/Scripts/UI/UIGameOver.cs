using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : UIBase
{
    [SerializeField] private Button RetryBtn;
    [SerializeField] private Button ExitBtn;
    
    private void OnEnable()
    {
        RetryBtn.onClick.AddListener(OnRetry);
        ExitBtn.onClick.AddListener(OnExit);
    }

    private void OnDisable()
    {
        RetryBtn.onClick.RemoveListener(OnRetry);
        ExitBtn.onClick.RemoveListener(OnExit);
    }

    private void OnRetry()
    {
        SceneLoadManager.Instance.LoadScene(ESceneType.Battle);
    }

    private void OnExit()
    {
        SceneLoadManager.Instance.LoadScene(ESceneType.Menu);
    }
}
