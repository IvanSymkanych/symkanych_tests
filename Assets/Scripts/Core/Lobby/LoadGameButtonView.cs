using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Lobby
{
    public class LoadGameButtonView : MonoBehaviour
    {
        public event Action OnPlayButtonClick;

        private Button _playButton;

        public void Init()
        {
            _playButton = GetComponent<Button>();
            AddButtonListener();
        }

        public void CleanUp() => RemoveButtonListener();
        
        private void PlayButtonClick() => OnPlayButtonClick?.Invoke();
        private void AddButtonListener() => _playButton.onClick.AddListener(PlayButtonClick);
        private void RemoveButtonListener() => _playButton.onClick.RemoveAllListeners();
    }
}