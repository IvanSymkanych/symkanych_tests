using TMPro;
using UnityEngine;

namespace Core.Lobby
{
    public class HighScoreView : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI scoreText;

        public void SetScore(string value) => scoreText.SetText(value);
    }
}