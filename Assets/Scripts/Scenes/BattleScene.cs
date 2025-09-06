using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    Player player;
    UIStatus uistatus;
    public override void SceneEnter()
    {
        //�� ����
        ResourceManager.Instance.CreateMap<GameObject>(Prefab.Map);

        //�÷��̾�, ����ġ ����, ������ ����
        player = PlayerManager.Instance.Player;
        uistatus = UIManager.Instance.GetUI<UIStatus>();
        UIManager.Instance.GetUI<UIObserver>();

        //vitual ī�޶� �Ҵ�
        GameObject vCamera = ResourceManager.Instance.Create<GameObject>("Prefab/Character/Player/VirtualCamera");
        CinemachineVirtualCamera vCamera2 = vCamera.GetComponent<CinemachineVirtualCamera>();
        vCamera2.Follow = player.transform;
        vCamera2.LookAt = player.transform;

        //���̺� ������ �Ҵ�
        WaveData[] waves = Resources.LoadAll<WaveData>("WaveData/");
        List<WaveData> wavedatas = new();

        for(int i = 0; i<waves.Length-2; i++)
        {
            wavedatas.Add(waves[i]);
        }
        GameManager.Instance.waves = wavedatas;

        //���׸� �Ҵ�
        GameManager.Instance.damageZonePrefab = ResourceManager.Instance.Load<GameObject>(Path.MapElement + Prefab.Magma);
        Debug.Log(Path.MapElement + Prefab.Magma);

        //���� ���� UI�Ҵ�
        //GameManager.Instance._gameOverCanvas = 

        //ĳ���� ���� �Ҵ�
        GameManager.Instance.playerTransform = player.transform;

        player.levelText = uistatus.currentLevelTxt;
        player.expSlider = uistatus.currentExpSlider;

        PlayerItemManager.Instance.ClearItems();
    }

    public override void SceneExit()
    {
    }

    public override void SceneLoad()
    {
    }
}
