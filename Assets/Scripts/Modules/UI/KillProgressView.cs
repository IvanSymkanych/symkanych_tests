using TMPro;
using UnityEngine;

namespace Modules.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class KillProgressView : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        public void SetScore(int value) =>
            _text.SetText($"Kills: {value}");
        
        private void Awake()
        { 
            _text = GetComponent<TextMeshProUGUI>();
            _text.SetText($"Kills: 0");
        }
    }
}