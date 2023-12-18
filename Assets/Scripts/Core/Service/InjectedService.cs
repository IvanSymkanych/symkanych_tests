using UnityEngine;
using Zenject;

namespace Core.Service
{
    public class InjectedService : IInjectService
    {
       private DiContainer _container;

        public InjectedService(DiContainer container) =>
            _container = container;

        public void Set(DiContainer container) => _container = container;
        
        public void Inject(object injectable) => _container.Inject(injectable);

        public GameObject InstantiatePrefab(Object prefab) => _container.InstantiatePrefab(prefab);
        
        public GameObject InstantiatePrefab(Object prefab, Transform parentTransform) =>
            _container.InstantiatePrefab(prefab, parentTransform);
        
        public GameObject InstantiatePrefab(Object prefab, Vector3 position, Quaternion rotation) =>
            _container.InstantiatePrefab(prefab, position, rotation, null);
        
        public GameObject InstantiatePrefab(Object prefab, Vector3 position, Quaternion rotation, Transform parentTransform) =>
            _container.InstantiatePrefab(prefab, position, rotation, parentTransform);
        
        public T InstantiatePrefabAndComponent<T>(Object prefab, out GameObject instance) where T : Component
        {
            instance = InstantiatePrefab(prefab);
            var component = InstantiateSingleComponent<T>(instance);
            return component;
        }
        
        public T InstantiatePrefabForComponent<T>(T prefab) where T : Object => _container.InstantiatePrefabForComponent<T>(prefab);
        
        public T InstantiatePrefabForComponent<T>(Object prefab, Transform parentTransform) =>
            _container.InstantiatePrefabForComponent<T>(prefab, parentTransform);
        
        public T InstantiatePrefabForComponent<T>(Object prefab, Vector3 position, Quaternion rotation, Transform parentTransform) =>
            _container.InstantiatePrefabForComponent<T>(prefab, position, rotation, parentTransform);
        
        public GameObject CreateEmptyGameObject(string name) => _container.CreateEmptyGameObject(name);

        public T InstantiateSingleComponent<T>(GameObject gameObject) where T : Component
        {
            InstantiateComponentInternal<T>(gameObject, out var component).AsSingle().NonLazy();
            return component;
        }
        
        public T InstantiateTransientComponent<T>(GameObject gameObject) where T : Component
        {
            InstantiateComponentInternal<T>(gameObject, out var component).AsTransient().NonLazy();
            return component;
        }
        
        private ScopeConcreteIdArgConditionCopyNonLazyBinder InstantiateComponentInternal<T>(GameObject gameObject, out T component) where T : Component
        {
            component = _container.InstantiateComponent<T>(gameObject);
            return _container.BindInterfacesAndSelfTo<T>().FromInstance(component);
        }
    }
}