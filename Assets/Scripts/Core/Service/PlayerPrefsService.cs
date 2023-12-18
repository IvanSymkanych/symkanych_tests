using UnityEngine;

namespace Core.Service
{
    public class PlayerPrefsService : IPlayerPrefsService
    {
        public void Save() => PlayerPrefs.Save();
        
        public void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);
        
        public int GetInt(string key) => PlayerPrefs.GetInt(key);
    }
}