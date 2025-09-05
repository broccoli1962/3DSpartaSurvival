using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    private Dictionary<ESceneType, BaseScene> _sceneDictionary = new();
    private BaseScene _currentScene;
    private BaseScene _prevScene;

    Coroutine _coroutine;

    private void Awake()
    {
        _sceneDictionary.Add(ESceneType.Menu, new MenuScene());
        _sceneDictionary.Add(ESceneType.Battle, new BattleScene()); 
    }

    public void LoadScene(ESceneType sceneType)
    {
        if (_coroutine != null) StopCoroutine(_coroutine);

        if (!_sceneDictionary.TryGetValue(sceneType, out BaseScene scene))
        {
            Debug.LogError($"Can't find SceneType : {sceneType}");
            return;
        }

        if (_currentScene == scene) return;

        _coroutine = StartCoroutine(LoadSceneProcess(sceneType));
    }

    IEnumerator LoadSceneProcess(ESceneType sceneType)
    {
        BaseScene scene = _sceneDictionary[sceneType];
        _currentScene?.SceneExit();

        _prevScene = _currentScene;
        _currentScene = scene;

        var operation = SceneManager.LoadSceneAsync(sceneType.ToString());
        operation.allowSceneActivation = false;

        _currentScene.SceneLoad();

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        yield return null;

        _currentScene.SceneEnter();
        _coroutine = null;
    }
}
