using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();
    private Transform _canvasTransform;

    [Header("플로팅 텍스트 설정")]
    public GameObject floatingTextPrefab;

    public void OpenUI<T>() where T : UIBase
    {
        GetUI<T>()?.OpenUI();
    }

    public void CloseUI<T>() where T : UIBase
    {
        GetUI<T>()?.CloseUI();
    }

    public T GetUI<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        if (uiDictionary.TryGetValue(uiName, out UIBase existingUI) && existingUI != null)
        {
            return existingUI as T;
        }
        return CreateUI<T>(uiName);
    }

    private T CreateUI<T>(string uiName) where T : UIBase
    {
        if (_canvasTransform == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                return null;
            }
            _canvasTransform = canvas.transform;
        }

        string prefabPath = $"UI/{uiName}";
        GameObject uiPrefab = Resources.Load<GameObject>(prefabPath);
        if (uiPrefab == null)
        {
            return null;
        }

        GameObject uiInstance = Instantiate(uiPrefab, _canvasTransform);

        T uiComponent = uiInstance.GetComponent<T>();
        uiDictionary[uiName] = uiComponent;
        uiInstance.SetActive(false);

        return uiComponent;
    }

    public void ShowFloatingText(string text, Vector3 worldPosition)
    {
        if (floatingTextPrefab == null || _canvasTransform == null) 
            return;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        GameObject textInstance = Instantiate(floatingTextPrefab, _canvasTransform);

        textInstance.transform.position = screenPosition;
        textInstance.GetComponent<FloatingText>().SetText(text);
    }
}