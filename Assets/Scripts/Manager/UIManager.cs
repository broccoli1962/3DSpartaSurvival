using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();
    private Transform _canvasTransform; // UI들이 생성될 부모 캔버스

    private void Awake()
    {
        //// 싱글톤 인스턴스를 '확정'시키는 과정
        //// Instance를 한번 호출함으로써, _instance 변수에 이 오브젝트가 할당되도록 합니다.
        //var checkInstance = Instance;

        //// 만약 이미 다른 UIManager Instance가 존재하는데, 내가 또 생긴 경우 (중복 방지)
        //if (_instance != this)
        //{
        //    Destroy(gameObject); // 나는 스스로를 파괴한다.
        //    return;
        //}

        //// 내가 유일한 Instance임이 확인되었으므로, 나 자신을 파괴되지 않도록 설정한다.
        //DontDestroyOnLoad(gameObject);

        // 이제 씬 로드 이벤트를 구독합니다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


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
            GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");
            //Canvas canvas = FindAnyObjectByType<Canvas>();
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _canvasTransform = null;
        Debug.Log($"[UIManager] '{scene.name}' 씬 로딩 완료 감지!");

        // 새로 로드된 씬의 이름이 "UIBattle_Test" 라면,
        if (scene.name == "UIBattle_Test")
        {
            // 이 씬에서 필요한 UI들을 미리 딕셔너리에 등록(캐싱)합니다.
            Debug.Log("[UIManager] 배틀 씬에 필요한 UI들을 미리 로드합니다.");
            GetUI<UIOption>();
            GetUI<UIResult>();
            GetUI<UISelectItem>();
            PlayerItemManager.Instance.ClearItems();
        }
        // 다른 씬에서 필요하면 여기서 구현
    }
}
