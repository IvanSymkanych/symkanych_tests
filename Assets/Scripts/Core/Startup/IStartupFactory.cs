using Cysharp.Threading.Tasks;

namespace Core.Startup
{
    public interface IStartupFactory
    {
        UniTask Initialize();
    }
}