using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Messy
{
    public class EnemyManager : MonoBehaviour
    {
        public Player player;

        private static readonly float defaultChaserSpawnRate = 5f;
        private static readonly float defaultSwarmSpawnRate = 25f;
        private static readonly float defaultBoss1SpawnRate = 60f;

        private IDictionary<EnemyEnum, float> enemySpawnManager = new Dictionary<EnemyEnum, float>();


        private List<Boss1> boss1List = new List<Boss1>();

        void Start()
        {
            enemySpawnManager.Add(EnemyEnum.Chaser, -1f); // First chaser immediately
            enemySpawnManager.Add(EnemyEnum.Swarm, 65f); // First Swarm after first boss
            enemySpawnManager.Add(EnemyEnum.Boss1, defaultBoss1SpawnRate);
            player = ObjectRetriever.GetPlayer();
            StartCoroutine(Spawn());
        }

        public bool IsBossAlive()
        {
            return IsBoss1Alive();
        }

        public bool IsBoss1Alive()
        {
            boss1List.RemoveAll(item => item == null);
            return boss1List.Count > 0;
        }
        private void Update()
        {
            if (!IsBossAlive())
            {
                foreach (KeyValuePair<EnemyEnum, float> entry in enemySpawnManager.ToList())
                {
                    enemySpawnManager[entry.Key] = entry.Value - Time.deltaTime;
                }
            }
        }

        public IEnumerator Spawn()
        {
            while (true)
            {
                float currentLevel = ObjectRetriever.GetGameManager().CurrentLevel;
                if (!IsBoss1Alive() && enemySpawnManager[EnemyEnum.Boss1] < 0)
                {
                    enemySpawnManager[EnemyEnum.Boss1] = defaultBoss1SpawnRate;
                    SpawnBoss1(Mathf.FloorToInt(currentLevel /60f));
                }

                if (enemySpawnManager[EnemyEnum.Chaser] < 0)
                {
                    SpawnChaser();

                    float chaserRespawnRate = defaultChaserSpawnRate;
                    if (currentLevel < 60)
                    {
                        chaserRespawnRate = defaultChaserSpawnRate * (currentLevel+20) / 60f;
                    }
                    Debug.Log(chaserRespawnRate);
                    enemySpawnManager[EnemyEnum.Chaser] = chaserRespawnRate;
                }
                if (enemySpawnManager[EnemyEnum.Swarm] < 0)
                {
                    SpawnSwarm();
                    enemySpawnManager[EnemyEnum.Swarm] = defaultSwarmSpawnRate;
                }

                yield return null;
            }
        }

        public void SpawnBoss1(int crowns)
        {
            Vector2 enemyPos = CircleUtil.GetPointOnCircle(Vector2.zero, Random.Range(0f, 360f)) * 1.4f * ObjectRetriever.GetGameManager().ViewportSize.x;

            // Project on camera border
            enemyPos = Camera.main.ClosestBoundPoint(enemyPos);
            Boss1 boss1 = Boss1.Create(enemyPos, crowns);
            boss1List.Add(boss1);
        }

        private void SpawnChaser()
        {
            float currentLevel = ObjectRetriever.GetGameManager().CurrentLevel;

            int spawner = Random.Range(1, 3);
            int numberToSpawn = (int)(currentLevel / 10f);
            numberToSpawn = Mathf.Max(4, numberToSpawn);

            switch (spawner)
            {

                // Spiral
                case 1:
                    float waitTime = Mathf.Max(.5f, 1 / currentLevel);
                    StartCoroutine(SpawnCircle(numberToSpawn, waitTime, EnemyEnum.Chaser));
                    break;


                // Circle
                case 2:
                default:
                    StartCoroutine(SpawnCircle(numberToSpawn, 0f, EnemyEnum.Chaser));
                    break;
            }
        }

        private void SpawnSwarm()
        {
            float currentLevel = ObjectRetriever.GetGameManager().CurrentLevel;
            StartCoroutine(SpawnRandom(Mathf.FloorToInt(currentLevel /10f), 0f, EnemyEnum.Swarm));
        }

        public IEnumerator SpawnRandom(int number, float waitTime, EnemyEnum enemyType)
        {
            int instanciated = 0;
            while (instanciated < number)
            {
                Vector2 enemyPos = Vector2.zero;
                while (enemyPos.x == 0 && enemyPos.y == 0) {
                    enemyPos = Random.insideUnitCircle.normalized * 1.4f * ObjectRetriever.GetGameManager().ViewportSize.x;
                }

                // Project on camera border
                enemyPos = Camera.main.ClosestBoundPoint(enemyPos);

                Enemy.Create(enemyPos, enemyType);
                instanciated++;

                if (waitTime != 0f)
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }
            yield return null;
        }

        public IEnumerator SpawnCircle(int number, float waitTime, EnemyEnum enemyType)
        {
            int angle = 0;
            int firstAngle = Random.Range(0, 359);
            int instanciated = 0;
            while (instanciated < number)
            {   
                Vector2 enemyPos = CircleUtil.GetPointOnCircle(Vector2.zero, firstAngle + angle) * 1.3f * ObjectRetriever.GetGameManager().ViewportSize.x;

                // Project on camera border
                //enemyPos = Camera.main.ClosestBoundPoint(enemyPos);

                angle += 360 / number;

                Enemy.Create(enemyPos, enemyType);
                instanciated++;

                if (waitTime != 0f)
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }
            yield return null;
        }
    }
}