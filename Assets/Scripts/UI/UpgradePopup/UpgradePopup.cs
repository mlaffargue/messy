using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Messy
{


    public class UpgradePopup : MonoBehaviour
    {
        public static UpgradePopup instance = null;

        private UpgradePopupChoice choice1;
        private UpgradePopupChoice choice2;
        private UpgradePopupChoice choice3;

        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            choice1 = GameObject.FindGameObjectWithTag("Choice1").GetComponent<UpgradePopupChoice>();
            choice2 = GameObject.FindGameObjectWithTag("Choice2").GetComponentInChildren<UpgradePopupChoice>();
            choice3 = GameObject.FindGameObjectWithTag("Choice3").GetComponentInChildren<UpgradePopupChoice>();
            GetComponent<Canvas>().worldCamera = Camera.main;
            gameObject.SetActive(false);
        }

        public void RandomizeLoot()
        {
            // Choose 3 random loot
            List<Enum> choices = CreateLootChoiceList();

            choice1.upgrade = choices[0];
            choice2.upgrade = choices[1];
            choice3.upgrade = choices[2];
        }

        public void RandomizeWeapon()
        {
            // Choose 3 random loot
            List<Enum> choices = CreateWeaponChoiceList();

            choice1.upgrade = choices[0];
            choice2.upgrade = choices[1];
            choice3.upgrade = choices[2];
        }

        private List<Enum> CreateLootChoiceList()
        {
            List<Enum> choices = new List<Enum>();
            while (choices.Count != 3)
            {
                Enum randomUpgrade = (Enum)UpgradesEnumHelper.GetRandomUpgradeByRarity();
                if (!choices.Contains(randomUpgrade))
                {
                    choices.Add(randomUpgrade);
                }
            }

            return choices;
        }
        private List<Enum> CreateWeaponChoiceList()
        {
            List<Enum> choices = new List<Enum>();
            while (choices.Count != 3)
            {
                Enum randomWeapon = (Enum)UpgradesEnumHelper.GetRandomWeaponByRarity();
                if (!choices.Contains(randomWeapon))
                {
                    choices.Add(randomWeapon);
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