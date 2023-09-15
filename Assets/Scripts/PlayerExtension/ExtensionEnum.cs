using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public enum ExtensionEnum
    {   KoopaHammer,
        Laser,
        Pistol,
        Rifle,
        RocketLauncher,
        Shotgun
    }

    public static class ExtensionEnumHelper
    {
        public static GameObject GetPrefab(ExtensionEnum extensionEnum)
        {
            switch (extensionEnum)
            {
                case ExtensionEnum.KoopaHammer:
                    return GameAssets.i.koopaHammerPrefab;
                case ExtensionEnum.Laser:
                    return GameAssets.i.laserPrefab;
                case ExtensionEnum.Pistol:
                    return GameAssets.i.pistolPrefab;
                case ExtensionEnum.Rifle:
                    return GameAssets.i.riflePrefab;
                case ExtensionEnum.RocketLauncher:
                    return GameAssets.i.rocketLauncherPrefab;
                case ExtensionEnum.Shotgun:
                    return GameAssets.i.shotgunPrefab;
            }
            return null;
        }
    }
}