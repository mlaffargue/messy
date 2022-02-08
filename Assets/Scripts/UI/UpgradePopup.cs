using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Messy
{
    public class UpgradePopup : MonoBehaviour
    {
        private UpgradePopupChoice choice1;
        private UpgradePopupChoice choice2;
        private UpgradePopupChoice choice3;
        // Start is called before the first frame update
        void Start()
        {
            choice1 = GameObject.FindGameObjectWithTag("Choice1").GetComponent<UpgradePopupChoice>();
            choice2 = GameObject.FindGameObjectWithTag("Choice2").GetComponentInChildren<UpgradePopupChoice>();
            choice3 = GameObject.FindGameObjectWithTag("Choice3").GetComponentInChildren<UpgradePopupChoice>();
        }

        public void RandomizeLoot()
        {
            // Choose 3 random loot
            List<UpgradesEnum> choices = CreateChoiceList();

            choice1.upgrade = choices[0];
            choice2.upgrade = choices[1];
            choice3.upgrade = choices[2];
        }

        private List<UpgradesEnum> CreateChoiceList()
        {
            List<UpgradesEnum> choices = new List<UpgradesEnum>();
            while (choices.Count != 3)
            {
                UpgradesEnum randomUpgrade = UpgradesEnumHelper.GetRandomUpgradeByRarity();
                if (!choices.Contains(randomUpgrade))
                {
                    choices.Add(randomUpgrade);
                }
            }

            return choices;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}