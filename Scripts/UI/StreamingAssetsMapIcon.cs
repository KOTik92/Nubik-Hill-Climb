using Menu.Map;
using UnityEngine;

namespace UI
{
    public class StreamingAssetsMapIcon : StreamingAssetsItemIcon
    {
        protected override string Path => $"{Application.streamingAssetsPath}/Maps/";
        
        private void Start()
        {
            var typeMap = GetComponent<MapItem>().TypeMap;
            LoadIcon(typeMap.ToString());
        }
    }
}