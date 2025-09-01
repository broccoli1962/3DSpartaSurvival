using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject, ICollectable
{
    [field: SerializeField] public Sprite Icon {  get; private set; }
    [field: SerializeField] public int Id { get; private set; }


    public Item Clone()
    {
        Item newItem = Instantiate(this);
        return newItem;
    }

    public virtual void AddItem() {}
}
