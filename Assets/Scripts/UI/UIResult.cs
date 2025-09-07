using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 외부에서 호출을 편하게 하기 위해 게임의 결과 상태를 받아서 처리하는 인터페이스
//
public interface IDataReceiver<T>
{
    /// <summary>
    /// 데이터를 전달받아 처리합니다.
    /// </summary>
    void ReceiveData(T data);
}

// 호출하는 상황
// 플레이어가 죽는 "Die" 상태일 때,
// 보스를 잡고 게임을 "클리어"한 상황일 때
public class UIResult : UIBase, IDataReceiver<GameResultData>
{
    [field: Header("버튼 연결")]
    [field: SerializeField] public Button btnRestart { get; private set; }
    [field: SerializeField] public Button btnMainMenu { get; private set; }

    [field: Header("UI 요소 연결")]
    [field: SerializeField] public TextMeshProUGUI titleText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI playtimeText { get; private set; }
    [field: SerializeField] public Image resultIcon { get; private set; }


    [field: Header("결과 아이콘 스프라이트")]
    [field: SerializeField] public Sprite victoryIcon { get; private set; }
    [field: SerializeField] public Sprite defeatIcon { get; private set; }

    protected override void OnOpen()
    {
        base.OnOpen();
        Time.timeScale = 0f;

        // 버튼 리스너 등록
        btnRestart?.onClick.AddListener(OnClickRestart);
        btnMainMenu?.onClick.AddListener(OnClickMainMenu);
    }

    protected override void OnClose()
    {
        base.OnClose();
        Time.timeScale = 1f;

        // 버튼 리스너 해제
        btnRestart?.onClick.RemoveAllListeners();
        btnMainMenu?.onClick.RemoveAllListeners();
    }

    // Player.Die()에서 호출해야 함
    //GameResultData resultData = new GameResultData
    //{
    //    ResultType = EGameResultType.Defeat,
    //    Playtime = this.Playtime // GameManager가 측정하고 있는 플레이 시간
    //};
    // UIManager.Instance.OpenUI<UIResult>(resultData);
public void ReceiveData(GameResultData data)
    {
        // 이 안의 내용은 이전에 만들었던 override OpenUI(object data)의 로직과 동일합니다.
        if (data.ResultType == EGameResultType.Victory)
        {
            titleText.text = "승리";
            resultIcon.sprite = victoryIcon;
        }
        else
        {
            titleText.text = "패배";
            resultIcon.sprite = defeatIcon;
        }

        int minutes = (int)data.Playtime / 60;
        int seconds = (int)data.Playtime % 60;
        playtimeText.text = $"{minutes:D2}:{seconds:D2}";
    }

    // ======================================
    // 버튼 클릭 이벤트
    private void OnClickRestart()
    {
        GameManager.Instance.RestartGame();
    }

    private void OnClickMainMenu()
    {
        GameManager.Instance.MainMenu();
    }
}
