using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public ESceneType sceneType;
    private void Start()
    {
        SceneLoadManager.Instance.LoadScene(sceneType);
    }
}
