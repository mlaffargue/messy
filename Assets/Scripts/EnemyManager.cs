using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class EnemyManager : MonoBehaviour
    {
        public Player player;

        public float baseSpawnRate = 5f;
        public float swarmSpawnRate = 45f;

        private float lastSpawnTime=-5000;
        private float lastSwarmTime = -5000;


        private Vector3 viewportSize;
        void Start()
        {
            Camera mainCam = Camera.main;
            viewportSize = mainCam.ViewportToWorldPoint(new Vector3(mainCam.rect.width, mainCam.rect.height, 0));
            player = ObjectRetriever.GetPlayer();
            StartCoroutine(Spawn());
        }

        public IEnumerator Spawn()
        {
            while (true)
            {
                float currentLevel = ObjectRetriever.GetGameManager().currentLevel;

                Debug.Log(currentLevel);
                SpawnDefault(currentLevel);
                SpawnSwarm(currentLevel);

                yield return null;
            }
        }

        private void SpawnDefault(float currentLevel)
        {
            if (Time.time - lastSpawnTime > baseSpawnRate)
            {
                lastSpawnTime = Time.time;

                int spawner = Random.Range(1, 2);
                switch (spawner)
                {

                    // Spiral
                    case 1:
                        StartCoroutine(SpawnCircle((int)currentLevel * 2, 1 / currentLevel, currentLevel, EnemyEnum.Chaser));
                        break;


                    // Circle
                    case 2:
                    default:
                        StartCoroutine(SpawnCircle((int)currentLevel * 2, 0f, currentLevel, EnemyEnum.Chaser));
                        break;
                }
            }
        }

        private void SpawnSwarm(float currentLevel)
        {
            if (Time.time - lastSwarmTime > swarmSpawnRate)
            {
                lastSwarmTime = Time.time;
                StartCoroutine(SpawnRandom((int)currentLevel * 50, 0f, currentLevel, EnemyEnum.Swarm));
            }
        }

        public IEnumerator SpawnRandom(int number, float waitTime, float level, EnemyEnum enemyType)
        {
            int instanciated = 0;
            while (instanciated < number)
            {
                Vector2 enemyPos = Vector2.zero;
                while (enemyPos.x == 0 && enemyPos.y == 0) {
                    enemyPos = Random.insideUnitCircle.normalized * 2 * viewportSize.x;
                }

                // Project on camera border
                Bounds cameraBounds = Camera.main.OrthographicBounds();
                cameraBounds.size *= 1.1f;

                enemyPos = cameraBounds.ClosestPoint(enemyPos);

                Enemy.Create(enemyPos, level, enemyType);
                instanciated++;

                if (waitTime != 0f)
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }
            yield return null;
        }

        public IEnumerator SpawnCircle(int number, float waitTime, float level, EnemyEnum enemyType)
        {
            int angle = 0;
            int firstAngle = Random.Range(0, 359);
            while (angle < 360)
            {   
                Vector2 enemyPos = GetPointOnCircle(player.transform.position, firstAngle + angle) * 2 * viewportSize.x;
                // Project on camera border
                Bounds cameraBounds = Camera.main.OrthographicBounds();
                cameraBounds.size *= 1.1f;

                enemyPos = cameraBounds.ClosestPoint(enemyPos);

                angle += 360 / number;

                Enemy.Create(enemyPos, level, enemyType);

                if (waitTime != 0f)
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }
            yield return null;
        }

        Vector2 GetPointOnCircle(Vector2 center, int a)
        {
            float ang = a;
            Vector2 pos;
            pos.x = center.x + Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y + Mathf.Cos(ang * Mathf.Deg2Rad);
            return pos;
        }
    }
}