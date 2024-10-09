using UnityEngine;

namespace Sdk.Saving
{
    public static class SavesFacade
    {
        public static int ABBranch
        {
            get
            {
                var result = Saves.GetInt("ABBranch", -1);
                if (result == -1)
                {
                    result = Random.Range(0, 2);
                    Saves.SetInt("ABBranch", result);
                    Saves.Save();
                }
                return result;
            }
        }
        
        public static bool IsAlreadyAskedToRate
        {
            get => Saves.GetBool("IsAlreadyAskedToRate");
            set
            {
                Saves.SetBool("IsAlreadyAskedToRate", value);
                Saves.Save();
            }
        }

        public static bool IsFirstLaunch
        {
            get => Saves.GetBool("IsFirstLaunch", true);
            set
            {
                Saves.SetBool("IsFirstLaunch", value);
                Saves.Save();
            }
        }
        
        public static string Car
        {
            get => Saves.GetString("Car", "None");
            set
            {
                Saves.SetString("Car", value);
            }
        }
        
        public static string Skin
        {
            get => Saves.GetString("Skin", "None");
            set
            {
                Saves.SetString("Skin", value);
            }
        }
        
        public static string Map
        {
            get => Saves.GetString("Map", "None");
            set
            {
                Saves.SetString("Map", value);
            }
        }
        
        public static int TotalTries
        {
            get => Saves.GetInt("NumberLoadTotalMap", 0);
            set
            {
                Saves.SetInt("NumberLoadTotalMap", value);
            }
        }
        
        public static int Money
        {
            get => Saves.GetInt("Money", 0);
            set
            {
                Saves.SetInt("Money", value);
            }
        }
        
        public static bool TutorialGame
        {
            get => Saves.GetBool("TutorialGame", false);
            set
            {
                Saves.SetBool("TutorialGame", value);
            }
        }
        
        public static bool TutorialMenu
        {
            get => Saves.GetBool("TutorialMenu", false);
            set
            {
                Saves.SetBool("TutorialMenu", value);
            }
        }
        
        public static bool Sound
        {
            get => Saves.GetBool("Sound", false);
            set
            {
                Saves.SetBool("Sound", value);
            }
        }

        public static void SetMeters(string name, int meters)
        {
            Saves.SetInt(name, meters);
        }

        public static int GetMeters(string name)
        {
            return Saves.GetInt(name);
        }
        
        public static void SetNumberLoadMap(string name, int number)
        {
            Saves.SetInt(name, number);
        }
        
        public static int GetNumberLoadMap(string name)
        {
            return Saves.GetInt(name);
        }
        
        public static void SetUpgrade(string name, int number)
        {
            Saves.SetInt(name, number);
        }
        
        public static int GetUpgrade(string name)
        {
            return Saves.GetInt(name);
        }
        
        public static void SetBuy(string name)
        {
            Saves.SetBool(name, true);
        }
        
        public static bool GetBuy(string name)
        {
            return Saves.GetBool(name);
        }
    }
}