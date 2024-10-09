using System;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class VersionIndicator : MonoBehaviour
    {
        private void Awake()
        {
            var text = GetComponent<TextMeshProUGUI>();
            text.text = $"v{Application.version}";
        }
    }
}