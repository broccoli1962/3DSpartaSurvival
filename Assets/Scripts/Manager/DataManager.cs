using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// DataManager 오브젝트를 생성하고 이 스크립트를 컴포넌트로 추가한다.
/// DataManager는 전투 씬에 배치하고 전투씬에 로딩할 때 리소스 폴더에서 ItemData를 받아온다.
/// </summary>
public class DataManager : Singleton<DataManager>
{
    public List<ItemData> itemDatas = new();

    private void Awake()
    {
        // Resources 폴더 및 그 하위 폴더에 있는 모든 ItemData 타입의 에셋을 불러옵니다.
        // "ItemData"는 Resources 폴더 아래의 경로입니다. 비워두면 Resources 전체에서 찾습니다.
        ItemData[] loadedItems = Resources.LoadAll<ItemData>("ItemData");

        // 불러온 배열을 리스트로 변환하여 할당합니다.
        // (선택사항) Id 순서대로 정렬하면 항상 일관된 순서를 유지할 수 있습니다.
        itemDatas = loadedItems.OrderBy(item => item.Id).ToList();

        Debug.Log($"[DataManager] {itemDatas.Count}개의 아이템 데이터를 로드했습니다.");
    }
}
