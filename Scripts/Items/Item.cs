using System;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public event Action<Item> Take;
    
    protected void OnTake()
    {
        Take?.Invoke(this);
    }
}
