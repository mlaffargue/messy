using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public enum UpgradesEnum {
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
        public static class UpgradesEnumHelper
    {
        public static string GetText(UpgradesEnum upgradesEnum)
        {
            switch (upgradesEnum)
            {
                case UpgradesEnum.PlayerAbsorbDistance:
                    return "XP Absorption distance +1";
                case UpgradesEnum.PlayerMaxLife:
                    return "Max Life +10";
                case UpgradesEnum.PlayerMoveSpeed:
                    return "Move Speed +%";
                case UpgradesEnum.ShootCriticalDamage:
                    return "Shoot Critical +dmg ";
                case UpgradesEnum.ShootCriticalFrequency:
                    return "Shoot Critical +% ";
                case UpgradesEnum.ShootDamage:
                    return "Shoot +dmg ";
                case UpgradesEnum.ShootSpeed:
                    return "Shoot Speed +%";
                case UpgradesEnum.ShootRecoil:
                    return "Shoot Recoil +1";
                case UpgradesEnum.ShootTraversal:
                    return "Shoot traversal +1 (no crit, less dmg)";
                default:
                    return "Unknown UpgradesEnum";
            }
        }

        public static int GetProbabilty(UpgradesEnum upgradesEnum)
        {
            switch (upgradesEnum)
            {
                case UpgradesEnum.PlayerAbsorbDistance:
                    return 100;
                case UpgradesEnum.PlayerMaxLife:
                    return 150;
                case UpgradesEnum.PlayerMoveSpeed:
                    return 100;
                case UpgradesEnum.ShootCriticalDamage:
                    return 150;
                case UpgradesEnum.ShootCriticalFrequency:
                    return 150;
                case UpgradesEnum.ShootDamage:
                    return 150;
                case UpgradesEnum.ShootSpeed:
                    return 100;
                case UpgradesEnum.ShootRecoil:
                    return 50;
                case UpgradesEnum.ShootTraversal:
                    return 50;
                default:
                    return 0;
            }
        }

        public static int GetProbabiltiesTotal()
        {
            int total = 0;
            ;
            foreach (UpgradesEnum upgrade in Enum.GetValues(typeof(UpgradesEnum)))
            {
                total += GetProbabilty(upgrade);
            }

            return total;

        }

        public static UpgradesEnum GetRandomUpgradeByRarity()
        {
            UpgradesEnum? upgradesEnum = null;

            int perCent = UnityEngine.Random.Range(0, GetProbabiltiesTotal());          


            upgradesEnum = CheckPercentAgainst(UpgradesEnum.PlayerAbsorbDistance, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(UpgradesEnum.PlayerMaxLife, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(UpgradesEnum.PlayerMoveSpeed, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(UpgradesEnum.ShootCriticalDamage, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(UpgradesEnum.ShootCriticalFrequency, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(UpgradesEnum.ShootDamage, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(UpgradesEnum.ShootRecoil, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(UpgradesEnum.ShootSpeed, ref perCent);
            upgradesEnum ??= CheckPercentAgainst(UpgradesEnum.ShootTraversal, ref perCent);

            if (upgradesEnum == null)
            {
                throw new Exception("Wrong random upgrade :" + perCent);
            }

            return (UpgradesEnum)upgradesEnum;
        }

        private static UpgradesEnum? CheckPercentAgainst(UpgradesEnum upgradesEnum, ref int perCent)
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