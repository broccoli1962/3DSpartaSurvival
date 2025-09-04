using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T> : IState where T : MonoBehaviour
{
    protected T Component;

    public BaseState(T component)
    {
        Component = component;
    }

    public abstract void Start();
    public abstract void Update();
    public abstract void HandleUpdate();
    public abstract void PhysicsUpdate();
    public abstract void End();
    public virtual void StartAnimation(int animationHash)
    {
        if(Component.TryGetComponent<Animator>(out Animator anim))
        {
            anim.SetBool(animationHash, true);
        }
    }
    public virtual void StopAnimation(int animationHash)
    {
        if (Component.TryGetComponent<Animator>(out Animator anim))
        {
            anim.SetBool(animationHash, false);
        }
    }
}