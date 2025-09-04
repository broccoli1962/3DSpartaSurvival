using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISelectItem : UIBase
{
    [field: Header("������ ����")]
    [field: SerializeField] public GameObject levelUpSelectButtonPrefab { get; private set; } // ������ ���� ��ư ������
    [field: SerializeField] public Transform selectButtonParents { get; private set; } // ���� ��ư ��ġ

    [field: Header("�÷��̾� ���� ǥ�� ����")]
    [field: SerializeField] public TextMeshProUGUI totalStatsText { get; private set; } // ���� ���� ���� �ؽ�Ʈ
    [field: SerializeField] public Transform ownedItemIconParent { get; private set; } // �ϴ� ���� ������ ������ ��ġ
    [field: SerializeField] public GameObject ownedItemIconPrefab { get; private set; } // �ϴ� ������ ������

    protected override void OnOpen()
    {
        base.OnOpen();
        Time.timeScale = 0f;

        // ������ ���� �� ǥ��
        ShowChoices();
        // ���� ���� ���� ������Ʈ
        UpdateTotalStatsDisplay();
        // �ϴ� ���� ������ ��� ������Ʈ
        UpdateOwnedItemsDisplay();
    }

    protected override void OnClose()
    {
        base.OnClose();
        Time.timeScale = 1f;

        // ������ �����ߴ� ������ ��ư���� ��� �����Ͽ� ������ ���·� �����
        foreach (Transform child in selectButtonParents)
        {
            Destroy(child.gameObject);
        }
    }

    private void ShowChoices()
    {
        // 1. �ĺ� ������ ����Ʈ ����
        List<ItemData> candidateItems = new List<ItemData>();

        // 2. ��� �������� ������� ���͸�
        foreach (var itemData in DataManager.Instance.itemDatas)
        {
            int currentStack = 0;
            PlayerItemManager.Instance.ownedItems.TryGetValue(itemData, out currentStack);
            if (currentStack < itemData.Stack)
            {
                candidateItems.Add(itemData);
            }
        }

        // 3. �ĺ� ����Ʈ�� �������� ����
        var random = new System.Random();
        var shuffledCandidates = new List<ItemData>(candidateItems);
        for (int i = 0; i < shuffledCandidates.Count; i++)
        {
            int rand = random.Next(i, shuffledCandidates.Count);
            (shuffledCandidates[i], shuffledCandidates[rand]) = (shuffledCandidates[rand], shuffledCandidates[i]);
        }

        // 4. �ִ� 3���� �������� ȭ�鿡 ǥ��
        int choiceCount = Mathf.Min(3, shuffledCandidates.Count);
        for (int i = 0; i < choiceCount; i++)
        {
            ItemData itemToShow = shuffledCandidates[i];
            GameObject buttonGO = Instantiate(levelUpSelectButtonPrefab, selectButtonParents);

            // �� �߿�: ������ ��ư(����)�� ������ ������ �Ѱ��ְ�, Ŭ������ �� � �Լ��� ȣ������ �˷���� �մϴ�.
            // �� ����� ���� ��ư �����տ� UI_ChoiceSlot ���� ������ ��ũ��Ʈ�� �ʿ��մϴ�.
            UIChoiceSlot slot = buttonGO.GetComponent<UIChoiceSlot>();
            slot.Initialize(itemToShow, this);
        }
    }

    private void UpdateTotalStatsDisplay()
    {
        // PlayerInventory�� OwnedItems�� ������� ���� ������ ����ϰ�
        // totalStatsText.text�� �ݿ��ϴ� ����
        float totalPower = 0f;
        float totalAttackRange = 0f;

        foreach (var itemPair in PlayerItemManager.Instance.ownedItems)
        {
            ItemData item = itemPair.Key;
            int count = itemPair.Value;
            totalPower += item.Power * count;
            totalAttackRange += item.AttackRange * count;
        }

        totalStatsText.text = $"";
    }

    private void UpdateOwnedItemsDisplay()
    {
        // ownedItemIconParent �Ʒ��� ���� �����ܵ��� ��� ����
        foreach (Transform child in ownedItemIconParent) Destroy(child.gameObject);

        // PlayerInventory�� OwnedItems�� ������� �����ܰ� ������ ǥ��
        foreach (var itemPair in PlayerItemManager.Instance.ownedItems)
        {
            GameObject iconGO = Instantiate(ownedItemIconPrefab, ownedItemIconParent);
            // ... iconGO�� �̹����� �ؽ�Ʈ�� itemPair.Key(ItemData), itemPair.Value(����)�� ���� ...
        }
    }

    // ������ ChoiceItemButton()���� ����μ̴� �κ��Դϴ�.
    // ���� �� ������ ������ ���� �� �Լ��� ȣ���ϰ� �����, � �������� ���õǾ����� ��Ȯ�� �� �� �ֽ��ϴ�.
    public void OnItemSelected(ItemData selectedItem)
    {
        // 1. ���õ� �������� �κ��丮�� �߰�
        PlayerItemManager.Instance.AddItem(selectedItem);

        // 2. ������ ������ �������Ƿ� UI�� ����
        CloseUI();
    }
}
