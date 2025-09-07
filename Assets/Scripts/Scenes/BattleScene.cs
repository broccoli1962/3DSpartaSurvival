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
        //맵 생성
        ResourceManager.Instance.CreateMap<GameObject>(Prefab.Map);

        //플레이어, 경험치 스탯, 옵저버 생성
        player = PlayerManager.Instance.Player;
        uistatus = UIManager.Instance.GetUI<UIStatus>();
        UIManager.Instance.GetUI<UIObserver>();

        //vitual 카메라 할당
        GameObject vCamera = ResourceManager.Instance.Create<GameObject>("Prefab/Character/Player/VirtualCamera");
        CinemachineVirtualCamera vCamera2 = vCamera.GetComponent<CinemachineVirtualCamera>();
        vCamera2.Follow = player.transform;
        vCamera2.LookAt = player.transform;

        //웨이브 데이터 할당
        WaveData[] waves = Resources.LoadAll<WaveData>("WaveData/");
        List<WaveData> wavedatas = new();

        for(int i = 0; i<waves.Length-2; i++)
        {
            wavedatas.Add(waves[i]);
        }
        GameManager.Instance.waves = wavedatas;

        //마그마 할당
        GameManager.Instance.damageZonePrefab = ResourceManager.Instance.Load<GameObject>(Path.MapElement + Prefab.Magma);
        Debug.Log(Path.MapElement + Prefab.Magma);

        //게임 오버 UI할당
        //GameManager.Instance._gameOverCanvas = 

        //캐릭터 레벨 할당
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
