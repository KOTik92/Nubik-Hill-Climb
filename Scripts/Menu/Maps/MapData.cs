using System.Collections.Generic;
using Game.Car.AI;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Game/New Map")]
public class MapData : ScriptableObject
{
    public TypeMap typeMap;
    public int money;
    [Space]
    public float gravity;
    public float friction;
    public Material skybox;

    [ColorUsage(true, true)] public Color skyColor = Color.white;
    [ColorUsage(true, true)] public Color equatorColor = Color.white;
    [ColorUsage(true, true)] public Color groundColor = Color.white;

    [Header("AI")] 
    public TypeCar typeCar;
    public TypeSkin typeSkin;
    public float speed;
    public float speedRotation;
    public float grip;
    public List<Settings> settings;
}
