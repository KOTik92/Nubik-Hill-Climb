using System;
using System.Collections.Generic;
using UnityEngine;

public enum TypeItems
{
    None,
    Money,
    Fuel
}

[Serializable]
public class Items
{
    public TypeItems Type;
    public int StartChunk;
    public int MaxChunk;
}

[Serializable]
public class ItemsGenerator
{
    [SerializeField] private Items[] itemsArray;

    private List<int> _amountChunks = new List<int>();

    public void Init()
    {
        for (int i = 0; i < itemsArray.Length; i++)
            _amountChunks.Add(itemsArray[i].StartChunk);
    }

    public TypeItems CheckChunk(int amountChunk)
    {
        for (int i = 0; i < itemsArray.Length; i++)
        {
            if (amountChunk >= _amountChunks[i])
            {
                _amountChunks[i] += itemsArray[i].MaxChunk;
                return itemsArray[i].Type;
            }
        }

        return TypeItems.None;
    }
}
