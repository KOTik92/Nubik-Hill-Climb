using System;
using UnityEngine;

[Serializable]
public class FuelGenerator
{
    [SerializeField] private float height;
    [SerializeField] private float z;
    [SerializeField] private float distance;

    private Item _item;
    private PoolFuel _poolFuel;
    
    public void Init(PoolFuel poolFuel)
    {
        _poolFuel = poolFuel;
    }

    public void Generate(Vector2 pos, Transform transform)
    {
        ItemFuel fuelItem = _poolFuel.Fuel();
        fuelItem.gameObject.SetActive(true);
        fuelItem.transform.position = transform.TransformPoint(new Vector3(pos.x, pos.y + height, z));
        fuelItem.Take += RemoveItem;
        _item = fuelItem;
    }
    
    private void RemoveItem(Item item)
    {
        item.Take -= RemoveItem;
        _item = null;
    }

    public void ClearItem()
    {
        if(_item == null)
            return;
        
        _item.gameObject.SetActive(false);
        _item = null;
    }
}
