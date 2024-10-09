using Sdk.RemoteConfig;
using TMPro;
using UnityEngine;
using Wallet;

public class ItemMoney : Item
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject obj;
    
    private int _amount;
    private bool _isTake;

    private void OnEnable()
    {
        _isTake = false;
        obj.SetActive(true);
        audioSource.Stop();
        if(particleSystem.isPlaying)
            particleSystem.Stop();
    }
    
    public void SetAmount(int amount)
    {
        _amount = amount * FlagController.IncomeMultiplier;
        textMeshProUGUI.text = ConvertingNumberToText.Conversion(_amount);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out MoneyPlayer player) && !_isTake)
        {
            if (player.AddMoney(_amount))
            {
                OnTake();
                _isTake = true;
                EffectTake();
            }
        }
    }
    
    private void EffectTake()
    {
        obj.SetActive(false);
        audioSource.Play();
        if(particleSystem.isStopped)
            particleSystem.Play(true);
        
        Invoke("DeactivateItem", 1);
    }

    private void DeactivateItem()
    {
        gameObject.SetActive(false);
    }
}
