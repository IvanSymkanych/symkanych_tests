using Cysharp.Threading.Tasks;

namespace Core.Service
{
    public interface ISceneLoadService
    {
        UniTask LoadSceneAsync(string name);
    }
}