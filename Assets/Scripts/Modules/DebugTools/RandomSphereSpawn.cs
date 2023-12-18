using System.Collections;
using Core.Service;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Modules.DebugTools
{
    public class RandomSphereSpawn : MonoBehaviour
    {
        
        // Ваш SphereCollider
        public SphereCollider sphereCollider;

        // Шар (Layer) для перевірки
        public LayerMask targetLayer;

        // Метод, який повертає випадкову точку в межах SphereCollider
        private Vector3 GetRandomPointInSphereCollider()
        {
            float randomRadius = Random.Range(0, sphereCollider.radius);
            float randomAngle = Random.value * 2 * Mathf.PI;

            // Випадкові координати в межах SphereCollider
            float x = randomRadius * Mathf.Cos(randomAngle);
            float z = randomRadius * Mathf.Sin(randomAngle);

            // Отримання поточної позиції об'єкта
            Vector3 currentPosition = transform.position;

            // Повертаємо знайдену точку
            return currentPosition + new Vector3(x, 0, z);
        }

        // Корутина для виклику методу кожні 10 секунд
        private IEnumerator SpawnSphereEvery10Seconds()
        {
           // _arenaService.Initialize();
            while (true)
            {
                // Отримуємо випадкову точку в межах SphereCollider
                Vector3 randomPointInCollider = Vector3.down;

            //   NavMesh.SamplePosition(randomPointInCollider, out var navMeshHit, 10.0f, NavMesh.AllAreas);

                // Повертаємо знайдену точку

                // Перевіряємо, чи в даній точці є об'єкти з потрібним шаром (Layer)
                Collider[] colliders = Physics.OverlapSphere(randomPointInCollider, 1.0f, targetLayer);

                // Якщо немає об'єктів з вказаним шаром, створюємо сферу
                if (colliders.Length == 0)
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = randomPointInCollider;
                    sphere.transform.localScale = Vector3.one / 10;
                }

                // Чекаємо 10 секунд перед наступною ітерацією
                yield return new WaitForSeconds(1.0f);
            }
        }

        // Починаємо корутину при запуску сцени
        void Start()
        {
            if (sphereCollider == null)
            {
                Debug.LogError("SphereCollider is not assigned.");
                return;
            }

            StartCoroutine(SpawnSphereEvery10Seconds());
        }
    }
}