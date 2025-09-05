using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();
    private Transform _canvasTransform; // UI들이 생성될 부모 캔버스
    // Todo : 어떻게 딕셔너리에 UI를 넣어둘지 고민중

    // UI를 여는 메서드
    public void OpenUI<T>() where T : UIBase
    {
        var ui = GetUI<T>();
        if (ui != null)
        {
            ui?.OpenUI();
        }
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

        // 1. 딕셔너리에 있는지 먼저 확인 (이미 만들어둔게 있다면 그걸 사용)
        if (uiDictionary.TryGetValue(uiName, out UIBase existingUI) && existingUI != null)
        {
            return existingUI as T;
        }

        // 2. 딕셔너리에 없다면, Resources 폴더에서 프리팹을 찾아 새로 생성
        return CreateUI<T>(uiName);
    }

    // UI를 새로 생성하고 딕셔너리에 등록하는 내부 메서드
    private T CreateUI<T>(string uiName) where T : UIBase
    {
        // UI를 담을 최상위 캔버스를 찾아옵니다. (최초 한번만 실행)
        if (_canvasTransform == null)
        {
            // FindObjectOfType은 비용이 비싸므로, 정말 필요할 때 한번만 호출합니다.
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("UI를 담을 Canvas가 씬에 없습니다! Canvas를 먼저 생성해주세요.");
                return null;
            }
            _canvasTransform = canvas.transform;
        }

        // Resources/UI/{uiName} 경로에서 프리팹을 로드합니다.
        string prefabPath = $"UI/{uiName}";
        GameObject uiPrefab = Resources.Load<GameObject>(prefabPath);

        if (uiPrefab == null)
        {
            Debug.LogError($"{prefabPath} 에서 UI 프리팹을 찾을 수 없습니다! 경로와 프리팹 이름을 확인해주세요.");
            return null;
        }

        // 프리팹을 캔버스 자식으로 생성(Instantiate)
        GameObject uiInstance = Instantiate(uiPrefab, _canvasTransform);
        T uiComponent = uiInstance.GetComponent<T>();

        // 생성된 UI를 딕셔너리에 등록
        uiDictionary[uiName] = uiComponent;

        uiInstance.SetActive(false);

        return uiComponent;
    }

    // 찾으려는 UI가 딕셔너리에 존재하는지 확인하는 메서드
    public bool IsExistUI<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        return uiDictionary.ContainsKey(uiName) && uiDictionary[uiName] != null;
    }
}
