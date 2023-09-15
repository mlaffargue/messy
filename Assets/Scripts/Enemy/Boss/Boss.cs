using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public abstract class Boss : Enemy
    {
        protected override void Destroyed()
        {
           GameObject xpInstance = Instantiate(GameAssets.i.weaponLootPrefab, transform.position, Quaternion.identity, ObjectRetriever.GetTreeFolderXPs().transform);
        }

    }
}