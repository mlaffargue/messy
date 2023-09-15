using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{
    public enum EnhancementEnum {
        PlayerAbsorbDistance,
        PlayerMaxLife,
        PlayerMoveSpeed,
        ShootCriticalDamage,
        ShootCriticalFrequency,
        ShootDamage,
        ShootRecoil,
        ShootSpeed,
        ShootTraversal
    }

    public enum WeaponEnum
    {
        Pistol,
        Rifle,
        RocketLauncher,
        Shotgun
    }

    public static class UpgradesEnumHelper
    {
        public static string GetText(Enum upgrade)
        {
            switch (upgrade)
            {
                // Enhancements
                case EnhancementEnum.PlayerAbsorbDistance:
                    return "XP Absorption distance +1";
                case EnhancementEnum.PlayerMaxLife:
                    return "Max Life +10";
                case EnhancementEnum.PlayerMoveSpeed:
                    return "Move Speed +%";
                case EnhancementEnum.ShootCriticalDamage:
                    return "Shoot Critical +dmg ";
                case EnhancementEnum.ShootCriticalFrequency:
                    return "Shoot Critical +% ";
                case EnhancementEnum.ShootDamage:
                    return "Shoot +dmg ";
                case EnhancementEnum.ShootSpeed:
                    return "Shoot Speed +%";
                case EnhancementEnum.ShootRecoil:
                    return "Shoot Recoil +1";
                case EnhancementEnum.ShootTraversal:
                    return "Shoot traversal +1 (no crit, less dmg)";

                // Weapons
                case WeaponEnum.Pistol:
                    return "Pistol";
                case WeaponEnum.Rifle:
                    return "Rifle";
                case WeaponEnum.RocketLauncher:
                    return "Rocket Launcher";
                case WeaponEnum.Shotgun:
                    return "Shotgun";

                default:
                    return "Unknown UpgradesEnum";
            }
        }

        public static int GetProbabilty(Enum upgradesEnum)
        {

            switch (upgradesEnum)
            {
                // Enhancements
                case EnhancementEnum.PlayerAbsorbDistance:
                    return 100;
                case EnhancementEnum.PlayerMaxLife:
                    return 150;
                case EnhancementEnum.PlayerMoveSpeed:
                    return 100;
                case EnhancementEnum.ShootCriticalDamage:
                    return 150;
                case EnhancementEnum.ShootCriticalFrequency:
                    return 150;
                case EnhancementEnum.ShootDamage:
                    return 150;
                case EnhancementEnum.ShootSpeed:
                    return 100;
                case EnhancementEnum.ShootRecoil:
                    return 50;
                case EnhancementEnum.ShootTraversal:
                    return 50;

                // Weapons
                case WeaponEnum.Pistol:
                    return 33;
                case WeaponEnum.Rifle:
                    return 33;
                case WeaponEnum.Shotgun:
                    return 33;
                case WeaponEnum.RocketLauncher:
                    return 33;

                default:
                    return 0;
            }
        }

        public static int GetProbabiltiesTotal(Type type)
        {
            int total = 0;
            ;
            foreach (Enum upgrade in Enum.GetValues(type))
            {
                total += GetProbabilty(upgrade);
            }

            return total;

        }

        public static Enum GetRandomUpgradeByRarity()
        {
            Enum upgradesEnum = null;

            int perCent = UnityEngine.Random.Range(0, GetProbabiltiesTotal(typeof(EnhancementEnum)));


            upgradesEnum = CheckPercentAgainst(EnhancementEnum.PlayerAbsorbDistance, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(EnhancementEnum.PlayerMaxLife, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(EnhancementEnum.PlayerMoveSpeed, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(EnhancementEnum.ShootCriticalDamage, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(EnhancementEnum.ShootCriticalFrequency, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(EnhancementEnum.ShootDamage, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(EnhancementEnum.ShootRecoil, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(EnhancementEnum.ShootSpeed, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(EnhancementEnum.ShootTraversal, ref perCent);

            if (upgradesEnum == null)
            {
                throw new Exception("Wrong random upgrade :" + perCent);
            }

            return (EnhancementEnum)upgradesEnum;
        }
        
        public static Enum GetRandomWeaponByRarity()
        {
            Enum weaponEnum = null;

            int perCent = UnityEngine.Random.Range(0, GetProbabiltiesTotal(typeof(WeaponEnum)));          

            weaponEnum = CheckPercentAgainst(WeaponEnum.Pistol, ref perCent);
            weaponEnum ??= CheckPercentAgainst(WeaponEnum.Rifle, ref perCent);
            weaponEnum ??= CheckPercentAgainst(WeaponEnum.RocketLauncher, ref perCent);
            weaponEnum ??= CheckPercentAgainst(WeaponEnum.Shotgun, ref perCent);

            if (weaponEnum == null)
            {
                throw new Exception("Wrong random upgrade :" + perCent);
            }

            return (WeaponEnum)weaponEnum;
        }

        private static Enum CheckPercentAgainst(Enum upgradesEnum, ref int perCent)
        {
            if (perCent < GetProbabilty(upgradesEnum))
            {
                return upgradesEnum;
            }
            perCent -= GetProbabilty(upgradesEnum);

            return null;
        }
    }
}