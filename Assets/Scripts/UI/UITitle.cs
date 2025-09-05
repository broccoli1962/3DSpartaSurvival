using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITitle : UIBase
{
    [field: SerializeField] public Button btnGameStart { get; private set; }
    [field: SerializeField] public Button btnOption { get; private set; }


    // Title UI를 호출했을 때 초기화 동작을 진행하는 메서드
    protected override void OnOpen()
    {
        base.OnOpen();
        btnGameStart?.onClick.AddListener(OnClickBtnGameStart);
        btnOption?.onClick.AddListener(OnClickBtnOption);
        // cur, best 스코어 점수 받아오기
        // 받아온 점수를 UI로 보여주는 메서드
    }

    protected override void OnClose()
    {
        base.OnClose();
        btnGameStart?.onClick.RemoveListener(OnClickBtnGameStart);
        btnOption?.onClick.RemoveListener(OnClickBtnOption);
    }
    
    public void OnClickBtnGameStart()
    {
        // 씬 로드
        Debug.Log("게임 시작 버튼 클릭! UIBattle_Test 씬을 로드합니다.");
        SceneManager.LoadScene("UIBattle_Test");
    }

    public void OnClickBtnOption()
    {
        // UIOption 을 오픈
        //UIManager.Instance.OpenUI<UIOption>();
    }
}
