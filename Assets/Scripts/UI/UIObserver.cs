using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObserver : UIBase
{
    [SerializeField] private Player _player;

    private void Awake()
    {
        _player = PlayerManager.Instance.Player;
    }

    void OnEnable()
    {
        if (_player != null)
        {
            _player.OnLevelChanged += HandlePlayerLevelUp;
        }
    }


    void OnDisable()
    {
        if (_player != null)
        {
            _player.OnLevelChanged -= HandlePlayerLevelUp;
        }
    }

    // 각 이벤트에 대한 처리기 메소드들입니다.
    // 플레이어 레벨업 메서드
    private void HandlePlayerLevelUp()
    {
        Debug.Log("GameUIObserver: 플레이어 레벨업 감지! 레벨업 UI를 요청합니다.");
        UIManager.Instance.OpenUI<UISelectItem>();
    }

    // 게임 종료 메서드
    private void HandleGameEnd()
    {
        Debug.Log("GameUIObserver: 게임 종료를 감지! 결과창 UI를 요청합니다.");
        UIManager.Instance.OpenUI<UIResult>();
    }
}
