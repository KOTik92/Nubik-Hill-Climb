using UnityEngine;

public class PoolFuel : MonoBehaviour
{
    [SerializeField] private int poolCount = 3;
    [SerializeField] private bool autoExpand = false;
    [SerializeField] private ItemFuel prefab;

    private PoolMono<ItemFuel> _pool;

    public void Init()
    {
        _pool = new PoolMono<ItemFuel>(prefab, poolCount, transform);
        _pool.AutoExpand = autoExpand;
    }

    public ItemFuel Fuel()
    {
        return _pool.GetFreeElement();
    }
}