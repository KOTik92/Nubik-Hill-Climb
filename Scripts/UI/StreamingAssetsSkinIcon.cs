using Skin;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(SkinItem))]
    public class StreamingAssetsSkinIcon : StreamingAssetsItemIcon
    {
        protected override string Path => $"{Application.streamingAssetsPath}/Skins/";

        private void Start()
        {
            var typeSkin = GetComponent<SkinItem>().TypeSkin;
            LoadIcon(typeSkin.ToString());
        }
    }
}