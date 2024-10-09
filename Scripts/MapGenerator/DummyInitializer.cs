using System;
using UnityEngine;

namespace Game
{
    public class DummyInitializer : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator;
        
        private void Start()
        {
            mapGenerator.Init(transform, null);
        }
    }
}