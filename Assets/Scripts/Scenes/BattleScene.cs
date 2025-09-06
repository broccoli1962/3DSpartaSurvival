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
        player = ResourceManager.Instance.CreateCharacter<Player>("Player/Player");
        uistatus = UIManager.Instance.GetUI<UIStatus>();

        //vitual 카메라 할당
        GameObject vCamera = ResourceManager.Instance.Create<GameObject>("Prefab/Character/Player/VirtualCamera");
        CinemachineVirtualCamera vCamera2 = vCamera.GetComponent<CinemachineVirtualCamera>();
        vCamera2.Follow = player.transform;
        vCamera2.LookAt = player.transform;

        //웨이브 데이터 할당
        WaveData[] waves = Resources.LoadAll<WaveData>("WaveData/");
        List<WaveData> wavedatas = new();

        for(int i = 0; i<waves.Length; i++)
        {
            wavedatas.Add(waves[i]);
        }
        GameManager.Instance.waves = wavedatas;

        //마그마 할당
        GameManager.Instance.damageZonePrefab = ResourceManager.Instance.Load<GameObject>("Prefab/Map/Envionment/Magma");

        //캐릭터 레벨 할당
        GameManager.Instance.playerTransform = player.transform;

        player.levelText = uistatus.currentLevelTxt;
        player.expSlider = uistatus.currentExpSlider;

        PlayerItemManager.Instance.ClearItems();

        //UIManager.Instance.GetUI<UIResult>();
        //UIManager.Instance.GetUI<UISelectItem>();
        //UIManager.Instance.GetUI<UIPause>();
    }

    public override void SceneExit()
    {
    }

    public override void SceneLoad()
    {
    }
}
