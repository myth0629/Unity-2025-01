using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akila.FPSFramework
{
    [AddComponentMenu("Akila/FPS Framework/Managers/Spwan Manager")]
    public class SpawnManager : MonoBehaviour
    {
        public float spawnRadius = 5;
        public float respawnDelay = 5;
        public SpwanPoint[] sides;
        
        [Header("Enemy Spawn Settings")]
        public GameObject enemyPrefab;        // 생성할 적 프리팹
        public int maxEnemyCount = 5;         // 최대 적 수
        public float spawnInterval = 3f;      // 적 생성 간격
        
        private List<GameObject> activeEnemies = new List<GameObject>();
        private bool isSpawning = false;

        public static SpawnManager Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            StartEnemySpawn();
        }

        public void StartEnemySpawn()
        {
            if (!isSpawning)
            {
                isSpawning = true;
                StartCoroutine(SpawnEnemyRoutine());
            }
        }

        public void StopEnemySpawn()
        {
            isSpawning = false;
            StopAllCoroutines();
        }

        private IEnumerator SpawnEnemyRoutine()
        {
            while (isSpawning)
            {
                if (activeEnemies.Count < maxEnemyCount)
                {
                    SpawnEnemy();
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnEnemy()
        {
            if (enemyPrefab == null || sides.Length == 0) return;

            // 랜덤한 스폰 포인트 그룹 선택
            SpwanPoint spawnGroup = sides[Random.Range(0, sides.Length)];
            
            if (spawnGroup.points.Length == 0) return;

            // 선택된 그룹에서 랜덤한 스폰 포인트 선택
            Transform spawnPoint = spawnGroup.points[Random.Range(0, spawnGroup.points.Length)];
            
            // 스폰 포인트 주변의 랜덤한 위치 계산
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0; // Y축 고정 (필요한 경우)
            Vector3 spawnPosition = spawnPoint.position + randomOffset;

            // 적 생성
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);

            // 적이 제거될 때 리스트에서 제거하기 위한 이벤트 설정
            HealthSystem enemyHealth = enemy.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath.AddListener(() => RemoveEnemy(enemy));
            }
        }

        private void RemoveEnemy(GameObject enemy)
        {
            if (activeEnemies.Contains(enemy))
            {
                activeEnemies.Remove(enemy);
            }
        }

        public void ClearAllEnemies()
        {
            foreach (GameObject enemy in activeEnemies.ToArray())
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }
            activeEnemies.Clear();
        }

        private void OnDrawGizmos()
        {
            foreach (SpwanPoint point in sides)
            {
                foreach (Transform transform in point.points)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireSphere(transform.position, spawnRadius * transform.lossyScale.magnitude);
                }
            }
        }

        [System.Serializable]
        public class SpwanPoint
        {
            public Transform[] points;
        }
    }
}
