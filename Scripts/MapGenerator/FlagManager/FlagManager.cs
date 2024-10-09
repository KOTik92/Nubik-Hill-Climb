using Game;
using Game.Car;
using Lean.Localization;
using Sdk.RemoteConfig;
using UnityEngine;
using Wallet;

public class FlagManager : MonoBehaviour
{
    [SerializeField] private Flag flagPrefab;
    [SerializeField] private int startChunk;
    [SerializeField] private int maxChunk;
    [SerializeField] private LoadCar loadCar;
    [SerializeField] private LoadMap loadMap;
    [SerializeField] private WalletGame walletGame;
    [SerializeField] private int award;
    [SerializeField] private TextMoney textMoney;

    private int _amountChunk;

    public void Init()
    {
        _amountChunk += startChunk;
    }
    
    public bool CheckChunk(int amountChunk)
    {
        if (!FlagController.IsAIEnabled)
            return false;
        
        if (amountChunk >= _amountChunk)
        {
            _amountChunk += maxChunk;
            return true;
        }

        return false;
    }

    public Flag GetFlag()
    {
        Flag flag = Instantiate(flagPrefab);
        flag.Init(loadCar.SelectedCar, loadMap.LoadCarAI.SelectedCarAI);
        flag.OnWin += AwardIssuance;

        return flag;
    }

    private void AwardIssuance(bool isFirst, Flag flag)
    {
        flag.OnWin -= AwardIssuance;

        int tempAward = isFirst ? award * 2 : award;
        tempAward *= FlagController.IncomeMultiplier;
        
        walletGame.AddMoney(tempAward);
        textMoney.ActivateText(LeanLocalization.GetTranslationText("award") + " +" + tempAward);
    }
}
