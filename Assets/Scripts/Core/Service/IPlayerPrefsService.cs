namespace Core.Service
{
    public interface IPlayerPrefsService
    {
        void Save();
        void SetInt(string key, int value);
        int GetInt(string key);
    }
}