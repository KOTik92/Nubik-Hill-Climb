using Sdk.Saving;
using UnityEngine;
using UnityEngine.UI;

public class TurningOffSound : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private SwitchSprite switchSprite;

    public bool IsSoundTurnedOff => _isSoundTurnedOff;
    
    private bool _isSoundTurnedOff;
    private bool _isSwitchSound;
    
    private void Awake()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { Activator(); });
        _isSoundTurnedOff = SavesFacade.Sound;
        _isSwitchSound = true;

        Switch();
    }

    public void Activator()
    {
        _isSoundTurnedOff = !_isSoundTurnedOff;
        SavesFacade.Sound = _isSoundTurnedOff;
        Switch();
    }

    private void Switch()
    {
        switchSprite.Switch(_isSoundTurnedOff);
        
        if(!_isSwitchSound)
            return;
        
        AudioListener.volume = !_isSoundTurnedOff ? 1 : 0;
    }

    public void SetSwitch(bool isSwitch)
    {
        _isSwitchSound = isSwitch;
    }
}
