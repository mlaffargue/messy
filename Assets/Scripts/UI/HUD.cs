using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{
    public class HUD : MonoBehaviour
    {
        public static HUD instance = null;

        private TMPro.TextMeshProUGUI text;
        private GameManager gameManager;

        private void Awake()
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
            text = GameObject.FindGameObjectWithTag("Time").GetComponent<TMPro.TextMeshProUGUI>();
            gameManager = Camera.main.GetComponentInChildren<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {
            text.text = TimeSpan.FromSeconds(Time.time - gameManager.StartTime).ToString("mm\\:ss");
        }
    }
}