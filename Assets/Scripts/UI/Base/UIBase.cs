using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
        OnOpen();
    }

    public virtual void CloseUI()
    {
        OnClose();
        gameObject.SetActive(false);
    }


    // OnEnable, OnDisable 대신 사용 (호출 순서의 명확함)
    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }
}
