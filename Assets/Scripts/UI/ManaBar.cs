using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Messy
{
    public class ManaBar : MonoBehaviour
    {
        [SerializeField]
        private Image mask = null;

        private Player player;

        // Start is called before the first frame update
        void Start()
        {
            player = ObjectRetriever.GetPlayer();
        }

        // Update is called once per frame
        void Update()
        {
            GetCurrentFill();
        }


        void GetCurrentFill()
        {
            float fillAmount = (float)player.CurrentXP / (float)player.GetNextLevelXP();
            mask.fillAmount = fillAmount;
        }
    }
}
