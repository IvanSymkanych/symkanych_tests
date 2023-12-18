using UnityEngine;
using Zenject;

namespace Core.Service
{
    public interface IInjectService
    {
        void Set(DiContainer container);
        void Inject(object injectable);

        GameObject InstantiatePrefab(Object prefab);
        GameObject InstantiatePrefab(Object prefab, Transform parentTransform);
        GameObject InstantiatePrefab(Object prefab, Vector3 position, Quaternion rotation);
        GameObject InstantiatePrefab(Object prefab, Vector3 position, Quaternion rotation, Transform parentTransform);
        
        T InstantiatePrefabForComponent<T>(T prefab) where T : Object;
        T InstantiatePrefabAndComponent<T>(Object prefab, out GameObject instance) where T : Component;
        T InstantiatePrefabForComponent<T>(Object prefab, Transform parentTransform);
        T InstantiatePrefabForComponent<T>(Object prefab, Vector3 position, Quaternion rotation, Transform parentTransform);
        
        GameObject CreateEmptyGameObject(string name);
        T InstantiateSingleComponent<T>(GameObject gameObject) where T : Component;
        T InstantiateTransientComponent<T>(GameObject gameObject) where T : Component;
    }
}