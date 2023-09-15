using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public enum EnemyEnum
    {
        Chaser,
        Swarm,
        Boss1
    }

    public static class EnemyEnumHelper
    {
        public static GameObject GetPrefab(EnemyEnum enemyEnum)
        {
            switch (enemyEnum)
            {
                case EnemyEnum.Chaser:
                    return GameAssets.i.chaserPrefab;
                case EnemyEnum.Swarm:
                    return GameAssets.i.swarmPrefab;
            }
            return null;
        }
    }
}