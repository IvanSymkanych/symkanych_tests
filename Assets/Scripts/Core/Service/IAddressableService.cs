using Cysharp.Threading.Tasks;

namespace Core.Service
{
    public interface IAddressableService
    {
        UniTask<T> LoadAssetAsync<T>(string key) where T : class;
    }
}