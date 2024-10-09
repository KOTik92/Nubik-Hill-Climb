using UnityEngine;

public class PoolMoney : MonoBehaviour
{
    [SerializeField] private int poolCount = 3;
    [SerializeField] private bool autoExpand = false;
    [SerializeField] private ItemMoney prefab;

    private PoolMono<ItemMoney> _pool;

    public void Init()
    {
        _pool = new PoolMono<ItemMoney>(prefab, poolCount, transform);
        _pool.AutoExpand = autoExpand;
    }

    public ItemMoney Money()
    {
        return _pool.GetFreeElement();
    }
}
