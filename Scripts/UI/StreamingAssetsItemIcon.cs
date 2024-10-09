using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UI
{
    public abstract class StreamingAssetsItemIcon : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        protected abstract string Path { get; }
        
        protected void LoadIcon(string iconName)
        {
            Debug.Log($"Loading icon: {iconName}, from: {Path}");
#if (UNITY_WEBGL || UNITY_ANDROID) && !UNITY_EDITOR
            var url = $"{Path}/{iconName}.png";
            StartCoroutine(DownloadImage(url));
#else
            var path = $"{Path}/{iconName}.png";
            SetImageFromPath(path);
#endif
        }
        
#if (UNITY_WEBGL || UNITY_ANDROID) && !UNITY_EDITOR
        private IEnumerator DownloadImage(string url)
        {
            var webRequest = new UnityWebRequest(url);
            webRequest.downloadHandler = new DownloadHandlerTexture();
            var timer = Time.time;
            yield return webRequest.SendWebRequest();
            timer = Time.time - timer;
            Debug.Log($"{url}\nDownloaded in: {timer} seconds");
            
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                var texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
        }
#else
        private void SetImageFromPath(string path)
        {
            var texture = new Texture2D(2, 2);
            texture.LoadImage(System.IO.File.ReadAllBytes(path));
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
#endif
    }
}