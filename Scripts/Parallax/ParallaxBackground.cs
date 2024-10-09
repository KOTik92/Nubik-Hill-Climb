using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    private Transform _player;
    
    public void Init(Transform player)
    {
        _player = player;
        
        foreach (ParallaxLayer layer in parallaxLayers)
            layer.Init();
        
        SetLayers();
    }

    private void SetLayers()
    {
        parallaxLayers.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }
    private void Update()
    {
        foreach (ParallaxLayer layer in parallaxLayers)
            layer.Move(_player.position.x);
    }
}
