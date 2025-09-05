using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public List<ItemData> itemDatas = new();

    private void Awake()
    {
        // Resources ���� �� �� ���� ������ �ִ� ��� ItemData Ÿ���� ������ �ҷ��ɴϴ�.
        // "ItemData"�� Resources ���� �Ʒ��� ����Դϴ�. ����θ� Resources ��ü���� ã���ϴ�.
        ItemData[] loadedItems = Resources.LoadAll<ItemData>("ItemData");

        // �ҷ��� �迭�� ����Ʈ�� ��ȯ�Ͽ� �Ҵ��մϴ�.
        // (���û���) Id ������� �����ϸ� �׻� �ϰ��� ������ ������ �� �ֽ��ϴ�.
        itemDatas = loadedItems.OrderBy(item => item.Id).ToList();

        Debug.Log($"[DataManager] {itemDatas.Count}���� ������ �����͸� �ε��߽��ϴ�.");
    }
}
