using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();

    // Todo : 어떻게 딕셔너리에 UI를 넣어둘지 고민중

    // UI를 여는 메서드
    public void OpenUI<T>() where T : UIBase
    {
        var ui = GetUI<T>();
        ui?.OpenUI();
    }

    // UI를 닫는 메서드
    public void CloseUI<T>() where T : UIBase
    {
        if (IsExistUI<T>())
        {
            var ui = GetUI<T>();
            ui?.CloseUI();
        }
    }

    // 딕셔너리에서 UI를 찾아오는 메서드
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
            Debug.Log($"{uiName}이 딕셔너리에 존재하지 않습니다.");
            ui = null;
            // 여기서 ui를 만들어 줄지 아닐지 결정해야 합니다.
        }

        return ui as T;
    }

    // 찾으려는 UI가 딕셔너리에 존재하는지 확인하는 메서드
    public bool IsExistUI<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        return uiDictionary.TryGetValue(uiName, out var ui) && ui != null;
    }
}
