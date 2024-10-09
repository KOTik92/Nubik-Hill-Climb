using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Car", menuName = "Game/New Car")]
public class CarData : ScriptableObject
{
    public TypeCar typeCar;
    public int cost;
    [Space]
    public float initialSpeed;
    public float initialRotationSpeed;
    public float initialGrip;
    
    [Header("Upgrade")]
    public float speedUpgradeStep;
    public float rotationSpeedUpgradeStep;
    public float gripUpgradeStep;
    
    [Header("Max Levels")]
    public int maxSpeedLevel;
    public int maxRotationLevel;
    public int maxGripLevel;
    
    [Header("Upgrade Costs")]
    public CostCurve speedUpgradeCost;
    public CostCurve rotationUpgradeCost;
    public CostCurve gripUpgradeCost;
}

//https://docs.google.com/spreadsheets/d/1IvzpCpau-r0x8vHPt-oSM-yZmxG4jsWgmRSEQ1-5WHE/edit?usp=sharing 
[Serializable]
public class CostCurve
{
    public int initialCost;
    public float costMultiplier;
    public int costStep;
    
    public int GetCost(int level)
    {
        return (int) (initialCost + costStep * Mathf.Pow(costMultiplier, level));
    }
}