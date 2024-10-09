using System;
using Sdk.Saving;

namespace Wallet
{
    [Serializable]
    public class Wallet
    {
        public int MoneyValue
        {
            get
            {
                int data = SavesFacade.Money;
                return data;
            }
            set
            {
                SavesFacade.Money = value;
                MoneyAmountChanged?.Invoke();
            }
        }
        
        public event Action MoneyAmountChanged;
    }
}