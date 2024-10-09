using System;
using Game.Car;
using Game.Car.AI;
using Game.Car.Noobik;
using Sdk.Saving;
using Skin;
using UnityEngine;

[Serializable]
public class Maps
{
    public MapGenerator map;
    public ParallaxGenerator parallaxGenerator;
}

public class LoadMap : MonoBehaviour
{
    [SerializeField] private PhysicsMaterial2D physicsMaterial2D;
    [SerializeField] private Maps[] maps;
    [SerializeField] private LoadCarAI loadCarAI;

    public MapGenerator SelectedMap => _selectedMap;
    public ParallaxGenerator SelectedParallax => _selectedParallax;
    public int NumberLoadMap => _numberLoadMap;
    public LoadCarAI LoadCarAI => loadCarAI;

    private MapGenerator _selectedMap;
    private ParallaxGenerator _selectedParallax;
    private int _numberLoadMap;
    private int _numberLoadTotalMaps;

    public void Load()
    {
        string typeMap = SavesFacade.Map;

        if (typeMap != "None")
        {
            for (int i = 0; i < maps.Length; i++)
            {
                if (maps[i].map.MapData.typeMap.ToString() == typeMap)
                {
                    _selectedMap = maps[i].map;
                    _selectedParallax = maps[i].parallaxGenerator;
                    _selectedMap.gameObject.SetActive(true);
                    _selectedParallax.gameObject.SetActive(true);
                }
                else
                {
                    maps[i].map.gameObject.SetActive(false);
                    maps[i].parallaxGenerator.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach (var map in maps)
            {
                map.map.gameObject.SetActive(false);
                map.parallaxGenerator.gameObject.SetActive(false);
            }

            _selectedMap = maps[0].map;
            _selectedParallax = maps[0].parallaxGenerator;
            _selectedMap.gameObject.SetActive(true);
            _selectedParallax.gameObject.SetActive(true);
        }
        
        loadCarAI.Load(_selectedMap);

        Physics2D.gravity = new Vector2(0, _selectedMap.MapData.gravity);
        RenderSettings.skybox = _selectedMap.MapData.skybox;
        RenderSettings.ambientSkyColor = _selectedMap.MapData.skyColor;
        RenderSettings.ambientEquatorColor = _selectedMap.MapData.equatorColor;
        RenderSettings.ambientGroundColor = _selectedMap.MapData.groundColor;
        physicsMaterial2D.friction = _selectedMap.MapData.friction;
        _selectedMap.SetPhysicsMaterial(physicsMaterial2D);

        _numberLoadMap = SavesFacade.GetNumberLoadMap($"NumberLoadMap_{_selectedMap.MapData.typeMap}");
        _numberLoadMap++;
        SavesFacade.SetNumberLoadMap($"NumberLoadMap_{_selectedMap.MapData.typeMap}", _numberLoadMap);
        SavesFacade.TotalTries++;
    }
}
