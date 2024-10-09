using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class MoneyGenerator
{
    [SerializeField] private int minMoney, maxMoney;
    [SerializeField] private float distance;
    [SerializeField] private float height;
    [SerializeField] private float z;

    private List<Item> _items = new List<Item>();
    private float _distance;
    private int _randomAmount;
    private int _amount;
    private PoolMoney _poolMoney;

    private int[] _moneyValues = { 500, 100, 50, 25, 10, 5 };

    public void Init(PoolMoney poolMoney)
    {
        _poolMoney = poolMoney;
    }

    public void Generate(List<Vector2> pos, Transform transform)
    {
        int j = 0;
        int layer = 1;
        _amount = 0;
        _randomAmount = Random.Range(minMoney, maxMoney);
        _distance = 0;

        List<int> moneyValues = MoneyValues();

        while (_amount < _randomAmount)
        {
            if (j >= pos.Count - 1)
            {
                layer++;
                _distance = 0;
                j = 0;
            }

            if (pos[j].x >= _distance)
            {
                ItemMoney moneyItem = _poolMoney.Money();
                moneyItem.gameObject.SetActive(true);
                moneyItem.transform.position =
                    transform.TransformPoint(new Vector3(pos[j].x, pos[j].y + (height * layer), z));
                moneyItem.SetAmount(moneyValues[_amount]);
                moneyItem.Take += RemoveItem;
                _items.Add(moneyItem);
                _distance += distance;
                _amount++;
            }

            j++;
        }
    }

    private List<int> MoneyValues()
    {
        List<int> values = new List<int>();
        int num = 0;
        int numValue = Random.Range(0, _moneyValues.Length - 1);

        while (num < _randomAmount)
        {
            values.Add(_moneyValues[numValue]);
            
            if (numValue < _moneyValues.Length - 1)
            {
                int random = Random.Range(0, 2);
                if (random == 1)
                    numValue++;
            }
            
            num++;
        }
        
        return values;
    }

    private void RemoveItem(Item item)
    {
        _items.Remove(item);
        item.Take -= RemoveItem;
    }

    public void ClearItems()
    {
        if(_items.Count == 0)
            return;

        foreach (var item in _items)
        {
            item.gameObject.SetActive(false);
            item.Take -= RemoveItem;
        }

        _items.Clear();
    }
}
