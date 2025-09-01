using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();

    // Todo : ��� ��ųʸ��� UI�� �־���� �����

    // UI�� ���� �޼���
    public void OpenUI<T>() where T : UIBase
    {
        var ui = GetUI<T>();
        ui?.OpenUI();
    }

    // UI�� �ݴ� �޼���
    public void CloseUI<T>() where T : UIBase
    {
        if (IsExistUI<T>())
        {
            var ui = GetUI<T>();
            ui?.CloseUI();
        }
    }

    // ��ųʸ����� UI�� ã�ƿ��� �޼���
    public T GetUI<T>() where T : UIBase
    {
        UIBase ui;
        string uiName = typeof(T).Name;

        if(IsExistUI<T>())
        {
            ui = uiDictionary[uiName];
        }
        else
        {
            Debug.Log($"{uiName}�� ��ųʸ��� �������� �ʽ��ϴ�.");
            ui = null;
            // ���⼭ ui�� ����� ���� �ƴ��� �����ؾ� �մϴ�.
        }

        return ui as T;
    }

    // ã������ UI�� ��ųʸ��� �����ϴ��� Ȯ���ϴ� �޼���
    public bool IsExistUI<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        return uiDictionary.TryGetValue(uiName, out var ui) && ui != null;
    }
}
