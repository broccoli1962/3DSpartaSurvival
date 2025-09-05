using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIObserver : MonoBehaviour
{
    [SerializeField] private Player _player;

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

    // �� �̺�Ʈ�� ���� ó���� �޼ҵ���Դϴ�.
    // �÷��̾� ������ �޼���
    private void HandlePlayerLevelUp()
    {
        Debug.Log("GameUIObserver: �÷��̾� ������ ����! ������ UI�� ��û�մϴ�.");
        UIManager.Instance.OpenUI<UISelectItem>();
    }

    // ���� ���� �޼���
    private void HandleGameEnd()
    {
        Debug.Log("GameUIObserver: ���� ���Ḧ ����! ���â UI�� ��û�մϴ�.");
        UIManager.Instance.OpenUI<UIResult>();
    }
}
